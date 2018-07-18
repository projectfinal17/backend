using API.Entities;
using API.Infrastructure;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("UserCustomers")]
    public class UserCustomerController : Controller
    {
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;

        private readonly DatabaseContext _context;
        private readonly DbSet<UserEntity> _entity;
        private readonly IUserCustomerRepository _userCustomerRepository;

        public UserCustomerController(
            IOptions<IdentityOptions> identityOptions,
            SignInManager<UserEntity> signInManager,
            UserManager<UserEntity> userManager,
            DatabaseContext context,
            IUserCustomerRepository userCustomerRepository
            )
        {
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _userCustomerRepository = userCustomerRepository;
            _entity = _context.Set<UserEntity>();

        }

        [HttpPost]
        public async Task<IActionResult> CreateEntityAsync([FromBody] UserForCreationDto creationDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            try
            {
                var ProductId = await _userCustomerRepository.CreateAsync(creationDto);
                return Created("", new { id = ProductId });
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse(ex.Message));
            }
        }

        [Authorize]
        public async Task<IActionResult> GetUsersAsync(
          [FromQuery] int offset,
          [FromQuery] int limit,
          [FromQuery] string keyword,
          [FromQuery] SortOptions<UserDto, UserEntity> sortOptions,
          [FromQuery] FilterOptions<UserDto, UserEntity> filterOptions)
        {

            IQueryable<UserEntity> querySearch;

            querySearch = _entity.Where(x =>
                 x.FirstName.Contains(keyword)
                 || x.LastName.Contains(keyword)
                 || x.PhoneNumber.Contains(keyword)
                 || x.Email.Contains(keyword)
                );

            var handledData = await _userCustomerRepository.GetListAsync(offset, limit, keyword, sortOptions, filterOptions, querySearch);

            var items = handledData.Items.ToArray();
            int totalSize = handledData.TotalSize;

            return Ok(new { data = items, totalSize });
        }

        [Authorize]
        [Route("self")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUsersAsync()
        {
            try
            {
                var user = await _userCustomerRepository.GetCurrentUser();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse(ex.Message));
            }

        }
    }
}
