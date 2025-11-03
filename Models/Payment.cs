using System.Text.Json.Serialization;

namespace PayLink.Models
{
    public class Payment
    {
        public int Id { get; set; }                  // Clave primaria interna para EF y la base de datos
        public string TransactionId { get; set; }    // ID único del pago 
        public int BusinessId { get; set; }          // Relación con el negocio
        public string FacturaId { get; set; }        // Código o número de factura
        public decimal Monto { get; set; }           // Monto del pago
        public DateTime Fecha { get; set; }          // Fecha y hora del pago
        public string Estado { get; set; }           // "Aprobado", "Pendiente", "Rechazado", etc.

        [JsonIgnore]
        public Business Business { get; set; }      // Relación con el negocio (FK)
    }
}
