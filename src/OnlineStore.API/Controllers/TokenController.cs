using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Options;
using OnlineStore.Data.Abstract;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using OnlineStore.API.Models;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private IUserRepository _userRepository;
        private readonly JsonSerializerSettings _serializerSettings;
        //private IUserInRoleRepository _userInRoleRepository;
        private IRoleRepository _roleRepository;

        public TokenController(IOptions<JwtIssuerOptions> jwtOptions, IUserRepository userRepository,
             IRoleRepository roleRepository)
        {
            _jwtOptions = jwtOptions.Value;
            _userRepository = userRepository;

            _roleRepository = roleRepository;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromBody] ApplicationUser applicationUser)
        {
            var userIdentity = await GetClaimIdentity(applicationUser);
            if (userIdentity == null)
            {
                return BadRequest("invalid credential");
            }
            var roles = await _userRepository.GetSingleAsync(x => x.UserName == applicationUser.Username,
                y => y.UserRole);
            var confirmed = roles.isConfirmed;
            var roleClaims = new List<Claim>();
            foreach (var item in roles.UserRole)
            {
                var role = _roleRepository.GetSingle(item.RoleId);
                roleClaims.Add(new Claim("Roles", role.RoleName));
            }
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                    userIdentity.FindFirst("Username")
                };

            var claimlist = claims.ToList();
            claimlist.AddRange(roleClaims);


            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claimlist.AsEnumerable(),
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                Confirmed = confirmed,
                access_token = encodedJwt,

                expires_in = (int)_jwtOptions.ValidFor.TotalDays
            };
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        private static long ToUnixEpochDate(DateTime date)
         => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private Task<ClaimsIdentity> GetClaimIdentity(ApplicationUser appUser)
        {
            var result = _userRepository.GetSingle(x => x.UserName == appUser.Username && x.isActive == true);

            if (result != null && result.Password == StringHash.GetHash(appUser.Password + result.Salt))
            {
                var identity = new ClaimsIdentity(
                    new GenericIdentity(appUser.Username, "Token"),
                    new[]
                    {
                        new Claim("Username",appUser.Username)

                    }
                    );

                return Task.FromResult(identity);
            }
            return Task.FromResult<ClaimsIdentity>(null);
        }


    }
}
