namespace GetWell.Core.Enums
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