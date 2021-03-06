using System;

#nullable disable

namespace Prison.Model
{
    public partial class Worker : ICloneable
    {
        public int? Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? GenderId { get; set; }
        public int? PostId { get; set; }
        public string AdditionalInformation { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
