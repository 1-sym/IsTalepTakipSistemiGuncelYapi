using IsTakipSistemi.Data;
using System.Collections.Generic;
using System.Linq;

namespace IsTakipSistemi.Models
{
    public class KullaniciTuruModel
    {
        public KullaniciTuru kartVerisi { get; set; }
        public List<KullaniciTuruAYRINTI> dokumVerisi { get; set; }
      
   
        public  void veriCek()
        { Data.varlik vari = new varlik();
            kartVerisi = new KullaniciTuru();
            dokumVerisi = vari.KullaniciTuruAYRINTIler.Where(q=>q.varmi==1).ToList();
        }
        public  void veriCek(int kimlik)
        { Data.varlik vari = new varlik();
            kartVerisi = vari.KullaniciTuruler.FirstOrDefault (q => q.kullaniciTuruID == kimlik);
            dokumVerisi = new List<KullaniciTuruAYRINTI>();
        }
    }
}
