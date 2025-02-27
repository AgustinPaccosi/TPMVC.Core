using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using MVC.Core.Services.Services;
using System.Diagnostics;
using TPMVC.Core.Web.Models;

namespace TPMVC.Core.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IShoesService shoesService;
        private readonly IShoesSizesService shoesSizesService;
        private readonly ISizesService sizesService;
        private readonly IMapper mapper;
        public HomeController(ILogger<HomeController> logger, IShoesService ShoeService, IShoesSizesService ShoesSizesService,
            ISizesService SizesService, IMapper Mapper)
        {
            _logger = logger;
            shoesService = ShoeService ?? throw new ApplicationException("Dependencies not set");
            shoesSizesService=ShoesSizesService ?? throw new ApplicationException("Dependencies not set");
            mapper = Mapper ?? throw new ApplicationException("Dependencies not set");
            sizesService = SizesService ?? throw new ApplicationException("Dependencies not set");
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Hero()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
