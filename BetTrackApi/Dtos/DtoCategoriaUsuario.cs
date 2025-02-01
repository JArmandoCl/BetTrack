namespace BetTrackApi.Dtos
{
    public class DtoCategoriaUsuario
    {
        public long CategoriaUsuarioId { get; set; }
        public long UsuarioId { get; set; }
        public int EstatusCategoriaId { get; set; }
        public string Nombre { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
