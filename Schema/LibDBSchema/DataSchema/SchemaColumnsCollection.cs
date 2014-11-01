using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Colecci�n de columnas
	/// </summary>
	public class SchemaColumnsCollection : SchemaItemsCollection<SchemaColumn>
	{
		public SchemaColumnsCollection(Schema objParent) : base(objParent)
		{
		}
	}
}