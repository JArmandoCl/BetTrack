using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.Dtos
{
    public class DtoEstatusApuesta
    {
        public int EstatusApuestaId { get; set; }
        public string Descripcion { get; set; } = "";
    }
    public class DtoTipoApuesta
    {
        public int TipoApuestaId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoDeporte
    {
        public long DeporteId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoTipoBankroll:BindableBase
    {
        private int tipoBankrollId;
        public int TipoBankrollId
        {
            get { return tipoBankrollId; }
            set { SetProperty(ref tipoBankrollId, value); }
        }
        private string nombre="";
        public string Nombre
        {
            get { return nombre; }
            set { SetProperty(ref nombre, value); }
        }
    }
    public class DtoFormatoCuota
    {
        public int FormatoCuotaId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoMoneda
    {
        public int MonedaId { get; set; }
        public string Descripcion { get; set; } = "";
    }
    public class DtoEstatusBankroll
    {
        public int EstatusBankrollId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoEstatusUsuario
    {
        public int EstatusUsuarioId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoEstatusUsuarioCasino
    {
        public long EstatusUsuarioCasinoId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoEstatusCategoria
    {
        public int EstatusCategoriaId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoUsuario : BindableBase, ICloneable
    {
        public long UsuarioId { get; set; }
        public int EstatusUsuarioId { get; set; } = 1;//Active
        private string email = "";
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }
        private string contrasenia = "";
        public string Contrasenia
        {
            get { return contrasenia; }
            set { SetProperty(ref contrasenia, value); }
        }
        private string nickname = "";
        public string Nickname
        {
            get { return nickname; }
            set { SetProperty(ref nickname, value); }
        }
        private string nombre = "";
        public string Nombre
        {
            get { return nombre; }
            set { SetProperty(ref nombre, value); }
        }
        private string pensamiento = "";
        public string Pensamiento
        {
            get { return pensamiento; }
            set { SetProperty(ref pensamiento, value); }
        }
        private DateTime fechaRegistro;
        public DateTime FechaRegistro
        {
            get { return fechaRegistro; }
            set { SetProperty(ref fechaRegistro, value); }
        }
        private string fotoUsuario = "default_user_profile.png";
        public string FotoUsuario
        {
            get { return fotoUsuario; }
            set { SetProperty(ref fotoUsuario, value); }
        }
        #region Extras
        public string CurrentToken { get; set; } = "";

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
    public class DtoCasino
    {
        public long CasinoId { get; set; }
        public string Nombre { get; set; } = "";
        public string Icono { get; set; } = "";
    }
    public class DtoUsuarioTipster
    {
        public long UsuarioTipsterId { get; set; }
        public long UsuarioId { get; set; }
        public string NombreTipster { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
    }
    public class DtoUsuarioCasino
    {
        public long UsuarioCasinoId { get; set; }
        public long UsuarioId { get; set; }
        public long EstatusUsuarioCasinoId { get; set; }
        public long CasinoId { get; set; }
        public string Nombre { get; set; } = "";
        public string Icono { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
    }
    public class DtoUsuarioBankroll : BindableBase
    {
        public long UsuarioBankrollId { get; set; }
        public long UsuarioId { get; set; }
        public string NombreBankroll { get; set; } = "";
        public decimal? CapitalInicial { get; set; } = null;
        public int EstatusBankrollId { get; set; } = 1;
        private int formatoCuotaId = 1;
        public int FormatoCuotaId
        {
            get { return formatoCuotaId; }
            set { SetProperty(ref formatoCuotaId, value); }
        }
        private int tipoBankrollId = 1;
        public int TipoBankrollId
        {
            get { return tipoBankrollId; }
            set { SetProperty(ref tipoBankrollId, value); }
        }
        private int monedaId = 1;
        public int MonedaId
        {
            get { return monedaId; }
            set { SetProperty(ref monedaId, value); }
        }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        #region Extras
        private List<DtoFormatoCuota> formatoCuotas=new List<DtoFormatoCuota>();
        public List<DtoFormatoCuota> FormatoCuotas
        {
            get { return formatoCuotas; }
            set { SetProperty(ref formatoCuotas, value); }
        }
        private List<DtoTipoBankroll> tiposBankroll=new List<DtoTipoBankroll>();
        public List<DtoTipoBankroll> TiposBankroll
        {
            get { return tiposBankroll; }
            set { SetProperty(ref tiposBankroll, value); }
        }
        private List<DtoMoneda> monedas = new List<DtoMoneda>();
        public List<DtoMoneda> Monedas
        {
            get { return monedas; }
            set { SetProperty(ref monedas, value); }
        }
        private DtoMoneda moneda=new DtoMoneda();
        public DtoMoneda Moneda
        {
            get { return moneda; }
            set { SetProperty(ref moneda, value); }
        }
        private DtoFormatoCuota formatoCuota = new DtoFormatoCuota();
        public DtoFormatoCuota FormatoCuota
        {
            get { return formatoCuota; }
            set { SetProperty(ref formatoCuota, value); }
        }
        private DtoTipoBankroll tipoBankroll = new DtoTipoBankroll();
        public DtoTipoBankroll  TipoBankroll
        {
            get { return tipoBankroll; }
            set { SetProperty(ref tipoBankroll, value); }
        }
        #endregion
    }
    public class DtoCategoriaUsuario
    {
        public long CategoriaUsuarioId { get; set; }
        public long UsuarioId { get; set; }
        public int EstatusCategoriaId { get; set; }
        public string Nombre { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
    public class DtoApuestas
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
    public class DtoDetalleApuesta
    {
        public long DetalleApuestaId { get; set; }
        public long ApuestaId { get; set; }
        public long DeporteId { get; set; }
        public int EstatusApuestaId { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Cuota { get; set; }
    }
    public class DtoDepositoRetiro
    {
        public long DepositoRetiroId { get; set; }
        public long UsuarioBankrollId { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = "";
        public decimal Monto { get; set; }
    }
    public class DtoSeguidor
    {
        public long SeguidorId { get; set; }
        public long UsuarioSeguidorId { get; set; }
        public long UsuarioSeguidoId { get; set; }
        public DateTime Fecha { get; set; }
    }
    #region Extras
    public class DtoReestablecerContrasenia
    {
        public string Email { get; set; } = "";
        public string Token { get; set; } = "";
        public string NewPassword { get; set; } = "-";
        public string NewConfirmedPassword { get; set; } = "-";
    }
    #endregion
}
