using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using ExodusKorea.Model;
using AutoMapper;
using ExodusKorea.API.Services.Interfaces;
using ExodusKorea.Data.Interfaces;

namespace ExodusKorea.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {     
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAccountRepository _repository;
        private readonly IMessageService _messageService;
        private readonly IDataProtector _protector;
        private readonly IGoogleRecaptchaService _gRecaptchaService;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IAccountRepository repository,
                                 IMessageService messageService,
                                 IDataProtectionProvider provider,
                                 IGoogleRecaptchaService gRecaptchaService)
        {         
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _repository = repository;
            _messageService = messageService;
            _protector = provider.CreateProtector("ExodusKorea");
            _gRecaptchaService = gRecaptchaService;
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

            var temporaryPwd = Guid.NewGuid().ToString("n").Substring(0, 6); // Generate a temporary password
            var passwordRequirement = "P" + temporaryPwd + "n1"; // 숫자, 대소문자 requirement
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);           
            var result = await _userManager.ResetPasswordAsync(user, token, passwordRequirement); // Temporary password is now the user's current password            
            var emailTemplateVM = new EmailTemplateVM(user.Email, "http://localhost:4200/", passwordRequirement);
            var emailFormat = emailTemplateVM.GetForgotPasswordEmailFormat();

            await _messageService.SendEmailAsync("admin@exoduscorea.com", user.Email, emailFormat.Item1, null, emailFormat.Item2);         

            return new OkResult();
        }

        [HttpPost]
        [Route("recaptcha")]
        public async Task<IActionResult> GetReCaptchaResponse([FromBody] ReCaptchaVM model)
        {
            if (string.IsNullOrWhiteSpace(model.Response))
                return BadRequest();

            var response = await _gRecaptchaService.ReCaptchaPassedAsync(model.Response);

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

            var allUsers = _repository.GetAll();

            foreach (var au in allUsers)
                if (au.NickName.ToLower().Trim().Equals(model.NickName.ToLower().Trim()))
                    return BadRequest("DuplicateNickName");

            ApplicationUser newUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NickName = model.NickName,
                DateCreated = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);  
                var userRole = new IdentityRole("User");

                if (!await _userManager.IsInRoleAsync(user, userRole.Name))
                    await _userManager.AddToRoleAsync(user, userRole.Name);

                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = new Uri(Url.Link("ConfirmEmail", new { id = _protector.Protect(user.Id), token = emailConfirmationToken }));
                var emailTemplateVM = new EmailTemplateVM(user.Email, callbackUrl.AbsoluteUri);
                var emailFormat = emailTemplateVM.GetConfirmEmailFormat();

                await _messageService.SendEmailAsync("admin@exoduscorea.com", user.Email, emailFormat.Item1, null, emailFormat.Item2);

                var userVM = Mapper.Map<ApplicationUser, ApplicationUserVM>(user);

                return CreatedAtRoute("GetUser", new { controller = "Account", id = user.Id }, userVM);
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
            var emailTemplateVM = new EmailTemplateVM(user.Email, callbackUrl.AbsoluteUri);
            var emailFormat = emailTemplateVM.GetConfirmEmailFormat();

            await _messageService.SendEmailAsync("admin@exoduscorea.com", user.Email, emailFormat.Item1, null, emailFormat.Item2);         

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

        [HttpGet]
        [Authorize]
        [Route("profile", Name = "GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var profileVM = new ProfileVM
            {
                Email = user.Email,
                NickName = user.NickName,
                DateCreated = user.DateCreated,
                DateVisitedRecent = await _repository.GetUserRecentVisit(user.Id),
                VisitCount = await _repository.GetVisitCountByUserId(user.Id),
                HasCanceledSubscription = user.HasCanceledSubscription
            };

            return new OkObjectResult(profileVM);
        }
        
        [HttpPut]
        [Route("{nickName}/change-nickName")]
        [Authorize]
        public async Task<IActionResult> ChangeNickName(string nickName)
        {
            if (string.IsNullOrWhiteSpace(nickName))
                return BadRequest();

            if (!(nickName.Length > 0 && nickName.Length < 10))
                return BadRequest("MaxLengthExceeded");

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var allUsers = _repository.GetAll();

            foreach (var au in allUsers)
                if (au.NickName.ToLower().Trim().Equals(nickName.ToLower().Trim()))
                    return BadRequest("DuplicateNickName");

            user.NickName = nickName;
            await _repository.CommitAsync();

            var userVM = Mapper.Map<ApplicationUser, ApplicationUserVM>(user);

            return new OkObjectResult(userVM);
        }

        [HttpPut]
        [Route("{hasCanceledSubscription}/update-subscription")]
        [Authorize]
        public async Task<IActionResult> UpdateSubscription(bool hasCanceledSubscription)
        {
            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            user.HasCanceledSubscription = hasCanceledSubscription;

            _repository.Update(user);
            await _repository.CommitAsync();

            return new NoContentResult();
        }

        [HttpDelete("{reason}/{password}/delete-account")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount(string reason, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var result = await _signInManager.PasswordSignInAsync(user.Email, password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                await _repository.LogWithdrewUser(reason, user);
                await _userManager.DeleteAsync(user);
                await _repository.DeleteMyVideosAsync(user.Id);

                return new NoContentResult();
            }
            else
                return BadRequest("InvalidAttempt");
        }

        [HttpPost]
        [Authorize]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                string errorMsg = null;
                foreach (var m in ModelState.Values)
                    foreach (var msg in m.Errors)
                        errorMsg = msg.ErrorMessage;
                return BadRequest(errorMsg);
            }

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                    return new NoContentResult();
                else
                {
                    string errorMsg = null;
                    foreach (var e in result.Errors)
                        errorMsg = e.Code;
                    return new BadRequestObjectResult(errorMsg);
                }                   
            }

            return NotFound();
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