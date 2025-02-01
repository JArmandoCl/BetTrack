namespace BetTrackApi.Dtos
{
    public class DtoDepositoRetiro
    {
        public long DepositoRetiroId { get; set; }
        public long UsuarioBankrollId { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = "";
        public decimal Monto { get; set; }
        public bool EsRetiro { get; set; }
    }
}
