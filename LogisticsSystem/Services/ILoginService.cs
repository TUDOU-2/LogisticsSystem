using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Services
{
    public interface ILoginService
    {
        Task<ApiResponse<UsersDto>> LoginAsync(UsersDto dto);
        Task<ApiResponse> RegisterAsync(UsersDto dto);
    }
}
