using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class EstatusApuesta
{
    [Key]
    public int EstatusApuestaId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Descripcion { get; set; } = null!;

    [InverseProperty("EstatusApuesta")]
    public virtual ICollection<RelDetallesApuesta> RelDetallesApuesta { get; set; } = new List<RelDetallesApuesta>();
}
