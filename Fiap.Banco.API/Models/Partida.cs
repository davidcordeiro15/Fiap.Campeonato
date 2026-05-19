using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiap.Banco.API.Models
{
    [Table("PARTIDAS")]
    public class Partida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TimeCasaId { get; set; }
        public Time? TimeCasa { get; set; }

        public int TimeVisitanteId { get; set; }
        public Time? TimeVisitante { get; set; }

        public DateTime DataPartida { get; set; }

        public int? GolsCasa { get; set; }
        public int? GolsVisitante { get; set; }

        public int CampeonatoId { get; set; }
        public Campeonato? Campeonato { get; set; }
    }
}
