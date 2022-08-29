using System;
using System.Collections.Generic;

namespace Remake.Models
{
    public partial class Kullanıcı
    {
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Email { get; set; }
        public string? Psswrd { get; set; }
        public int? RolId { get; set; }

        public virtual Roller? Rol { get; set; }
    }
}
