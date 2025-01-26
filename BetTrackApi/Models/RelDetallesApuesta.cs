using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class RelDetallesApuesta
{
    [Key]
    public long DetalleApuestaId { get; set; }

    public long ApuestaId { get; set; }

    public long DeporteId { get; set; }

    public int EstatusApuestaId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Column(TypeName = "decimal(19, 2)")]
    public decimal Cuota { get; set; }

    [ForeignKey("ApuestaId")]
    [InverseProperty("RelDetallesApuesta")]
    public virtual RelApuesta Apuesta { get; set; } = null!;

    [ForeignKey("DeporteId")]
    [InverseProperty("RelDetallesApuesta")]
    public virtual Deporte Deporte { get; set; } = null!;

    [ForeignKey("EstatusApuestaId")]
    [InverseProperty("RelDetallesApuesta")]
    public virtual EstatusApuesta EstatusApuesta { get; set; } = null!;
}
