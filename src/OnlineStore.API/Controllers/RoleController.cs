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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private IUserRepository _userRepository;
        private IUserInRoleRepository _userInRoleRepository;
        private IRoleRepository _roleRepository;
        private readonly JsonSerializerSettings _serializerSettings;
        int page = 1;
        int pageSize = 10;

        public RoleController(IUserRepository userRepository, IRoleRepository roleRepository,
            IUserInRoleRepository userInRoleRepository)
        {
            _userInRoleRepository = userInRoleRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> Get()
        {
            var pagination = Request.Headers["Pagination"];
            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out page);
                int.TryParse(vals[1], out pageSize);
            }
            var q = Request.Headers["q"].ToString();
            q = (q == null) ? "" : q;
            int currentPage = page;
            int currentPageSize = pageSize;
            
            var data = await _roleRepository.FindByAsync(x => x.RoleName.Contains(q) || x.Description.Contains(q));
            var totalData = data.Count();
            var totalPages = (int)Math.Ceiling((double)totalData / pageSize);
            Response.AddPagination(page, pageSize, totalData, totalPages);
            data.Skip(page * pageSize).Take(pageSize);
            var _result = Mapper.Map<IEnumerable<Role>, IEnumerable<RoleViewModel>>(data);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _roleRepository.GetAll();
            var _result = Mapper.Map<IEnumerable<Role>, IEnumerable<RoleViewModel>>(data);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            var _role = await _roleRepository.GetSingleAsync(Id);
            var _result = Mapper.Map<Role, RoleViewModel>(_role);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }
        
        [HttpPost("Add")]
        public async Task<IActionResult> Post([FromBody]RoleViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _newRole = Mapper.Map<RoleViewModel, Role>(role);
            _roleRepository.Add(_newRole);
            await _roleRepository.Commit();
            return new NoContentResult();

        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(long Id, [FromBody]RoleViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (role == null || role.Id != Id)
            {
                return BadRequest();
            }
            var _role = Mapper.Map<RoleViewModel, Role>(role);
            _roleRepository.Update(_role);
            await _roleRepository.Commit();
            return new NoContentResult();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            Role _role = await _roleRepository.GetSingleAsync(Id);
            if (_role == null)
            {
                return new NotFoundResult();
            }
            _roleRepository.Delete(_role);
            await _roleRepository.Commit();
            return new NoContentResult();
        }

        [HttpGet("{RoleName}")]
        public async Task<IActionResult> Get(string RoleName)
        {
            var _roles = await _roleRepository.FindByAsync(x => x.RoleName == RoleName);
            var exist = (_roles.Count() > 0);
            if (exist)
            {
                return BadRequest("Role is already exist");
            }
            var result = new
            {
                Message = "role available"
            };
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

    }
}
