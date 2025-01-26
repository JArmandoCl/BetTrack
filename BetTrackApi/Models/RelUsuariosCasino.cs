using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class RelUsuariosCasino
{
    [Key]
    public long UsuarioCasinoId { get; set; }

    public long UsuarioId { get; set; }

    public long EstatusUsuarioCasinoId { get; set; }

    public int? CasinoId { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Icono { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [ForeignKey("EstatusUsuarioCasinoId")]
    [InverseProperty("RelUsuariosCasinos")]
    public virtual EstatusUsuariosCasino EstatusUsuarioCasino { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("RelUsuariosCasinos")]
    public virtual Usuario Usuario { get; set; } = null!;
}
