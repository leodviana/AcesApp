using System;
using System.Collections.Generic;
using System.Text;

namespace AcesApp.Models
{
    public class Pessoa
    {

        public Pessoa()
        {
           /* this.enderecos = new List<Endereco>();
            this.clientes = new List<Cliente>();
            this.dentistas = new List<Dentista>();
            this.convenios = new List<Convenio>();
            this.usuarios = new List<Usuario>();*/
        }
        
        public int Id_grlbasico { get; set; }
        public string Id_grltopessoa { get; set; }
        public string nome { get; set; }
        public string razao_social { get; set; }
        public string Nome_fantasia { get; set; }
        public string insc_municipal { get; set; }
        public string insc_estadual { get; set; }
        public string cgc { get; set; }

        public int sexo { get; set; }

        public DateTime? dt_nascimento { get; set; }
        public int? Id_grlprofi { get; set; }
        public int? Id_grlcidad { get; set; }
        public int? Id_grlcivil { get; set; }
        public string cpf { get; set; }
        public string rg { get; set; }

        public string contato { get; set; }
        public string ddd_telefone { get; set; }
        public int? Id_grlidtel { get; set; }
        public string ddd_telefone2 { get; set; }
        public int? Id_grlidtel02 { get; set; }
        public string email { get; set; }
        public string Email2 { get; set; }

        public string status { get; set; }
        public int? Id_grlcdusu { get; set; }


       /* public Sexo gercdsexo { get; set; }
        public EstadoCivil IdGrlcivilNavigation { get; set; }
        public TipoTelefone grlidtel { get; set; }
        public TipoTelefone grlidtel1 { get; set; }

        public Profissao profissao { get; set; }

        public IEnumerable<Endereco> enderecos { get; set; }

        public IEnumerable<Cliente> clientes { get; set; }

        public IEnumerable<Dentista> dentistas { get; set; }

        public IEnumerable<Convenio> convenios { get; set; }

        public IEnumerable<Usuario> usuarios { get; set; }

        public IEnumerable<Fornecedor> Fornecedores { get; set; }*/

    }
}
