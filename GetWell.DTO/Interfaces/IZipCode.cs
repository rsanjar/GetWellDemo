namespace GetWell.DTO.Interfaces;

public interface IZipCode : IBaseModel
{
	public double? Latitude { get; set; }
	public double? Longitude { get; set; }
	public int RegionID { get; set; }
}