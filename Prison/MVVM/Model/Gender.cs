using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Gender
    {
        public Gender()
        {
            Prisoners = new HashSet<Prisoner>();
            Workers = new HashSet<Worker>();
        }

        public int? Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Prisoner> Prisoners { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
