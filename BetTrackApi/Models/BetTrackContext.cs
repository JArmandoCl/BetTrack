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

    public virtual DbSet<RelDepositoRetiro> RelDepositoRetiros { get; set; }

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
            entity.HasKey(e => e.CasinoId).HasName("PK__Casinos__D0F8D6CDC2CC3E96");
        });

        modelBuilder.Entity<Deporte>(entity =>
        {
            entity.HasKey(e => e.DeporteId).HasName("PK__Deportes__A8A8685B9E549C95");
        });

        modelBuilder.Entity<EstatusApuesta>(entity =>
        {
            entity.HasKey(e => e.EstatusApuestaId).HasName("PK__EstatusA__AA229DBBED59FB8A");
        });

        modelBuilder.Entity<EstatusBankroll>(entity =>
        {
            entity.HasKey(e => e.EstatusBankrollId).HasName("PK__EstatusB__13CF5F809E733725");
        });

        modelBuilder.Entity<EstatusCategoria>(entity =>
        {
            entity.HasKey(e => e.EstatusCategoriaId).HasName("PK__EstatusC__D411AD4126502315");
        });

        modelBuilder.Entity<EstatusUsuario>(entity =>
        {
            entity.HasKey(e => e.EstatusUsuarioId).HasName("PK__EstatusU__0FE2351B29207046");
        });

        modelBuilder.Entity<EstatusUsuariosCasino>(entity =>
        {
            entity.HasKey(e => e.EstatusUsuarioCasinoId).HasName("PK__EstatusU__EA0064BA6ADE3987");
        });

        modelBuilder.Entity<FormatosCuota>(entity =>
        {
            entity.HasKey(e => e.FormatoCuotaId).HasName("PK__Formatos__2AB1FAE51B8EB86D");
        });

        modelBuilder.Entity<RelApuesta>(entity =>
        {
            entity.HasKey(e => e.ApuestaId).HasName("PK__RelApues__5F647243025EDA1B");

            entity.HasOne(d => d.CategoriaUsuario).WithMany(p => p.RelApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelApuest__Categ__70FDBF69");

            entity.HasOne(d => d.TipoApuesta).WithMany(p => p.RelApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelApuest__TipoA__6F1576F7");

            entity.HasOne(d => d.UsuarioBankroll).WithMany(p => p.RelApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelApuest__Usuar__6E2152BE");

            entity.HasOne(d => d.UsuarioTipster).WithMany(p => p.RelApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelApuest__Usuar__70099B30");
        });

        modelBuilder.Entity<RelCategoriasUsuario>(entity =>
        {
            entity.HasKey(e => e.CategoriaUsuarioId).HasName("PK__RelCateg__D53723329B2C8232");

            entity.HasOne(d => d.EstatusCategoria).WithMany(p => p.RelCategoriasUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelCatego__Estat__6B44E613");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RelCategoriasUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelCatego__Usuar__6A50C1DA");
        });

        modelBuilder.Entity<RelDepositoRetiro>(entity =>
        {
            entity.HasKey(e => e.DepositoRetiroId).HasName("PK__RelDepos__422A21ECAC6C231A");

            entity.HasOne(d => d.UsuarioBankroll).WithMany(p => p.RelDepositoRetiros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelDeposi__Usuar__789EE131");
        });

        modelBuilder.Entity<RelDetallesApuesta>(entity =>
        {
            entity.HasKey(e => e.DetalleApuestaId).HasName("PK__RelDetal__4FE5E008D7FF7524");

            entity.HasOne(d => d.Apuesta).WithMany(p => p.RelDetallesApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelDetall__Apues__73DA2C14");

            entity.HasOne(d => d.Deporte).WithMany(p => p.RelDetallesApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelDetall__Depor__74CE504D");

            entity.HasOne(d => d.EstatusApuesta).WithMany(p => p.RelDetallesApuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelDetall__Estat__75C27486");
        });

        modelBuilder.Entity<RelSeguidore>(entity =>
        {
            entity.HasKey(e => e.SeguidorId).HasName("PK__RelSegui__EAE128CFD85E7BD6");

            entity.HasOne(d => d.UsuarioSeguido).WithMany(p => p.RelSeguidoreUsuarioSeguidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelSeguid__Usuar__7C6F7215");

            entity.HasOne(d => d.UsuarioSeguidor).WithMany(p => p.RelSeguidoreUsuarioSeguidors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelSeguid__Usuar__7B7B4DDC");
        });

        modelBuilder.Entity<RelUsuarioBankroll>(entity =>
        {
            entity.HasKey(e => e.UsuarioBankrollId).HasName("PK__RelUsuar__CE4DD3EECDD9F797");

            entity.HasOne(d => d.EstatusBankroll).WithMany(p => p.RelUsuarioBankrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Estat__658C0CBD");

            entity.HasOne(d => d.FormatoCuota).WithMany(p => p.RelUsuarioBankrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Forma__668030F6");

            entity.HasOne(d => d.TipoBankroll).WithMany(p => p.RelUsuarioBankrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__TipoB__6774552F");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RelUsuarioBankrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Usuar__6497E884");
        });

        modelBuilder.Entity<RelUsuarioTipster>(entity =>
        {
            entity.HasKey(e => e.UsuarioTipsterId).HasName("PK__RelUsuar__2FEA7E08F6D4880D");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RelUsuarioTipsters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Usuar__5DEAEAF5");
        });

        modelBuilder.Entity<RelUsuariosCasino>(entity =>
        {
            entity.HasKey(e => e.UsuarioCasinoId).HasName("PK__RelUsuar__DCCE427D369345FC");

            entity.Property(e => e.CasinoId).HasComputedColumnSql("((100)+(1))", false);

            entity.HasOne(d => d.EstatusUsuarioCasino).WithMany(p => p.RelUsuariosCasinos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Estat__61BB7BD9");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RelUsuariosCasinos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RelUsuari__Usuar__60C757A0");
        });

        modelBuilder.Entity<TiposApuesta>(entity =>
        {
            entity.HasKey(e => e.TipoApuestaId).HasName("PK__TiposApu__08DC78AE7A914497");
        });

        modelBuilder.Entity<TiposBankroll>(entity =>
        {
            entity.HasKey(e => e.TipoBankrollId).HasName("PK__TiposBan__7A35ED20E27CDA97");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B82D5587FA");

            entity.HasOne(d => d.EstatusUsuario).WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__Estatu__592635D8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
