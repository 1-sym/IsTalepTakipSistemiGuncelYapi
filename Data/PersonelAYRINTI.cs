using System.ComponentModel.DataAnnotations;

namespace IsTakipSistemi.Data
{
    public  class PersonelAYRINTI
    {
        [Key]
        public int personelID { get; set; }
        public string adi { get; set; }
        public string soyadi { get; set; }
        public string TcNo { get; set; }
        public string telefon { get; set; }
        public byte izinlimi { get; set; }
        public int i_birimID { get; set; }
        public DateTime girisTarihi { get; set; }
        public DateTime? ayrilisTarihi { get; set; }
        public string birimAdi { get; set; }
        public byte personelVarmi { get; set; }
        public byte birimVarmi { get; set; }
        public int i_kullaniciID { get; set; }
        public string kullaniciTuru { get; set; }
    }
}
