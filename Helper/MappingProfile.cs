public class MappingProfile : AutoMapper.Profile
{
  public MappingProfile()
  {
    // CreateMap<Product, ProductDTO>();
    CreateMap<ProductDTO, Product>()
      .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
      .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
      .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => HelperFunction.StringToSlug(src.Name ?? "")))
      .ForMember(dest => dest.Deleted, opt => opt.MapFrom(_ => false))
      .ReverseMap(); // auto map in both directions

    CreateMap<ProductUpdateDTO, Product>()
      .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
      .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
      .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => HelperFunction.StringToSlug(src.Name ?? "")))
      .ReverseMap(); // auto map in both directions
  }
}