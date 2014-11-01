using System;
using System.Collections.Generic;
using System.Data;

using Bau.Libraries.LibDBProvidersBase.Providers;
using Bau.Libraries.LibDBProvidersBase.Parameters;
using Bau.Libraries.LibDBProvidersBase.RepositoryData;

namespace Bau.Libraries.LibDBProvidersBase.RepositoryData
{
	/// <summary>
	///		Base de los repository
	/// </summary>
	public abstract class AbstractBaseRepository<TypeData> where TypeData : new()
	{ 
		public AbstractBaseRepository(string strProviderKey, ProviderServiceLocator objServiceLocator)
		{ ProviderKey = strProviderKey;
			Locator = objServiceLocator;
		}

		///// <summary>
		/////		Añade un ID de un objeto como parámetro	
		///// </summary>
		//protected void AddParameter(ParametersDBCollection objColParametersDB, string strParameter, Model.Base.BaseModel objData)
		//{ if (objData == null)
		//		objColParametersDB.Add(strParameter, null);
		//	else
		//		objColParametersDB.Add(strParameter, objData.ID);
		//}

		/// <summary>
		///		Carga los datos de un elemento
		/// </summary>
		public virtual TypeData Load(int? intID)
		{ TypeData objData = new TypeData();

				// Captura los datos
					if (intID != null)
						using (IProviderRepository objRepository = Locator.Create(ProviderKey))
							{ objData = objRepository.LoadObject<TypeData>(GetSQLSelectByID(), NameParameterID, intID, CommandType.Text, AssignData);
							}
				// Devuelve el objeto
					return objData;
		}

		/// <summary>
		///		Carga los datos de un objeto
		/// </summary>
		public virtual TypeData LoadObject(string strSQL, string strParameterName, 
																			 int? intParameterValue, CommandType intCommandType = CommandType.Text)
		{ ParametersDBCollection objColParametersDB = new ParametersDBCollection();

				// Asigna los parámetros
					objColParametersDB.Add(strParameterName, intParameterValue);
				// Carga los datos
					return LoadObject(strSQL, objColParametersDB, intCommandType);
		}

		/// <summary>
		///		Carga los datos de un objeto
		/// </summary>
		public virtual TypeData LoadObject(string strSQL, string strParameterName, string strParameterValue, 
																			 int intLength, CommandType intCommandType = CommandType.Text)
		{ ParametersDBCollection objColParametersDB = new ParametersDBCollection();

				// Asigna los parámetros
					objColParametersDB.Add(strParameterName, strParameterValue, intLength);
				// Carga los datos
					return LoadObject(strSQL, objColParametersDB, intCommandType);
		}

		/// <summary>
		///		Carga los datos de un elemento
		/// </summary>
		public virtual TypeData LoadObject(string strSQL, ParametersDBCollection objColParametersDB, 
																			 CommandType intCommandType = CommandType.Text)
		{ TypeData objData = new TypeData();

				// Captura los datos
					using (IProviderRepository objRepository = Locator.Create(ProviderKey))
						{ objData = objRepository.LoadObject<TypeData>(strSQL, objColParametersDB, intCommandType, AssignData);
						}
				// Devuelve el objeto
					return objData;
		}

		/// <summary>
		///		Carga una colección
		/// </summary>
		public void LoadCollection(IList<TypeData> objColData, string strSQL, string strFieldName, int? intFieldValue, 
															 CommandType intCommandType = CommandType.Text, bool blnCheckNullsBefore = false)
		{ if (!blnCheckNullsBefore || (blnCheckNullsBefore && intFieldValue != null))
				{ ParametersDBCollection objColParametersDB = new ParametersDBCollection();

						// Asigna los parámetros
							objColParametersDB.Add(strFieldName, intFieldValue);
						// Carga los datos
							LoadCollection(objColData, strSQL, objColParametersDB, intCommandType);
				}
		}

		/// <summary>
		///		Carga una colección
		/// </summary>
		public void LoadCollection(IList<TypeData> objColData, string strSQL, string strFieldName, 
															 string strFieldValue, int intLength, CommandType intCommandType = CommandType.Text)
		{ ParametersDBCollection objColParametersDB = new ParametersDBCollection();

				// Asigna los parámetros
					objColParametersDB.Add(strFieldName, strFieldValue, intLength);
				// Carga los datos
					LoadCollection(objColData, strSQL, objColParametersDB, intCommandType);
		}

		/// <summary>
		///		Carga una colección
		/// </summary>
		public void LoadCollection(IList<TypeData> objColData, string strSQL, ParametersDBCollection objColParametersDB, 
															 CommandType intCommandType = CommandType.Text)
		{ // Carga los datos
				using (IProviderRepository objRepository = Locator.Create(ProviderKey))
					{ List<TypeData> objColRepository = new List<TypeData>();
						
							// Crea la colección
								objRepository.LoadCollection<TypeData>(objColData, strSQL, objColParametersDB, intCommandType, AssignData);
							// Recorre la colección creando la colección de datos de salida
								foreach (TypeData objData in objColRepository)
									objColData.Add(objData);
					}
		}

		/// <summary>
		///		Asigna los datos de un recordset al objeto
		/// </summary>
		protected abstract object AssignData(IDataReader rdoData);
		
		/// <summary>
		///		Graba los datos de un registro
		/// </summary>
		public virtual void Save(object objData, ref int? intObjDataId)
		{ Save((TypeData) objData, ref intObjDataId);
		}

		/// <summary>
		///		Graba los datos de un registro
		/// </summary>
		public virtual void Save(TypeData objData, ref int? intObjDataId)
		{ using (IProviderRepository objRepository = Locator.Create(ProviderKey))
				Save(objRepository, objData, FillParametersSave(objData), ref intObjDataId);
		}

		/// <summary>
		///		Graba los datos de un registro
		/// </summary>
		public virtual void Save(IProviderRepository objRepository, TypeData objData, ParametersDBCollection objColParametersDB, ref int? intObjDataId)
		{ string strSQLUpdate = GetSQLUpdate();
			
				// Graba los datos
					if (intObjDataId == null || string.IsNullOrWhiteSpace(strSQLUpdate))
						intObjDataId = objRepository.ExecuteGetIdentity(GetSQLInsert(), objColParametersDB, CommandType.Text);
					else
						objRepository.Execute(strSQLUpdate, objColParametersDB, CommandType.Text);
		}

		/// <summary>
		///		Obtiene los parámetros de grabación
		/// </summary>
		protected abstract ParametersDBCollection FillParametersSave(TypeData objData);

		/// <summary>
		///		Ejecuta una cadena SQL sobre la base de datos
		/// </summary>
		protected void Execute(string strSQL, string strParameterName, int? intParameterValue, 
													 bool blnCheckNullsBefore = true, CommandType intCommandType = CommandType.Text)
		{ if (!blnCheckNullsBefore || (blnCheckNullsBefore && intParameterValue != null))
				{ ParametersDBCollection objColParametersDB = new ParametersDBCollection();

						// Asigna los parámetros
							objColParametersDB.Add(strParameterName, intParameterValue);
						// Ejecuta la cadena SQL
							Execute(strSQL, objColParametersDB, intCommandType);
				}
		}

		/// <summary>
		///		Ejecuta una cadena SQL sobre la base de datos
		/// </summary>
		protected void Execute(string strSQL, ParametersDBCollection objColParametersDB, CommandType intCommandType = CommandType.Text)
		{ using (IProviderRepository objRepository = Locator.Create(ProviderKey))
				{ objRepository.Execute(strSQL, objColParametersDB, intCommandType);
				}
		}

		/// <summary>
		///		Ejecuta una cadena SQL sobre la base de datos y devuelve un escalar
		/// </summary>
		protected object ExecuteScalar(string strSQL, string strParameterName, int? intParameterValue, CommandType intCommandType = CommandType.Text)
		{ ParametersDBCollection objColParametersDB = new ParametersDBCollection();

			// Asigna los parámetros
				objColParametersDB.Add(strParameterName, intParameterValue);
			// Ejecuta la cadena SQL
				return ExecuteScalar(strSQL, objColParametersDB, intCommandType);
		}

		protected object ExecuteScalar(string strSQL, ParametersDBCollection objColParametersDB, CommandType intCommandType = CommandType.Text)
		{ using (IProviderRepository objRepository = Locator.Create(ProviderKey))
				{ return objRepository.ExecuteScalar(strSQL, objColParametersDB, intCommandType);
				}
		}

		/// <summary>
		///		Comprueba si puede borrar un registro (el método base siempre devuelve true)
		/// </summary>
		public virtual bool CanDelete(int? intID)
		{ return true;
		}

		/// <summary>
		///		Borra los datos de un registro
		/// </summary>
		public virtual void Delete(int? intID)
		{ if (intID != null)
				using (IProviderRepository objRepository = Locator.Create(ProviderKey))
					{ Delete(objRepository, intID);
					}
		}

		/// <summary>
		///		Borra los datos de un registro
		/// </summary>
		protected virtual void Delete(IProviderRepository objRepository, int? intID)
		{ objRepository.Execute(GetSQLDeleteByID(), NameParameterID, intID, CommandType.Text);
		}

		/// <summary>
		///		Obtiene la cadena SQL de selección por ID del registro
		/// </summary>
		protected abstract string GetSQLSelectByID();

		/// <summary>
		///		Obtiene la cadena SQL de inserción de un registro
		/// </summary>
		protected abstract string GetSQLInsert();

		/// <summary>
		///		Obtiene la cadena SQL de modificación de un registro
		/// </summary>
		protected abstract string GetSQLUpdate();

		/// <summary>
		///		Obtiene la cadena SQL de borrado de un registro
		/// </summary>
		protected abstract string GetSQLDeleteByID();

		/// <summary>
		///		Normaliza las fechas
		/// </summary>
		protected void NormalizeDates(ref DateTime dtmStart, ref DateTime dtmEnd)
		{ DateTime dtmInter = dtmStart;

				// Intercambia las fechas
					dtmStart = dtmEnd;
					dtmEnd = dtmInter;
				// Deja la fecha de inicio a 0 y la fecha de fin al final del día
					dtmStart = dtmStart.Date;
					dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
		}

		/// <summary>
		///		Obtiene una cadena con una condición de filtros de fechas
		/// </summary>
		/// <remarks>
		///		Dependiendo de las fechas pasadas al filtro devuelve una cadena del estilo:
		///			* strField BETWEEN strParameterStart AND strParameterEnd
		///			* strField &gt;= strParameterStart
		///			* strField &lt;= strParameterEnd
		/// </remarks>
		protected string GetSQLFilterDates(string strField, string strParameterStart, string strParameterEnd, 
																			 DateTime? dtmStart, DateTime? dtmEnd) 
		{ string strSQL = "";

				// Asigna la cadena SQL
					if (dtmStart != null && dtmEnd != null)
						strSQL += string.Format(" {0} BETWEEN {1} AND {2}", strField, strParameterStart, strParameterEnd);
					else if (dtmStart != null && dtmEnd == null)
						strSQL += string.Format(" {0} >= {1}", strField, strParameterStart);
					else if (dtmStart == null && dtmEnd != null)
						strSQL += string.Format(" {0} <= {1}", strField, strParameterEnd);
				// Devuelve la cadena SQL
					return strSQL;
		}

		/// <summary>
		///		Obtiene el SQL para una condición IN
		/// </summary>
		protected string GetSQLIn(List<int?> objColIDs)
		{ string strSQL = "";

				// Añade los IDs a la cadena
					foreach (int? intID in objColIDs)
						if (intID != null)
							{ // Añade una coma si es necesario
									if (!string.IsNullOrEmpty(strSQL))					
										strSQL += ",";
								// Añade el ID
									strSQL += intID.ToString();
							}
				// Devuelve la condición SQL	
					return strSQL;
		}

		/// <summary>
		///		Nombre del parámetro que se utiliza como ID
		/// </summary>
		protected abstract string NameParameterID { get; }

		/// <summary>
		///		Tipo de los datos
		/// </summary>
		protected string ProviderKey { get; private set; }

		/// <summary>
		///		Servicio de localización de repositorios
		/// </summary>
		protected ProviderServiceLocator Locator { get; private set; }
	}
}