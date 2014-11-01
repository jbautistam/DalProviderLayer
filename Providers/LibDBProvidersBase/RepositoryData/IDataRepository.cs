using System;

namespace Bau.Libraries.LibDBProvidersBase.RepositoryData
{
	/// <summary>
	///		Interface genérico para las clases de Repository
	/// </summary>
	public interface IDataRepository
	{
		/// <summary>
		///		Carga un registro a partir de su ID
		/// </summary>
		TypeData Load<TypeData>(int? intID) where TypeData : new();

		/// <summary>
		///		Graba un registro
		/// </summary>
		void Save(object objData);

		/// <summary>
		///		Comprueba si puede borrar un objeto
		/// </summary>
		bool CanDelete(int? intID);

		/// <summary>
		///		Borra un objeto
		/// </summary>
		void Delete(int? intID);
	}
}
