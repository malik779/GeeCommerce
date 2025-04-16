using AutoMapper;
using Gee.Core.Interfaces;
using TenantApi.Service.Infrastructure.Domain;

namespace TenantApi.Service.Infrastructure
{
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        public MapperConfiguration()
        {
            CreateMap<Tenant, TenantModel>()
            .ForMember(model => model.SeName, options => options.Ignore());
            CreateMap<TenantModel, Tenant>();
        }
        public int Order => 2;
    }
}
