using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using MVC.Core.Services.Services;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.Country;
using X.PagedList.Extensions;

namespace TPMVC.Core.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

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
            var countries = _services.GetAll().ToPagedList();
            return View(countries);
        }
        public IActionResult UpSert(int id)
        {
            if (_services == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "NO FUNCIONAN LOS SERVICES. HDP");

            }
            CountryEditVM countryEditVM;
            if (id == 0 || id == null)
            {
                countryEditVM = new CountryEditVM();
            }
            else
            {
                try
                {
                    Country country = _services.Get(filter: c => c.CountryId == id);
                    if (country == null)
                    {
                        return NotFound();
                    }
                    countryEditVM = _mapper.Map<CountryEditVM>(country);
                    return View(countryEditVM);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(countryEditVM);

        }
        [HttpPost]
        public IActionResult UpSert(CountryEditVM countryEditVM)
        {
            if (!ModelState.IsValid)
            {
                return View(countryEditVM);
            }

            if (_services == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "NO FUNCIONAN LOS SERVICES. HDP");

            }
            try
            {
                //countryEditVM=_mapper.Map<CountryEditVM>(country);
                Country country = _mapper.Map<Country>(countryEditVM);
                if (_services.Existe(country))
                {
                    ModelState.AddModelError(String.Empty, "Pais Existe");
                    return View(countryEditVM);
                }
                _services.Guardar(country);
                TempData["success"] = "Pais Agregado/Editado puto EXCELENTEMENTE";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "No anda");
                return View(countryEditVM);
                throw;
            }

        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Country? country = _services?.Get(filter: c => c.CountryId == id);
            if (country is null)
            {
                return NotFound();
            }
            try
            {
                if (_services == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }

                //if (_services.EstaRelacionado(country.CountryId))
                //{
                //    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                //}
                _services.Eliminar(country);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }
        }
    }
}
