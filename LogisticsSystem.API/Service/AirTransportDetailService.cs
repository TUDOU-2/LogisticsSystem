using AutoMapper;
using LogisticsSystem.Api;
using LogisticsSystem.API.Context;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;

namespace LogisticsSystem.API.Service
{
    public class AirTransportDetailService : IAirTransportDetailService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AirTransportDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(AirTransportDetailDto model)
        {
            try
            {
                var AirTransportDetail = mapper.Map<AirTransportDetail>(model);
                await unitOfWork.GetRepository<AirTransportDetail>().InsertAsync(AirTransportDetail);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, AirTransportDetail);

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
                var repository = unitOfWork.GetRepository<AirTransportDetail>();
                var airTransportDetail = await unitOfWork.GetRepository<AirTransportDetail>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

                repository.Delete(airTransportDetail);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, airTransportDetail);

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
                var repository = unitOfWork.GetRepository<AirTransportDetail>();
                var airTransportDetail = await repository.GetPagedListAsync(predicate: x =>
                    string.IsNullOrEmpty(parameter.Search) ? true : x.AirTransportId.ToString().Equals(parameter.Search),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderBy(t => t.InsertDate));

                return new ApiResponse(true, airTransportDetail);
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
                var repository = unitOfWork.GetRepository<AirTransportDetail>();
                var airTransportDetail = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, airTransportDetail);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(AirTransportDetailDto model)
        {
            try
            {
                var dbairTransportDetail = mapper.Map<AirTransportDetail>(model);
                var repository = unitOfWork.GetRepository<AirTransportDetail>();
                var airTransportDetail = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbairTransportDetail.Id));

                airTransportDetail.AirTransportId = dbairTransportDetail.AirTransportId;
                airTransportDetail.Count = dbairTransportDetail.Count;
                airTransportDetail.Weight = dbairTransportDetail.Weight;
                airTransportDetail.Volume = dbairTransportDetail.Volume;
                airTransportDetail.Width = dbairTransportDetail.Width;
                airTransportDetail.Height = dbairTransportDetail.Height;
                airTransportDetail.Length = dbairTransportDetail.Length;
                airTransportDetail.Note = dbairTransportDetail.Note;
                airTransportDetail.ReceiveDate = dbairTransportDetail.ReceiveDate;
                airTransportDetail.Tag = dbairTransportDetail.Tag;

                repository.Update(airTransportDetail);
                repository.Update(airTransportDetail);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, airTransportDetail);

                return new ApiResponse(false, "数据更新异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
