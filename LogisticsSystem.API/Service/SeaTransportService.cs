using AutoMapper;
using LogisticsSystem.Api;
using LogisticsSystem.API.Context;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;

namespace LogisticsSystem.API.Service
{
    public class SeaTransportService : ISeaTransportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SeaTransportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(SeaTransportDto model)
        {
            try
            {
                var seaTransport = mapper.Map<SeaTransport>(model);
                await unitOfWork.GetRepository<SeaTransport>().InsertAsync(seaTransport);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, seaTransport);

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
                var repository = unitOfWork.GetRepository<SeaTransport>();
                var seaTransport = await unitOfWork.GetRepository<SeaTransport>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

                repository.Delete(seaTransport);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, seaTransport);

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
                var repository = unitOfWork.GetRepository<SeaTransport>();
                var seaTransports = await repository.GetPagedListAsync(predicate: x =>
                    (string.IsNullOrEmpty(parameter.Search) ? true : x.BoxNumber.Contains(parameter.Search)) &&
                (!parameter.CustomerId.HasValue || x.CustomerId == parameter.CustomerId.Value),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.InsertDate));

                var count = await repository.GetAllAsync();

                return new ApiResponse(true, seaTransports,count.Count);
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
                var repository = unitOfWork.GetRepository<SeaTransport>();
                var seaTransport = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, seaTransport);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(SeaTransportDto model)
        {
            try
            {
                var dbSeaTransport = mapper.Map<SeaTransport>(model);
                var repository = unitOfWork.GetRepository<SeaTransport>();
                var seaTransport = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbSeaTransport.Id));

                seaTransport.CustomerId = dbSeaTransport.CustomerId;
                seaTransport.BoxModel = dbSeaTransport.BoxModel;
                seaTransport.BoxNumber = dbSeaTransport.BoxNumber;
                seaTransport.SourcePlace = dbSeaTransport.SourcePlace;
                seaTransport.TargetPlace = dbSeaTransport.TargetPlace;
                seaTransport.SendData = dbSeaTransport.SendData;
                seaTransport.Batch = dbSeaTransport.Batch;
                seaTransport.Note = dbSeaTransport.Note;
                seaTransport.Price = dbSeaTransport.Price;
                seaTransport.OtherMoney = dbSeaTransport.OtherMoney;
                seaTransport.OtherDescription = dbSeaTransport.OtherDescription;
                seaTransport.PayMoney = dbSeaTransport.PayMoney;
                seaTransport.PayDescription = dbSeaTransport.PayDescription;
                seaTransport.PayDate = dbSeaTransport.PayDate;
                seaTransport.Tag = dbSeaTransport.Tag;

                repository.Update(seaTransport);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, seaTransport);

                return new ApiResponse(false, "数据更新异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
