namespace BetTrackApi.Dtos
{
    public class DtoUsuarioCasino
    {
        public long UsuarioCasinoId { get; set; }
        public long UsuarioId { get; set; }
        public long EstatusUsuarioCasinoId { get; set; }
        public int CasinoId { get; set; }
        public string Nombre { get; set; } = "";
        public string Icono { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
    }
}
