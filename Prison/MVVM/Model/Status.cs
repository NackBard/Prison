using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Status : ICloneable
    {
        public Status()
        {
            Prisoners = new HashSet<Prisoner>();
        }

        public int? Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Prisoner> Prisoners { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
