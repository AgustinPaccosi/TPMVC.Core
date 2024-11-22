using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TPMVC.Core.Web.ViewModels.Shoe
{
    public class ShoeEditVm
    {
        public int ShoeId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must have to select a brand")]
        [DisplayName("Brand")]
        public int BrandId { get; set; }
        [ValidateNever]
        public List<SelectListItem> Brands { get; set; } = null!;



        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must have to select a genre")]
        [DisplayName("Genre")]
        public int GenreId { get; set; }
        [ValidateNever]
        public List<SelectListItem> Genres { get; set; } = null!;



        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must have to select a sport")]
        [DisplayName("Sport")]
        public int SportId { get; set; }
        [ValidateNever]
        public List<SelectListItem> Sports { get; set; } = null!;



        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must have to select a colour")]
        [DisplayName("Colour")]
        public int ColourId { get; set; }
        [ValidateNever]
        public List<SelectListItem> Colours { get; set; } = null!;


        [Required(ErrorMessage = "{0} is required")]
        [StringLength(200, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
        [DisplayName("Descripcion")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(200, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
        [DisplayName("Modelo")]
        public string Model { get; set; } = null!;

        [Required(ErrorMessage = "{0} is required")]
        [Range(0.05, 1000000, ErrorMessage = "{0} must be greater than zero")]
        [DisplayName("Price")]
        public decimal Price { get; set; }
    }
}
