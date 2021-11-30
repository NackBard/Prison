﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Dish : ICloneable
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? SetId { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
