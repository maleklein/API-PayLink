using PayLink.Data;   
using PayLink.Models;  

namespace PayLink.Services
{
    public class BusinessService : IBusinessService // Implementa la interfaz IBusinessService.
    {
        private readonly PayLinkDbContext _context; // Acceso a la base de datos.

        public BusinessService(PayLinkDbContext context) // Constructor que recibe el contexto por inyección de dependencias.
        {
            _context = context;
        }

        public IEnumerable<Business> GetAll() // Devuelve todos los negocios.
        {
            return _context.Businesses.ToList();
        }

        public Business? GetById(int id) // Busca un negocio por su ID.
        {
            return _context.Businesses.Find(id);
        }

        public Business Create(Business business) // Crea un nuevo negocio.
        {
            _context.Businesses.Add(business);
            _context.SaveChanges(); // Guarda los cambios en la base.
            return business;
        }

        public Business? Update(int id, Business updated) // Actualiza un negocio.
        {
            var existing = _context.Businesses.Find(id); // Busca el negocio.
            if (existing == null) return null; // Si no existe, devuelve null.

            // Actualiza los datos.
            existing.Nombre = updated.Nombre;
            existing.Cuit = updated.Cuit;
            existing.ApiUrl = updated.ApiUrl;
            existing.ApiKey = updated.ApiKey;
            _context.SaveChanges(); // Guarda los cambios.

            return existing; // Devuelve el negocio actualizado.
        }

        public bool Delete(int id) // Elimina un negocio.
        {
            var business = _context.Businesses.Find(id); // Busca por ID.
            if (business == null) return false; // Si no existe, devuelve false.

            _context.Businesses.Remove(business); // Lo elimina.
            _context.SaveChanges(); // Guarda los cambios.
            return true; // Confirma la eliminación.
        }
    }
}
