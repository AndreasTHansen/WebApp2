using System;
using System.ComponentModel.DataAnnotations;

namespace KundeApp2.Model
{
     public class Kunde
     {
        public int id { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string fornavn { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,50}$")]
        public string etternavn { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")]
        public string epost { get; set; }
        [RegularExpression(@"^[0-9\.\ \-]{8,12}$")]
        public string mobilnummer { get; set; }
        public string kortnummer { get; set; }
        public string utlopsdato { get; set; }
        public int cvc { get; set; }

    }
}
