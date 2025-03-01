using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.ApplicationUser;
using TPMVC.Core.Web.ViewModels.Brand;
using TPMVC.Core.Web.ViewModels.City;
using TPMVC.Core.Web.ViewModels.Colour;
using TPMVC.Core.Web.ViewModels.Country;
using TPMVC.Core.Web.ViewModels.Genre;
using TPMVC.Core.Web.ViewModels.Shoe;
using TPMVC.Core.Web.ViewModels.ShoppingCart;
using TPMVC.Core.Web.ViewModels.Size;
using TPMVC.Core.Web.ViewModels.Sport;
using TPMVC.Core.Web.ViewModels.State;

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
            LoadCountryMapping();
            LoadStateMapping();
            LoadCityMapping();
            LoadApplicationUser();
            LoadShoppinCartMapping();
        }

        private void LoadShoppinCartMapping()
        {
            CreateMap<ShoppingCartDetailVm, ShoppingCart>()
                .ForMember(dest => dest.ShoeSizeId, opt => opt.MapFrom(src => src.ShoeSizeId))
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.ApplicationUserId));

            //CreateMap<ShoppingCart, OrderDetail>()
            //    .ForMember(dest => dest.OrderHeaderId, opt => opt.Ignore())
            //    .ForMember(dest => dest.ShoeSizes, opt => opt.Ignore())
            //    .ForMember(dest => dest.ShoeSizeId, opt => opt.MapFrom(src => src.ShoeSizeId))
            //    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            //    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (src.Quantity == 1 ? src.ShoeSize.Shoe.Price : src.ShoeSize.Shoe.Price * 0.9M)));
        }

        private void LoadApplicationUser()
        {
            CreateMap<ApplicationUser, ApplicationUserListVm>().ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.CountryName))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.StateName))
                .ForMember(dest=>dest.City, opt=>opt.MapFrom(src=>src.City.CityName));
        }

        private void LoadCityMapping()
        {
            CreateMap<City, CityListVm>().ForMember(dest=>dest.CountryName, opt=>opt.MapFrom(src=>src.Country.CountryName))
                .ForMember(dest=>dest.StateName, opt=>opt.MapFrom(src=>src.States.StateName));
            CreateMap<City, CityEditVm>().ReverseMap();
        }

        private void LoadStateMapping()
        {
            CreateMap<State, StateListVM>().ForMember(dest=>dest.Country, opt=>opt.MapFrom(src=>src.Country.CountryName));
            CreateMap<State, StateEditVM>().ReverseMap();
        }

        private void LoadCountryMapping()
        {
            CreateMap<Country, CountryEditVM>().ReverseMap();
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
                 opt => opt.MapFrom(p => p.Model))
                 .ForMember(dest=>dest.PrecioEfectivo, opt=>opt.MapFrom(p=>p.Price*0.6m)).ReverseMap();
            CreateMap<Shoe, ShoeEditVm>().ReverseMap();
            
        }

        private void LoadSizeMapping()
        {
            CreateMap<Size, SizeListVm>();
            CreateMap<Size, SizeEditVm>().ReverseMap();
        }
       
    }
}
