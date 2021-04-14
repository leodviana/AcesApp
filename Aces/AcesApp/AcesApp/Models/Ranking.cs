using System;
namespace AcesApp.Models
{
    public class Ranking
    {

        public int id_ranking { get; set; }
        public int id_grlbasico { get; set; }
        public string categoria { get; set; }
        public double pontos { get; set; }
        public int id_grlbasico_dupla { get; set; }
        public int posicao { get; set; }

        public string jogador { get; set; }
        public string jogador2 { get; set; }
    }
}
