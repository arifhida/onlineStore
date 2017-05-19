using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data.Abstract;
using Newtonsoft.Json;
using OnlineStore.API.Models;
using OnlineStore.Model.Entities;
using AutoMapper;
using OnlineStore.API.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using OnlineStore.API.Core;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private IUserRepository _userRepository;
        private IUserInRoleRepository _userInRoleRepository;
        private IRoleRepository _roleRepository;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly appSettings _appSettings;
        private IHostingEnvironment _environment;
        private IEmailSender _mailSender;
        public CustomerController(IOptions<JwtIssuerOptions> jwtOptions, IUserRepository userRepository, IRoleRepository roleRepository,
           IUserInRoleRepository userInRoleRepository, IHostingEnvironment environment,IOptions<appSettings> appSettings, IEmailSender emailsender)
        {
            _jwtOptions = jwtOptions.Value;
            _roleRepository = roleRepository;
            _userInRoleRepository = userInRoleRepository;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _environment = environment;
            _mailSender = emailsender;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User _newUser = Mapper.Map<UserViewModel, User>(user);
            _newUser.Salt = StringHash.SaltGen();
            _newUser.Confirmation = _newUser.Salt;
            _newUser.isConfirmed = false;
            _newUser.Password = StringHash.GetHash(_newUser.Password + _newUser.Salt);
            _userRepository.Add(_newUser);
            await _userRepository.Commit();
            var callbackUrl = "http://localhost:58969/" + Url.Action("Confirmation", "Customer", new { UserId = _newUser.Id, Code = _newUser.Confirmation });


            await _mailSender.SendEmail(_newUser.Email, "Confirm your account",
                 $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
            var identity = new ClaimsIdentity(
                   new GenericIdentity(_newUser.UserName, "Token"),
                   new[]
                   {
                        new Claim("Username",_newUser.UserName)

                   });
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, _newUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                    identity.FindFirst("Username")
                };

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var result = new
            {
                Status = "Registration Success",
                Confirmed = false,
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalDays
            };
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }
        private static long ToUnixEpochDate(DateTime date)
         => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        [HttpPost("CheckUserName")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckUserName([FromBody]string username)
        {
            var _users = await _userRepository.FindByAsync(x => x.UserName == username);
            var exist = _users.Count() > 0;
            if (exist)
            {
                return BadRequest("Username is already exist");
            }
            var result = new
            {
                Message = "user available"
            };
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }
        [HttpPost("CheckEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmail([FromBody]string email)
        {
            var _users = await _userRepository.FindByAsync(x => x.Email == email);
            var exist = _users.Count() > 0;
            if (exist)
            {
                return BadRequest("email is already exist");
            }
            var result = new
            {
                Message = "email available"
            };
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody]UserViewModel user)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            if(user.UserName != username)
            {
                return new NotFoundResult();
            }
            var _user = Mapper.Map<UserViewModel, User>(user);
            _userRepository.Update(_user, excludeProperties: "Password,UserName,Salt,isActive");
            await _userRepository.Commit();
            return new NoContentResult();
        }

        [HttpPost("PostImage")]
        public async Task<IActionResult> PostImage(IFormFile file)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var _user = await _userRepository.GetSingleAsync(x => x.UserName == username);
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            var ext = "";

            if (file.Length > 0)
            {
                ext = Path.GetExtension(file.FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, username + ext), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            var pathUrl = uploads.Replace(_environment.WebRootPath, _appSettings.HostUrl);
            Uri baseUrl = new Uri(pathUrl);
            Uri returnUrl = new Uri(baseUrl, "uploads\\" + username + ext);
            string _result = returnUrl.ToString();

            
            _user.Photo = _result;
            _userRepository.Update(_user, "Password,UserName,Salt,isActive");
            await _userRepository.Commit();
            return new OkObjectResult(_result);
        }
        
        [HttpGet("Confirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> Confirmation(long UserId, string Code)
        {
            var _user = await _userRepository.GetSingleAsync(x => x.Id == UserId && x.Confirmation == Code);
            if(_user == null)
            {
                return new NotFoundResult();
            }
            var _role = await _roleRepository.GetSingleAsync(x => x.RoleName == "user");
            _user.UserRole.Add(new UserInRole { Id = 0, RoleId = _role.Id, Role = _role });
            _user.isConfirmed = true;
            _userRepository.Update(_user, excludeProperties: "Password,UserName,Salt,isActive");
            await _userRepository.Commit();
            return Redirect("http://localhost:58969?q=login");
        }
    }
}
