using System;

namespace GetWell.Data
{
    public interface IDateUpdatedLoggable
    {
        public DateTime? DateUpdated { get; set; }
    }
}