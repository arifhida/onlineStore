using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineStore.API.Options;
using Microsoft.AspNetCore.Hosting;
using OnlineStore.Data.Abstract;
using Microsoft.Extensions.Options;
using AutoMapper;
using OnlineStore.Model.Entities;
using OnlineStore.API.Models;
using System.Security.Claims;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private IUserRepository _userRepository;
        private IStoreRepository _storeRepository;
        private IProductRepository _productRepository;
        private IProductImageRepository _imageRepository;
        private ICategoryRepository _categoryRepository;
        private IBrandRepository _brandRepository;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly appSettings _appSettings;
        private IHostingEnvironment _environment;
        int page = 1;
        int pageSize = 10;


        public ProductController(IUserRepository userRepository,IStoreRepository storeRepository, 
            IProductRepository productRepository, IProductImageRepository imageRepository,
            ICategoryRepository categoryRepository, IBrandRepository brandRepository,
            IHostingEnvironment environment, IOptions<appSettings> appSettings
            )
        {
            _userRepository = userRepository;
            _storeRepository = storeRepository;
            _appSettings = appSettings.Value;
            _environment = environment;
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _appSettings = appSettings.Value;
            _environment = environment;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpGet]
        [AllowAnonymous]
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

            var data = await _productRepository.FindByAsyncIncluding(x => x.SKU.ToLower().Contains(q) || x.ProductName.ToLower().Contains(q) ||
                        x.ProductDescription.ToLower().Contains(q) || x.Store.StoreName.ToLower().Contains(q) || 
                        x.Brand.BrandName.ToLower().Contains(q), x => x.Brand, x => x.Category, x=> x.Image, x=> x.Store);
            var totalData = data.Count();
            var totalPages = (int)Math.Ceiling((double)totalData / pageSize);
            Response.AddPagination(page, pageSize, totalData, totalPages);
            data.Skip(page * pageSize).Take(pageSize);
            var _result = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(data);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);

        }
        [HttpGet("Category/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetbyCategory(long Id)
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

            var data = await _productRepository.FindByAsyncIncluding(x => (x.SKU.ToLower().Contains(q) || x.ProductName.ToLower().Contains(q) ||
                        x.ProductDescription.ToLower().Contains(q) || x.Store.StoreName.ToLower().Contains(q) ||
                        x.Brand.BrandName.ToLower().Contains(q)) && x.CategoryId == Id, x => x.Brand, x => x.Category, x => x.Image, x => x.Store);
            
            var totalData = data.Count();
            var totalPages = (int)Math.Ceiling((double)totalData / pageSize);
            Response.AddPagination(page, pageSize, totalData, totalPages);
            data.Skip(page * pageSize).Take(pageSize);
            var _result = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(data);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);

        }

        [HttpPost("Add")]
        public async Task<IActionResult> Post([FromBody]ProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var _user = await _userRepository.GetSingleAsync(x => x.UserName == username,x=> x.Store);
            if (_user == null || _user.Store == null)
            {
                return BadRequest();
            }
            var _newProd = Mapper.Map<ProductViewModel, Product>(product);
            _newProd.StoreId = _user.Store.Id;
            _newProd.Store = _user.Store;
            _productRepository.Add(_newProd);
            await _productRepository.Commit();
            return new NoContentResult();
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(long Id, [FromBody]ProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (product == null || product.Id != Id)
            {
                return BadRequest();
            }
            var _prod = Mapper.Map<ProductViewModel, Product>(product);
            foreach (var item in _prod.Image)
            {
                item.ProductId = _prod.Id;
            }
            _productRepository.Update(_prod, excludeProperties: "StoreId");
            await _productRepository.Commit();
            return new NoContentResult();

        }
        [HttpGet("User")]
        public async Task<IActionResult> GetByUser()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var _user = await _userRepository.GetSingleAsync(x => x.UserName == username, x => x.Store);
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
            var data = await _productRepository.FindByAsyncIncluding(x => (x.SKU.ToLower().Contains(q) || x.ProductName.ToLower().Contains(q) ||
                        x.ProductDescription.ToLower().Contains(q) || x.Brand.BrandName.ToLower().Contains(q)) && x.StoreId == _user.Store.Id,
                        x => x.Brand, x => x.Category, x=> x.Image);
            var totalData = data.Count();
            
            var totalPages = (int)Math.Ceiling((double)totalData / pageSize);
            Response.AddPagination(page, pageSize, totalData, totalPages);
            data.Skip(page * pageSize).Take(pageSize);
            var _result = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(data);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost("PostImage")]
        public async Task<IActionResult> PostImage(List<IFormFile> files)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var username = claims.Where(c => c.Type == "Username").FirstOrDefault().Value;
            var _user = await _userRepository.GetSingleAsync(x => x.UserName == username);
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            var _path = Path.Combine(uploads, username);
            if (!Directory.Exists(_path))
                await Task.Run(() => Directory.CreateDirectory(_path));
            var images = new List<ProductImageViewModel>();
            var pathUrl = uploads.Replace(_environment.WebRootPath, _appSettings.HostUrl);
            Uri baseUrl = new Uri(pathUrl);
            foreach (var file in files)
            {
                var ext = "";
                var filename = Guid.NewGuid().ToString();
                if (file.Length > 0)
                {
                    ext = Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(_path, filename + ext), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                Uri returnUrl = new Uri(baseUrl, "uploads\\" + username + "\\" + filename + ext);
                string _result = returnUrl.ToString();
                images.Add(new ProductImageViewModel { Id = 0, ImageUrl = _result, ProductId = 0, ProductName = "" });
            }
            var json = JsonConvert.SerializeObject(images, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet("CheckSKU/{SKU}/{StoreId}")]
        public async Task<IActionResult> CheckSKU(string SKU,long StoreId)
        {
            var _product = await _productRepository.FindByAsync(x => x.SKU == SKU && x.StoreId == StoreId);
            var exist = _product.Count() > 0;
            if (exist)
            {
                return BadRequest("SKU is already exist");
            }
            var result = new
            {
                Message = "SKU available"
            };
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            var _del = await _productRepository.GetSingleAsync(Id);
            if(_del == null)
            {
                return new NotFoundResult();
            }
            _productRepository.Delete(_del);
            await _productRepository.Commit();
            return new NoContentResult();
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetbyId(long Id)
        {
            var product = await _productRepository.GetSingleAsync(x => x.Id == Id, x => x.Image, x => x.Brand, x => x.Category);

            var _result = Mapper.Map<Product, ProductViewModel>(product);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpDelete("Image/{Id}")]
        public async Task<IActionResult> DeleteImage(long Id)
        {
            var _image = await _imageRepository.GetSingleAsync(x=> x.Id ==Id);
            if(_image == null)
            {
                return new NotFoundResult();
            }
            var url = _image.ImageUrl;
            _imageRepository.Delete(_image);
            await _imageRepository.Commit();

            Uri u = new Uri(url);
            var path = u.LocalPath.Replace("/", "\\");
            var localPath = string.Format("{0}{1}", _environment.WebRootPath, path);
            
            FileInfo file = new FileInfo(localPath);
            await Task.Factory.StartNew(() => file.Delete());

            return new NoContentResult();

        }

    }
}
