using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Exam.Models;
using Exam.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
        }
        private User GetUserInDb()
        {
            return _context.Users.FirstOrDefault( u => u.UserId == HttpContext.Session.GetInt32("UserId"));
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(UserForm reg)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(u => u.Email == reg.Register.Email))
                {
                    ModelState.AddModelError("Register.Email","Email already taken");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    reg.Register.Password = Hasher.HashPassword(reg.Register, reg.Register.Password);
                    _context.Users.Add(reg.Register);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", reg.Register.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost("login")]
            public IActionResult Login(UserForm log)
            {
                if(ModelState.IsValid)
                {
                    User userInDb = _context.Users.FirstOrDefault(u => u.Email == log.Login.LoginEmail);
                    if(userInDb != null)
                    {
                        var hash = new PasswordHasher<LoginUser>();
                        var result = hash.VerifyHashedPassword(log.Login, userInDb.Password, log.Login.LoginPassword);
                        if(result == 0)
                        {
                        ModelState.AddModelError("Login.LoginEmail","Invalid Email/Password");
                        ModelState.AddModelError("Login.LoginPassword","Invalid Email/Password");
                        return View("Index");
                        }
                        else
                        {
                            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                            return RedirectToAction("Dashboard");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Login.LoginEmail","Invalid Email/Password");
                        ModelState.AddModelError("Login.LoginPassword","Invalid Email/Password");
                        return View("Index");
                    }
                }
                else
                {
                    return View("Index");
                }
            }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User userInDb = GetUserInDb();
            if(userInDb != null)
            {
                ViewBag.User = userInDb;
                List<Event> AllEvents = _context.Events
                                                    .Include(m => m.Organizer)
                                                    .Include(m => m.Participants)
                                                    .ThenInclude(p => p.participant)
                                                    .Where(m => m.Time >= DateTime.Now)
                                                    .OrderBy(m => m.Date)
                                                    .ToList();
                return View(AllEvents);
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }
        [HttpGet("create")]
        public IActionResult Create()
        {
            User userInDb = GetUserInDb();
            if(userInDb != null)
            {
                ViewBag.User = userInDb;
                return View();
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }
        [HttpPost("create/event")]
        public IActionResult CreateEvent(Event newEvent)
        {
            User userInDb = GetUserInDb();
            if(userInDb != null)
            {
                if(ModelState.IsValid)
                {
                    newEvent.UserId = userInDb.UserId;
                    _context.Events.Add(newEvent);
                    _context.SaveChanges();
                    return RedirectToAction("View", newEvent);
                }
                else
                {
                    return View("Create");
                }
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }
        [HttpGet("leave/{eventId}")]   
        public IActionResult Leave(int eventId)
        {
            User userInDb = GetUserInDb();
            if(userInDb != null)
            {
                People leaving = _context.Peoples.FirstOrDefault(p => p.UserId == userInDb.UserId && p.EventId == eventId);
                _context.Peoples.Remove(leaving);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }
        [HttpGet("join/{eventId}")]
        public IActionResult Join(int eventId)
        {
            User userInDb = GetUserInDb();
            if(userInDb != null)
            {
                People joining = new People();
                joining.UserId = userInDb.UserId;
                joining.EventId = eventId;
                _context.Peoples.Add(joining);
                _context.SaveChanges();
                ViewBag.User = userInDb;
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Logout");
            }   
        }
        [HttpGet("cancel/{eventId}")]
        public IActionResult Cancel(int eventId)
        {
            User userInDb = GetUserInDb();
            if(userInDb != null)
            {
                Event cancelling = _context.Events.FirstOrDefault(e => e.EventId == eventId);
                _context.Events.Remove(cancelling);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }
        [HttpGet("view/{eventId}")]
        public IActionResult View(int eventId)
        {
            User userInDb = GetUserInDb();
            if(userInDb != null)
            {
                ViewBag.User = userInDb;
                Event view = _context.Events
                                    .Include(e => e.Organizer)
                                    .Include(e => e.Participants)
                                    .ThenInclude(p => p.participant)
                                    .FirstOrDefault(e => e.EventId == eventId);
                return View(view);
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }
    }
}
