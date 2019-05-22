using AutoMapper;
using InfrastructureModels = Sciensoft.Samples.Products.Api.Infrastructure.Models;

namespace Sciensoft.Samples.Products.Api.Application
{
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            this.CreateMap<ViewModels.Product.Create, InfrastructureModels.Product>()
                .ForMember(target => target.Price, options => options.MapFrom(source => source.Price.Value))
                .ReverseMap()
                .ForMember(target => target.Price, options => options.MapFrom(source => new ViewModels.Price(source.Price, false)));

            this.CreateMap<ViewModels.Product.Update, InfrastructureModels.Product>()
                .ForMember(target => target.Price, options => options.MapFrom(source => source.Price.Value))
                .ReverseMap()
                .ForMember(target => target.Price, options => options.MapFrom(source => new ViewModels.Price(source.Price, false)));

            this.CreateMap<ViewModels.Product.Output, InfrastructureModels.Product>()
                .ReverseMap();
        }
    }
}
