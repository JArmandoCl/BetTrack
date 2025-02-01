using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class RelDepositosRetiro
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

    public bool? EsRetiro { get; set; }

    [ForeignKey("UsuarioBankrollId")]
    [InverseProperty("RelDepositosRetiros")]
    public virtual RelUsuarioBankroll UsuarioBankroll { get; set; } = null!;
}
