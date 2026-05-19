using System.ComponentModel.DataAnnotations;

namespace Fiap.Banco.API.Models
{
    public class Jogador : Pessoa
    {
        [MaxLength(50)]
        public string Posicao { get; set; } = string.Empty;

        public int NumeroCamisa { get; set; }

        public int TimeId { get; set; }
        public Time? Time { get; set; }
    }
}
