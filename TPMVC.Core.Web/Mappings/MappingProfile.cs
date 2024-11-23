using AutoMapper;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.Brand;
using TPMVC.Core.Web.ViewModels.Colour;
using TPMVC.Core.Web.ViewModels.Genre;
using TPMVC.Core.Web.ViewModels.Shoe;
using TPMVC.Core.Web.ViewModels.Size;
using TPMVC.Core.Web.ViewModels.Sport;

namespace TPMVC.Core.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            LoadBrandsMap();
            LoadColoursMap();
            LoadSportsMap();
            LoadGenreMap();
            LoadShoeMapping();
            LoadSizeMapping();
        }

        private void LoadBrandsMap()
        {
            CreateMap<Brand, BrandEditViewModel>().ReverseMap();
        }
        private void LoadColoursMap()
        {
            CreateMap<Colour, ColourEditViewModel>().ReverseMap();
        }
        private void LoadSportsMap()
        {
            CreateMap<Sport, SportEditViewModel>().ReverseMap();
        }
        private void LoadGenreMap()
        {
            CreateMap<Genre, GenreEditViewModel>().ReverseMap();
        }
        private void LoadShoeMapping()
        {
            CreateMap<Shoe, ShoeViewModel>().
               ForMember(dest => dest.BrandName,
               opt => opt.MapFrom(b => b.Brand!.BrandName))
               .ForMember(dest => dest.ColourName,
               opt => opt.MapFrom(c => c.Colour!.ColourName))
                .ForMember(dest => dest.GenreName,
               opt => opt.MapFrom(g => g.Genre!.GenreName))
                 .ForMember(dest => dest.SportName,
               opt => opt.MapFrom(s => s.Sport!.SportName))
                 .ForMember(dest => dest.Price,
                 opt => opt.MapFrom(p => p.Price))
                 .ForMember(dest => dest.Description,
                 opt => opt.MapFrom(p => p.Description))
                 .ForMember(dest => dest.Model,
                 opt => opt.MapFrom(p => p.Model)).ReverseMap();
            CreateMap<Shoe, ShoeEditVm>().ReverseMap();
        }

        private void LoadSizeMapping()
        {
            CreateMap<Size, SizeListVm>();
            CreateMap<Size, SizeEditVm>().ReverseMap();
        }
    }
}
