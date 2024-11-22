using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace TPMVC.Core.Web.ViewModels.Shoe
{
    public class ShoeFilterVM
    {
        public IPagedList<ShoeViewModel>? Shoes { get; set; }
        public List<SelectListItem>? Brands { get; set; }
        public List<SelectListItem>? Sports { get; set; }
        public List<SelectListItem>? Genres { get; set; }
        public List<SelectListItem>? Colors { get; set; }
    }
}
