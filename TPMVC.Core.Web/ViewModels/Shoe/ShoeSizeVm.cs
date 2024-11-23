using Microsoft.AspNetCore.Mvc.Rendering;
//using TPMVC.Core.Entities; 

namespace TPMVC.Core.Web.ViewModels.Shoe
{

    public class ShoeSizeVm
    {
        public int ShoeId { get; set; }
        public TPMVC.Core.Entities.Brand Brand { get; set; }
        public TPMVC.Core.Entities.Colour Colour { get; set; }
        public TPMVC.Core.Entities.Genre Genre { get; set; }
        public TPMVC.Core.Entities.Sport Sport { get; set; }
        public int SizeId { get; set; }
        public int Stock { get; set; }

        public List<SelectListItem> Sizes { get; set; } = new List<SelectListItem>();
    }
}


