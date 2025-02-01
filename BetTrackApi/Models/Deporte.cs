using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class Deporte
{
    [Key]
    public long DeporteId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string NombreEsp { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string NombreIng { get; set; } = null!;

    [InverseProperty("Deporte")]
    public virtual ICollection<RelDetallesApuesta> RelDetallesApuesta { get; set; } = new List<RelDetallesApuesta>();
}
