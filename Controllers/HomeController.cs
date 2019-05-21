using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fiefdom.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("/game")]
		public IActionResult Game(string userName)
		{
			ViewBag.UserName = userName;
			return View();
		}

		[HttpPost("/login")]
		public IActionResult Login(string userName)
		{
			return RedirectToAction("game", new { userName });
		}
	}
}
