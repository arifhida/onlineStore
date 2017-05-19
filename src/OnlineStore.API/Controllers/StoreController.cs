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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    public class StoreController : Controller
    {

        private IUserRepository _userRepository;
        private IStoreRepository _storeRepository;
        private IProductRepository _productRepository;
        private IProductImageRepository _productImageRepository;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly appSettings _appSettings;
        private IHostingEnvironment _environment;
        int page = 1;
        int pageSize = 10;
        public StoreController(IUserRepository userRepository, IStoreRepository storeRepository,
            IProductRepository productRepository, IProductImageRepository productImageRepository,
            IHostingEnvironment environment, IOptions<appSettings> appSettings)
        {
            _userRepository = userRepository;
            _storeRepository = storeRepository;
            _appSettings = appSettings.Value;
            _environment = environment;
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
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
            int currentPage = page;
            int currentPageSize = pageSize;
            var data = await _storeRepository.FindByAsync(x => x.StoreName == q || x.Motto == q || x.User.UserName == q || x.Address == q);
            var totalData = data.Count();
            var totalPages = (int)Math.Ceiling((double)totalData / pageSize);
            Response.AddPagination(page, pageSize, totalData, totalPages);
            data.Skip(page * pageSize).Take(pageSize);
            var _result = Mapper.Map<IEnumerable<Store>, IEnumerable<StoreViewModel>>(data);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet("GetbyLogin")]
        public async Task<IActionResult> GetbyLogin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var data = await _storeRepository.GetSingleAsync(x => x.User.UserName == username);
            if(data == null)
            {
                return new NotFoundResult();
            }
            var _result = Mapper.Map<Store, StoreViewModel>(data);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }
        [HttpPost("Save")]
        public async Task<IActionResult> Post([FromBody]StoreViewModel store)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var _user = await _userRepository.GetSingleAsync(x => x.UserName == username);
            if(_user == null)
            {
                return new BadRequestResult();
            }
            var _newStore = Mapper.Map<StoreViewModel, Store>(store);
            _newStore.UserId = _user.Id;
            _newStore.User = _user;
            if (_newStore.Id == 0)
                _storeRepository.Add(_newStore);
            else
                _storeRepository.Update(_newStore);
            await _storeRepository.Commit();
            return new NoContentResult();
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            var _store = await _storeRepository.GetSingleAsync(Id);
            if(_store == null)
            {
                return new NotFoundResult();
            }
            _storeRepository.Delete(_store);
            await _storeRepository.Commit();
            return new NoContentResult();
        }

        [HttpPost("PostLogo")]
        public async Task<IActionResult> PostLogo(IFormFile file)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var _user = await _userRepository.GetSingleAsync(x => x.UserName == username);
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            var _path = Path.Combine(uploads, username);
            if (!Directory.Exists(_path))
                await Task.Run(() => Directory.CreateDirectory(_path));
            var ext = "";
            if (file.Length > 0)
            {
                ext = Path.GetExtension(file.FileName);
                using (var fileStream = new FileStream(Path.Combine(_path, "StoreLogo" + ext), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            var pathUrl = uploads.Replace(_environment.WebRootPath, _appSettings.HostUrl);
            Uri baseUrl = new Uri(pathUrl);
            Uri returnUrl = new Uri(baseUrl, "uploads\\" + username + "\\StoreLogo" + ext);
            string _result = returnUrl.ToString();
            Random rnd = new Random();
            int r = rnd.Next(1, 100);
            _result += "?q=" + r.ToString();
            return new OkObjectResult(_result);
        }

        [HttpGet("{StoreName}")]
        public async Task<IActionResult> Get(string StoreName)
        {
            var _store = await _storeRepository.FindByAsync(x => x.StoreName == StoreName);
            if (_store.Count() > 0)
            {
                return BadRequest("Store name is already exist");
            }
            var result = new
            {
                Message = "name available"
            };
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
