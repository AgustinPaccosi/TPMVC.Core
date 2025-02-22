using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace TPMVC.Core.Web.ViewModels.State
{
    public class StateFilterVM
    {
        public IPagedList<StateListVM>? States { get; set; }
        public List<SelectListItem>? Countries { get; set; }
    }
}
