using System;
using System.Data;
using System.Data.SqlClient;

using Bau.Libraries.LibDBProvidersBase.Parameters;

namespace Bau.Libraries.LibDBProvidersBase.Providers.SQLServer
{
	/// <summary>
	///		Proveedor para SQL Server
	/// </summary>
	public class SQLServerProvider : DBProviderBase
	{ 
		public SQLServerProvider(IConnectionString objConnectionString) : base(objConnectionString) {}

		/// <summary>
		///		Crea la conexión
		/// </summary>
		protected override IDbConnection GetInstance()
		{ return new SqlConnection(ConnectionString.ConnectionString);
		}

		/// <summary>
		///		Obtiene un comando
		/// </summary>
		protected override IDbCommand GetCommand(string strText)
		{ return new SqlCommand(strText, Connection as SqlConnection);
		}

		/// <summary>
		///		Obtiene un dataTable
		/// </summary>
		protected override DataTable FillDataTable(IDbCommand objCommand)
		{	DataTable dtTable = new DataTable();
		
				// Rellena la tabla con los datos
					using (SqlDataAdapter objAdapter = new SqlDataAdapter(objCommand as SqlCommand))
						objAdapter.Fill(dtTable);
				// Devuelve la tabla
					return dtTable;
		}

		/// <summary>
		///		Convierte un parámetro
		/// </summary>
		protected override IDataParameter ConvertParameter(ParameterDB objParameter)
		{	if (objParameter.Direction == ParameterDirection.ReturnValue)
				return new SqlParameter(objParameter.Name, SqlDbType.Int);
			else if (objParameter.Value == null)
				return new SqlParameter(objParameter.Name, null);
			else if (objParameter.IsText)
				return new SqlParameter(objParameter.Name, SqlDbType.Text);
			else if (objParameter.Value is bool?)
				return new SqlParameter(objParameter.Name, SqlDbType.Bit);
			else if (objParameter.Value is int?)
				return new SqlParameter(objParameter.Name, SqlDbType.Int);
			else if (objParameter.Value is double?)
				return new SqlParameter(objParameter.Name, SqlDbType.Float);
			else if (objParameter.Value is string)
				return new SqlParameter(objParameter.Name, SqlDbType.VarChar, objParameter.Length);
			else if (objParameter.Value is byte [])
				return new SqlParameter(objParameter.Name, SqlDbType.Image);
			else if (objParameter.Value is DateTime?)
				return  new SqlParameter(objParameter.Name, SqlDbType.DateTime);
			else
				throw new NotSupportedException("Tipo del parámetro " + objParameter.Name + "desconocido");
		}
	}
}