using AutoMapper;
using GetWell.Data;

namespace GetWell.Service
{
	public static class ObjectMapper<TModel, TData>
		where TModel : class, DTO.Interfaces.IBaseModel, new() 
		where TData : class, IBaseModel, new()
	{
        // ReSharper disable once StaticMemberInGenericType
        private static readonly MapperConfiguration MapperConfiguration;
		public static IMapper Mapper => new Mapper(MapperConfiguration);

		static ObjectMapper()
		{
			MapperConfiguration ??= CreateMap();
		}

        private static MapperConfiguration CreateMap()
		{
			return new (cfg =>
			{
				cfg.CreateMap<TData, TModel>(MemberList.Destination);
				cfg.CreateMap<TModel, TData>(MemberList.Source);
			});
		}
    }
}