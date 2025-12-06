using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SporMerkeziIsletmesi.Controllers
{
    // Bu controller'a sadece Admin rolündeki kullanıcılar erişebilsin
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
