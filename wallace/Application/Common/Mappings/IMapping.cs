using AutoMapper;

namespace Wallace.Application.Common.Mappings
{
    public interface IMapping<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}