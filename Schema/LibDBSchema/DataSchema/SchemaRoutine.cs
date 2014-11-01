using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Clase que mantiene los datos de una rutina
	/// </summary>
	public class SchemaRoutine : SchemaItem
	{ // Enumerados
			public enum RoutineType
				{	Unknown,
					Procedure,
					Function
				}

		public SchemaRoutine(Schema objParent) : base(objParent)
		{
		}
		
		public string Definition { get; set; }
		
		public RoutineType Type { get; set; }
	}
}
