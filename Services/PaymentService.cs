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
        public Payment Create(Payment payment)
        {
            // Verifica que el negocio asociado exista.
            var business = _context.Businesses.Find(payment.BusinessId);
            if (business == null)
                throw new Exception($"El negocio con ID {payment.BusinessId} no existe.");

            // Valida el formato de los datos del pago.
            if (string.IsNullOrWhiteSpace(payment.FacturaId))
                throw new Exception("El código de factura (FacturaId) es obligatorio.");

            if (payment.Monto <= 0)
                throw new Exception("El monto debe ser mayor que 0.");

            if (payment.Fecha == default)
                throw new Exception("La fecha del pago no es válida.");

            var estadosValidos = new[] { "Aprobado", "Rechazado", "Pendiente" };
                if (!estadosValidos.Contains(payment.Estado))
                    throw new Exception("El estado del pago debe ser: Aprobado, Rechazado o Pendiente.");

             // Generar un TransactionId único si no se envió
            if (string.IsNullOrWhiteSpace(payment.TransactionId))
                payment.TransactionId = Guid.NewGuid().ToString();
         
            _context.Payments.Add(payment); // Guarda el pago.
            _context.SaveChanges();

            return payment;
        }
    }
}
