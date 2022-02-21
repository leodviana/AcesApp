using System;
using System.Globalization;

namespace AcesApp.Models
{
    
        public class AulasLog
        {
            
            public int idGercdAulasLog { get; set; }
            public int? id_Stqcporcamento_inicio { get; set; }

            public DateTime inicio { get; set; }
            public string dt_inicio => inicio.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            public DateTime Fim { get; set; }
            public string dt_fim => Fim.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            public string status_inicial { get; set; }
            public string dia_semana_inicial { get; set; }
            public string Subject_inicial { get; set; }
            public string nome_contrato_inicial { get; set; }
            public int? id_grldentista_inicial { get; set; }
            public int idGercdaulas_inicial { get; set; }
            public int idGercdaulas_final { get; set; }
            public int? id_Stqcporcamento_final { get; set; }

            public DateTime horario_inicio_final { get; set; }
            public DateTime hora_final_final { get; set; }
            public string status_final { get; set; }
            public string dia_semana_final { get; set; }
            public string Subject_final { get; set; }
            public int? id_grldentista_final { get; set; }

            public int id_usuario { get; set; }
        public string nome_aluno { get; set; }
        public string nome_aluno_final { get; set; }

        public string descricao => (dt_inicio.ToString().Substring(0,10)+ " " + (inicio.Hour.ToString().Length == 1 ? "0" : "") + inicio.Hour.ToString() + ":" + inicio.Minute.ToString() + (inicio.Minute.ToString().Length == 1 ? "0" : "") + " - " + (Fim.Hour.ToString().Length == 1 ? "0" : "") + Fim.Hour.ToString() + ":" + Fim.Minute.ToString() + (Fim.Minute.ToString().Length == 1 ? "0" : ""));
        public string descricao_final => (dt_fim.ToString().Substring(0, 10) + " " + (horario_inicio_final.Hour.ToString().Length == 1 ? "0" : "") + horario_inicio_final.Hour.ToString() + ":" + horario_inicio_final.Minute.ToString() + (horario_inicio_final.Minute.ToString().Length == 1 ? "0" : "") + " - " + (hora_final_final.Hour.ToString().Length == 1 ? "0" : "") + hora_final_final.Hour.ToString() + ":" + hora_final_final.Minute.ToString() + (hora_final_final.Minute.ToString().Length == 1 ? "0" : ""));
        public string nome_dentista_inicial { get; set; }
          public string nome_dentista_final { get; set; }

        public string Exibicao => "Modificação No." + idGercdAulasLog;
    }

    
    
}
