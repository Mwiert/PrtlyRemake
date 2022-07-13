using System;
using System.Collections.Generic;

namespace Remake.Models
{
    public partial class Roller
    {
        public Roller()
        {
            Kullanıcıs = new HashSet<kullanıcı>();
        }

        public int RolId { get; set; }
        public string? RolAdi { get; set; }

        public virtual ICollection<kullanıcı> Kullanıcıs { get; set; }
    }
}
