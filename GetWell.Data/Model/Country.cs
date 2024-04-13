﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Data.Model
{
    [Table("Country")]
    [Index("Name", Name = "IX_Country", IsUnique = true)]
    public partial class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }

        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string NameUz { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string NameEn { get; set; }

        [InverseProperty("Country")]
        public virtual ICollection<City> Cities { get; set; }
    }
}