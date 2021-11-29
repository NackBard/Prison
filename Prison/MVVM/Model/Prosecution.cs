using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Prosecution : ICloneable
    {
        public Prosecution()
        {
            Prisoners = new HashSet<Prisoner>();
        }

        public int? Id { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }

        public virtual ICollection<Prisoner> Prisoners { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
