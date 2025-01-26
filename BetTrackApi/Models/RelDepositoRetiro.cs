using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class RelDepositoRetiro
{
    [Key]
    public long DepositoRetiroId { get; set; }

    public long UsuarioBankrollId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Descripcion { get; set; } = null!;

    [Column(TypeName = "decimal(19, 2)")]
    public decimal Monto { get; set; }

    [ForeignKey("UsuarioBankrollId")]
    [InverseProperty("RelDepositoRetiros")]
    public virtual RelUsuarioBankroll UsuarioBankroll { get; set; } = null!;
}
