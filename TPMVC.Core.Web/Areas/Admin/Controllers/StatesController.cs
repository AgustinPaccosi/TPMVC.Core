using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Core.Services.Interfaces;
using MVC.Core.Services.Services;
using System.CodeDom;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.State;
using X.PagedList.Extensions;

namespace TPMVC.Core.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class StatesController : Controller
    {

        private readonly IStatesService _services;
        private readonly IMapper? _mapper;
        private readonly ICountriesService? _countriesService;

        public StatesController(IStatesService? services, ICountriesService countriesService, IMapper mapper)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _countriesService = countriesService ?? throw new ArgumentException("Dependencies not set"); ;
        }
        public IActionResult Index(int? page, int? filterId, int pageSize = 10, bool viewAll = false)
        {
            int pageNum = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            IEnumerable<State>? states;
            states = _services!
              .GetAll(orderBy: o => o.OrderBy(s => s.StateName),
              propertiesNames: "Country");
            //if (filterId is null || viewAll)
            //{
            //states = _service!
            //    .GetAll(orderBy: o => o.OrderBy(s => s.StateName),
            //    propertiesNames: "Country", filter: c => c.CountryId == 11);

            //}
            //else
            //{
            //    states = _service!
            //         .GetAll(orderBy: o => o.OrderBy(s => s.StateName),
            //                 filter: s => s.CountryId == filterId,
            //         propertiesNames: "Country");
            //    ViewBag.currentFilterCountryId = filterId;
            //}
            var statesVm = _mapper!
                .Map<List<StateListVM>>(states);
            var stateFilterVm = new StateFilterVM
            {
                States = statesVm.ToPagedList(pageNum, pageSize),
                Countries = _countriesService!
                    .GetAll(orderBy: o => o.OrderBy(c => c.CountryName))
                    .Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    })
                    .ToList()


            };
            return View(stateFilterVm);
        }
        public IActionResult UpSert(int ? id)
        {

            StateEditVM stateVm;
            if (id == null || id == 0)
            {
                stateVm = new StateEditVM();
                stateVm.Countries =
                    _countriesService!
                    .GetAll(orderBy: o => o.OrderBy(c => c.CountryName))
                    .Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    })
                    .ToList();
            }
            else
            {
                try
                {
                    State? state = _services!.Get(filter: c => c.StateId == id);
                    if (state == null)
                    {
                        return NotFound();
                    }
                    stateVm = _mapper!.Map<StateEditVM>(state);
                    stateVm.Countries =
                        _countriesService!
                        .GetAll(orderBy: o => o.OrderBy(c => c.CountryName))
                        .Select(c => new SelectListItem
                        {
                            Text = c.CountryName,
                            Value = c.CountryId.ToString()
                        })
                        .ToList();

                    return View(stateVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            return View(stateVm);
        }
        [HttpPost]
        public IActionResult UpSert(StateEditVM stateVm)
        {
            if (!ModelState.IsValid)
            {
                stateVm.Countries =
                    _countriesService!
                    .GetAll(orderBy: o => o.OrderBy(c => c.CountryName))
                    .Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    })
                    .ToList();

                return View(stateVm);
            }

            if (_services == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                State state = _mapper.Map<State>(stateVm);

                if (_services.Existe(state))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    stateVm.Countries =
                        _countriesService!
                        .GetAll(orderBy: o => o.OrderBy(c => c.CountryName))
                        .Select(c => new SelectListItem
                        {
                            Text = c.CountryName,
                            Value = c.CountryId.ToString()
                        })
                        .ToList();

                    return View(stateVm);
                }

                _services.Guardar(state);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                stateVm.Countries =
                        _countriesService!
                        .GetAll(orderBy: o => o.OrderBy(c => c.CountryName))
                        .Select(c => new SelectListItem
                        {
                            Text = c.CountryName,
                            Value = c.CountryId.ToString()
                        })
                        .ToList();

                return View(stateVm);
            }
        }
    }
}
