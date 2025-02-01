using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class BetTrackContext : DbContext
{
    public BetTrackContext()
    {
    }

    public BetTrackContext(DbContextOptions<BetTrackContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Casino> Casinos { get; set; }

    public virtual DbSet<Deporte> Deportes { get; set; }

    public virtual DbSet<EstatusApuesta> EstatusApuestas { get; set; }

    public virtual DbSet<EstatusBankroll> EstatusBankrolls { get; set; }

    public virtual DbSet<EstatusCategoria> EstatusCategorias { get; set; }

    public virtual DbSet<EstatusUsuario> EstatusUsuarios { get; set; }

    public virtual DbSet<EstatusUsuariosCasino> EstatusUsuariosCasinos { get; set; }

    public virtual DbSet<FormatosCuota> FormatosCuotas { get; set; }

    public virtual DbSet<RelApuesta> RelApuestas { get; set; }

    public virtual DbSet<RelCategoriasUsuario> RelCategoriasUsuarios { get; set; }

    public virtual DbSet<RelDepositosRetiro> RelDepositosRetiros { get; set; }

    public virtual DbSet<RelDetallesApuesta> RelDetallesApuestas { get; set; }

    public virtual DbSet<RelSeguidore> RelSeguidores { get; set; }

    public virtual DbSet<RelUsuarioBankroll> RelUsuarioBankrolls { get; set; }

    public virtual DbSet<RelUsuarioTipster> RelUsuarioTipsters { get; set; }

    public virtual DbSet<RelUsuariosCasino> RelUsuariosCasinos { get; set; }

    public virtual DbSet<TiposApuesta> TiposApuestas { get; set; }

    public virtual DbSet<TiposBankroll> TiposBankrolls { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=BetTrack.mssql.somee.com;Database=BetTrack;User Id=BetTrack_SQLLogin_1;Password=x9ov5jyj1s;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Casino>(entity =>
        {
            entity.HasKey(e => e.CasinoId).HasName("PK__Casinos__D0F8D6CD0D4186A5");
        });

        modelBuilder.Entity<Deporte>(entity =>
        {
            entity.HasKey(e => e.DeporteId).HasName("PK__Deportes__A8A8685B2AA8CC39");
        });

        modelBuilder.Entity<EstatusApuesta>(entity =>
        {
            entity.HasKey(e => e.EstatusApuestaId).HasName("PK__EstatusA__AA229DBB6B6DEFF1");
        });

        modelBuilder.Entity<EstatusBankroll>(entity =>
        {
            entity.HasKey(e => e.EstatusBankrollId).HasName("PK__EstatusB__13CF5F80B9861F6B");
        });

        modelBuilder.Entity<EstatusCategoria>(entity =>
        {
            entity.HasKey(e => e.EstatusCategoriaId).HasName("PK__EstatusC__D411AD4163B7EB6A");
        });

        modelBuilder.Entity<EstatusUsuario>(entity =>
        {
            entity.HasKey(e => e.EstatusUsuarioId).HasName("PK__EstatusU__0FE2351B6EE70312");
        });

        modelBuilder.Entity<EstatusUsuariosCasino>(entity =>
        {
            entity.HasKey(e => e.EstatusUsuarioCasinoId).HasName("PK__EstatusU__EA0064BA058E6428");
        });

        modelBuilder.Entity<FormatosCuota>(entity =>
        {
            entity.HasKey(e => e.FormatoCuotaId).HasName("PK__Formatos__2AB1FAE594BEA9FE");
        });

        modelBuilder.Entity<RelApuesta>(entity =>
        {
            entity.HasKey(e => e.ApuestaId).HasName("PK__RelApues__5F64724394B36584");

            entity.HasOne(d => d.CategoriaUsuario).WithMany(p => p.RelApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelApuest__Categ__284DF453");

            entity.HasOne(d => d.TipoApuesta).WithMany(p => p.RelApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelApuest__TipoA__2665ABE1");

            entity.HasOne(d => d.UsuarioBankroll).WithMany(p => p.RelApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelApuest__Usuar__257187A8");

            entity.HasOne(d => d.UsuarioTipster).WithMany(p => p.RelApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelApuest__Usuar__2759D01A");
        });

        modelBuilder.Entity<RelCategoriasUsuario>(entity =>
        {
            entity.HasKey(e => e.CategoriaUsuarioId).HasName("PK__RelCateg__D537233290F03E7D");

            entity.HasOne(d => d.EstatusCategoria).WithMany(p => p.RelCategoriasUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelCatego__Estat__22951AFD");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RelCategoriasUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelCatego__Usuar__21A0F6C4");
        });

        modelBuilder.Entity<RelDepositosRetiro>(entity =>
        {
            entity.HasKey(e => e.DepositoRetiroId).HasName("PK__RelDepos__422A21ECA2105494");

            entity.HasOne(d => d.UsuarioBankroll).WithMany(p => p.RelDepositosRetiros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelDeposi__Usuar__39788055");
        });

        modelBuilder.Entity<RelDetallesApuesta>(entity =>
        {
            entity.HasKey(e => e.DetalleApuestaId).HasName("PK__RelDetal__4FE5E008CADF7F4B");

            entity.HasOne(d => d.Apuesta).WithMany(p => p.RelDetallesApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelDetall__Apues__2B2A60FE");

            entity.HasOne(d => d.Deporte).WithMany(p => p.RelDetallesApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelDetall__Depor__2C1E8537");

            entity.HasOne(d => d.EstatusApuesta).WithMany(p => p.RelDetallesApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelDetall__Estat__2D12A970");
        });

        modelBuilder.Entity<RelSeguidore>(entity =>
        {
            entity.HasKey(e => e.SeguidorId).HasName("PK__RelSegui__EAE128CFB9C6E38B");

            entity.HasOne(d => d.UsuarioSeguido).WithMany(p => p.RelSeguidoreUsuarioSeguidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelSeguid__Usuar__33BFA6FF");

            entity.HasOne(d => d.UsuarioSeguidor).WithMany(p => p.RelSeguidoreUsuarioSeguidors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelSeguid__Usuar__32CB82C6");
        });

        modelBuilder.Entity<RelUsuarioBankroll>(entity =>
        {
            entity.HasKey(e => e.UsuarioBankrollId).HasName("PK__RelUsuar__CE4DD3EE7A227529");

            entity.HasOne(d => d.EstatusBankroll).WithMany(p => p.RelUsuarioBankrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Estat__1CDC41A7");

            entity.HasOne(d => d.FormatoCuota).WithMany(p => p.RelUsuarioBankrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Forma__1DD065E0");

            entity.HasOne(d => d.TipoBankroll).WithMany(p => p.RelUsuarioBankrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__TipoB__1EC48A19");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RelUsuarioBankrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Usuar__1BE81D6E");
        });

        modelBuilder.Entity<RelUsuarioTipster>(entity =>
        {
            entity.HasKey(e => e.UsuarioTipsterId).HasName("PK__RelUsuar__2FEA7E08E744DE49");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RelUsuarioTipsters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Usuar__153B1FDF");
        });

        modelBuilder.Entity<RelUsuariosCasino>(entity =>
        {
            entity.HasKey(e => e.UsuarioCasinoId).HasName("PK__RelUsuar__DCCE427D181D8EA8");

            entity.Property(e => e.CasinoId).HasComputedColumnSql("((100)+(1))", false);

            entity.HasOne(d => d.EstatusUsuarioCasino).WithMany(p => p.RelUsuariosCasinos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Estat__190BB0C3");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RelUsuariosCasinos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Usuar__18178C8A");
        });

        modelBuilder.Entity<TiposApuesta>(entity =>
        {
            entity.HasKey(e => e.TipoApuestaId).HasName("PK__TiposApu__08DC78AEAF400015");
        });

        modelBuilder.Entity<TiposBankroll>(entity =>
        {
            entity.HasKey(e => e.TipoBankrollId).HasName("PK__TiposBan__7A35ED20D0785A0B");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B89DEA3498");

            entity.HasOne(d => d.EstatusUsuario).WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__Estatu__10766AC2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
