using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class EstatusUsuario
{
    [Key]
    public int EstatusUsuarioId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("EstatusUsuario")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
