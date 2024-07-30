using AutoMapper;
using LogisticsSystem.Api;
using LogisticsSystem.API.Context;
using LogisticsSystem.Shared;
using LogisticsSystem.Shared.Dtos;

namespace LogisticsSystem.API.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LoginService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> LoginAsync(string account, string password)
        {
            try
            {
                password.GetMD5();
                var model = await unitOfWork.GetRepository<Users>().GetFirstOrDefaultAsync(predicate: x =>
                x.Account.Equals(account) && x.Password.Equals(password));

                if (model == null)
                    return new ApiResponse("账号或密码错误，请重试。");
                
                return new ApiResponse(true,model);
            }
            catch (Exception ex)
            {
                return new ApiResponse("网络异常，登录失败。");
            }
        }

        public async Task<ApiResponse> RegisterAsync(UsersDto user)
        {
            try
            {
                var model = mapper.Map<Users>(user);
                var repository = unitOfWork.GetRepository<Users>();

                var userModel = await repository.GetFirstOrDefaultAsync(predicate: x => x.Name.Equals(model.Name));
                if(userModel != null)
                    return new ApiResponse($"用户名{model.Name}已存在，请更换用户名。");

                model.Password.GetMD5();

                await repository.InsertAsync(model);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);

                return new ApiResponse("注册失败，请稍后重试。");
            }
            catch (Exception ex)
            {
                return new ApiResponse("网络异常，请稍后重试。");
            }
        }
    }
}
