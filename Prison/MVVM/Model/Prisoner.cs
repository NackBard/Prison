using System;

#nullable disable

namespace Prison.Model
{
    public partial class Prisoner : ICloneable
    {
        public int? Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirthday { get; set; }
        public int? GenderId { get; set; }
        public DateTime DateOfConclusion { get; set; }
        public int? ProsecutionId { get; set; }
        public string Verdict { get; set; }
        public int? StatusId { get; set; }
        public string AdditionalInformation { get; set; }
        public bool IsDeleted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
