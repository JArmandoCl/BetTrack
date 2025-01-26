using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class RelApuesta
{
    [Key]
    public long ApuestaId { get; set; }

    public long UsuarioBankrollId { get; set; }

    public int TipoApuestaId { get; set; }

    public long UsuarioTipsterId { get; set; }

    public long CategoriaUsuarioId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Column(TypeName = "decimal(19, 2)")]
    public decimal Importe { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal MontoCobrado { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal Ganancia { get; set; }

    public bool ApuestaEnVivo { get; set; }

    public bool EsApuestaGratuita { get; set; }

    public bool EsApuestaPagada { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal Cashout { get; set; }

    [ForeignKey("CategoriaUsuarioId")]
    [InverseProperty("RelApuesta")]
    public virtual RelCategoriasUsuario CategoriaUsuario { get; set; } = null!;

    [InverseProperty("Apuesta")]
    public virtual ICollection<RelDetallesApuesta> RelDetallesApuesta { get; set; } = new List<RelDetallesApuesta>();

    [ForeignKey("TipoApuestaId")]
    [InverseProperty("RelApuesta")]
    public virtual TiposApuesta TipoApuesta { get; set; } = null!;

    [ForeignKey("UsuarioBankrollId")]
    [InverseProperty("RelApuesta")]
    public virtual RelUsuarioBankroll UsuarioBankroll { get; set; } = null!;

    [ForeignKey("UsuarioTipsterId")]
    [InverseProperty("RelApuesta")]
    public virtual RelUsuarioTipster UsuarioTipster { get; set; } = null!;
}
