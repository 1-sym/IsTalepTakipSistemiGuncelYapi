using System.ComponentModel.DataAnnotations;

namespace IsTakipSistemi.Data
{
    public  class KullaniciTuruAYRINTI
    {
        [Key]
        public int kullaniciTuruID { get; set; } 
            public string kullaniciTuru { get; set; }
        public byte varmi { get; set; }

    }
}
