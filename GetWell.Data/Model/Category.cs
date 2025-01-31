﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Data.Model
{
    [Table("Category")]
    [Index("Name", Name = "IX_Category", IsUnique = true)]
    public partial class Category
    {
        public Category()
        {
            ServiceCategories = new HashSet<ServiceCategory>();
        }

        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string Name { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string Description { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string IconUrl { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string HexColor { get; set; }
        public int SortOrder { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string NameUz { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string NameEn { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string DescriptionUz { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string DescriptionEn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateUpdated { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<ServiceCategory> ServiceCategories { get; set; }
    }
}