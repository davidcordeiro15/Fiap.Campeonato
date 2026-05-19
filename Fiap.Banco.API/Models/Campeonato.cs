using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiap.Banco.API.Models
{
    [Table("CAMPEONATOS")]
    public class Campeonato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Temporada { get; set; } = string.Empty;

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public ICollection<TimeCampeonato> Times { get; set; } = new List<TimeCampeonato>();
        public ICollection<Partida> Partidas { get; set; } = new List<Partida>();
    }
}
