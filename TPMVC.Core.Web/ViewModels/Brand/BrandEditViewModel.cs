using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TPMVC.Core.Web.ViewModels.Brand
{
    public class BrandEditViewModel
    {
        public int BrandId { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "No se admiten más de 50 caracteres")]
        [DisplayName("Nombre de marca")]
        public string? BrandName { get; set; }

    }
}
