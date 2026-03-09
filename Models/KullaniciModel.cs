using IsTakipSistemi.Data;
using System.Collections.Generic;
using System.Linq;

namespace IsTakipSistemi.Models
{
    public class KullaniciModel
    {
        public Kullanici kartVerisi { get; set; }
        public List<KullaniciAYRINTI> dokumVerisi { get; set; }
      
   
        public  void veriCek()
        { Data.varlik vari = new varlik();
            kartVerisi = new Kullanici();
            dokumVerisi = vari.KullaniciAYRINTIler.Where(q=>q.kullaniciVarmi==1 && q.turvarmi==1).ToList();
        }
        public  void veriCek(int kimlik)
        { Data.varlik vari = new varlik();
            kartVerisi = vari.Kullaniciler.FirstOrDefault (q => q.kullaniciID == kimlik);
            dokumVerisi = new List<KullaniciAYRINTI>();
        }
    }
}
