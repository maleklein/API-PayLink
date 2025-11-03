namespace PayLink.Dto
{
    public class PaymentDto
    {
        public string TransactionId { get; set; }
        public int BusinessId { get; set; }
        public string FacturaId { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string BusinessNombre { get; set; }
    }
}
