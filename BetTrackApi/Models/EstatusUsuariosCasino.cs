using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class EstatusUsuariosCasino
{
    [Key]
    public long EstatusUsuarioCasinoId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("EstatusUsuarioCasino")]
    public virtual ICollection<RelUsuariosCasino> RelUsuariosCasinos { get; set; } = new List<RelUsuariosCasino>();
}
