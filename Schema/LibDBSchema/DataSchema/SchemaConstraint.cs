using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Clase con los datos de una restricción
	/// </summary>
	public class SchemaConstraint : SchemaItem
	{ // Enumerados
			public enum ConstraintType
				{ Unknown,
					PrimaryKey,
					ForeignKey,
					Unique
				}
			
		public SchemaConstraint(Schema objParent) : base(objParent)
		{
		}
			
		public string Table { get; set; }

		public string Column { get; set; }

		public ConstraintType Type { get; set; }

		public int Position { get; set; }
	}
}