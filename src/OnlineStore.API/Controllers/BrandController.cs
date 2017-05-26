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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    public class BrandController : Controller
    {
        private IProductImageRepository _productImageRepository;
        private IProductRepository _productRepository;
        private IBrandRepository _brandRepository;
        private ICategoryRepository _categoryRepository;
        private readonly appSettings _appSettings;
        private IHostingEnvironment _environment;
        private readonly JsonSerializerSettings _serializerSettings;

        public BrandController(IProductImageRepository productImageRepository, IProductRepository productRepository,
            IBrandRepository brandRepository, ICategoryRepository categoryRepository, IHostingEnvironment environment, IOptions<appSettings> appSettings)
        {
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _appSettings = appSettings.Value;
            _environment = environment;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }
        int page = 1;
        int pageSize = 10;

        [HttpGet("GetBrand")]
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
            var data = await _brandRepository.FindByAsync(x => x.BrandName.Contains(q) || x.Description.Contains(q));
            var totalData = data.Count();
            var totalPages = (int)Math.Ceiling((double)totalData / pageSize);
            Response.AddPagination(page, pageSize, totalData, totalPages);
            data.Skip(page * pageSize).Take(pageSize);
            var result = Mapper.Map<IEnumerable<Brand>, IEnumerable<BrandViewModel>>(data);
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost("AddBrand", Name = "AddBrand")]
        public async Task<IActionResult> Post([FromBody]BrandViewModel brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _newBrand = Mapper.Map<BrandViewModel, Brand>(brand);
            _brandRepository.Add(_newBrand);
            await _brandRepository.Commit();
            var _result = Mapper.Map<Brand, BrandViewModel>(_newBrand);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _brandRepository.GetAll();
            var _result = Mapper.Map<IEnumerable<Brand>, IEnumerable<BrandViewModel>>(data);
            var json = JsonConvert.SerializeObject(_result, _serializerSettings);
            return new OkObjectResult(json);
        }
        [HttpPost("PostLogo")]
        public async Task<IActionResult> PostLogo(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            var ext = "";
            var filename = Guid.NewGuid().ToString();
            if (file.Length > 0)
            {
                ext = Path.GetExtension(file.FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, filename + ext), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            var pathUrl = uploads.Replace(_environment.WebRootPath, _appSettings.HostUrl);
            Uri baseUrl = new Uri(pathUrl);
            Uri returnUrl = new Uri(baseUrl, "uploads\\" + filename + ext);
            string _result = returnUrl.ToString();
            return new OkObjectResult(_result);
        }

    }
}
