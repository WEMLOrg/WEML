using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WEML.Areas.Identity.Data;
using WEML.Models;
namespace WEML.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

       
        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
