namespace GetWell.Data.Model.Interface
{
	public interface IZipCode : IBaseModel
	{
		double? Latitude { get; set; }
		double? Longitude { get; set; }
		int RegionID { get; set; }
	}
}