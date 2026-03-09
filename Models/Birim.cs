using IsTakipSistemi.Data;
using System.Collections.Generic;
using System.Linq;

namespace IsTakipSistemi.Models
{
    public class BirimModel
    {
        public Birim kartVerisi { get; set; }
        public List<BirimAYRINTI> dokumVerisi { get; set; }
      
   
        public  void veriCek()
        { Data.varlik vari = new varlik();
            kartVerisi = new Birim();
            dokumVerisi = vari.BirimAYRINTIler.Where(q=>q.varmi==1).ToList();
        }
        public  void veriCek(int kimlik)
        { Data.varlik vari = new varlik();
            kartVerisi = vari.Birimler.FirstOrDefault (q => q.birimID == kimlik);
            dokumVerisi = new List<BirimAYRINTI>();
        }
    }
}
