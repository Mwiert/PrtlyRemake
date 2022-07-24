using System;
using System.Collections.Generic;

namespace Remake.Models
{
    public partial class Urunholder
    {
        public int Id { get; set; }
        public int? AlanId { get; set; }
        public int? UrunId { get; set; }
        public int? UrunAdet { get; set; }
    }
}
