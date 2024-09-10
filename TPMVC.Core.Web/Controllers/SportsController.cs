using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.Sport;
using X.PagedList.Extensions;

namespace TPMVC.Core.Web.Controllers
{
    public class SportsController : Controller
    {
        private readonly ISportsService? _services;

        private readonly IMapper? _mapper;

        public SportsController(ISportsService? services, IMapper mapper)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IActionResult Index(int? page)
        {
            int pageNum = page ?? 1;
            int pageSize = 8;
            var colours = _services.GetAll().ToPagedList(pageNum, pageSize);
            return View(colours);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SportEditViewModel sportVM)
        {
            if (!ModelState.IsValid)
            {
                return View(sportVM);
            }
            Sport sport = _mapper.Map<Sport>(sportVM);
            if (sport is null)
            {
                ModelState.AddModelError(string.Empty, "Deporte es nulo");
                return View(sportVM);
            }
            if (_services.Existe(sport))
            {
                ModelState.AddModelError(string.Empty, "El registro ya existe");
                return View(sportVM);
            }
            _services.Guardar(sport);
            TempData["success"] = "Registro agregado satisfactoriamente";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Sport sport = _services.Get(filter: c => c.SportId == id);
            if (sport is null)
            {
                return NotFound();
            }
            SportEditViewModel sportVM = _mapper.Map<SportEditViewModel>(sport);
            return View(sportVM);
        }

        [HttpPost]
        public IActionResult Edit(SportEditViewModel sportVM)
        {
            if (!ModelState.IsValid)
            {
                return View(sportVM);
            }
            Sport sport = _mapper.Map<Sport>(sportVM);
            if (sport is null)
            {
                ModelState.AddModelError(string.Empty, "Deporte es nula");
                return View(sportVM);
            }
            if (_services.Existe(sport))
            {
                ModelState.AddModelError(string.Empty, "El registro ya existe");
                return View(sportVM);
            }
            _services.Guardar(sport);
            TempData["success"] = "Registro editado satisfactoriamente";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Sport sport = _services.Get(filter: b => b.SportId == id);
            if (sport is null)
            {
                return NotFound();
            }
            return View(sport);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Sport sport = _services.Get(filter: b => b.SportId == id);
            if (sport is null)
            {
                return NotFound();
            }
            if (_services.EstaRelacionado(sport.SportId))
            {
                ModelState.AddModelError(string.Empty, "El registro no puede borrarse, está relacionado");
                return View(sport);
            }
            _services.Eliminar(sport);
            TempData["success"] = "Registro eliminado correctamente";
            return RedirectToAction("Index");
        }


    }
}
