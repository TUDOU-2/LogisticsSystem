using AutoMapper;
using LogisticsSystem.API.Context;
using LogisticsSystem.Api;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;

namespace LogisticsSystem.API.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(CustomerDto model)
        {
            try
            {
                var customer = mapper.Map<Customer>(model);
                await unitOfWork.GetRepository<Customer>().InsertAsync(customer);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, customer);

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
                var repository = unitOfWork.GetRepository<Customer>();
                var customer = await unitOfWork.GetRepository<Customer>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

                repository.Delete(customer);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, customer);

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
                var repository = unitOfWork.GetRepository<Customer>();
                var customers = await repository.GetPagedListAsync(predicate: x =>
                    string.IsNullOrEmpty(parameter.Search) ? true : x.Name.Contains(parameter.Search),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.InsertDate));

                return new ApiResponse(true, customers);
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
                var repository = unitOfWork.GetRepository<Customer>();
                var customer = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, customer);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(CustomerDto model)
        {
            try
            {
                var dbcustomer = mapper.Map<Customer>(model);
                var repository = unitOfWork.GetRepository<Customer>();
                var customer = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbcustomer.Id));

                customer.UserId = dbcustomer.UserId;
                customer.Name = dbcustomer.Name;
                customer.Telephone = dbcustomer.Telephone;
                customer.Nation = dbcustomer.Nation;
                customer.Address = dbcustomer.Address;
                customer.Description = dbcustomer.Description;
                customer.Tag = dbcustomer.Tag;

                repository.Update(customer);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, customer);

                return new ApiResponse(false, "数据更新异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
