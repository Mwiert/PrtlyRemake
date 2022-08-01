using System;
using System.Collections.Generic;

namespace Remake.Models
{
    public partial class Paketholder
    {
        public int Id { get; set; }
        public int? UrunId { get; set; }
        public int? PaketId { get; set; }
        public int? UrunAdeti { get; set; }
    }
}
