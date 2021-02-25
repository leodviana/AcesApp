using System;
using System.Collections.Generic;
using System.Text;

namespace AcesApp.Models
{
    public class Events
    {

        public int EventID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string ThemeColor { get; set; }
        public string IsFullDay { get; set; }

        //[NotMapped]
        public int? contrato { get; set; }
       // [NotMapped]
        public int? professor { get; set; }
        public string nome_professor { get; set; }
    }
}
