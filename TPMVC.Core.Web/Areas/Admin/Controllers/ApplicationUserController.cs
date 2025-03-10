﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.ApplicationUser;
using X.PagedList.Extensions;

namespace TPMVC.Core.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ApplicationUserController : Controller
    {
        private readonly IApplicationUserService? _usersService;
        private readonly IMapper? _mapper;

        public ApplicationUserController(IApplicationUserService? usersService,
            IMapper mapper)
        {
            _usersService = usersService ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set"); ;
        }
        public IActionResult Index(int? page, string? searchTerm = null, bool viewAll = false, int pageSize = 10)
        {
            int pageNumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            IEnumerable<ApplicationUser>? users;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    users = _usersService?
                        .GetAll(orderBy: o => o.OrderBy(c => c.LastName).ThenBy(c => c.FirstName),
                            filter: c => c.FirstName.Contains(searchTerm)
                            || c.LastName!.Contains(searchTerm),
                            propertiesNames: "Country,State,City");
                    ViewBag.currentSearchTerm = searchTerm;
                }
                else
                {
                    users = _usersService?
                            .GetAll(orderBy: o => o.OrderBy(c => c.LastName).ThenBy(c => c.FirstName),
                        propertiesNames: "Country,State,City");
                }
            }
            else
            {
                users = _usersService?
                        .GetAll(orderBy: o => o.OrderBy(c => c.LastName).ThenBy(c => c.FirstName),
                        propertiesNames: "Country,State,City");
            }

            var usersVm = _mapper?.Map<List<ApplicationUserListVm>>(users)
                    .ToPagedList(pageNumber, pageSize);
            return View(usersVm);
        }
        public IActionResult Details(string id)
        {
            var applicationUser = _usersService!.Get(filter: o => o.Id == id,
                propertiesNames: "Country,State,City,OrderHeaders");
            return View(applicationUser);
        }
    }
}
