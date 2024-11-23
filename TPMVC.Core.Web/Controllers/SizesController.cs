﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using System.Drawing.Printing;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.Size;
using X.PagedList.Extensions;

namespace TPMVC.Core.Web.Controllers
{
    public class SizesController : Controller
    {

        private readonly ISizesService _service;
        private readonly IMapper _mapper;
        public SizesController(ISizesService? service, IMapper? mapper)
        {
            this._service = service;
            _mapper = mapper;
        }
        public IActionResult Index(int? page, int pageSize = 10)
        {
            int pageNumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            var Sizes = _service?.GetAll
                (orderBy: o => o.OrderBy(c => c.SizeNumber));
            var SizesVm = _mapper?.Map<List<SizeListVm>>(Sizes);
            //.ToPagedList(pageNumber, pageSize);
            foreach (var size in SizesVm!)
            {
                size.CantidadZapatillas = (int)(_service?.ContarZapatillasPorTalle(size.SizeId))!;
            }
            return View(SizesVm.OrderByDescending(o => o.CantidadZapatillas).
                ToPagedList(pageNumber, pageSize));
        }

        public IActionResult UpSert(int? id)
        {
            if (_service == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }
            SizeEditVm SizeVm;
            if (id == null || id == 0)
            {
                SizeVm = new SizeEditVm();
            }
            else
            {
                try
                {
                    Size? size = _service.Get(filter: g => g.SizeId == id);
                    if (size == null)
                    {
                        return NotFound();
                    }
                    SizeVm = _mapper.Map<SizeEditVm>(size);
                    return View(SizeVm);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            return View(SizeVm);

        }



        [HttpPost]
        public IActionResult UpSert(SizeEditVm SizeVm)
        {
            if (!ModelState.IsValid)
            {
                return View(SizeVm);
            }

            if (_service == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Size size = _mapper.Map<Size>(SizeVm);

                if (_service.Existe(size))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    return View(SizeVm);
                }

                _service.Guardar(size);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(SizeVm);
            }
        }

        //TODO HACER RELACION

        [HttpGet]
        public IActionResult Details(int id)
        {
            var shoe = _service?.GetShoesForSize(id);
            if (shoe == null || shoe.Count == 0)
            {
                ViewData["Mensaje"] = "No hay zapatillas asociadas a este talle.";
            }
            return View(shoe);
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Size? size = _service?.Get(filter: g => g.SizeId == id);
            if (size is null)
            {
                return NotFound();
            }
            try
            {
                if (_service == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }
                if (_service.EstaRelacionado(size))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _service.Eliminar(size);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;
            }
        }

    }
}