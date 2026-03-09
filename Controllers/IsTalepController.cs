using IsTakipSistemi.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IsTakipSistemi.Controllers
{
    public class IsTalepController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("mevcutKullanici") == null)
            {
                return RedirectToAction("Index", "Giris");
            }
            else
            {
                string kullanici = HttpContext.Session.GetString("mevcutKullanici");
                KullaniciAYRINTI kisi = JsonConvert.DeserializeObject<KullaniciAYRINTI>(kullanici);
                ViewBag.mevcut = kisi;
                if (kisi.i_kullaniciTuruID == (int)enum_KullaniciTuru.Yazilimci || kisi.i_kullaniciTuruID == (int)enum_KullaniciTuru.Yonetici)
                {

                   

                Models.IsTalepModel modeli = new Models.IsTalepModel();
                modeli.veriCek(kisi);
                return View(modeli);
                }
                else
                {
                    return RedirectToAction("Index", "AnaSayfa");

                }
            }
            
        }

        public ActionResult Kart(int id)
        {
            if (HttpContext.Session.GetString("mevcutKullanici") == null)
            {
                return RedirectToAction("Index", "Giris");
            }
            else
            {
                string kullanici = HttpContext.Session.GetString("mevcutKullanici");
                KullaniciAYRINTI kisi = JsonConvert.DeserializeObject<KullaniciAYRINTI>(kullanici);
                ViewBag.mevcut = kisi;
                if (kisi.i_kullaniciTuruID == (int)enum_KullaniciTuru.Yazilimci || kisi.i_kullaniciTuruID == (int)enum_KullaniciTuru.Yonetici)
                {



                    Models.IsTalepModel modeli = new Models.IsTalepModel();
                modeli.veriCek(id);
                return View(modeli);
                }
                else
                {
                    return RedirectToAction("Index", "AnaSayfa");

                }
            }
          
        }
        [HttpPost]
        public ActionResult Sil(string id)
        {
            try
            {
                string kullanici = HttpContext.Session.GetString("mevcutKullanici");
                KullaniciAYRINTI kisi = JsonConvert.DeserializeObject<KullaniciAYRINTI>(kullanici);
                varlik vari = new varlik();
                List<string> kayitlar = id.Split(',').ToList();
                for (int i = 0; i < kayitlar.Count; i++)
                {
                    Int32 kimlik = Convert.ToInt32(kayitlar[i]);
                    var bulunan = vari.IsTalebiler.FirstOrDefault(p => p.talepID == kimlik);
                    IsTalebi silinecek = vari.IsTalebiler.FirstOrDefault(q => q.talepID == kimlik);
                    silinecek.varmi = 0;
                    vari.Entry(bulunan).CurrentValues.SetValues(silinecek);
                    vari.SaveChanges();
                }
                Models.IsTalepModel modeli = new Models.IsTalepModel();
                modeli.veriCek(kisi);
                return Json(new
                {
                    success = true,
                    message = "İşlem Başarılı",
                    satirID = "0"
                });
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

        [HttpPost]
        public ActionResult Kaydet(Models.IsTalepModel gelen)
        {
            try
            {
                string kullanici = HttpContext.Session.GetString("mevcutKullanici");
                KullaniciAYRINTI kisi = JsonConvert.DeserializeObject<KullaniciAYRINTI>(kullanici);
                varlik vari = new varlik();
                IsTalebi ekle = new IsTalebi();

                ekle.talepTarihi = DateTime.Now;


                ekle.talepAyrinti = gelen.kartVerisi.talepAyrinti;
                ekle.talepTamamlandimi = gelen.kartVerisi.talepTamamlandimi;

                ekle.i_personelID = gelen.kartVerisi.i_personelID;
                ekle.i_talepEdenKullaniciID = 1;
                ekle.i_oncelikID = gelen.kartVerisi.i_oncelikID;
                ekle.varmi = 1;
                vari.IsTalebiler.Add(ekle);
                vari.SaveChanges();
                Models.IsTalepModel modeli = new Models.IsTalepModel();
                modeli.veriCek(kisi);
                return Json(new
                {
                    success = true,
                    message = "İşlem Başarılı",
                    satirID = "0"
                });

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
