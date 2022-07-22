using System;
using System.Collections.Generic;

namespace Remake.Models
{
    public partial class Roller
    {
        public Roller()
        {
            Kullanıcıs = new HashSet<Kullanıcı>();
        }

        public int RolId { get; set; }
        public string? RolAdi { get; set; }

        public virtual ICollection<Kullanıcı> Kullanıcıs { get; set; }
    }
}
