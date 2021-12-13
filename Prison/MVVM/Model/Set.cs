﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Set : ICloneable
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
