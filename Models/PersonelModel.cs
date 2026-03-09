using IsTakipSistemi.Data;
using System.Collections.Generic;
using System.Linq;

namespace IsTakipSistemi.Models
{
    public class PersonelModel
    {
        public Personel kartVerisi { get; set; }
        public List<PersonelAYRINTI> dokumVerisi { get; set; }
      
   
        public  void veriCek()
        { Data.varlik vari = new varlik();
            kartVerisi = new Personel();
            dokumVerisi = vari.PersonelAYRINTIler.Where(q=>q.personelVarmi==1 && q.birimVarmi==1).ToList();
        }
        public  void veriCek(int kimlik)
        { Data.varlik vari = new varlik();
            kartVerisi = vari.Personeller.FirstOrDefault (q => q.personelID == kimlik);
            dokumVerisi = new List<PersonelAYRINTI>();
        }
    }
}
