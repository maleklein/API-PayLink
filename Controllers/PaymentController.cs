using Microsoft.AspNetCore.Mvc;
using PayLink.Models;
using PayLink.Services;
using PayLink.Dto;

namespace PayLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // La ruta será: api/payments
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        // El controlador recibe el servicio por inyección de dependencias.
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/payments
        [HttpGet]
        public ActionResult<IEnumerable<Payment>> GetAll()
        {
            return Ok(_paymentService.GetAll());
        }

        // GET: api/payments/{id}
        [HttpGet("{id}")]
        public ActionResult<Payment> GetById(int id)
        {
            var payment = _paymentService.GetById(id);
            if (payment == null)
                return NotFound($"No se encontró el pago con ID {id}");
            return Ok(payment);
        }

        // POST: api/payments
        [HttpPost] // POST: api/payments
        public ActionResult<Payment> Create(PaymentDto dto)
        {
            try
            {
                // Creamos el objeto Payment usando los datos del DTO
                var payment = new Payment
                {
                    TransactionId = dto.TransactionId,
                    FacturaId = dto.FacturaId,
                    Monto = dto.Monto,
                    Fecha = dto.Fecha,
                    Estado = dto.Estado,
                    BusinessId = dto.BusinessId 
                };

                // Llamamos al método que ya tenés en el servicio
                var newPayment = _paymentService.Create(payment);

                // Devolvemos el resultado como antes
                return CreatedAtAction(nameof(GetById), new { id = newPayment.Id }, newPayment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // ✅ GET: api/payments/transaction/{transactionId}
        // Consulta el estado de un pago usando el ID de transacción (único por negocio)
        [HttpGet("transaction/{transactionId}")]
        public ActionResult<Payment> GetByTransactionId(string transactionId)
        {
            var payment = _paymentService.GetByTransactionId(transactionId);
            if (payment == null)
                return NotFound($"No se encontró ningún pago con el Transaction ID: {transactionId}");
            return Ok(payment);
        }

        // ✅ GET: api/payments/bill/{billId}
        // Devuelve todos los pagos asociados a una factura específica
        [HttpGet("bill/{billId}")]
        public ActionResult<IEnumerable<Payment>> GetByBillId(string billId)
        {
            var payments = _paymentService.GetByBillId(billId);
            if (payments == null || !payments.Any())
                return NotFound($"No se encontraron pagos asociados a la factura {billId}");
            return Ok(payments);
        }
        

    }
}
