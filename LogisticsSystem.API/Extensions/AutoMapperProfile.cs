using AutoMapper;
using LogisticsSystem.API.Context;
using LogisticsSystem.Shared.Dtos;

namespace LogisticsSystem.API.Extensions
{
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 用于Dto类与数据库实体类之间的映射
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<Users, UsersDto>().ReverseMap();
            CreateMap<Customer,CustomerDto>().ReverseMap();
            CreateMap<AirTransport, AirTransportDto>().ReverseMap();
            CreateMap<AirTransportDetail, AirTransportDetailDto>().ReverseMap();
            CreateMap<SeaTransport, SeaTransportDto>().ReverseMap();
            CreateMap<SeaTransportDetail, SeaTransportDetailDto>().ReverseMap();
            CreateMap<ExpressTransport, ExpressTransportDto>().ReverseMap();
            CreateMap<ExpressTransportDetail, ExpressTransportDetailDto>().ReverseMap();
        }
    }
}
