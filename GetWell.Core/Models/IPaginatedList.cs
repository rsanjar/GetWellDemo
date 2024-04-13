using System.Collections.Generic;
using GetWell.DTO;
using GetWell.DTO.Interfaces;
using Newtonsoft.Json;

namespace GetWell.Core.Models
{
    public interface IPaginatedList<T> : IBasePagination where T : IBaseModel
    {
        [JsonProperty] List<T> ResultSet { get; }
    }
}