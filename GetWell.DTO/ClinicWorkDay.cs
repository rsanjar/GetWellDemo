using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class ClinicWorkDay
    {
        public int ID { get; set; }
        public int WeekDayID { get; set; }
        public int ClinicID { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
    }
}
