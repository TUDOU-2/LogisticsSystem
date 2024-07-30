using AutoMapper;
using LogisticsSystem.Api;
using LogisticsSystem.API.Context;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;

namespace LogisticsSystem.API.Service
{
    public class ExpressTransportDetailService : IExpressTransportDetailService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ExpressTransportDetailService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(ExpressTransportDetailDto model)
        {
            try
            {
                var expressTransportDetail = mapper.Map<ExpressTransportDetail>(model);
                var repository = await unitOfWork.GetRepository<ExpressTransportDetail>().InsertAsync(expressTransportDetail);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, expressTransportDetail);

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
                var repository = unitOfWork.GetRepository<ExpressTransportDetail>();
                var expressTransportDetail = await unitOfWork.GetRepository<ExpressTransportDetail>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(expressTransportDetail);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, expressTransportDetail);

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
                var repository = unitOfWork.GetRepository<ExpressTransportDetail>();
                var expressTransportDetail = await repository.GetPagedListAsync(predicate: x =>
                    string.IsNullOrEmpty(parameter.Search) ? true : x.ExpressTransportId.ToString().Equals(parameter.Search),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderBy(t => t.InsertDate));

                var count = await repository.CountAsync();

                return new ApiResponse(true, expressTransportDetail, count);
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
                var repository = unitOfWork.GetRepository<ExpressTransportDetail>();
                var expressTransportDetail = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, expressTransportDetail);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(ExpressTransportDetailDto model)
        {
            try
            {
                var dbExpressTransportDetail = mapper.Map<ExpressTransportDetail>(model);
                var repository = unitOfWork.GetRepository<ExpressTransportDetail>();
                var expressTransportDetail = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbExpressTransportDetail.Id));

                expressTransportDetail.UserId = dbExpressTransportDetail.UserId;
                expressTransportDetail.ExpressTransportId = dbExpressTransportDetail.ExpressTransportId;
                expressTransportDetail.Productor = dbExpressTransportDetail.Productor;
                expressTransportDetail.ReceiveDate = dbExpressTransportDetail.ReceiveDate;
                expressTransportDetail.Note = dbExpressTransportDetail.Note;
                expressTransportDetail.Tag = dbExpressTransportDetail.Tag;
                expressTransportDetail.Count = dbExpressTransportDetail.Count;
                expressTransportDetail.Weight = dbExpressTransportDetail.Weight;
                expressTransportDetail.Volume = dbExpressTransportDetail.Volume;

                repository.Update(expressTransportDetail);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, expressTransportDetail);

                return new ApiResponse(false, "数据更新异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
