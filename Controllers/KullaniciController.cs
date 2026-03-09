using IsTakipSistemi.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;

namespace IsTakipSistemi.Controllers
{
    public class KullaniciController : Controller
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
                if (kisi.i_kullaniciTuruID==(int)enum_KullaniciTuru.Yazilimci || kisi.i_kullaniciTuruID==(int)enum_KullaniciTuru.Yonetici) {
                    Models.KullaniciModel modeli = new Models.KullaniciModel();
                    modeli.veriCek();
                    ViewBag.mevcut = kisi;
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

                    Models.KullaniciModel modeli = new Models.KullaniciModel();
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
                varlik vari = new varlik();
                List<string> kayitlar = id.Split(',').ToList();
                for (int i = 0; i < kayitlar.Count; i++)
                {
                    Int32 kimlik = Convert.ToInt32(kayitlar[i]);
                    var bulunan = vari.Kullaniciler.FirstOrDefault(p => p.kullaniciID == kimlik);
                    Kullanici silinecek = vari.Kullaniciler.FirstOrDefault(q => q.kullaniciID == kimlik);
                    silinecek.varmi = 0;
                    vari.Entry(bulunan).CurrentValues.SetValues(silinecek);
                    vari.SaveChanges();
                }
                Models.KullaniciModel modeli = new Models.KullaniciModel();
                modeli.veriCek();
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
        public ActionResult Kaydet(Models.KullaniciModel gelen)
        {
            try
            {
                varlik vari = new varlik();
                Kullanici ekle = new Kullanici();
                ekle.adi = gelen.kartVerisi.adi;
                ekle.soyadi = gelen.kartVerisi.soyadi;
                ekle.TcNo = gelen.kartVerisi.TcNo;
                ekle.telefon = gelen.kartVerisi.telefon;
                ekle.sifre = gelen.kartVerisi.sifre;
                ekle.i_kullaniciTuruID = gelen.kartVerisi.i_kullaniciTuruID;
                ekle.varmi = 1;
                
                if (gelen.kartVerisi.kullaniciID != 0)
                {
                    ekle.kullaniciID = gelen.kartVerisi.kullaniciID;
                    var bulunan = vari.Kullaniciler.FirstOrDefault(p => p.kullaniciID == gelen.kartVerisi.kullaniciID);

                    vari.Entry(bulunan).CurrentValues.SetValues(ekle);
                }
                else
                {
                    vari.Kullaniciler.Add(ekle);
                }

                vari.SaveChanges();
                Models.KullaniciModel modeli = new Models.KullaniciModel();
                modeli.veriCek();
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
