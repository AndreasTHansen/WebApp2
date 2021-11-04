using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kunde_SPA.Model
{
    public class Reise
    {
        public int id { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,50}$")]
        public string reiseTil { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,50}$")]
        public string reiseFra { get; set; }
        [RegularExpression(@"^[0-9./\:]{2,20}$")]
        public string tidspunktFra { get; set; }
        [RegularExpression(@"^[0-9./\:]{2,20}$")]
        public string tidspunktTil { get; set; }
        [RegularExpression(@"^[0-9./\:]{2,20}$")]
        public string datoAvreise { get; set; }
        [RegularExpression(@"^[0-9./\:]{2,20}$")]
        public string datoAnkomst { get; set; }
        public double reisePris { get; set; }
    }
}
