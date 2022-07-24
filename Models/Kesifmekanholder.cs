using System;
using System.Collections.Generic;

namespace Remake.Models
{
    public partial class Kesifmekanholder
    {
        public int Id { get; set; }
        public int? KesifId { get; set; }
        public int? MekanId { get; set; }
    }
}
