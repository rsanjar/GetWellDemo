using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class Specialty
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public string NameUz { get; set; }
        public string NameEn { get; set; }
    }
}
