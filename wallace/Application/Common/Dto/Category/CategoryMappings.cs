using AutoMapper;
using Wallace.Application.Common.Mappings;
using Wallace.Domain.Entities;

namespace Wallace.Application.Common.Dto
{
    public class CategoryMappings : IMapping<CategoryDto, Category>
    {
        public void Mapping(Profile profile)
        {
            profile
                .CreateMap<CategoryDto, Category>()
                .ForMember(c => c.Owner, opts => opts.Ignore())
                .ForMember(c => c.OwnerId, opts => opts.Ignore())
                .ForMember(c => c.Transactions, opts => opts.Ignore());
            
            profile.CreateMap<Category, CategoryDto>();
        }
    }
}