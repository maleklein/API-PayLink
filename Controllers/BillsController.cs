using Microsoft.AspNetCore.Mvc;
using PayLink.Services;

namespace PayLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/bills
    public class BillsController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly ExternalApiService _externalApiService;

        public BillsController(IBusinessService businessService, ExternalApiService externalApiService)
        {
            _businessService = businessService;
            _externalApiService = externalApiService;
        }

        // GET: api/bills/{billId}?businessId=#
        // PayLink consulta la API del negocio para obtener los datos de esa factura
        [HttpGet("{billId}")]
        public async Task<ActionResult<object>> GetBillDetails(int billId, [FromQuery] int businessId)
        {
            var business = _businessService.GetById(businessId);
            if (business == null)
                return NotFound($"No se encontró el negocio con ID {businessId}");

            // Llamamos al servicio que consulta la API externa del negocio
            var billData = await _externalApiService.GetInvoiceFromBusinessAsync(
                business.ApiUrl,
                billId
            );
    
            if (billData == null)
                return StatusCode(502, "Error al consultar la API del negocio.");

            return Ok(billData);
        }
    }
}