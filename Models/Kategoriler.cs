using System;
using System.Collections.Generic;

namespace Remake.Models
{
    public partial class Kategoriler
    {
        public int Id { get; set; }
        public int? UstId { get; set; }
        public string? KategoriAdi { get; set; }
    }
}
