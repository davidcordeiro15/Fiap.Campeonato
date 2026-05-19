using Microsoft.EntityFrameworkCore;
using Fiap.Banco.API.Models;

namespace Fiap.Banco.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Time> Times { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Jogador> Jogadores { get; set; }
        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<Campeonato> Campeonatos { get; set; }
        public DbSet<CampeonatoPontosCorridos> CampeonatosPontosCorridos { get; set; }
        public DbSet<TimeCampeonato> TimesCampeonatos { get; set; }
        public DbSet<Partida> Partidas { get; set; }
        public DbSet<Estatistica> Estatisticas { get; set; }
        public DbSet<Trofeu> Trofeus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPH: Pessoa -> Jogador | Tecnico
            modelBuilder.Entity<Pessoa>()
                .HasDiscriminator<string>("Tipo")
                .HasValue<Jogador>("Jogador")
                .HasValue<Tecnico>("Tecnico");

            // TPH: Campeonato -> CampeonatoPontosCorridos
            modelBuilder.Entity<Campeonato>()
                .HasDiscriminator<string>("Tipo")
                .HasValue<Campeonato>("Base")
                .HasValue<CampeonatoPontosCorridos>("PontosCorridos");

            modelBuilder.Entity<CampeonatoPontosCorridos>()
                .Property(c => c.ConfrontosGerados)
                .HasConversion<short>()
                .HasColumnType("NUMBER(1)");
            // Composite PK
            modelBuilder.Entity<TimeCampeonato>()
                .HasKey(tc => new { tc.TimeId, tc.CampeonatoId });

            modelBuilder.Entity<TimeCampeonato>()
                .HasOne(tc => tc.Time)
                .WithMany(t => t.Campeonatos)
                .HasForeignKey(tc => tc.TimeId);

            modelBuilder.Entity<TimeCampeonato>()
                .HasOne(tc => tc.Campeonato)
                .WithMany(c => c.Times)
                .HasForeignKey(tc => tc.CampeonatoId);

            modelBuilder.Entity<Partida>()
                .HasOne(p => p.TimeCasa)
                .WithMany()
                .HasForeignKey(p => p.TimeCasaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Partida>()
                .HasOne(p => p.TimeVisitante)
                .WithMany()
                .HasForeignKey(p => p.TimeVisitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Estatistica>()
                .HasIndex(e => new { e.TimeId, e.CampeonatoId })
                .IsUnique();
        }
    }
}
