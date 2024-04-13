using System.Collections.Generic;
using GetWell.DTO;

namespace GetWell.Dashboard.Models
{
    public class ServiceClinicModel
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public int ClinicID { get; set; }
        
        public Dictionary<int, List<ServiceClinic>> ServiceClinics { get; set; }

        public List<ServiceClinic> GetCurrentList => ServiceClinics[CategoryID];

        public Dictionary<int, string> ServiceCategories { get; set; }
    }
}