using System.ComponentModel.DataAnnotations;

namespace IsTakipSistemi.Data
{
    public class BirimAYRINTI
    {
        [Key]
        public int birimID { get; set; }
            public string birimAdi { get; set; }
            public byte varmi { get; set; }
           
        
    }
}
