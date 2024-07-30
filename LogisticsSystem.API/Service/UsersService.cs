using AutoMapper;
using LogisticsSystem.Api;
using LogisticsSystem.API.Context;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;

namespace LogisticsSystem.API.Service
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UsersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(UsersDto model)
        {
            try
            {
                var users = mapper.Map<Users>(model);
                var repository = unitOfWork.GetRepository<Users>();
                var result = await repository.GetFirstOrDefaultAsync(predicate: x => x.Account.Equals(users.Account));
                if (result != null)
                    return new ApiResponse("账号已存在。");

                await repository.InsertAsync(users);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, users);

                return new ApiResponse("添加数据失败。");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<Users>();
                var users = await unitOfWork.GetRepository<Users>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

                repository.Delete(users);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, users);

                return new ApiResponse("删除数据失败。");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<Users>();
                var users = await repository.GetPagedListAsync(predicate: x =>
                    string.IsNullOrEmpty(parameter.Search) ? true : x.Account.Contains(parameter.Search),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderBy(t => t.InsertDate));

                return new ApiResponse(true, users);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<Users>();
                var users = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, users);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(UsersDto model)
        {
            try
            {
                var dbUser = mapper.Map<Users>(model);
                var repository = unitOfWork.GetRepository<Users>();
                var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbUser.Id));

                user.Account = dbUser.Account;
                user.Name = dbUser.Name;
                user.Password = dbUser.Password;
                user.Level = dbUser.Level;
                user.Tag = dbUser.Tag;

                repository.Update(user);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, user);

                return new ApiResponse(false, "数据更新异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
