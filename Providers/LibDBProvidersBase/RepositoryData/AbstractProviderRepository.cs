using System;
using System.Collections.Generic;
using System.Data;

using Bau.Libraries.LibDBProvidersBase.Parameters;

namespace Bau.Libraries.LibDBProvidersBase.RepositoryData
{
	/// <summary>
	///		Objeto base para los objetos que cumplan la interface <see cref="IProviderRepository"/>
	/// </summary>
	public abstract class AbstractProviderRepository : IProviderRepository
	{
		public AbstractProviderRepository(IDBProvider objConnection)
		{ Connection = objConnection;
		}

		/// <summary>
		///		Obtiene una instancia de este tipo de repositorio
		/// </summary>
		public abstract IProviderRepository GetInstance();

		/// <summary>
		///		Carga los datos de una colección
		/// </summary>
		public void LoadCollection<TypeData>(IList<TypeData> objColData, string strText, CommandType intCommandType, AssignDataCallBack fncCallBack) 
											where TypeData : new()
		{ LoadCollection<TypeData>(objColData, strText, null, intCommandType, fncCallBack);
		}

		/// <summary>
		///		Carga una colección utilizando genéricos para un procedimiento con un único 
		/// parámetro de entrada alfanumérico
		/// </summary>
		public void LoadCollection<TypeData>(IList<TypeData> objColData, string strText, string strParameterName, string strParameterValue, 
																				 int intParameterLength, CommandType intCommandType, AssignDataCallBack fncCallBack) 
											where TypeData : new()
		{ LoadCollection<TypeData>(objColData, strText, GetParameters(strParameterName, strParameterValue, intParameterLength),	
															 intCommandType, fncCallBack);
		}

		/// <summary>
		///		Carga una colección utilizando genéricos para un procedimiento con un único 
		/// parámetro de entrada numérico
		/// </summary>
		public void LoadCollection<TypeData>(IList<TypeData> objColData, string strText, string strParameterName, 
																				 int? intParameterValue, CommandType intCommandType, AssignDataCallBack fncCallBack) 
											where TypeData : new()
		{ LoadCollection<TypeData>(objColData, strText, GetParameters(strParameterName, intParameterValue), 
															 intCommandType, fncCallBack);
		}

		/// <summary>
		///		Carga una colección utilizando genéricos
		/// </summary>
		public void LoadCollection<TypeData>(IList<TypeData> objColData, string strText, Parameters.ParametersDBCollection objColParameters,
																				 CommandType intCommandType, AssignDataCallBack fncCallBack) 
											where TypeData : new()
		{ // Abre la conexión
				Connection.Open();
			// Carga los datos
				using (IDataReader rdoData = Connection.ExecuteReader(strText, objColParameters, intCommandType))
					{ // Lee los datos
							while (rdoData.Read())
								objColData.Add((TypeData) fncCallBack(rdoData));
						// Cierra el recordset
							rdoData.Close();
					}
			// Cierra la conexión
				Connection.Close();
		}
		
		/// <summary>
		///		Carga un objeto utilizando genéricos para un procedimiento con un único parámetro alfanumérico
		/// </summary>
		public TypeData LoadObject<TypeData>(string strText, string strParameterName, string strParameterValue, int intParameterLength, 
																				 CommandType intCommandType, AssignDataCallBack fncCallBack) where TypeData : new()
		{ ParametersDBCollection objColParametersDB = new ParametersDBCollection();

				// Asigna los parámetros
					objColParametersDB.Add(strParameterName, strParameterValue, intParameterLength);
				// Carga los datos
					return LoadObject<TypeData>(strText, objColParametersDB, intCommandType, fncCallBack);
		}
		
		/// <summary>
		///		Carga un objeto utilizando genéricos para un procedimiento con un único parámetro numérico
		/// </summary>
		public TypeData LoadObject<TypeData>(string strText, string strParameterName, int? intParameterValue,
																				 CommandType intCommandType, AssignDataCallBack fncCallBack) where TypeData : new()
		{ ParametersDBCollection objColParametersDB = new ParametersDBCollection();

				// Asigna los parámetros
					objColParametersDB.Add(strParameterName, intParameterValue);
				// Carga los datos
					return LoadObject<TypeData>(strText, objColParametersDB, intCommandType, fncCallBack);
		}
		
		/// <summary>
		///		Carga un objeto utilizando genéricos
		/// </summary>
		public TypeData LoadObject<TypeData>(string strText, Parameters.ParametersDBCollection objColParametersDB, 
																				 CommandType intCommandType, AssignDataCallBack fncCallBack) where TypeData : new()
		{ TypeData objData = default(TypeData);

				// Abre la conexión
					Connection.Open();
				// Lee los datos
					using (IDataReader rdoData = Connection.ExecuteReader(strText, objColParametersDB, intCommandType))
						{ // Lee los datos
								if (rdoData.Read())
									objData = (TypeData) fncCallBack(rdoData);
								else
									objData = (TypeData) fncCallBack(null);
							// Cierra el recordset
								rdoData.Close();
						}
				// Cierra la conexión
					Connection.Close();
				// Devuelve el objeto
					return objData;
		}
		
		/// <summary>
		///		Carga un objeto utilizando genéricos para un procedimiento con un único parámetro alfanumérico
		/// </summary>
		public void Execute(string strText, string strParameterName, string strParameterValue, int intParameterLength, 
												CommandType intCommandType = CommandType.Text)
		{ Execute(strText, GetParameters(strParameterName, strParameterValue, intParameterLength), intCommandType);
		}
		
		/// <summary>
		///		Carga un objeto utilizando genéricos para un procedimiento con un único parámetro numérico
		/// </summary>
		public void Execute(string strText, string strParameterName, int? intParameterValue, CommandType intCommandType = CommandType.Text)
		{ Execute(strText, GetParameters(strParameterName, intParameterValue), intCommandType);
		}
		
		/// <summary>
		///		Ejecuta una sentencia sobre la conexión
		/// </summary>
		public void Execute(string strText, Parameters.ParametersDBCollection objColParametersDB, CommandType intCommandType = CommandType.Text)
		{ // Abre la conexión
				Connection.Open();
			// Ejecuta sobre la conexión
				Connection.Execute(strText, objColParametersDB, intCommandType);
			// Cierra la conexión
				Connection.Close();
		}
		
		/// <summary>
		///		Ejecuta una sentencia sobre la conexión y devuelve un escalar
		/// </summary>
		public object ExecuteScalar(string strText, string strParameterName, string strParameterValue, int intParameterLength, CommandType intCommandType = CommandType.Text)
		{ return ExecuteScalar(strText, GetParameters(strParameterName, strParameterValue, intParameterLength), intCommandType);
		}
		
		/// <summary>
		///		Ejecuta una sentencia sobre la conexión y devuelve un escalar
		/// </summary>
		public object ExecuteScalar(string strText, string strParameterName, int? intParameterValue, CommandType intCommandType = CommandType.Text)
		{ return ExecuteScalar(strText, GetParameters(strParameterName, intParameterValue), intCommandType);
		}
		
		/// <summary>
		///		Ejecuta una sentencia sobre la conexión y devuelve un escalar
		/// </summary>
		public object ExecuteScalar(string strText, Parameters.ParametersDBCollection objColParametersDB, CommandType intCommandType = CommandType.Text)
		{ object objValue = null;

				// Abre la conexión
					Connection.Open();
				// Ejecuta sobre la conexión
					objValue = Connection.ExecuteScalar(strText, objColParametersDB, intCommandType);
				// Cierra la conexión
					Connection.Close();
				// Devuelve el resultado
					return objValue;
		}

		/// <summary>
		///		Ejecuta un INSERT sobre la base de datos para obtener el valor de identidad de la clave primaria
		/// </summary>
		public abstract int? ExecuteGetIdentity(string strText, ParametersDBCollection objColParametersDB, CommandType intCommandType = CommandType.Text);

		/// <summary>
		///		Obtiene una colección de parámetros con un único parámetro de tipo cadena
		/// </summary>
		private ParametersDBCollection GetParameters(string strParameterName, string strParameterValue, int intLength)
		{ ParametersDBCollection objColParametersDB = new ParametersDBCollection();

				// Asigna los parámetros
					objColParametersDB.Add(strParameterName, strParameterValue, intLength);
				// Devuelve los parámetros
					return objColParametersDB;
		}
		
		/// <summary>
		///		Obtiene una colección de parámetros con un único parámetro de tipo entero
		/// </summary>
		private ParametersDBCollection GetParameters(string strParameterName, int? intParameterValue)
		{ ParametersDBCollection objColParametersDB = new ParametersDBCollection();

				// Asigna los parámetros
					objColParametersDB.Add(strParameterName, intParameterValue);
				// Devuelve los parámetros
					return objColParametersDB;
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
				Connection.Dispose();
		}

		/// <summary>
		///		Destruye el objeto
		/// </summary>
	  ~AbstractProviderRepository() 
	  {	Dispose(false);
		}

		/// <summary>
		///		Conexión a la base de datos
		/// </summary>
		public IDBProvider Connection { get; private set; }
	}
}
