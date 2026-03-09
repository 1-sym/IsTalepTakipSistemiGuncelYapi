using IsTakipSistemi.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace IsTakipSistemi.Controllers
{
    public class GirisController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GirisYap(Models.KullaniciModel gelen)
        {
            try
            {
                varlik vari = new varlik();
            KullaniciAYRINTI kontrol = vari.KullaniciAYRINTIler.FirstOrDefault(q => q.TcNo == gelen.kartVerisi.TcNo && q.sifre == gelen.kartVerisi.sifre);
            if(kontrol==null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Kullanıcı adı veya şifre hatalıdır.",
                        satirID = "0"
                    });
                }
                else
                {
                    string jsonser = JsonConvert.SerializeObject(kontrol);
                    HttpContext.Session.SetString("mevcutKullanici", jsonser);
                    HttpContext.Session.SetInt32("kullaniciKimlik", kontrol.kullaniciID);
                    
                    return Json(new
                    {
                        success = true,
                        message = "Sistem girişi onaylandı",
                        satirID = "0"
                    });
                }
                    

            }
            catch (Exception istisna)
            {
                return Json(new
                {
                    success = false,
                    message = istisna.Message,
                    satirID = "0"
                });
            }
        }
    }
}
