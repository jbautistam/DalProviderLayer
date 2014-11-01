using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Colección de columnas
	/// </summary>
	public class SchemaColumnsCollection : SchemaItemsCollection<SchemaColumn>
	{
		public SchemaColumnsCollection(Schema objParent) : base(objParent)
		{
		}
	}
}