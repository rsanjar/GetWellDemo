using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetWell.Data
{
	public interface IBaseModel
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
	}
}