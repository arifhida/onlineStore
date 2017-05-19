using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data.Abstract;
using Newtonsoft.Json;
using OnlineStore.API.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using OnlineStore.API.Models;
using AutoMapper;
using OnlineStore.Model.Entities;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private IUserRepository _userRepository;
        private IUserInRoleRepository _userInRoleRepository;
        private IRoleRepository _roleRepository;
        private ICustomerRepository _customerRepository;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly appSettings _appSettings;
        private IHostingEnvironment _environment;


        public AddressController(IOptions<JwtIssuerOptions> jwtOptions, IUserRepository userRepository, IUserInRoleRepository userInRoleRepository,
            IRoleRepository roleRepository, ICustomerRepository customerRepository,
            IHostingEnvironment environment, IOptions<appSettings> appSettings)        
        {
            _jwtOptions = jwtOptions.Value;
            _roleRepository = roleRepository;
            _userInRoleRepository = userInRoleRepository;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _environment = environment;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]CustomerViewModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var _customer = Mapper.Map<CustomerViewModel, Customer>(customer);
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var _user = await _userRepository.GetSingleAsync(x => x.UserName == username);
            _customer.User = _user;
            _customer.UserId = _user.Id;
            _customerRepository.Add(_customer);
            await _customerRepository.Commit();
            return new NoContentResult();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(long Id, [FromBody]CustomerViewModel customer)
        {
            if(customer.Id != Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _address = Mapper.Map<CustomerViewModel, Customer>(customer);
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var _customer = Mapper.Map<CustomerViewModel, Customer>(customer);
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var _user = await _userRepository.GetSingleAsync(x => x.UserName == username);
            _address.UserId = _user.Id;
            _address.User = _user;
            _customerRepository.Update(_address);
            await _customerRepository.Commit();
            return new NoContentResult();
        }

        [HttpGet("GetAddress")]
        public async Task<IActionResult> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;            
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var _user = await _userRepository.GetSingleAsync(x => x.UserName == username);
            var _customer = await _customerRepository.GetSingleAsync(x => x.UserId == _user.Id);
            if(_customer == null)
            {
                return new NoContentResult();
            }
            var _address = Mapper.Map<Customer, CustomerViewModel>(_customer);
            var json = JsonConvert.SerializeObject(_address, _serializerSettings);
            return new OkObjectResult(json);
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
