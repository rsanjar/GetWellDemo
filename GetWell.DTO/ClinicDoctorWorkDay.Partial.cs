using System;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
    [MetadataType(typeof(IClinicDoctorWorkDay))]
    [ModelMetadataType(typeof(IClinicDoctorWorkDay))]
    public partial class ClinicDoctorWorkDay : IClinicDoctorWorkDay
    {
        private TimeSpan? _openTimeNullable;
        private TimeSpan? _closeTimeNullable;

        public TimeSpan? OpenTimeNullable
        {
            get { return _openTimeNullable; }
            set
            {
                if (value != null)
                    StartTime = value.GetValueOrDefault();

                _openTimeNullable = value;
            }
        }

        public TimeSpan? CloseTimeNullable
        {
            get { return _closeTimeNullable; }
            set
            {
                if (value != null)
                    EndTime = value.GetValueOrDefault();

                _closeTimeNullable = value;
            }
        }

        public string OpenTimeString => OpenTimeNullable.HasValue ? OpenTimeNullable.Value.ToString("hh\\:mm") : "";

        public string CloseTimeString => CloseTimeNullable.HasValue ? CloseTimeNullable.Value.ToString("hh\\:mm") : "";

        public string BreakStartTimeString => BreakStartTime.HasValue ? BreakStartTime.Value.ToString("hh\\:mm") : "";

        public string BreakEndTimeString => BreakEndTime.HasValue ? BreakEndTime.Value.ToString("hh\\:mm") : "";
    }
}
