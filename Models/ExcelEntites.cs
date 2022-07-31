namespace Remake.Models
{
    public class ExcelEntites
    {
        public string KesifAdi { get; set; }
        public string HazirlayanAdi { get; set; }
        public decimal DolarKuru { get; set; }
        public List<Urunler> UrunListesi { get; set; }

    }
}
