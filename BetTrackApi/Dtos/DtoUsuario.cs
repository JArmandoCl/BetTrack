namespace BetTrackApi.Dtos
{
    public class DtoUsuario
    {
        public long UsuarioId { get; set; }
        public int EstatusUsuarioId { get; set; }
        public string Email { get; set; } = "";
        public string Contrasenia { get; set; } = "";
        public string Nickname { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Alias { get; set; } = "";
        public string Pensamiento { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
    }

  public  class DtoSeguidor
    {
        public long SeguidorId { get; set; }
        public long UsuarioSeguidorId { get; set; }
        public long UsuarioSeguidoId { get; set; }
        public DateTime Fecha { get; set; }
    }
}
