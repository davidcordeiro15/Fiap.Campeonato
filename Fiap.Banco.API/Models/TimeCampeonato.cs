using System.ComponentModel.DataAnnotations.Schema;

namespace Fiap.Banco.API.Models
{
    [Table("TIMES_CAMPEONATOS")]
    public class TimeCampeonato
    {
        public int TimeId { get; set; }
        public Time? Time { get; set; }

        public int CampeonatoId { get; set; }
        public Campeonato? Campeonato { get; set; }
    }
}
