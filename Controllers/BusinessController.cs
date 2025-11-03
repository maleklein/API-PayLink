using Microsoft.AspNetCore.Mvc; 
using PayLink.Models;
using PayLink.Services; 
using PayLink.Dto;       

namespace PayLink.Controllers
{
    [ApiController] // Indica que esta clase es un controlador de API.
    [Route("api/[controller]")] // Define la ruta base: api/business
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService; // Servicio para manejar los negocios.

        public BusinessController(IBusinessService businessService) // Constructor que recibe el servicio por inyección de dependencias.
        {
            _businessService = businessService;
        }

        [HttpGet] // GET: api/business
        public ActionResult<IEnumerable<Business>> GetAll()
        {
            return Ok(_businessService.GetAll()); // Devuelve todos los negocios.
        }

        [HttpGet("{id}")] // GET: api/business/{id}
        public ActionResult<Business> GetById(int id)
        {
            var business = _businessService.GetById(id); // Busca por ID.
            if (business == null)
                return NotFound(); // Si no existe, devuelve 404.
            return Ok(business); // Devuelve el negocio encontrado.
        }

        [HttpPost] // POST: api/business
        public ActionResult<Business> Create(BusinessCreateDto dto)
        {
            // Mapeás manualmente el DTO a tu entidad Business
            var business = new Business
            {
                Nombre = dto.Nombre,
                Cuit = dto.Cuit,
                ApiUrl = dto.ApiUrl,
                ApiKey = dto.ApiKey
            };

            var newBusiness = _businessService.Create(business);

            // Devuelve 201 (Created) con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetById), new { id = newBusiness.Id }, newBusiness);
        }

        [HttpPut("{id}")] // PUT: api/business/{id}
        public IActionResult Update(int id, BusinessCreateDto dto)
        {
            var business = new Business
            {
                Nombre = dto.Nombre,
                Cuit = dto.Cuit,
                ApiUrl = dto.ApiUrl,
                ApiKey = dto.ApiKey
            };

            var updated = _businessService.Update(id, business);

            if (updated == null)
                return NotFound(); // 404 si no existe

            return NoContent(); // 204 si se actualiza correctamente
        }


        [HttpDelete("{id}")] // DELETE: api/business/{id}
        public IActionResult Delete(int id)
        {
            var deleted = _businessService.Delete(id); // Elimina el negocio.
            if (!deleted)
                return NotFound(); // Si no existe, devuelve 404.
            return NoContent(); // Si se elimina, devuelve 204.
        }
    }
}
