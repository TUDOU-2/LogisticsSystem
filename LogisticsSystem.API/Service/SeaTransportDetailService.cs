using AutoMapper;
using LogisticsSystem.Api;
using LogisticsSystem.API.Context;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;

namespace LogisticsSystem.API.Service
{
    public class SeaTransportDetailService : ISeaTransportDetailService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SeaTransportDetailService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(SeaTransportDetailDto model)
        {
            try
            {
                var seaTransportDetail = mapper.Map<SeaTransportDetail>(model);
                var repository = await unitOfWork.GetRepository<SeaTransportDetail>().InsertAsync(seaTransportDetail);
                if(await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, seaTransportDetail);

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
                var repository = unitOfWork.GetRepository<SeaTransportDetail>();
                var seaTransportDetail = await unitOfWork.GetRepository<SeaTransportDetail>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

                repository.Delete(seaTransportDetail);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, seaTransportDetail);

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
                try
                {
                    var repository = unitOfWork.GetRepository<SeaTransportDetail>();
                    var airTransport = await repository.GetPagedListAsync(predicate: x =>
                        string.IsNullOrEmpty(parameter.Search) ? true : x.SeaTransportId.ToString().Equals(parameter.Search),
                        pageIndex: parameter.PageIndex,
                        pageSize: parameter.PageSize,
                        orderBy: source => source.OrderBy(t => t.InsertDate));

                    var count = await repository.CountAsync();

                    return new ApiResponse(true, airTransport, count);
                }
                catch (Exception ex)
                {
                    return new ApiResponse(ex.Message);
                }
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
                var repository = unitOfWork.GetRepository<SeaTransportDetail>();
                var seaTransportDetail = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, seaTransportDetail);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(SeaTransportDetailDto model)
        {
            try
            {
                var dbSeaTransportDetail = mapper.Map<SeaTransportDetail>(model);
                var repository = unitOfWork.GetRepository<SeaTransportDetail>();
                var seaTransportDetail = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbSeaTransportDetail.Id));

                seaTransportDetail.SeaTransportId = dbSeaTransportDetail.SeaTransportId;
                seaTransportDetail.USerId = dbSeaTransportDetail.USerId;
                seaTransportDetail.Productor = dbSeaTransportDetail.Productor;
                seaTransportDetail.ReceiveDate = dbSeaTransportDetail.ReceiveDate;
                seaTransportDetail.Count = dbSeaTransportDetail.Count;
                seaTransportDetail.Volume = dbSeaTransportDetail.Volume;
                seaTransportDetail.Note = dbSeaTransportDetail.Note;
                seaTransportDetail.Tag = dbSeaTransportDetail.Tag;

                repository.Update(seaTransportDetail);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, seaTransportDetail);

                return new ApiResponse(false, "数据更新异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
