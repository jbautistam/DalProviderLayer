using System;
using System.Data;
using System.Data.Odbc;

using Bau.Libraries.LibDBProvidersBase.Parameters;

namespace Bau.Libraries.LibDBProvidersBase.Providers.ODBC
{
	/// <summary>
	///		Proveedor para ODBC
	/// </summary>
	public class ODBCProvider : DBProviderBase
	{ 
		public ODBCProvider(IConnectionString objConnectionString) : base(objConnectionString) {}

		/// <summary>
		///		Crea la conexión
		/// </summary>
		protected override IDbConnection GetInstance()
		{ return new OdbcConnection(ConnectionString.ConnectionString);
		}

		/// <summary>
		///		Obtiene un comando
		/// </summary>
		protected override IDbCommand GetCommand(string strText)
		{ return new OdbcCommand(strText, Connection as OdbcConnection);
		}

		/// <summary>
		///		Obtiene un dataTable
		/// </summary>
		protected override DataTable FillDataTable(IDbCommand objCommand)
		{	DataTable dtTable = new DataTable();
		
				// Rellena la tabla con los datos
					using (OdbcDataAdapter objAdapter = new OdbcDataAdapter(objCommand as OdbcCommand))
						objAdapter.Fill(dtTable);
				// Devuelve la tabla
					return dtTable;
		}

		/// <summary>
		///		Obtiene un parámetro SQLServer a partir de un parámetro genérico
		/// </summary>
		protected override IDataParameter ConvertParameter(ParameterDB objParameter)
		{ if (objParameter.Direction == ParameterDirection.ReturnValue)
				return new OdbcParameter(objParameter.Name, OdbcType.Int);
			else if (objParameter.IsText)
				return new OdbcParameter(objParameter.Name, OdbcType.VarChar);
			else if (objParameter.Value is bool)
				return new OdbcParameter(objParameter.Name, OdbcType.Bit);
			else if (objParameter.Value is int)
				return new OdbcParameter(objParameter.Name, OdbcType.Int);
			else if (objParameter.Value is double)
				return new OdbcParameter(objParameter.Name, OdbcType.Double);
			else if (objParameter.Value is string)
				return new OdbcParameter(objParameter.Name, OdbcType.VarChar, objParameter.Length);
			else if (objParameter.Value is byte [])
				return new OdbcParameter(objParameter.Name, OdbcType.Binary);
			else if (objParameter.Value is DateTime)
				return new OdbcParameter(objParameter.Name, OdbcType.Date);
			else
				throw new NotSupportedException("Tipo del parámetro " + objParameter.Name + "desconocido");
		}
	}
}