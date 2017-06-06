using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data.Abstract;
using Microsoft.AspNetCore.Hosting;
using OnlineStore.API.Options;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using AutoMapper;
using OnlineStore.API.Models;
using OnlineStore.Model.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    public class FrontController : Controller
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
        int pageSize = 20;

        public FrontController(IUserRepository userRepository, IStoreRepository storeRepository,
            IProductRepository productRepository, IProductImageRepository imageRepository,
            ICategoryRepository categoryRepository, IBrandRepository brandRepository,
            IHostingEnvironment environment, IOptions<appSettings> appSettings)
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
                        x.Brand.BrandName.ToLower().Contains(q), x => x.Brand, x => x.Category);
            var totalData = data.Count();
            var totalPages = (int)Math.Ceiling((double)totalData / pageSize);
            Response.AddPagination(page, pageSize, totalData, totalPages);
            data.Skip(page * pageSize).Take(pageSize);
            var _result = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(data);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
