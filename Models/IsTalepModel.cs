using IsTakipSistemi.Data;
using System.Collections.Generic;
using System.Linq;

namespace IsTakipSistemi.Models
{
    public class IsTalepModel
    {
        public IsTalebi kartVerisi { get; set; }
        public List<IsTalebiAYRINTI> dokumVerisi { get; set; }
      
   
        public  void veriCek(KullaniciAYRINTI kisi)
        { Data.varlik vari = new varlik();
            kartVerisi = new IsTalebi();
            if (kisi.i_kullaniciTuruID == (int)enum_KullaniciTuru.Yazilimci || kisi.i_kullaniciTuruID == (int)enum_KullaniciTuru.Yonetici)
            {

                dokumVerisi = vari.IsTalebiAYRINTIler.ToList();
            }
            else
            {
                PersonelAYRINTI personeli = vari.PersonelAYRINTIler.FirstOrDefault(q => q.i_kullaniciID == kisi.kullaniciID);
                dokumVerisi = vari.IsTalebiAYRINTIler.Where(q=>q.i_personelID== personeli.personelID).ToList();
            }
        }
        public  void veriCek(int kimlik)
        { Data.varlik vari = new varlik();
            kartVerisi = vari.IsTalebiler.FirstOrDefault (q => q.talepID == kimlik);
            dokumVerisi = new List<IsTalebiAYRINTI>();
        }
    }
}
