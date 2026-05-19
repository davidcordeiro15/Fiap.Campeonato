using System.ComponentModel.DataAnnotations;

namespace Fiap.Banco.API.Models
{
    public class Tecnico : Pessoa
    {
        [MaxLength(100)]
        public string Especialidade { get; set; } = string.Empty;

        public int TimeId { get; set; }
        public Time? Time { get; set; }
    }
}
