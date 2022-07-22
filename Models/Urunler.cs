using System;
using System.Collections.Generic;

namespace Remake.Models
{
    public partial class Urunler
    {
        public int Id { get; set; }
        public string? UrunKodu { get; set; }
        public string? UrunAdi { get; set; }
        public string? Marka { get; set; }
        public float? UrunFiyati { get; set; }
        public float? SatisFiyati { get; set; }
        public int? UrunAdet { get; set; }
        public string? UrunKategorisi { get; set; }
        public string? KesifAitligi { get; set; }
    }
}
