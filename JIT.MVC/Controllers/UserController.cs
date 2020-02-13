﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using JIT.Business.Interfaces;
using JIT.Core.DTOs;
using JIT.MVC.Helpers;
using JIT.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JIT.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IJitService _jitService;
        private readonly AuthenticateUser _authenticateUser;
        private IConverter _converter;

        public UserController(IMapper mapper, IJitService jitService, AuthenticateUser authenticateUser, IConverter converter)
        {
            _mapper = mapper;
            _jitService = jitService;
            _authenticateUser = authenticateUser;
            _converter = converter;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(UserViewModel user)
        {
            var loggedUser = await _jitService.Login(_mapper.Map<UserViewModel, UserDto>(user));
            _authenticateUser.LoggedUserId = loggedUser.Id;

            return loggedUser == null ? RedirectToAction("Register") : RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNew(UserViewModel user)
        {
            if (!ModelState.IsValid) return View();
            user.Username = user.Username.ToLower();

            if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "Password & confirm password must be identical");
                return View("Register");
            }

            if (await _jitService.UserExists(_mapper.Map<UserViewModel, UserDto>(user)))
            {
                ModelState.AddModelError("Username", "This username is already taken");
                return View("Register");
            }

            await _jitService.Register(_mapper.Map<UserViewModel, UserDto>(user));
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Hours()
        {
            if (_authenticateUser.LoggedUserId == 0)
                return RedirectToAction("Index", "NotFound");
            ViewData["UserId"] = _authenticateUser.LoggedUserId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveHours(ProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "NotFound");
            }
            await _jitService.SaveNewProject(_mapper.Map<ProjectViewModel, ProjectDto>(model));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (_authenticateUser.LoggedUserId == 0)
                return RedirectToAction("NotFound", "Home");
            var projectsFromDb = await _jitService.GetAllProjectsByUserId(_authenticateUser.LoggedUserId);

            return View(_mapper.Map<ICollection<ProjectDto>, ICollection<ProjectViewModel>>(projectsFromDb));
        }

        public async Task<IActionResult> CreatePDF(ExportDatesViewModel model)
        {
            var getAllProjectsByUser = await _jitService.GetAllProjectsBetweenDates(_authenticateUser.LoggedUserId, model.StartDate, model.EndDate);

            var projectsForPDFCreation = _mapper.Map<ICollection<ProjectDto>, ICollection<ProjectViewModel>>(getAllProjectsByUser);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
            };

            //if we want to save it on our hard disk
            //Out = @"E:\Employee_Report.pdf"


            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(projectsForPDFCreation),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "pdf-generator.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            try
            {
                return File(file, "application/pdf");
            }
            catch (Exception)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

    }
}