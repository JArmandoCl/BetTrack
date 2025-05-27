using BetTrack.Resources.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

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
        public string NombreEsp { get; set; } = "";
        public string NombreIng { get; set; } = "";
        public string Nombre { get; set; } = "";
    }
    public class DtoTipoBankroll : BindableBase
    {
        private int tipoBankrollId;
        public int TipoBankrollId
        {
            get { return tipoBankrollId; }
            set { SetProperty(ref tipoBankrollId, value); }
        }
        private string nombre = "";
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
        [EmailAddress(ErrorMessageResourceType = typeof(AppResource),
            ErrorMessageResourceName = nameof(AppResource.LblUserEmailRequired))]
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }
        private string contrasenia = "";
        [Required(
           ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblUserPasswordRequired))]
        public string Contrasenia
        {
            get { return contrasenia; }
            set { SetProperty(ref contrasenia, value); }
        }
        private string nickname = "";

        [Required(
           ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblNicknameRequired))]
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

        [Required(
           ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblBankrollNameRequired))]
        public string NombreBankroll { get; set; } = "";

        [Range(0.01, double.MaxValue,
             ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblInitialCapitalRequired))]
        public decimal CapitalInicial { get; set; }
        public int EstatusBankrollId { get; set; } = 1;
        private int formatoCuotaId = 1;
        [Range(1, long.MaxValue,
            ErrorMessageResourceType = typeof(AppResource),
          ErrorMessageResourceName = nameof(AppResource.LblFormatoCuotaRequired))]
        public int FormatoCuotaId
        {
            get { return formatoCuotaId; }
            set { SetProperty(ref formatoCuotaId, value); }
        }
        private int tipoBankrollId = 1;
        [Range(1, long.MaxValue,
            ErrorMessageResourceType = typeof(AppResource),
          ErrorMessageResourceName = nameof(AppResource.LblTipoBankrollRequired))]
        public int TipoBankrollId
        {
            get { return tipoBankrollId; }
            set { SetProperty(ref tipoBankrollId, value); }
        }
        private int monedaId = 16;
        [Range(1, long.MaxValue,
            ErrorMessageResourceType = typeof(AppResource),
          ErrorMessageResourceName = nameof(AppResource.LblMonedaRequired))]
        public int MonedaId
        {
            get { return monedaId; }
            set { SetProperty(ref monedaId, value); }
        }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        #region Extras
        private List<DtoFormatoCuota> formatoCuotas = new List<DtoFormatoCuota>();
        public List<DtoFormatoCuota> FormatoCuotas
        {
            get { return formatoCuotas; }
            set { SetProperty(ref formatoCuotas, value); }
        }
        private List<DtoTipoBankroll> tiposBankroll = new List<DtoTipoBankroll>();
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
        private DtoMoneda moneda = new DtoMoneda();
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
        public DtoTipoBankroll TipoBankroll
        {
            get { return tipoBankroll; }
            set { SetProperty(ref tipoBankroll, value); }
        }
        #endregion
    }
    public class DtoCategoriaUsuario : BindableBase
    {
        public long CategoriaUsuarioId { get; set; }
        public long UsuarioId { get; set; }
        public int EstatusCategoriaId { get; set; } = 1;
        private string nombre = "";
        public string Nombre
        {
            get { return nombre; }
            set { SetProperty(ref nombre, value); }
        }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
    public class DtoApuesta : BindableBase
    {
        public long ApuestaId { get; set; }
        public long UsuarioBankrollId { get; set; }
        public int TipoApuestaId { get; set; }
        [Range(1, long.MaxValue,
             ErrorMessageResourceType = typeof(AppResource),
            ErrorMessageResourceName = nameof(AppResource.LblTipsterRequired))]
        public long UsuarioTipsterId { get; set; }
        [Range(1, long.MaxValue,
             ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblCategoryRequired))]
        public long CategoriaUsuarioId { get; set; }
        [Range(1, long.MaxValue,
             ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblCasinoRequired))]
        public long UsuarioCasinoId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Required(
           ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblBetNameRequired))]
        public string Nombre { get; set; } = "";
        [Range(0.01, double.MaxValue,
             ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblAmountBetRequired))]
        public decimal Importe { get; set; }
        public decimal MontoCobrado { get; set; }
        public decimal Ganancia { get; set; }
        public bool EsApuestaGratuita { get; set; }
        public bool EsApuestaPagada { get; set; }
        public decimal Cashout { get; set; }
        #region Extras
        private List<DtoUsuarioCasino> userCasinos = new List<DtoUsuarioCasino>();
        [JsonIgnore]
        public List<DtoUsuarioCasino> UserCasinos
        {
            get { return userCasinos; }
            set { SetProperty(ref userCasinos, value); }
        }
        private List<DtoUsuarioTipster> tipsters = new List<DtoUsuarioTipster>();
        [JsonIgnore]
        public List<DtoUsuarioTipster> Tipsters
        {
            get { return tipsters; }
            set { SetProperty(ref tipsters, value); }
        }
        private List<DtoCategoriaUsuario> categorias = new List<DtoCategoriaUsuario>();
        [JsonIgnore]
        public List<DtoCategoriaUsuario> Categorias
        {
            get { return categorias; }
            set { SetProperty(ref categorias, value); }
        }
        private List<DtoTipoApuesta> tiposApuesta = new List<DtoTipoApuesta>();
        [JsonIgnore]
        public List<DtoTipoApuesta> TiposApuesta
        {
            get { return tiposApuesta; }
            set { SetProperty(ref tiposApuesta, value); }
        }
        private DtoDetalleApuesta detalleApuesta = new DtoDetalleApuesta();
        public DtoDetalleApuesta DetalleApuesta
        {
            get { return detalleApuesta; }
            set { SetProperty(ref detalleApuesta, value); }
        }
        #endregion
    }
    public class DtoDetalleApuesta : BindableBase
    {
        public long DetalleApuestaId { get; set; }
        public long ApuestaId { get; set; }
        [Range(1, long.MaxValue,
             ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblSportRequired))]
        public long DeporteId { get; set; }
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblBetStatusRequired))]
        public int EstatusApuestaId { get; set; }
        public string Nombre { get; set; } = "";
        [Range(0.01, double.MaxValue,
             ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblOddRequired))]
        public decimal Cuota { get; set; }
        public bool PagoAnticipado { get; set; }
        public bool ApuestaEnVivo { get; set; }
        #region Extras
        [JsonIgnore]
        private List<DtoDeporte> deportes = new List<DtoDeporte>();
        public List<DtoDeporte> Deportes
        {
            get { return deportes; }
            set { SetProperty(ref deportes, value); }
        }
        [JsonIgnore]
        private List<DtoEstatusApuesta> estatusApuestas;
        public List<DtoEstatusApuesta> EstatusApuesta
        {
            get { return estatusApuestas; }
            set { SetProperty(ref estatusApuestas, value); }
        }
        #endregion
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
    public class DtoReestablecerContrasenia:BindableBase
    {
        private string email = "";

        [EmailAddress(
           ErrorMessageResourceType = typeof(AppResource),
           ErrorMessageResourceName = nameof(AppResource.LblUserEmailRequired))]
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }
        public string Token { get; set; } = "";
        public string NewPassword { get; set; } = "-";
        public string NewConfirmedPassword { get; set; } = "-";
    }
    #endregion
}
