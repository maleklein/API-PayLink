namespace PayLink.Models
{
    public class Business
    {
        public int Id { get; set; }             // Identificador único en la base de datos
        public string Nombre { get; set; }      // Nombre del negocio (Ej: Tienda Luna)
        public string Cuit { get; set; }        // CUIT del negocio
        public string ApiUrl { get; set; }      // URL base de su API (para consultar facturas)
        public string ApiKey { get; set; }      // Clave de autenticación del negocio

        // Relación 1:N → un negocio puede tener muchos pagos
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
