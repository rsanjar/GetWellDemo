using Microsoft.EntityFrameworkCore;

namespace GetWell.Data.Model
{
	public partial class GetWellContext
	{
		private readonly string _connectionString;

		public GetWellContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(_connectionString);
			}
		}
	}
}