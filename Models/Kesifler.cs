using System;
using System.Collections.Generic;

namespace Remake.Models
{
    public partial class Kesifler
    {
        public Kesifler()
        {
            Mekans = new HashSet<Mekan>();
        }

        public int Id { get; set; }
        public string? Ad { get; set; }

        public virtual ICollection<Mekan> Mekans { get; set; }
    }
}
