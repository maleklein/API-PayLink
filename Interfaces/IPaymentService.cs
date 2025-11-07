using PayLink.Models;

namespace PayLink.Services
{
    // Define las operaciones que el servicio de pagos debe cumplir.
    public interface IPaymentService
    {
        IEnumerable<Payment> GetAll(); // Devuelve todos los pagos registrados.
        Payment? GetById(int id);      // Busca un pago por su ID interno.
        Payment Create(Payment payment, string apiKey); // Registra un nuevo pago.
        Payment? GetByTransactionId(string transactionId);
        IEnumerable<Payment> GetByBillId(string billId);
    }
}
