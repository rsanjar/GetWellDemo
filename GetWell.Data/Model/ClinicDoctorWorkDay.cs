﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Data.Model
{
    [Table("ClinicDoctorWorkDay")]
    [Index("ClinicDoctorID", "WeekDayID", Name = "IX_ClinicDoctorWorkDay", IsUnique = true)]
    public partial class ClinicDoctorWorkDay
    {
        [Key]
        public int ID { get; set; }
        public int WeekDayID { get; set; }
        public int ClinicDoctorID { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan? BreakStartTime { get; set; }
        public TimeSpan? BreakEndTime { get; set; }

        [ForeignKey("ClinicDoctorID")]
        [InverseProperty("ClinicDoctorWorkDays")]
        public virtual ClinicDoctor ClinicDoctor { get; set; }
        [ForeignKey("WeekDayID")]
        [InverseProperty("ClinicDoctorWorkDays")]
        public virtual WeekDay WeekDay { get; set; }
    }
}