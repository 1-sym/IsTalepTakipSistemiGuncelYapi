using IsTakipSistemi.Data;
using System.Collections.Generic;
using System.Linq;

namespace IsTakipSistemi.Models
{
    public class TalepTakipModel
    {
        public TalepTakip kartVerisi { get; set; }
        public List<TalepTakipAYRINTI> dokumVerisi { get; set; }


        public void veriCek(KullaniciAYRINTI kisi)
        { Data.varlik vari = new varlik();
            PersonelAYRINTI personel = vari.PersonelAYRINTIler.FirstOrDefault(q => q.i_kullaniciID == kisi.kullaniciID);
            kartVerisi = new TalepTakip();
            if (kisi.i_kullaniciTuruID == (int)enum_KullaniciTuru.Yazilimci || kisi.i_kullaniciTuruID == (int)enum_KullaniciTuru.Yonetici)
            {

                dokumVerisi = vari.TalepTakipAYRINTIler.ToList();
            }
            else {
                dokumVerisi = vari.TalepTakipAYRINTIler.Where(q => q.i_personelID == personel.personelID && q.talepTakipVarmi == 1).ToList();
            } }
        public  void veriCek(int kimlik)
        { Data.varlik vari = new varlik();
            kartVerisi = vari.TalepTakipler.FirstOrDefault (q => q.talepTakipID == kimlik);
            dokumVerisi = new List<TalepTakipAYRINTI>();
        }
    }
}
