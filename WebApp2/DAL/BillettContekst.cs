using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace KundeApp2.DAL
{
    [ExcludeFromCodeCoverage]
    public class Billetter
    {
        public int id { get; set; }
        public int antallVoksne { get; set; }
        public int antallBarn { get; set; }
        public double totalPris { get; set; }
        public virtual Kunder kunde { get; set; }
        public virtual Reiser reise { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class Reiser
    {
        public int id { get; set; }
        public string reiseTil { get; set; }
        public string reiseFra { get; set; }
        public string tidspunktFra { get; set; }

        public string datoAvreise { get; set; }
        public string datoAnkomst { get; set; }
        public string tidspunktTil { get; set; }
        public double reisePris { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class Kunder
    {
        public int id { get; set; }
        public string fornavn { get; set; }
        public string etternavn { get; set; }
        public string epost { get; set; }
        public string mobilnummer { get; set; }
        public virtual Kort kort { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class Brukere
    {
        public int Id { get; set; }
        public string Brukernavn { get; set; }
        public byte[] Passord { get; set; }
        public byte[] Salt { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class Kort
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string kortnummer { get; set; }
        public string utlopsdato { get; set; }
        public int cvc { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class BillettContekst : DbContext
    {
        public BillettContekst(DbContextOptions<BillettContekst> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Billetter> Billetter { get; set; }
        public DbSet<Reiser> Reiser { get; set; }
        public DbSet<Kunder> Kunder { get; set; }
        public DbSet<Kort> Kort { get; set; }
        public DbSet<Brukere> Brukere { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
