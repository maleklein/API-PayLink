using PayLink.Data;
using PayLink.Models;

namespace PayLink.Services
{
    // Implementa la interfaz IPaymentService, con la lógica real.
    public class PaymentService : IPaymentService
    {
        private readonly PayLinkDbContext _context;

        public PaymentService(PayLinkDbContext context)
        {
            _context = context;
        }

        // Devuelve todos los pagos registrados.
        public IEnumerable<Payment> GetAll()
        {
            return _context.Payments.ToList();
        }

        // Busca un pago específico por su ID.
        public Payment? GetById(int id)
        {
            return _context.Payments.Find(id);
        }

        // Busca un pago por TransactionId
        public Payment? GetByTransactionId(string transactionId)
        {
            return _context.Payments.FirstOrDefault(p => p.TransactionId == transactionId);
        }

        // Devuelve todos los pagos asociados a una factura (FacturaId)
        public IEnumerable<Payment> GetByBillId(string billId)
        {
            return _context.Payments
                           .Where(p => p.FacturaId == billId)
                           .ToList();
        }

        // Crea un nuevo pago en la base de datos.

        // ✅ Crea un nuevo pago en la base de datos
        public Payment Create(Payment payment, string apiKey)
        {
            // Validar que la API Key corresponda a un negocio válido
            var business = _context.Businesses.FirstOrDefault(b => b.ApiKey == apiKey);
            if (business == null)
                throw new Exception("API Key inválida. Negocio no autorizado.");

            // Validar datos del pago
            if (string.IsNullOrWhiteSpace(payment.FacturaId))
                throw new Exception("El código de factura (FacturaId) es obligatorio.");

            if (string.IsNullOrWhiteSpace(payment.TransactionId))
                throw new Exception("El código de transacción (TransactionId) es obligatorio.");

            if (payment.Monto <= 0)
                throw new Exception("El monto debe ser mayor que 0.");

            // 3️⃣ Asignar datos automáticos
            payment.Fecha = DateTime.Now;
            payment.Estado = "Confirmado";
            payment.BusinessId = business.Id;

            _context.Payments.Add(payment);
            _context.SaveChanges();

            return payment;
        }
    }
}
