using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class RelSeguidore
{
    [Key]
    public long SeguidorId { get; set; }

    public long UsuarioSeguidorId { get; set; }

    public long UsuarioSeguidoId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    [ForeignKey("UsuarioSeguidoId")]
    [InverseProperty("RelSeguidoreUsuarioSeguidos")]
    public virtual Usuario UsuarioSeguido { get; set; } = null!;

    [ForeignKey("UsuarioSeguidorId")]
    [InverseProperty("RelSeguidoreUsuarioSeguidors")]
    public virtual Usuario UsuarioSeguidor { get; set; } = null!;
}
