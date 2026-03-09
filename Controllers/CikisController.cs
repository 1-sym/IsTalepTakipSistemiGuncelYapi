using Microsoft.AspNetCore.Mvc;

namespace IsTakipSistemi.Controllers
{
    public class CikisController : Controller
    {
        public ActionResult Index()
        {
            HttpContext.Session.Clear();
            


            return RedirectToAction("Index", "Giris");
             
        }
    }
}
