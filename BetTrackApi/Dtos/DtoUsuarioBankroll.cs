namespace BetTrackApi.Dtos
{
    public class DtoUsuarioBankroll
    {
        public long UsuarioBankrollId { get; set; }
        public long UsuarioId { get; set; }
        public string NombreBankroll { get; set; } = "";
        public decimal CapitalInicial { get; set; }
        public int EstatusBankrollId { get; set; }
        public int FormatoCuotaId { get; set; }
        public int TipoBankrollId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
