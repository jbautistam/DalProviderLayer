using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Clase con los datos de una vista
	/// </summary>
	public class SchemaView : SchemaItem
	{ 
		public SchemaView(Schema objParent) : base(objParent)
		{ Columns = new SchemaColumnsCollection(objParent);
		}
		
		public string Definition { get; set; }
		
		public string CheckOption { get; set; }
		
		public bool IsUpdatable { get; set; }
		
		public SchemaColumnsCollection Columns { get; set; }
	}
}
