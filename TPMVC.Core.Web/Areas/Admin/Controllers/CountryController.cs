using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using TPMVC.Core.Data.Interfaces;
using X.PagedList.Extensions;

namespace TPMVC.Core.Web.Areas.Admin.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountriesService _services;
        private readonly IMapper? _mapper;

        public CountryController(ICountriesService? services, IMapper mapper)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }
        public IActionResult Index(int? page)
        {
            int pageNum = page ?? 1;
            int pageSize = 8;
            var countries=_services.GetAll().ToPagedList()
            return View();
        }
    }
}
