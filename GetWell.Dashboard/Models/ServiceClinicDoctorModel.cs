using System.Collections.Generic;
using GetWell.DTO;

namespace GetWell.Dashboard.Models
{
    public class ServiceClinicDoctorModel
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public int ClinicID { get; set; }
        
        public int DoctorID { get; set; }

        public int ServiceClinicID { get; set; }
        
        public int ClinicDoctorID { get; set; }
        
        public Dictionary<int, List<ServiceClinicDoctor>> ServiceClinicDoctors { get; set; }

        public List<ServiceClinicDoctor> GetCurrentList => ServiceClinicDoctors[CategoryID];

        public Dictionary<int, string> ServiceCategories { get; set; }
    }
}