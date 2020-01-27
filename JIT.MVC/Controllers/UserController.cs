using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JIT.Business.Interfaces;
using JIT.Core.DTOs;
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

        public UserController(IMapper mapper, IJitService jitService)
        {
            _mapper = mapper;
            _jitService = jitService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> LoginUser(UserViewModel user)
        {
            var loggedUser = await _jitService.Login(_mapper.Map<UserViewModel, UserDto>(user));
            return loggedUser == null ? RedirectToAction("Register") : RedirectToAction("Authenticate", "Home", new { username = user.Username});
        }

        public IActionResult Register()
        {
            return View();
        }

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

        public IActionResult UserHours(int id)
        {
            return View(id);
        }
    }
}