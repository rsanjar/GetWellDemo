namespace GetWell.Data
{
	public class CrudResponse
	{
		public CrudResponse(Crud result)
		{
			MessageKey = result;
		}

		public Crud MessageKey { get; set; }

		public string Message
		{
			get
			{
				switch (MessageKey)
				{
					case Crud.Success: 
						return "Success";
					case Crud.Error: 
						return "Unexpected Error Occurred";
					case Crud.ValidationError: 
						return "Validation Error";
					case Crud.DuplicateEntryError:
						return "Error: Duplicate Record";
					case Crud.ItemNotFoundError:
						return "Item Not Found";
					case Crud.DeleteForeignKeyReferenceError:
						return "Foreign Key Reference Error";
					case Crud.AccessDeniedError:
						return "Access Denied";
					default:
						return "Error Occurred";
				}
			}
		}

        public static implicit operator CrudResponse(Crud result)
        {
            return new(result);
        }
	}
}