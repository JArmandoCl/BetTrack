using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class RelUsuarioBankroll
{
    [Key]
    public long UsuarioBankrollId { get; set; }

    public long UsuarioId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string NombreBankroll { get; set; } = null!;

    [Column(TypeName = "decimal(19, 2)")]
    public decimal CapitalInicial { get; set; }

    public int EstatusBankrollId { get; set; }

    public int FormatoCuotaId { get; set; }

    public int TipoBankrollId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaModificacion { get; set; }

    [ForeignKey("EstatusBankrollId")]
    [InverseProperty("RelUsuarioBankrolls")]
    public virtual EstatusBankroll EstatusBankroll { get; set; } = null!;

    [ForeignKey("FormatoCuotaId")]
    [InverseProperty("RelUsuarioBankrolls")]
    public virtual FormatosCuota FormatoCuota { get; set; } = null!;

    [InverseProperty("UsuarioBankroll")]
    public virtual ICollection<RelApuesta> RelApuesta { get; set; } = new List<RelApuesta>();

    [InverseProperty("UsuarioBankroll")]
    public virtual ICollection<RelDepositosRetiro> RelDepositosRetiros { get; set; } = new List<RelDepositosRetiro>();

    [ForeignKey("TipoBankrollId")]
    [InverseProperty("RelUsuarioBankrolls")]
    public virtual TiposBankroll TipoBankroll { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("RelUsuarioBankrolls")]
    public virtual Usuario Usuario { get; set; } = null!;
}
