using IsTakipSistemi.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IsTakipSistemi.Controllers
{
    public class BirimController : Controller
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

                Models.BirimModel modeli = new Models.BirimModel();
                modeli.veriCek();
                return View(modeli);
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

                   

                Models.BirimModel modeli = new Models.BirimModel();
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
                    var bulunan = vari.Birimler.FirstOrDefault(p => p.birimID == kimlik);
                    Birim silinecek = vari.Birimler.FirstOrDefault(q => q.birimID == kimlik);
                    silinecek.varmi = 0;
                    vari.Entry(bulunan).CurrentValues.SetValues(silinecek);
                    vari.SaveChanges();
                }
                Models.BirimModel modeli = new Models.BirimModel();
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
        public ActionResult Kaydet(Models.BirimModel gelen)
        {
            try
            {
                varlik vari = new varlik();
                Birim ekle = new Birim();

                ekle.birimAdi = gelen.kartVerisi.birimAdi;
               
                ekle.varmi = 1;
                
                if (gelen.kartVerisi.birimID != 0)
                {
                    ekle.birimID = gelen.kartVerisi.birimID;
                    var bulunan = vari.Birimler.FirstOrDefault(p => p.birimID == gelen.kartVerisi.birimID);

                    vari.Entry(bulunan).CurrentValues.SetValues(ekle);
                }
                else
                {
                    vari.Birimler.Add(ekle);
                }

                vari.SaveChanges();
                Models.BirimModel modeli = new Models.BirimModel();
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
