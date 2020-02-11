using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public UserController(IMapper mapper, IJitService jitService, AuthenticateUser authenticateUser)
        {
            _mapper = mapper;
            _jitService = jitService;
            _authenticateUser = authenticateUser;
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
            //ovo treba doraditi
            if (_authenticateUser.LoggedUserId <= 0 || _authenticateUser.LoggedUserId > 10)
                return RedirectToAction("Index", "Home");
            ViewData["UserId"] = _authenticateUser.LoggedUserId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveHours(ProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            await _jitService.SaveNewProject(_mapper.Map<ProjectViewModel, ProjectDto>(model));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (_authenticateUser.LoggedUserId == 0)
                return RedirectToAction("Index", "Home");
            var projectsFromDb = await _jitService.GetAllProjectsByUserId(_authenticateUser.LoggedUserId);

            return View(_mapper.Map<ICollection<ProjectDto>, ICollection<ProjectViewModel>>(projectsFromDb));
        }
    }
}