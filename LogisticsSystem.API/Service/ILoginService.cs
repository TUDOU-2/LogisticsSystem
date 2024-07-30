using LogisticsSystem.API.Context;
using LogisticsSystem.Shared.Dtos;

namespace LogisticsSystem.API.Service
{
    public interface ILoginService
    {
        Task<ApiResponse> LoginAsync(string name, string password);
        Task<ApiResponse> RegisterAsync(UsersDto user);
    }
}
