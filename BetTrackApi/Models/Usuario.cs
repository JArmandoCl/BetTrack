using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class Usuario
{
    [Key]
    public long UsuarioId { get; set; }

    public int EstatusUsuarioId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Contrasenia { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Nickname { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Alias { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Pensamiento { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [ForeignKey("EstatusUsuarioId")]
    [InverseProperty("Usuarios")]
    public virtual EstatusUsuario EstatusUsuario { get; set; } = null!;

    [InverseProperty("Usuario")]
    public virtual ICollection<RelCategoriasUsuario> RelCategoriasUsuarios { get; set; } = new List<RelCategoriasUsuario>();

    [InverseProperty("UsuarioSeguidor")]
    public virtual ICollection<RelSeguidore> RelSeguidoreUsuarioSeguidors { get; set; } = new List<RelSeguidore>();

    [InverseProperty("UsuarioSeguido")]
    public virtual ICollection<RelSeguidore> RelSeguidoreUsuarioSeguidos { get; set; } = new List<RelSeguidore>();

    [InverseProperty("Usuario")]
    public virtual ICollection<RelUsuarioBankroll> RelUsuarioBankrolls { get; set; } = new List<RelUsuarioBankroll>();

    [InverseProperty("Usuario")]
    public virtual ICollection<RelUsuarioTipster> RelUsuarioTipsters { get; set; } = new List<RelUsuarioTipster>();

    [InverseProperty("Usuario")]
    public virtual ICollection<RelUsuariosCasino> RelUsuariosCasinos { get; set; } = new List<RelUsuariosCasino>();
}
