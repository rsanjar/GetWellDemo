﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Data.Model
{
    [Index("Phone", Name = "IX_ClinicPhones", IsUnique = true)]
    public partial class ClinicPhone
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string Phone { get; set; }
        public bool IsMain { get; set; }
        [Required]
        public bool? IsMobile { get; set; }
        public bool IsDisabled { get; set; }
        public int SortOrder { get; set; }
        public int ClinicID { get; set; }

        [ForeignKey("ClinicID")]
        [InverseProperty("ClinicPhones")]
        public virtual Clinic Clinic { get; set; }
    }
}