﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Data.Model
{
    [Table("ClinicRole")]
    [Index("Name", Name = "IX_ClinicRole", IsUnique = true)]
    public partial class ClinicRole
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
    }
}