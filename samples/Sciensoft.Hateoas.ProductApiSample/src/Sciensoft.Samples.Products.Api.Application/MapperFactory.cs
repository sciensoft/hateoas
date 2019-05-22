using AutoMapper;

namespace Sciensoft.Samples.Products.Api.Application
{
    public static class MapperFactory
    {
        public static IMapper CreateMapper()
        {
            return new MapperConfiguration(config =>
            {
                // TODO : Use Reflection for AutoMapper Profiles discovery
                config.AddProfile<ModelMapperProfile>();
            }).CreateMapper();
        }
    }
}
