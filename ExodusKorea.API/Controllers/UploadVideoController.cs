using System;
using System.Threading.Tasks;
using AutoMapper;
using ExodusKorea.API.Exceptions;
using ExodusKorea.API.Services.Interfaces;
using ExodusKorea.Data;
using ExodusKorea.Data.Interfaces;
using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExodusKorea.API.Controllers
{
    [Route("api/[controller]")]
    public class UploadVideoController : Controller
    {
        private readonly IClientIPService _clientIP;
        private readonly IMessageService _email;
        private readonly IUploadVideoRepository _repository;

        public UploadVideoController(IClientIPService clientIP,
                                     IMessageService email,
                                     IUploadVideoRepository repository)
        {
            _clientIP = clientIP;
            _email = email;
            _repository = repository;
        }

        [HttpPost]
        [Route("upload-video")]
        public async Task<IActionResult> UploadVideo([FromBody] UploadVideoVM vm)
        {
            if (!ModelState.IsValid)
            {
                string errorMsg = null;
                foreach (var m in ModelState.Values)
                    foreach (var msg in m.Errors)
                        errorMsg = msg.ErrorMessage;
                return BadRequest(errorMsg);
            }

            var ipAddress = _clientIP.GetClientIP();
            //var countryCode = await _clientIP.GetCountryCodeByIP(ipAddress);

            UploadVideo newUploadVideo = new UploadVideo
            {
                Email = vm.Email,               
                YoutubeAddress = vm.YoutubeAddress,
                DateCreated = DateTime.UtcNow,
                IpAddress = ipAddress,         
                //Country = countryCode
            };

            await _repository.AddAsync(newUploadVideo);
            await _repository.CommitAsync();

            var body = "탈조선 가입 이메일: " + vm.Email + "\r\n\r\n" +
                       "유튜브 영상 주소: " + vm.YoutubeAddress + "\r\n\r\n";

            await _email.SendEmailAsync(vm.Email, "admin@talchoseon.com", "[탈조선] 영상 신청", body, null);

            var uploadVideoVM = Mapper.Map<UploadVideo, UploadVideoVM>(newUploadVideo);

            return CreatedAtRoute("GetUploadVideo", new
            {
                controller = "UploadVideo",
                id = newUploadVideo.UploadVideoId
            }, uploadVideoVM);
        }

        [HttpGet("{id}/upload-video", Name = "GetUploadVideo")]
        public IActionResult GetUploadVideo(int? id)
        {
            if (id == null)
                return NotFound();

            var uploadVideo = _repository.GetSingle(c => c.UploadVideoId == id);

            if (uploadVideo != null)
            {
                var uploadVideoVM = Mapper.Map<UploadVideo, UploadVideoVM>(uploadVideo);

                return new OkObjectResult(uploadVideoVM);
            }
            else
                return NotFound();
        }
    }
}
