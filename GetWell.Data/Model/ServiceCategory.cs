﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Data.Model
{
    [Table("ServiceCategory")]
    [Index("Name", Name = "IX_ServiceCategory", IsUnique = true)]
    public partial class ServiceCategory
    {
        public ServiceCategory()
        {
            Services = new HashSet<Service>();
        }

        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(250)]
        [Unicode(false)]
        public string Name { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string Description { get; set; }
        public int SortOrder { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        [StringLength(250)]
        [Unicode(false)]
        public string NameUz { get; set; }
        [StringLength(250)]
        [Unicode(false)]
        public string NameEn { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string DescriptionUz { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string DescriptionEn { get; set; }
        [StringLength(250)]
        [Unicode(false)]
        public string IconUrl { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string HexColor { get; set; }
        public int CategoryID { get; set; }
        public int TitleID { get; set; }

        [ForeignKey("CategoryID")]
        [InverseProperty("ServiceCategories")]
        public virtual Category Category { get; set; }
        [ForeignKey("TitleID")]
        [InverseProperty("ServiceCategories")]
        public virtual Title Title { get; set; }
        [InverseProperty("ServiceCategory")]
        public virtual ICollection<Service> Services { get; set; }
    }
}