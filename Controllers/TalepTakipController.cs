using IsTakipSistemi.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IsTakipSistemi.Controllers
{
    public class TalepTakipController : Controller
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

                Models.TalepTakipModel modeli = new Models.TalepTakipModel();
                modeli.veriCek(kisi);
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

                Models.TalepTakipModel modeli = new Models.TalepTakipModel();
                modeli.veriCek(id);
                return View(modeli);
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
                    var bulunan = vari.TalepTakipler.FirstOrDefault(p => p.talepTakipID == kimlik);
                    TalepTakip silinecek = vari.TalepTakipler.FirstOrDefault(q => q.talepTakipID == kimlik);
                    silinecek.varmi = 0;
                    vari.Entry(bulunan).CurrentValues.SetValues(silinecek);
                    vari.SaveChanges();
                }
                Models.TalepTakipModel modeli = new Models.TalepTakipModel();
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
        public ActionResult Kaydet(Models.TalepTakipModel gelen)
        {
            try
            {
                string kullanici = HttpContext.Session.GetString("mevcutKullanici");
                KullaniciAYRINTI kisi = JsonConvert.DeserializeObject<KullaniciAYRINTI>(kullanici);
                varlik vari = new varlik();
                TalepTakip ekle = new TalepTakip();


                ekle.i_talepID = gelen.kartVerisi.i_talepID;
                ekle.tarih = DateTime.Now ;
                ekle.talepDurumBilgi = gelen.kartVerisi.talepDurumBilgi;
                ekle.tamamlanmaOrani = gelen.kartVerisi.tamamlanmaOrani;
                
                ekle.varmi = 1;
                
                if (gelen.kartVerisi.talepTakipID != 0)
                {
                    ekle.talepTakipID = gelen.kartVerisi.talepTakipID;
                    var bulunan = vari.TalepTakipler.FirstOrDefault(p => p.talepTakipID == gelen.kartVerisi.talepTakipID);

                    vari.Entry(bulunan).CurrentValues.SetValues(ekle);
                }
                else
                {
                    vari.TalepTakipler.Add(ekle);
                }

                vari.SaveChanges();

                if(ekle.tamamlanmaOrani==100)
                {
                  
                    var guncellecekTalep = vari.IsTalebiler.FirstOrDefault(p => p.talepID == gelen.kartVerisi.i_talepID);
                    guncellecekTalep.talepTamamlandimi = 1;
                    var bulunanISTalebi = vari.IsTalebiler.FirstOrDefault(p => p.talepID == gelen.kartVerisi.i_talepID);
                    vari.Entry(bulunanISTalebi).CurrentValues.SetValues(guncellecekTalep);

                    vari.SaveChanges();
                }
                Models.TalepTakipModel modeli = new Models.TalepTakipModel();
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
