namespace BetTrackApi.Dtos
{
    public class DtoUsuarioTipster
    {
        public long UsuarioTipsterId { get; set; }
        public long UsuarioId { get; set; }
        public string NombreTipster { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
    }
}
