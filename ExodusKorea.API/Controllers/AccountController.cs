using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ExodusKorea.API.Services;
using System;
using Microsoft.AspNetCore.DataProtection;
using AutoMapper.Configuration;
using Microsoft.Extensions.Options;
using ExodusKorea.Model;
using AutoMapper;
using ExodusKorea.API.Services.Interfaces;

namespace ExodusKorea.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {     
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMessageService _messageService;
        private readonly IDataProtector _protector;
        private readonly IGoogleRecaptchaService _gRecaptchaService;
        private readonly GoogleReCaptcha _accessor;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IMessageService messageService,
                                 IDataProtectionProvider provider,
                                 IGoogleRecaptchaService gRecaptchaService,
                                 IOptionsSnapshot<GoogleReCaptcha> gReCaptchaAccessor)
        {         
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _messageService = messageService;
            _protector = provider.CreateProtector("ExodusKorea");
            _gRecaptchaService = gRecaptchaService;
            _accessor = gReCaptchaAccessor.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordVM model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(model.Email.Trim());

            if (user == null)
                return NotFound();

            if (!user.EmailConfirmed)
                return BadRequest("EmailNotConfirmed");
        
            await _userManager.UpdateSecurityStampAsync(user); // Generate new security stamp       

            var temporaryPwd = Guid.NewGuid().ToString("n").Substring(0, 8); // Generate a temporary password
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);           
            var result = await _userManager.ResetPasswordAsync(user, token, temporaryPwd); // Temporary password is now the user's current password            
            var message = string.Format(@"<p>안녕하세요 엑소더스 코리아 회원님! <br><br>
                                        <p><strong>이메일: </strong>{0}<br>
                                        <p><strong>비밀번호: </strong>{1}<br><br><br>
                                        <p>임시 비밀번호를 통해 로그인하시고 곧바로 비밀번호를 재변경하시기 바랍니다. <br><br><br>", model.Email, temporaryPwd);

            await _messageService.SendEmailAsync(user.Email, "엑소더스 코리아 비밀번호 찾기", null, message);

            return new OkResult();
        }

        [HttpPost]
        [Route("recaptcha")]
        public async Task<IActionResult> GetReCaptchaResponse([FromBody] ReCaptchaVM model)
        {
            if (string.IsNullOrWhiteSpace(model.Response))
                return BadRequest();

            var response = await _gRecaptchaService.ReCaptchaPassedAsync(model.Response, _accessor.Secret);

            if (response)
                return new OkResult();
            else
                return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationVM model)
        {
            if (!ModelState.IsValid)
            {
                string errorMsg = null;

                foreach (var m in ModelState.Values)
                    foreach (var msg in m.Errors)
                        errorMsg = msg.ErrorMessage;

                return BadRequest(errorMsg);
            }

            var newUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NickName = model.NickName
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);  
                var userRole = new IdentityRole("User");
                // Add userAccount to a user role
                if (!await _userManager.IsInRoleAsync(user, userRole.Name))
                    await _userManager.AddToRoleAsync(user, userRole.Name);

                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = new Uri(Url.Link("ConfirmEmail", new { id = _protector.Protect(user.Id), token = emailConfirmationToken }));

                await _messageService.SendEmailAsync(user.Email,
                    "엑소더스 코리아 회원가입 인증", "회원가입을 완료하시려면 아래의 인증링크를 클릭하세요." + callbackUrl, null);

                return CreatedAtRoute("GetUser", new { controller = "Account", id = user.Id }, user);
            }            

            return new BadRequestObjectResult(result.Errors);
        }

        [HttpGet]
        [Route("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                ApplicationUserVM userVM = Mapper.Map<ApplicationUser, ApplicationUserVM>(user);
                return new OkObjectResult(userVM);
            }              
            else
                return NotFound();
        }

        [HttpGet]
        [Route("{email}/resend", Name = "Resend")]
        public async Task<IActionResult> Resend(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("ResendEmailRequired");

            var user = await _userManager.FindByEmailAsync(email.Trim());

            if (user == null)
                return NotFound();

            if (user.EmailConfirmed)
                return BadRequest("ResendEmailConfirmed");

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = new Uri(Url.Link("ConfirmEmail", new { id = _protector.Protect(user.Id), token = emailConfirmationToken }));

            //await _messageService.SendEmailAsync(user.Email,
            //    "엑소더스 코리아 회원가입 재인증", "회원가입을 완료하시려면 아래의 인증링크를 클릭하세요." + callbackUrl, null);

            return new OkResult();
        }

        [HttpGet]
        [Route("confirm-email", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(token))
                return Redirect("http://localhost:4200/error");

            var user = await _userManager.FindByIdAsync(_protector.Unprotect(id));

            if (user == null)
                return Redirect("http://localhost:4200/error");

            var emailConfirmationResult = await _userManager.ConfirmEmailAsync(user, token);

            if (!emailConfirmationResult.Succeeded)
                return Redirect("http://localhost:4200/token-expired?email=" + user.Email);

            return Redirect("http://localhost:4200/confirmed");
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return new OkResult();
        }

        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
        #endregion
    }
}