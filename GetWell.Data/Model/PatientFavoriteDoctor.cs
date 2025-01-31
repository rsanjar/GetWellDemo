﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Data.Model
{
    [Index("PatientID", "ClinicDoctorID", Name = "IX_PatientFavoriteDoctor", IsUnique = true)]
    public partial class PatientFavoriteDoctor
    {
        [Key]
        public int ID { get; set; }
        public int PatientID { get; set; }
        public int ClinicDoctorID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateUpdated { get; set; }
        public int SortOrder { get; set; }

        [ForeignKey("ClinicDoctorID")]
        [InverseProperty("PatientFavoriteDoctors")]
        public virtual ClinicDoctor ClinicDoctor { get; set; }
        [ForeignKey("PatientID")]
        [InverseProperty("PatientFavoriteDoctors")]
        public virtual Patient Patient { get; set; }
    }
}