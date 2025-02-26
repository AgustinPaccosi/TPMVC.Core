using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Core.Services.Interfaces;
using MVC.Core.Services.Services;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.City;
using X.PagedList.Extensions;

namespace TPMVC.Core.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CitiesController : Controller
    {
        private readonly ICitiesService _services;  
        private readonly IStatesService _statesServices;
        private readonly IMapper? _mapper;
        private readonly ICountriesService? _countriesService;

        public CitiesController(ICitiesService services, IStatesService? statesServices, ICountriesService countriesService, IMapper mapper)
        {
            _services= services ?? throw new ArgumentNullException(nameof(services));
            _statesServices = statesServices ?? throw new ArgumentNullException(nameof(statesServices));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _countriesService = countriesService ?? throw new ArgumentException("Dependencies not set");
        }

        public IActionResult Index(int? page, string? searchTerm, bool viewAll = false, int pageSize = 10)
        {
            int pageNumber = page ?? 1;
            IEnumerable<City>? cities;
            ViewBag.currentPageSize = pageSize;
            if (string.IsNullOrEmpty(searchTerm) || viewAll)
            {
                cities = _services?
                    .GetAll(orderBy: q => q.OrderBy(c => c.CityName),
                    propertiesNames: "Country,States");

            } 
            else
            {
                cities = _services?
                    .GetAll(orderBy: q => q.OrderBy(c => c.CityName),
                        filter: c => c.Country.CountryName.Contains(searchTerm)
                        || c.States.StateName.Contains(searchTerm),
                    propertiesNames: "Country,States");
                ViewBag.currentSearchTerm = searchTerm;
            }

            var citiesVm = _mapper?.Map<List<CityListVm>>(cities);

            return View(citiesVm!.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult UpSert(int? id)
        {
            CityEditVm cityVm;
            if (id == null || id == 0)
            {
                cityVm = new CityEditVm();
                cityVm.Countries = _countriesService!
                    .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                    .Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    }).ToList();
                cityVm.States = _statesServices!
                    .GetAll()
                    .Select(s => new SelectListItem
                    {
                        Text = s.StateName,
                        Value = s.StateId.ToString()
                    }).ToList();
            }
            else
            {
                try
                {
                    City? city = _services!.Get(c => c.CityId == id.Value,
                        propertiesNames: "Country,State");
                    if (city == null)
                    {
                        return NotFound();
                    }
                    cityVm = _mapper!.Map<CityEditVm>(city);
                    cityVm.Countries = _countriesService!
                        .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                        .Select(c => new SelectListItem
                        {
                            Text = c.CountryName,
                            Value = c.CountryId.ToString()
                        }).ToList();
                    cityVm.States = _statesServices!
                        .GetAll(filter: s => s.CountryId == cityVm.CountryId)
                        .Select(s => new SelectListItem
                        {
                            Text = s.StateName,
                            Value = s.StateId.ToString()
                        }).ToList();

                    return View(cityVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            cityVm.Countries = _countriesService
                .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                .Select(c => new SelectListItem
                {
                    Text = c.CountryName,
                    Value = c.CountryId.ToString()
                }).ToList();
            cityVm.States = _statesServices
                .GetAll(filter: s => s.CountryId == cityVm.CountryId)
                .Select(s => new SelectListItem
                {
                    Text = s.StateName,
                    Value = s.StateId.ToString()
                }).ToList();

            return View(cityVm);

        }

        [HttpPost]
        public IActionResult UpSert(CityEditVm cityVm)
        {
            if (!ModelState.IsValid)
            {
                cityVm.Countries = _countriesService!
                    .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                    .Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    }).ToList();
                cityVm.States = _statesServices!
                    .GetAll(filter: s => s.CountryId == cityVm.CountryId)
                    .Select(s => new SelectListItem
                    {
                        Text = s.StateName,
                        Value = s.StateId.ToString()
                    }).ToList();

                return View(cityVm);
            }


            try
            {
                City City = _mapper!.Map<City>(cityVm);

                if (_services!.Existe(City))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    cityVm.Countries = _countriesService!
                        .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                        .Select(c => new SelectListItem
                        {
                            Text = c.CountryName,
                            Value = c.CountryId.ToString()
                        }).ToList();
                    cityVm.States = _statesServices!
                        .GetAll(filter: s => s.CountryId == cityVm.CountryId)
                        .Select(s => new SelectListItem
                        {
                            Text = s.StateName,
                            Value = s.StateId.ToString()
                        }).ToList();

                    return View(cityVm);
                }

                _services.Guardar(City);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                cityVm.Countries = _countriesService!
                    .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                    .Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    }).ToList();
                cityVm.States = _statesServices!
                    .GetAll(filter: s => s.CountryId == cityVm.CountryId)
                    .Select(s => new SelectListItem
                    {
                        Text = s.StateName,
                        Value = s.StateId.ToString()
                    }).ToList();

                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(cityVm);
            }
        }
        [HttpDelete]
        
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            City? city = _services?.Get(filter: c => c.CityId == id.Value);
            if (city is null)
            {
                return NotFound();
            }
            try
            {

                if (_services!.EstaRelacionado(city.CityId))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _services.Eliminar(city);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }
        }


        public JsonResult GetStates(int countryId)
        {
            var states = _statesServices?
                .GetAll(filter: s => s.CountryId == countryId,
                orderBy: q => q.OrderBy(c => c.StateName));
            return Json(states);
        }
    }
}
