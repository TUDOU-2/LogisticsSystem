using AutoMapper;
using LogisticsSystem.Api;
using LogisticsSystem.API.Context;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;

namespace LogisticsSystem.API.Service
{
    public class ExpressTransportService : IExpressTransportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ExpressTransportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(ExpressTransportDto model)
        {
            try
            {
                var expressTransport = mapper.Map<ExpressTransport>(model);
                var repository = await unitOfWork.GetRepository<ExpressTransport>().InsertAsync(expressTransport);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, expressTransport);

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
                var repository = unitOfWork.GetRepository<ExpressTransport>();
                var expressTransport = await unitOfWork.GetRepository<ExpressTransport>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(expressTransport);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, expressTransport);

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
                var repository = unitOfWork.GetRepository<ExpressTransport>();
                var expressTransport = await repository.GetPagedListAsync(predicate: x =>
                (string.IsNullOrEmpty(parameter.Search) ? true : x.OrderNumber.Contains(parameter.Search)) &&
                (!parameter.CustomerId.HasValue || x.CustomerId == parameter.CustomerId.Value),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.InsertDate));

                var count = await repository.CountAsync();

                return new ApiResponse(true, expressTransport, count);
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
                var repository = unitOfWork.GetRepository<ExpressTransport>();
                var expressTransportDetail = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, expressTransportDetail);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(ExpressTransportDto model)
        {
            try
            {
                var dbExpressTransport = mapper.Map<ExpressTransport>(model);
                var repository = unitOfWork.GetRepository<ExpressTransport>();
                var expressTransport = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbExpressTransport.Id));

                expressTransport.UserId = dbExpressTransport.UserId;
                expressTransport.CustomerId = dbExpressTransport.CustomerId;
                expressTransport.OrderNumber = dbExpressTransport.OrderNumber;
                expressTransport.Channel = dbExpressTransport.Channel;
                expressTransport.Count = dbExpressTransport.Count;
                expressTransport.Volume = dbExpressTransport.Volume;
                expressTransport.Weight = dbExpressTransport.Weight;
                expressTransport.Price = dbExpressTransport.Price;
                expressTransport.SourcePlace = dbExpressTransport.SourcePlace;
                expressTransport.TargetPlace = dbExpressTransport.TargetPlace;
                expressTransport.SendData = dbExpressTransport.SendData;
                expressTransport.Note = dbExpressTransport.Note;
                expressTransport.CalcWeight = dbExpressTransport.CalcWeight;
                expressTransport.OtherMoney = dbExpressTransport.OtherMoney;
                expressTransport.OtherDescription = dbExpressTransport.OtherDescription;
                expressTransport.PayMoney = dbExpressTransport.PayMoney;
                expressTransport.PayDescription = dbExpressTransport.PayDescription;
                expressTransport.PayDate = dbExpressTransport.PayDate;
                expressTransport.Tag = dbExpressTransport.Tag;

                repository.Update(expressTransport);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, expressTransport);

                return new ApiResponse(false, "数据更新异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
