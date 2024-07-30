using LogisticsSystem.Api;
using LogisticsSystem.API.Service;
using LogisticsSystem.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService service;

        public LoginController(ILoginService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] UsersDto param) => await service.LoginAsync(param.Account, param.Password);

        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UsersDto model) => await service.RegisterAsync(model);
      
    }
}
