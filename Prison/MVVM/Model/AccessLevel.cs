using System;

#nullable disable

namespace Prison.Model
{
    public partial class AccessLevel : ICloneable
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
