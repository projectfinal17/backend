using API.Entities;
using API.Infrastructure;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class GenericController<TEntity, TDto, TCreationDto> : Controller where TDto : class where TCreationDto : class where TEntity : BaseEntity
    {
        private readonly IGenericRepository<TEntity, TDto, TCreationDto> _genericRepository;
        private readonly DatabaseContext _context;
        private readonly DbSet<TEntity> _entities;
        private IProductRepository productCategoryRepository;
        private DatabaseContext context;

        public GenericController(IGenericRepository<TEntity, TDto, TCreationDto> genericRepository, DatabaseContext context )
        {
            _context = context;
            _entities = _context.Set<TEntity>();
            _genericRepository = genericRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntityAsync(Guid id)
        {
            try
            {
                var handledData = await _genericRepository.GetSingleAsync(id);
                return Ok(handledData);
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse(ex.Message));
            }
        }

        [Route("all")]
        public async Task<IActionResult> GetAllEntitesAsync()
        {
            var handledData = await _genericRepository.GetAllAsync();

            var items = handledData.Items.ToArray();
            int totalSize = handledData.TotalSize;

            return Ok(new { data = items, totalSize });
        }

        [HttpPost]
        public async virtual Task<IActionResult> CreateEntityAsync([FromBody] TCreationDto creationDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            try
            {
                var ProductId = await _genericRepository.CreateAsync(creationDto);
                return Created("",new { id = ProductId });
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async virtual Task<IActionResult> UpdateEntityAsync(Guid id, [FromBody] TCreationDto creationDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            try
            {
                var ProductId = await _genericRepository.EditAsync(id, creationDto);
                return Ok(new { id = ProductId });

            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse(ex.Message));
            }

        }
        [HttpDelete("{id}")]
        public async virtual Task<IActionResult> DeleteEntityAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError(ModelState));
            }
            try
            {
                var ProductId = await _genericRepository.DeleteAsync(id);
                return Ok(new { id = ProductId });

            }
            catch (Exception ex)
            {
                string name = ex.GetType().Name;
                if (name == "DbUpdateException")
                {
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    // convert CamelCase to lower_case
                    string keyLanguage = "can_not_delete_" + Regex.Replace(controllerName, @"(\p{Ll})(\p{Lu})", "$1_$2").ToLower();
                    return BadRequest(new ExceptionResponse("DbUpdateException", keyLanguage));
                }
                return BadRequest(new ExceptionResponse(ex.Message));
            }

        }
    }
}
