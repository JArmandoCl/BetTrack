using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class FormatosCuota
{
    [Key]
    public int FormatoCuotaId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("FormatoCuota")]
    public virtual ICollection<RelUsuarioBankroll> RelUsuarioBankrolls { get; set; } = new List<RelUsuarioBankroll>();
}
