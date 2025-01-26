using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class RelCategoriasUsuario
{
    [Key]
    public long CategoriaUsuarioId { get; set; }

    public long UsuarioId { get; set; }

    public int EstatusCategoriaId { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaModificacion { get; set; }

    [ForeignKey("EstatusCategoriaId")]
    [InverseProperty("RelCategoriasUsuarios")]
    public virtual EstatusCategoria EstatusCategoria { get; set; } = null!;

    [InverseProperty("CategoriaUsuario")]
    public virtual ICollection<RelApuesta> RelApuesta { get; set; } = new List<RelApuesta>();

    [ForeignKey("UsuarioId")]
    [InverseProperty("RelCategoriasUsuarios")]
    public virtual Usuario Usuario { get; set; } = null!;
}
