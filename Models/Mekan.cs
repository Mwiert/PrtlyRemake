using System;
using System.Collections.Generic;

namespace Remake.Models
{
    public partial class Mekan
    {
        public int Id { get; set; }
        public int? UstId { get; set; }
        public string? MekanTuru { get; set; }

        public virtual Kesifler? Ust { get; set; }
    }
}
