using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiap.Banco.API.Models
{
    [Table("ESTATISTICAS")]
    public class Estatistica
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TimeId { get; set; }
        public Time? Time { get; set; }

        public int CampeonatoId { get; set; }
        public Campeonato? Campeonato { get; set; }

        public int Pontos { get; set; }
        public int Vitorias { get; set; }
        public int Derrotas { get; set; }
        public int Empates { get; set; }
        public int GolsPro { get; set; }
        public int GolsContra { get; set; }

        [NotMapped]
        public int SaldoGols => GolsPro - GolsContra;
    }
}
