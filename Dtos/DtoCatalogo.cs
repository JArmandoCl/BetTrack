using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.Dtos
{
    internal class DtoEstatusApuesta
    {
        public int EstatusApuestaId { get; set; }
        public string Descripcion { get; set; } = "";
    }
    internal class DtoTipoApuesta
    {
        public int TipoApuestaId { get; set; }
        public string Nombre { get; set; } = "";
    }
    internal class DtoDeporte
    {
        public long DeporteId { get; set; }
        public string Nombre { get; set; } = "";
    }
    internal class DtoTipoBankroll
    {
        public int TipoBankrollId { get; set; }
        public string Nombre { get; set; } = "";
    }
    internal class DtoFormatoCuota
    {
        public int FormatoCuotaId { get; set; }
        public string Nombre { get; set; } = "";
    }
    internal class DtoEstatusBankroll
    {
        public int EstatusBankrollId { get; set; }
        public string Nombre { get; set; } = "";
    }
    internal class DtoEstatusUsuario
    {
        public int EstatusUsuarioId { get; set; }
        public string Nombre { get; set; } = "";
    }
    internal class DtoEstatusUsuarioCasino {
        public long EstatusUsuarioCasinoId { get; set; }
        public string Nombre { get; set; } = "";
    }
    internal class DtoEstatusCategoria
    {
        public int EstatusCategoriaId { get; set; }
        public string Nombre { get; set; } = "";
    }
    internal class DtoUsuario
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
    internal class DtoCasino
    {
        public long CasinoId { get; set; }
        public string Nombre { get; set; } = "";
        public string Icono { get; set; } = "";
    }
    internal class DtoUsuarioTipster
    {
        public long UsuarioTipsterId { get; set; }
        public long UsuarioId { get; set; }
        public string NombreTipster { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
    }
    internal class DtoUsuarioCasino
    {
        public long UsuarioCasinoId { get; set; }
        public long UsuarioId { get; set; }
        public long EstatusUsuarioCasinoId { get; set; }
        public int CasinoId { get; set; }
        public string Nombre { get; set; } = "";
        public string Icono { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
    }
    internal class DtoUsuarioBankroll
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
    internal class DtoCategoriaUsuario
    {
        public long CategoriaUsuarioId { get; set; }
        public long UsuarioId { get; set; }
        public int EstatusCategoriaId { get; set; }
        public string Nombre { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
    internal class DtoApuestas
    {
        public long ApuestaId { get; set; }
        public long UsuarioBankrollId { get; set; }
        public int TipoApuestaId { get; set; }
        public long UsuarioTipsterId { get; set; }
        public long CategoriaUsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Importe { get; set; }
        public decimal MontoCobrado { get; set; }
        public decimal Ganancia { get; set; }
        public bool ApuestaEnVivo { get; set; }
        public bool EsApuestaGratuita { get; set; }
        public bool EsApuestaPagada { get; set; }
        public decimal Cashout { get; set; }
    }
    internal class DtoDetalleApuesta
    {
        public long DetalleApuestaId { get; set; }
        public long ApuestaId { get; set; }
        public long DeporteId { get; set; }
        public int EstatusApuestaId { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Cuota { get; set; }
    }
    internal class DtoDepositoRetiro {
        public long DepositoRetiroId { get; set; }
        public long UsuarioBankrollId { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = "";
        public decimal Monto { get; set; }
    }
    internal class DtoSeguidor
    {
        public long SeguidorId { get; set; }
        public long UsuarioSeguidorId { get; set; }
        public long UsuarioSeguidoId { get; set; }
        public DateTime Fecha { get; set; }
    }
    }
