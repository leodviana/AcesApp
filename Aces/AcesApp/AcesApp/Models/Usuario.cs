using System;
using System.Collections.Generic;
using System.Text;

namespace AcesApp.Models
{
    public class Usuario
    {
        public Usuario()
        {
            Servicos_itens = new List<ItemOrcamento>();
            _titulos = new List<Titulos>();
        }
        public string nome { get; set; }

        public DateTime? dt_nascimento { get; set; }
        public int UsuarioId { get; set; }
        public int? Id_grlbasico { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Administrador { get; set; }
        public string Ativo { get; set; }

        public string senha_sem { get; set; }
        public int Tipo_usuario { get; set; }

        public Int32 contratoId { get; set; }

        public DateTime? inicio { get; set; }
        public DateTime? Renovacao { get; set; }

        public int? id_plano { get; set; }
        public string plano { get; set; }

        public int? id_professor { get; set; }
        public string professor { get; set; }

        public List<ItemOrcamento> Servicos_itens { get; set; }
        public List<Titulos> _titulos { get; set; }

        public string ImagePath { get; set; }
        //public virtual Pessoa pessoas { get; set; }
        // public virtual ICollection<Fornecedor> fornecedores { get; set; }
        // public virtual ICollection<NotaEntrada> NotaEntradas { get; set; }
        // public virtual ICollection<NotaEntradaItem> NotaEntradaItems { get; set; }
        // public virtual ICollection<CentrodeCusto> CentrodeCustos { get; set; }
        //public virtual ICollection<TipoEntrada> TipoEntradas { get; set; }
        // public virtual ICollection<TipoNota> TipoNotas { get; set; }
    }
}
