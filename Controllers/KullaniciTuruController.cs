using IsTakipSistemi.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IsTakipSistemi.Controllers
{
    public class KullaniciTuruController : Controller
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
                if (kisi.i_kullaniciTuruID == (int)enum_KullaniciTuru.Yazilimci || kisi.i_kullaniciTuruID == (int)enum_KullaniciTuru.Yonetici)
                {

                    Models.KullaniciTuruModel modeli = new Models.KullaniciTuruModel();
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

                    Models.KullaniciTuruModel modeli = new Models.KullaniciTuruModel();
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
                    var bulunan = vari.KullaniciTuruler.FirstOrDefault(p => p.kullaniciTuruID == kimlik);
                    KullaniciTuru silinecek = vari.KullaniciTuruler.FirstOrDefault(q => q.kullaniciTuruID == kimlik);
                    silinecek.varmi = 0;
                    vari.Entry(bulunan).CurrentValues.SetValues(silinecek);
                    vari.SaveChanges();
                }
                Models.KullaniciTuruModel modeli = new Models.KullaniciTuruModel();
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
        public ActionResult Kaydet(Models.KullaniciTuruModel gelen)
        {
            try
            {
                varlik vari = new varlik();
                KullaniciTuru ekle = new KullaniciTuru();

                ekle.kullaniciTuru = gelen.kartVerisi.kullaniciTuru;
            
                ekle.varmi = 1;
                
                if (gelen.kartVerisi.kullaniciTuruID != 0)
                {
                    ekle.kullaniciTuruID = gelen.kartVerisi.kullaniciTuruID;
                    var bulunan = vari.KullaniciTuruler.FirstOrDefault(p => p.kullaniciTuruID == gelen.kartVerisi.kullaniciTuruID);

                    vari.Entry(bulunan).CurrentValues.SetValues(ekle);
                }
                else
                {
                    vari.KullaniciTuruler.Add(ekle);
                }

                vari.SaveChanges();
                Models.KullaniciTuruModel modeli = new Models.KullaniciTuruModel();
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
