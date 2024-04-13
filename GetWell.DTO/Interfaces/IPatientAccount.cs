namespace GetWell.DTO.Interfaces
{
    public interface IPatientAccount : IAccount
    {
        int PatientID { get; set; }
    }
}