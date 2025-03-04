using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using System.Security.Claims;

namespace TPMVC.Core.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer,Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderHeadersService? _headersService;
        private readonly IShoesSizesService _shoesSizesService;
        private readonly IMapper _mapper;

        public OrderController(IOrderHeadersService? headersService, IShoesSizesService shoesSizesService, IMapper mapper)
        {
            _headersService = headersService;
            _shoesSizesService = shoesSizesService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var orderHeader = _headersService!.Get(filter: o => o.OrderHeaderId == id,
                propertiesNames: "OrderDetail");
            foreach (var detail in orderHeader!.OrderDetail)
            {
                var shoesizeInDetail = _shoesSizesService.Get(filter: p => p.ShoeSizeId == detail.ShoeSizeId, propertiesNames: "Shoe,Size");
                detail.ShoeSizes = shoesizeInDetail;
            }
            return View(orderHeader);
        }

        #region API CALLS
        [HttpGet]
        public JsonResult GetAll()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var orderList = _headersService!.GetAll(filter:
                o => o.ApplicationUserId == claims!.Value);
            return Json(new { data = orderList });
        }
        #endregion
    }
}
