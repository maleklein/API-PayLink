using PayLink.Models;

namespace PayLink.Services
{
    // Defino la interfaz (contrato) para los servicios de negocios.
    public interface IBusinessService
    {
        IEnumerable<Business> GetAll(); // Devuelve todos los negocios.
        Business? GetById(int id);      // Busca un negocio por su ID.
        Business Create(Business business); // Crea un nuevo negocio.
        Business? Update(int id, Business business); // Actualiza un negocio existente.
        bool Delete(int id);            // Elimina un negocio por su ID.
    }
}
