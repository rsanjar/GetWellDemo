namespace GetWell.Data
{
	public enum Crud
	{
		Success,
		Error,
		ValidationError,
		DuplicateEntryError,
		ItemNotFoundError,
		DeleteForeignKeyReferenceError,
		AccessDeniedError
	}
}