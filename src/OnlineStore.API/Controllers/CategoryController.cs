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
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private IUserRepository _userRepository;
        private IUserInRoleRepository _userInRoleRepository;
        private IRoleRepository _roleRepository;
        private readonly JsonSerializerSettings _serializerSettings;
        private ICategoryRepository _categoryRepository;

        public CategoryController(IUserRepository userRepository, IRoleRepository roleRepository,
            IUserInRoleRepository userInRoleRepository, ICategoryRepository categoryRepository)
        {
            _roleRepository = roleRepository;
            _userInRoleRepository = userInRoleRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpGet("GetAll", Name = "GetTable")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var data = await _categoryRepository.GetAll();
            var result = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(data.Where(x => x.ParentId == null));
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost("AddCategory", Name = "AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryViewModel category)
        {
            var _newCategory = Mapper.Map<CategoryViewModel, Category>(category);
            _newCategory.ParentId = (_newCategory.ParentId == 0) ? null : _newCategory.ParentId;
            _categoryRepository.Add(_newCategory);
            await _categoryRepository.Commit();
            var result = Mapper.Map<Category, CategoryViewModel>(_newCategory);
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }
        [HttpDelete("Delete", Name = "DeleteCategory")]
        public async Task<IActionResult> Delete(long Id)
        {
            var _delCat = await _categoryRepository.GetSingleAsync(Id);
            if (_delCat == null)
            {
                return new NotFoundResult();
            }
            _categoryRepository.Delete(_delCat);
            await _categoryRepository.Commit();
            return new NoContentResult();
        }
        [HttpGet("GetCategory", Name = "GetCateogry")]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var data = await _categoryRepository.FindByAsync(x => x.isActive == true);
            var result = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(data.Where(x=> x.ParentId == null));
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }
    }
}
