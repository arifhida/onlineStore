using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data.Abstract;
using Newtonsoft.Json;
using OnlineStore.API.Options;
using AutoMapper;
using OnlineStore.Model.Entities;
using OnlineStore.API.Models;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserRepository _userRepository;
        private IUserInRoleRepository _userInRoleRepository;
        private IRoleRepository _roleRepository;
        private readonly JsonSerializerSettings _serializerSettings;
        int page = 1;
        int pageSize = 10;
        public UserController(IUserRepository userRepository, IRoleRepository roleRepository,
            IUserInRoleRepository userInRoleRepository)
        {
            _roleRepository = roleRepository;
            _userInRoleRepository = userInRoleRepository;
            _userRepository = userRepository;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> Get()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            //var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var pagination = Request.Headers["Pagination"];
            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out page);
                int.TryParse(vals[1], out pageSize);
            }
            var q = Request.Headers["q"].ToString();
            int currentPage = page;
            int currentPageSize = pageSize;
            var data = await _userRepository.FindByAsync(x => x.UserName.Contains(q) || x.FullName.Contains(q) || x.Email.Contains(q));
            var totalData = data.Count();
            var totalPages = (int)Math.Ceiling((double)totalData / pageSize);
            Response.AddPagination(page, pageSize, totalData, totalPages);
            data.Skip(page * pageSize).Take(pageSize);

            var _result = Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(data);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetById(long UserId)
        {
            var user = await _userRepository.GetSingleAsync(x => x.Id == UserId, r => r.UserRole);
            foreach (var item in user.UserRole)
            {
                var role = _roleRepository.GetSingle(item.RoleId);
                item.Role = role;

            }
            var _result = Mapper.Map<User, UserViewModel>(user);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }


        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(long Id, [FromBody]UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (user ==null || user.Id != Id)
            {
                return BadRequest();
            }

            var _user = Mapper.Map<UserViewModel, User>(user);
            foreach (var item in _user.UserRole)
            {
                item.UserId = _user.Id;
                item.User = _user;
            }
            _userRepository.Update(_user, excludeProperties: "Password,UserName,Salt,isActive");
            await _userRepository.Commit();
            return new NoContentResult();
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post([FromBody]UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _newUser = Mapper.Map<UserViewModel, User>(user);
            _newUser.Salt = StringHash.SaltGen();
            _newUser.Confirmation = _newUser.Salt;
            _newUser.isConfirmed = false;
            _newUser.Password = StringHash.GetHash(_newUser.Password + _newUser.Salt);
            _userRepository.Add(_newUser);
            await _userRepository.Commit();
            return new NoContentResult();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            var _user = await _userRepository.GetSingleAsync(Id);
            if(_user == null)
            {
                return new NotFoundResult();
            }
            _userRepository.Delete(_user);
            await _userRepository.Commit();
            return new NoContentResult();
        }
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> Current()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var _user = await _userRepository.GetSingleAsync(x => x.UserName == username);
            if (_user == null)
            {
                return new NotFoundResult();
            }
            var _result = Mapper.Map<User, UserViewModel>(_user);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }
    }
}
