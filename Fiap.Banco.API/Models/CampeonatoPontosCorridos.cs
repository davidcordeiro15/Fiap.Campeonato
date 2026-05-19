namespace Fiap.Banco.API.Models
{
    public class CampeonatoPontosCorridos : Campeonato
    {
        public int TotalRodadas { get; set; }
        public bool ConfrontosGerados { get; set; } = false;
    }
}
