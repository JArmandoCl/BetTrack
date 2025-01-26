using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class TiposApuesta
{
    [Key]
    public int TipoApuestaId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("TipoApuesta")]
    public virtual ICollection<RelApuesta> RelApuesta { get; set; } = new List<RelApuesta>();
}
