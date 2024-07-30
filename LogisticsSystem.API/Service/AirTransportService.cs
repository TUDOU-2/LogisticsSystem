using AutoMapper;
using LogisticsSystem.Api;
using LogisticsSystem.API.Context;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;

namespace LogisticsSystem.API.Service
{
    public class AirTransportService : IAirTransportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AirTransportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(AirTransportDto model)
        {
            try
            {
                var airTransport = mapper.Map<AirTransport>(model);
                await unitOfWork.GetRepository<AirTransport>().InsertAsync(airTransport);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, airTransport);

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
                var repository = unitOfWork.GetRepository<AirTransport>();
                var airTransport = await unitOfWork.GetRepository<AirTransport>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

                repository.Delete(airTransport);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, airTransport);

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
                var repository = unitOfWork.GetRepository<AirTransport>();
                var airTransport = await repository.GetPagedListAsync(predicate: x =>
                (string.IsNullOrEmpty(parameter.Search) ? true : x.OrderNumber.Contains(parameter.Search)) &&
                (!parameter.CustomerId.HasValue || x.CustomerId == parameter.CustomerId.Value),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.InsertDate));

                var count = await repository.CountAsync();

                return new ApiResponse(true, airTransport,count);
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
                var repository = unitOfWork.GetRepository<AirTransport>();
                var airTransport = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, airTransport);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(AirTransportDto model)
        {
            try
            {
                var dbAirTransport = mapper.Map<AirTransport>(model);
                var repository = unitOfWork.GetRepository<AirTransport>();
                var airTransport = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbAirTransport.Id));

                airTransport.CustomerId = dbAirTransport.CustomerId;
                airTransport.OrderNumber = dbAirTransport.OrderNumber;
                airTransport.SourcePlace = dbAirTransport.SourcePlace;
                airTransport.TargetPlace = dbAirTransport.TargetPlace;
                airTransport.SendData = dbAirTransport.SendData;
                airTransport.Batch = dbAirTransport.Batch;
                airTransport.Note = dbAirTransport.Note;
                airTransport.CalcWeight = dbAirTransport.CalcWeight;
                airTransport.Price = dbAirTransport.Price;
                airTransport.OtherMoney = dbAirTransport.OtherMoney;
                airTransport.OtherDescription = dbAirTransport.OtherDescription;
                airTransport.PayMoney = dbAirTransport.PayMoney;
                airTransport.PayDescription = dbAirTransport.PayDescription;
                airTransport.PayDate = dbAirTransport.PayDate;
                airTransport.Tag = dbAirTransport.Tag;

                repository.Update(airTransport);
                if(await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, airTransport);

                return new ApiResponse(false, "数据更新异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
