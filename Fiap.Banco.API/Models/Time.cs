using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiap.Banco.API.Models
{
    [Table("TIMES")]
    public class Time
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Cidade { get; set; } = string.Empty;

        public int AnoFundacao { get; set; }

        public ICollection<TimeCampeonato> Campeonatos { get; set; } = new List<TimeCampeonato>();
        public ICollection<Trofeu> Trofeus { get; set; } = new List<Trofeu>();
    }
}
