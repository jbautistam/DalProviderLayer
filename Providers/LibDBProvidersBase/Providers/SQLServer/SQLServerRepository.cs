using System;
using System.Data;

namespace Bau.Libraries.LibDBProvidersBase.Providers.SQLServer
{
	/// <summary>
	///		Clase para ayuda de repository de SQL Server
	/// </summary>
	public class SQLServerRepository<TypeData> : RepositoryData.AbstractProviderRepository
	{
		public SQLServerRepository(SQLServerProvider objConnection) : base(objConnection)
		{
		}

		/// <summary>
		///		Obtiene una instancia de este repositorio
		/// </summary>
		public override RepositoryData.IProviderRepository GetInstance()
		{ return new SQLServerRepository<TypeData>(base.Connection as SQLServerProvider);
		}

		/// <summary>
		///		Ejecuta un INSERT sobre la base de datos y obtiene el valor de identidad
		/// </summary>
		public override int? ExecuteGetIdentity(string strText, Parameters.ParametersDBCollection objColParametersDB, System.Data.CommandType intCommandType = CommandType.Text)
		{ int? intIdentity = null;

				// Abre la conexión
					Connection.Open();
				// Ejecuta sobre la conexión
					switch (intCommandType)
						{ case CommandType.Text:
									intIdentity = (int?) Connection.ExecuteScalar(NormalizeSqlInsert(strText), objColParametersDB, intCommandType);
									// intIdentity = (int?) ((decimal?) Connection.ExecuteScalar(NormalizeSqlInsert(strText), objColParametersDB, intCommandType));
								break;
							case CommandType.StoredProcedure:
									Connection.Execute(strText, objColParametersDB, intCommandType);
									intIdentity = (int?) objColParametersDB["@return_code"].Value;
								break;
							default:
									Connection.Execute(strText, objColParametersDB, intCommandType);
									intIdentity = null;
								break;
						}
				// Cierra la conexión
					Connection.Close();
				// Devuelve el valor identidad
					return intIdentity;
		}

		/// <summary>
		///		Normaliza una cadena SQL de inserción de datos
		/// </summary>
		private string NormalizeSqlInsert(string strSqlInsert)
		{ // Añade a la cadena de inserción una consulta para obtener el SCOPE_IDENTITY
				if (strSqlInsert.IndexOf("Scope_Identity()", StringComparison.CurrentCultureIgnoreCase) < 0)
					strSqlInsert += "; SELECT SCOPE_IDENTITY()";
			// Devuelve la cadena de inserción
				return strSqlInsert;
		}
	}
}
