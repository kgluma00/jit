using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using JIT.Business.Interfaces;
using JIT.Core.DTOs;
using JIT.MVC.Helpers;
using JIT.MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JIT.MVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IJitService _jitService;
        private IConverter _converter;
        private readonly IOptions<EmailOptions> _emailOptions;

        public UserController(IMapper mapper, IJitService jitService, IConverter converter, IOptions<EmailOptions> emailOptions)
        {
            _mapper = mapper;
            _jitService = jitService;
            _converter = converter;
            _emailOptions = emailOptions;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginUser(UserViewModel user)
        {
            var loggedUser = await _jitService.Login(_mapper.Map<UserViewModel, UserDto>(user));

            if (loggedUser == null)
            {
                //doraditi ovo
                ModelState.AddModelError("UserNotExits", "That user does not exist");
                return View("Login");
            }

            if (!loggedUser.isAuthenticated)
            {
                //doraditi ovo
                ModelState.AddModelError("Authenticate", "That user is not authenticated");
                return View("Authenticate", _mapper.Map<UserDto, UserViewModel>(loggedUser));
            }

            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, loggedUser.Username),
                new Claim(ClaimTypes.Name, loggedUser.Id.ToString()),
                new Claim("Loggin_Auth", "Ne dirajte mi ravnicu")
            };

            var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
            await HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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

            var registerUser = await _jitService.Register(_mapper.Map<UserViewModel, UserDto>(user), _emailOptions.Value.SendGridApiKey);

            return View("Authenticate", _mapper.Map<UserDto, UserViewModel>(registerUser));
        }

        [HttpGet]
        public IActionResult Hours()
        {
            //CLAIM
            var user = this.User.Claims.ToList();
            ViewData["UserId"] = user[1].Value;

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
            var user = this.User.Claims.ToList();
            var projectsFromDb = await _jitService.GetAllProjectsByUserId(Convert.ToInt32(user[1].Value));

            return View(_mapper.Map<ICollection<ProjectDto>, ICollection<ProjectViewModel>>(projectsFromDb));
        }

        [HttpGet]
        public IActionResult Logout()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<bool> DeleteProject(int id)
        {
            var removedProject = await _jitService.DeleteProject(id);

            return removedProject;
        }
        public async Task<IActionResult> CreatePDF(ExportDatesViewModel model)
        {
            var user = this.User.Claims.ToList();
            var getAllProjectsByUser = await _jitService.GetAllProjectsBetweenDates(Convert.ToInt32(user[1].Value), model.StartDate, model.EndDate);
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