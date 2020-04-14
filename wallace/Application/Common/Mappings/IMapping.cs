using AutoMapper;

namespace Wallace.Application.Common.Mappings
{
    public interface IMapping<TSource, TDestination>
    {
        void Mapping(Profile profile) => profile.CreateMap(
            typeof(TSource), 
            typeof(TDestination)
        );
    }
}