using System;
namespace AcesApp.Models
{
    public class Professor
    {

        public Professor()
        {
            
        }
        
        public int id_grldentista { get; set; }
        public int? Id_grlbasico { get; set; }
        //public string cd_usuario { get; set; }
        public virtual Pessoa Idgrlbasic { get; set; }
        public string Ativo { get; set; }

        //public virtual ICollection<Orcamento> Orcamentos { get; set; }
       // public virtual ICollection<HorarioProfessor> HorarioProfessor { get; set; }
       // public virtual ICollection<CefalometriaItem> cefalometriaItem { get; set; }


    }

}
