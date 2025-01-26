using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class RelUsuarioTipster
{
    [Key]
    public long UsuarioTipsterId { get; set; }

    public long UsuarioId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string NombreTipster { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [InverseProperty("UsuarioTipster")]
    public virtual ICollection<RelApuesta> RelApuesta { get; set; } = new List<RelApuesta>();

    [ForeignKey("UsuarioId")]
    [InverseProperty("RelUsuarioTipsters")]
    public virtual Usuario Usuario { get; set; } = null!;
}
