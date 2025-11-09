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
            try
            {
                // Mapeás manualmente el DTO a tu entidad Business
                var business = new Business
                {
                    Nombre = dto.Nombre,
                    Cuit = dto.Cuit,
                    ApiUrl = dto.ApiUrl,
                };

                var newBusiness = _businessService.Create(business);

                // Devuelve 201 (Created) con la ubicación del nuevo recurso
                return CreatedAtAction(nameof(GetById), new { id = newBusiness.Id }, newBusiness);
            }
            catch (Exception ex)
            {
                // Si el servicio lanza un error (por ejemplo, CUIT duplicado), devolver 400
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")] // PUT: api/business/{id}
        public IActionResult Update(int id, BusinessCreateDto dto)
        {
            var business = new Business
            {
                Nombre = dto.Nombre,
                Cuit = dto.Cuit,
                ApiUrl = dto.ApiUrl
            };

            var updated = _businessService.Update(id, business);

            if (updated == null)
                return NotFound(); // 404 si no existe

            return NoContent(); // 204 si se actualiza correctamente
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var deleted = _businessService.Delete(id);
                if (!deleted)
                    return NotFound(); // 404 si no existe

                return NoContent(); // 204 si se elimina correctamente
            }
            catch (Exception ex)
            {
                // ⚠ Si tiene pagos, devuelve 400 con el mensaje del servicio
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
