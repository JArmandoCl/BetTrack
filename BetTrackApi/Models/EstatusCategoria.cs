using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class EstatusCategoria
{
    [Key]
    public int EstatusCategoriaId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("EstatusCategoria")]
    public virtual ICollection<RelCategoriasUsuario> RelCategoriasUsuarios { get; set; } = new List<RelCategoriasUsuario>();
}
