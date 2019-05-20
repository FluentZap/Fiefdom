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

    [HttpGet("/New")]
    public IActionResult New()
    {
      return View();
    }

    [HttpPost("/Login")]
    public IActionResult Login()
    {
      return View();
    }
  }

}
