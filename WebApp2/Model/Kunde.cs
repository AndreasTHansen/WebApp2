using System;
using System.ComponentModel.DataAnnotations;

namespace KundeApp2.Model
{
     public class Kunde
     {
        public int id { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string fornavn { get; set; }
        public string etternavn { get; set; }
        public string epost { get; set; }
        public string mobilnummer { get; set; }
        public string kortnummer { get; set; }
        public string utlopsdato { get; set; }
        public int cvc { get; set; }

    }
}
