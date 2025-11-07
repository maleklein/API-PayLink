namespace PayLink.Dto
{
    public class PaymentDto
    {
        public string TransactionId { get; set; }
        public string FacturaId { get; set; }
        public decimal Monto { get; set; }
    }
}
