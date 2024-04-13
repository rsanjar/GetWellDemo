using System;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO
{
    [MetadataType(typeof(IClinicWorkDay))]
    [ModelMetadataType(typeof(IClinicWorkDay))]
    public partial class ClinicWorkDay : IClinicWorkDay
    {
        private TimeSpan? _openTimeNullable;
        private TimeSpan? _closeTimeNullable;

        public TimeSpan? OpenTimeNullable
        {
            get { return _openTimeNullable; }
            set
            {
                if (value != null)
                    OpenTime = value.GetValueOrDefault();
                
                _openTimeNullable = value;
            }
        }

        public TimeSpan? CloseTimeNullable
        {
            get { return _closeTimeNullable; }
            set
            {
                if(value != null)
                    CloseTime = value.GetValueOrDefault();

                _closeTimeNullable = value;
            }
        }

        public string OpenTimeString => OpenTimeNullable.HasValue ? OpenTimeNullable.Value.ToString("hh\\:mm") : "";

        public string CloseTimeString => CloseTimeNullable.HasValue ? CloseTimeNullable.Value.ToString("hh\\:mm") : "";
    }
}
