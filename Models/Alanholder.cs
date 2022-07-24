using System;
using System.Collections.Generic;

namespace Remake.Models
{
    public partial class Alanholder
    {
        public int Id { get; set; }
        public int? KesifId { get; set; }
        public int? MekanId { get; set; }
        public string? AlanAdi { get; set; }
        public string? Not { get; set; }
        public string? Konum { get; set; }
    }
}
