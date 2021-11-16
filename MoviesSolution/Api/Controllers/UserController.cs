using Api.Extensions;
using Business.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]

    public class UserController : ControllerBase
    {
        IUserService _service;

        public UserController(IUserService userService)
        {
            _service = userService;
        }

        [Route("GetWatchlistByUserId")]
        [HttpGet]
        public IActionResult GetUserWatchlist(Guid userId)
        {
            var serviceResult = _service.GetUserWatchlist(userId);

            return this.GenerateActionResult(serviceResult);
        }

        [Route("GetAllUsers")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var serviceResult = _service.GetUsers();

            return this.GenerateActionResult(serviceResult);
        }

        [Route("CreateNewUser")]
        [HttpPost]
        public IActionResult NewUser(string name, string surname, string email)
        {
            var serviceResult = _service.NewUser(name, surname, email);

            return this.GenerateActionResult(serviceResult);
        }
    }
}
