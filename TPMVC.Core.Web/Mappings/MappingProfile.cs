using AutoMapper;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.Brand;
using TPMVC.Core.Web.ViewModels.Colour;
using TPMVC.Core.Web.ViewModels.Genre;
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

    }
}
