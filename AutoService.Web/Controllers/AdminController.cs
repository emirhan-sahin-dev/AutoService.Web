using AutoService.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Web.Controllers;

[Authorize(Roles = AppRoles.Admin)]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}