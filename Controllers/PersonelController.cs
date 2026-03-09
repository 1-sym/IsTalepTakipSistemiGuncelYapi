using IsTakipSistemi.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IsTakipSistemi.Controllers
{
    public class PersonelController : Controller
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

                Models.PersonelModel modeli = new Models.PersonelModel();
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
                   

                Models.PersonelModel modeli = new Models.PersonelModel();
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
                    var bulunan = vari.Personeller.FirstOrDefault(p => p.personelID == kimlik);
                    Personel silinecek = vari.Personeller.FirstOrDefault(q => q.personelID == kimlik);
                    silinecek.varmi = 0;
                    vari.Entry(bulunan).CurrentValues.SetValues(silinecek);
                    var bulunanKullanici = vari.Kullaniciler.FirstOrDefault(p => p.kullaniciID == silinecek.i_kullaniciID);
                    Kullanici silinecekKullanici = vari.Kullaniciler.FirstOrDefault(q => q.kullaniciID == silinecek.i_kullaniciID);
                    silinecekKullanici.varmi = 0;
                    vari.Entry(bulunanKullanici).CurrentValues.SetValues(silinecekKullanici);
                    vari.SaveChanges();
                }
                Models.PersonelModel modeli = new Models.PersonelModel();
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
        public ActionResult Kaydet(Models.PersonelModel gelen)
        {
            try
            {
                varlik vari = new varlik();
                Personel ekle = new Personel();

                ekle.adi = gelen.kartVerisi.adi;
                ekle.soyadi = gelen.kartVerisi.soyadi;
                ekle.TcNo = gelen.kartVerisi.TcNo;
                ekle.telefon = gelen.kartVerisi.telefon;
                ekle.i_birimID = gelen.kartVerisi.i_birimID;
                ekle.izinlimi = gelen.kartVerisi.izinlimi;
                ekle.girisTarihi = gelen.kartVerisi.girisTarihi;
                ekle.ayrilisTarihi = gelen.kartVerisi.ayrilisTarihi;
   
                ekle.varmi = 1;
                
                if (gelen.kartVerisi.personelID != 0)
                {
                    ekle.personelID = gelen.kartVerisi.personelID;
                    var bulunan = vari.Personeller.FirstOrDefault(p => p.personelID == gelen.kartVerisi.personelID);

                    vari.Entry(bulunan).CurrentValues.SetValues(ekle);
                    vari.SaveChanges();

                    
                    var bulunanKullanici = vari.KullaniciAYRINTIler.FirstOrDefault(p => p.TcNo == gelen.kartVerisi.TcNo);
                    ekle.i_kullaniciID = bulunanKullanici.kullaniciID;
                    vari.Entry(bulunan).CurrentValues.SetValues(ekle);
                    vari.SaveChanges();
                }
                else
                {
                    vari.Personeller.Add(ekle);
                    vari.SaveChanges();
                    Kullanici kullaniciEkle = new Kullanici();
                    kullaniciEkle.adi = gelen.kartVerisi.adi;
                    kullaniciEkle.soyadi = gelen.kartVerisi.soyadi;
                    kullaniciEkle.TcNo = gelen.kartVerisi.TcNo;
                    kullaniciEkle.telefon = gelen.kartVerisi.telefon;
                    kullaniciEkle.sifre = gelen.kartVerisi.TcNo.Substring(3);
                    kullaniciEkle.i_kullaniciTuruID = (int)enum_KullaniciTuru.Personel;
                    kullaniciEkle.varmi = 1;

                   vari.Kullaniciler.Add(kullaniciEkle);
                    vari.SaveChanges();
                    var bulunanPersonel = vari.Personeller.FirstOrDefault(p => p.TcNo == gelen.kartVerisi.TcNo);
                    var bulunanKullanici = vari.KullaniciAYRINTIler.FirstOrDefault(p => p.TcNo == gelen.kartVerisi.TcNo);
                    ekle.i_kullaniciID = bulunanKullanici.kullaniciID;
                    vari.Entry(bulunanPersonel).CurrentValues.SetValues(ekle);
                    vari.SaveChanges();
                }

                
                Models.PersonelModel modeli = new Models.PersonelModel();
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
