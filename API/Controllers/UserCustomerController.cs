using API.Entities;
using API.Infrastructure;
using API.Models;
using API.Services;
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
    }
}
