using System;
using System.Data;

using Bau.Libraries.LibDBProvidersBase.Parameters;
using Bau.Libraries.LibDBProvidersBase.RepositoryData;

namespace Bau.Libraries.LibDBProviderSQLServerCE
{
	/// <summary>
	///		Clase para ayuda de los repository de bases de datos SQL Server Compact
	/// </summary>
	public class SQLServerCompactRepository : AbstractProviderRepository
	{
		public SQLServerCompactRepository(SQLServerCompactProvider objConnection) : base(objConnection) {}

		/// <summary>
		///		Obtiene una instancia de este repositorio
		/// </summary>
		public override IProviderRepository GetInstance()
		{ return new SQLServerCompactRepository(base.Connection as SQLServerCompactProvider);
		}

		/// <summary>
		///		Ejecuta un INSERT sobre la base de datos y obtiene el valor de identidad
		/// </summary>
		public override int? ExecuteGetIdentity(string strText, ParametersDBCollection objColParametersDB, CommandType intCommandType = CommandType.Text)
		{ int? intIdentity = null;

				// Abre la conexión
					Connection.Open();
				// Ejecuta sobre la conexión
					Connection.Execute(strText, objColParametersDB, intCommandType);
					intIdentity = (int?) ((decimal?) Connection.ExecuteScalar("SELECT @@IDENTITY", null, CommandType.Text));
				// Cierra la conexión
					Connection.Close();
				// Devuelve el valor identidad
					return intIdentity;
		}
	}
}
