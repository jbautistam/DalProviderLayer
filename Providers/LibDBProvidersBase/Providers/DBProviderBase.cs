using System;
using System.Data;

using Bau.Libraries.LibDBProvidersBase.Parameters;

namespace Bau.Libraries.LibDBProvidersBase.Providers
{
	/// <summary>
	///		Clase base para los proveedores de base de datos
	/// </summary>
	public abstract class DBProviderBase : IDBProvider
	{ 
		public DBProviderBase(IConnectionString objConnectionString)
		{ // Inicializa la conexión
				ConnectionString = objConnectionString;
			// Inicializa los objetos protegidos
				Connection = null;
				Transaction = null;
		}

		/// <summary>
		///		Abre la conexión a la base de datos
		/// </summary>
		public void Open()
		{	if (Connection == null || Connection.State != ConnectionState.Open)
				{	// Crea la conexión
						Connection = GetInstance();
					// Abre la conexión
						Connection.Open();
				}
		}

		/// <summary>
		///		Crea la conexión
		/// </summary>
		protected abstract IDbConnection GetInstance();

		/// <summary>
		///		Obtiene un comando
		/// </summary>
		protected abstract IDbCommand GetCommand(string strText);

		/// <summary>
		///		Cierra la conexión a la base de datos
		/// </summary>
		public virtual void Close()
		{ if (Connection != null && Connection.State == ConnectionState.Open)
				Connection.Close();
		}

		/// <summary>
		///		Ejecuta una sentencia o un procedimiento sobre la base de datos
		/// </summary>
		public void Execute(string strText, ParametersDBCollection objColParameters, CommandType intCommandType) 
		{ using (IDbCommand objCommand = GetCommand(strText))
				{ // Indica el tipo del comando
						objCommand.CommandType = intCommandType;
					// Añade los parámetros al comando
						AddParameters(objCommand, objColParameters);
					// Ejecuta la consulta
						objCommand.ExecuteNonQuery();
					// Pasa los valores de salida de los parámetros del comando a la colección de parámetros de entrada
						objColParameters = ReadOutputParameters(objCommand.Parameters);
				}						
		}

		/// <summary>
		///		Obtiene un DataReader
		/// </summary>
		public IDataReader ExecuteReader(string strText, ParametersDBCollection objParametersDB, CommandType intCommandType)
		{ IDataReader objReader = null; // ... supone que no se puede abrir el dataReader			
		
				// Ejecuta el comando
					using (IDbCommand objCommand = GetCommand(strText))
						{ // Indica el tipo de comando
								objCommand.CommandType = intCommandType;
							// Añade los parámetros
								AddParameters(objCommand, objParametersDB);
							// Obtiene el dataReader
								objReader = objCommand.ExecuteReader();
						}
				// Devuelve el dataReader
					return objReader;
		}
		
		/// <summary>
		///		Ejecuta una sentencia o procedimiento sobre la base de datos y devuelve un escalar
		/// </summary>
		public object ExecuteScalar(string strText, ParametersDBCollection objColParameters, CommandType intCommandType)
		{ object objResult = null;
		
				// Ejecuta el comando
					using (IDbCommand objCommand = GetCommand(strText))
						{ // Indica el tipo de comando
								objCommand.CommandType = intCommandType;
							// Añade los parámetros al comando
								AddParameters(objCommand, objColParameters);
							// Ejecuta la consulta
								objResult = objCommand.ExecuteScalar();
						}												
				// Devuelve el resultado
					return objResult;
		}

		/// <summary>
		///		Obtiene un dataTable a partir de un nombre de una sentencia o procedimiento y sus parámetros
		/// </summary>
		public DataTable GetDataTable(string strText, ParametersDBCollection objColParameters, CommandType intCommandType)
		{	DataTable dtTable = new DataTable();
		
				// Crea el comando SQL Server
					using (IDbCommand objCommand = GetCommand(strText))
						{ // Inicializa el tipo de comando
								objCommand.CommandType = intCommandType;
							// Pasa los parámetros al comando
								AddParameters(objCommand, objColParameters);
							// Rellena la tabla con los datos
								dtTable = FillDataTable(objCommand);
						}
				// Devuelve la tabla
					return dtTable;
		}

		/// <summary>
		///		Obtiene un adaptador de datos
		/// </summary>
		protected abstract DataTable FillDataTable(IDbCommand objCommand);

		/// <summary>
		///		Añade a un comando los parámetros de una clase <see cref="ParametersDBCollection"/>
		/// </summary>
		protected void AddParameters(IDbCommand objCommand, ParametersDBCollection objColParameters)
		{ // Limpia los parámetros antiguos
				objCommand.Parameters.Clear();
			// Añade los parámetros nuevos
				if (objColParameters != null)
					for (int intIndex = 0; intIndex < objColParameters.Count; intIndex++)
						objCommand.Parameters.Add(GetSQLParameter(objColParameters[intIndex]));
		}
		
		/// <summary>
		///		Obtiene un parámetro SQLServer a partir de un parámetro genérico
		/// </summary>
		private IDataParameter GetSQLParameter(ParameterDB objParameter)
		{ IDataParameter objDBParameter = ConvertParameter(objParameter);
		
				// Asigna el valor
					objDBParameter.Value = objParameter.GetDBValue();
				// Asigna la dirección
					objDBParameter.Direction = objParameter.Direction;
				// Devuelve el parámetro
					return objDBParameter;
		}

		/// <summary>
		///		Método abstracto para convertir un parámetro
		/// </summary>
		protected abstract IDataParameter ConvertParameter(ParameterDB objParameter);

		/// <summary>
		///		Lee los parámetros de salida
		/// </summary>
		private ParametersDBCollection ReadOutputParameters(IDataParameterCollection objColOutputParameters)
		{ ParametersDBCollection objColParameters = new ParametersDBCollection();

				// Recupera los parámetros
					foreach (IDataParameter objOutputParameter in objColOutputParameters)
						{ ParameterDB objParameter = new ParameterDB();
								
								// Asigna los datos
									objParameter.Name = objOutputParameter.ParameterName;
									objParameter.Direction = objOutputParameter.Direction;
									if (objOutputParameter.Value == DBNull.Value)
										objParameter.Value = null;
									else
										objParameter.Value = objOutputParameter.Value;
								// Añade el parámetro a la colección
									objColParameters.Add(objParameter);
						}
				// Devuelve la colección de parámetros
					return objColParameters;
		}

		/// <summary>
		///		Inicia una transacción
		/// </summary>
		public void BeginTransaction()
		{ Transaction = Connection.BeginTransaction();
		}
		
		/// <summary>
		///		Confirma una transacción
		/// </summary>
		public void Commit()
		{ if (Transaction != null)
				Transaction.Commit();
			Transaction = null;
		}
		
		/// <summary>
		///		Deshace una transacción
		/// </summary>
		public void RollBack()
		{ if (Transaction != null)
				Transaction.Rollback();
			Transaction = null;
		}

		/// <summary>
		///		Desconecta la conexión
		/// </summary>
		public void Dispose() 
		{	Dispose(true);
			GC.SuppressFinalize(this);
		}
	
		/// <summary>
		///		Desconecta la conexión
		/// </summary>
		private void Dispose(bool blnDispose) 
		{	if (blnDispose && Connection != null) 
				{	Close();
					Connection.Dispose();
				}
		}

		/// <summary>
		///		Destruye el objeto
		/// </summary>
	  ~DBProviderBase() 
	  {	Dispose(false);
		}

		/// <summary>
		///		Cadena de conexión
		/// </summary>
		public IConnectionString ConnectionString { get; set; }

		/// <summary>
		///		Conexión
		/// </summary>
		protected IDbConnection Connection { get; set; }

		/// <summary>
		///		Transacción
		/// </summary>
		protected IDbTransaction Transaction { get; set; }
	}
}