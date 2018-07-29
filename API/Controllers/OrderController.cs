using API.Entities;
using API.Infrastructure;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("Orders")]
    [Authorize]
    public class OrderController : GenericController<OrderEntity, OrderDto, OrderForCreationDto>
    {
        private readonly DatabaseContext _context;
        private readonly IOrderRepository _genericRepository;
        private readonly DbSet<OrderEntity> _entity;
        private static readonly HttpClient client = new HttpClient();


        public OrderController(IOrderRepository genericRepository, DatabaseContext context) : base(genericRepository, context)
        {
            _genericRepository = genericRepository;
            _context = context;
            _entity = _context.Set<OrderEntity>();

        }
        public async Task<IActionResult> GetOrdersAsync(
             [FromQuery] int offset,
             [FromQuery] int limit,
             [FromQuery] string keyword,
             [FromQuery] SortOptions<OrderDto, OrderEntity> sortOptions,
             [FromQuery] FilterOptions<OrderDto, OrderEntity> filterOptions)
        {
            IQueryable<OrderEntity> querySearch = _entity.Where(x => x.Code.Contains(keyword));

            var handledData = await _genericRepository.GetListAsync(offset, limit, keyword, sortOptions, filterOptions, querySearch);

            var items = handledData.Items.ToArray();
            int totalSize = handledData.TotalSize;

            return Ok(new { data = items, totalSize });
        }
        
        [HttpGet("listOrderUser")]
        public async Task<IActionResult> GetCustomerOrderASC()
        {
            try
            {
                var handledData = await _genericRepository.GetCustomerOrder();

                var items = handledData.Items.ToArray();
                int totalSize = handledData.TotalSize;

                return Ok(new { data = items, totalSize });
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse(ex.Message));
            }
        }

        [HttpPut("isDeleted/{id}/{isDeleted}")]
        public async Task<IActionResult> ChangeActiveStatus(Guid id, bool isDeleted)
        {
            try
            {
                var returnId = await _genericRepository.ChangeActiveStatusAsync(id, isDeleted);
                return Ok(new { id = returnId });
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse(ex.Message));
            }
        }


    }

}
