using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class TiposBankroll
{
    [Key]
    public int TipoBankrollId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("TipoBankroll")]
    public virtual ICollection<RelUsuarioBankroll> RelUsuarioBankrolls { get; set; } = new List<RelUsuarioBankroll>();
}
