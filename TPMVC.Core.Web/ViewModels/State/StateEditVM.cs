using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TPMVC.Core.Web.ViewModels.State
{
    public class StateEditVM
    {
        public int StateId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must have to select a country")]
        [DisplayName("Country")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(200, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
        [DisplayName("State Name")]
        public string StateName { get; set; } = null!;
        [ValidateNever]
        public List<SelectListItem> Countries { get; set; } = null!;
    }
}
