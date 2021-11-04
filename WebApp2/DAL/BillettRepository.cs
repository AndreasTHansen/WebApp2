using Kunde_SPA.Model;
using KundeApp2.DAL;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Kunde_SPA.DAL
{
    public class BillettRepository : IBillettRepository
    {

        private readonly DBContext _billettDb;

        private ILogger<BillettRepository> _log;

        public BillettRepository(DBContext billettDb, ILogger<BillettRepository> log)
        {
            _billettDb = billettDb;
            _log = log;

        }
        public async Task<List<Billett>> HentAlle()
        {
            try
            {
                List<Billett> alleBilletter = await _billettDb.Billetter.Select(k => new Billett
                {
                    id = k.id,
                    kundeId = k.kunde.id,
                    antallBarn = k.antallBarn,
                    antallVoksne = k.antallVoksne,
                    totalPris = k.totalPris,
                    fornavn = k.kunde.fornavn,
                    etternavn = k.kunde.etternavn,
                    epost = k.kunde.epost,
                    mobilnummer = k.kunde.mobilnummer,
                    kortnummer = k.kunde.kort.kortnummer,
                    utlopsdato = k.kunde.kort.utlopsdato,
                    cvc = k.kunde.kort.cvc,
                    reiseId = k.reise.id,
                    reiseFra = k.reise.reiseFra,
                    reiseTil = k.reise.reiseTil,
                    datoAnkomst = k.reise.datoAnkomst,
                    datoAvreise = k.reise.datoAvreise,
                    tidspunktFra = k.reise.tidspunktFra,
                    tidspunktTil = k.reise.tidspunktTil,
                    reisePris = k.reise.reisePris
                }).ToListAsync();

                return alleBilletter;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> Lagre(Billett innBillett)
        {
            try
            {
                var nyBillett = new Billetter();
                nyBillett.antallBarn = innBillett.antallBarn;
                nyBillett.antallVoksne = innBillett.antallVoksne;
                nyBillett.totalPris = innBillett.totalPris;

                var sjekkKunde = _billettDb.Kunder.Find(innBillett.kundeId);
                if (sjekkKunde == null)
                {
                    var nyKunde = new Kunder();
                    nyKunde.fornavn = innBillett.fornavn;
                    nyKunde.epost = innBillett.epost;
                    nyKunde.etternavn = innBillett.etternavn;
                    nyKunde.id = innBillett.kundeId;
                    nyKunde.mobilnummer = innBillett.mobilnummer;
                    nyBillett.kunde = nyKunde;

                    var sjekkKort = _billettDb.Kort.Find(innBillett.kortnummer);
                    if (sjekkKort == null)
                    {
                        var nyttKort = new Kort();
                        nyttKort.kortnummer = innBillett.kortnummer;
                        nyttKort.utlopsdato = innBillett.utlopsdato;
                        nyttKort.cvc = innBillett.cvc;
                        nyKunde.kort = nyttKort;
                    }
                    else
                    {
                        nyKunde.kort.kortnummer = sjekkKort.kortnummer;
                    }
                }
                else
                {
                    nyBillett.kunde.id = sjekkKunde.id;
                }

                var sjekkReise = _billettDb.Reiser.Find(innBillett.reiseId);
                if (sjekkReise == null)
                {

                    var nyReise = new Reiser();
                    nyReise.reiseFra = innBillett.reiseFra;
                    nyReise.reiseTil = innBillett.reiseTil;
                    nyReise.datoAvreise = innBillett.datoAvreise;
                    nyReise.datoAnkomst = innBillett.datoAnkomst;
                    nyReise.tidspunktFra = innBillett.tidspunktFra;
                    nyReise.tidspunktTil = innBillett.tidspunktTil;
                    nyReise.reisePris = innBillett.reisePris;

                    nyBillett.reise = nyReise;
                }

                else
                {
                    nyBillett.reise.id = sjekkReise.id;
                }

                _billettDb.Add(nyBillett);
                await _billettDb.SaveChangesAsync();
                return true;
            }

            catch
            {
                return false;
            }
        }
        public async Task<bool> Slett(int id)
        {
            try
            {
                Billetter enBillet = await _billettDb.Billetter.FindAsync(id);
                _billettDb.Remove(enBillet);
                await _billettDb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Billett> HentEnBillett(int id)
        {
            try
            {
                Billetter enBillett = await _billettDb.Billetter.FindAsync(id);
                var hentetBillett = new Billett()
                {
                    id = enBillett.id,
                    kundeId = enBillett.kunde.id,
                    fornavn = enBillett.kunde.fornavn,
                    etternavn = enBillett.kunde.etternavn,
                    epost = enBillett.kunde.epost,
                    mobilnummer = enBillett.kunde.mobilnummer,
                    totalPris = enBillett.totalPris,
                    antallBarn = enBillett.antallBarn,
                    antallVoksne = enBillett.antallVoksne,
                    reiseId = enBillett.reise.id,
                    reiseTil = enBillett.reise.reiseTil,
                    reiseFra = enBillett.reise.reiseFra,
                    datoAnkomst = enBillett.reise.datoAnkomst,
                    datoAvreise = enBillett.reise.datoAvreise,
                    tidspunktFra = enBillett.reise.tidspunktFra,
                    tidspunktTil = enBillett.reise.tidspunktTil,
                    reisePris = enBillett.reise.reisePris
                };
                return hentetBillett;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> EndreBillett(Billett endreBillett)
        {
            try
            {
                var endreObjekt = await _billettDb.Billetter.FindAsync(endreBillett.id);
                //Sjekke om kunden har blitt endret
                if (endreObjekt.kunde.id != endreBillett.kundeId)
                {
                    var sjekkKunde = _billettDb.Kunder.Find(endreBillett.kundeId);
                    if (sjekkKunde == null)
                    {
                        var kundeRad = new Kunder();
                        kundeRad.fornavn = endreBillett.fornavn;
                        kundeRad.etternavn = endreBillett.etternavn;
                        kundeRad.epost = endreBillett.epost;
                        kundeRad.mobilnummer = endreBillett.mobilnummer;

                        var sjekkKort = _billettDb.Kort.Find(endreBillett.kortnummer);
                        if (sjekkKort == null)
                        {
                            var kortRad = new Kort();
                            kortRad.kortnummer = endreBillett.kortnummer;
                            kortRad.cvc = endreBillett.cvc;
                            kortRad.utlopsdato = endreBillett.utlopsdato;

                            endreObjekt.kunde.kort = kortRad;
                        }
                        else
                        {
                            endreObjekt.kunde.kort.kortnummer = endreBillett.kortnummer;
                        }

                        //Kortet blir endret i endreKunde, skal ikke være nødvendig med å sjekke kort her

                        endreObjekt.kunde = kundeRad;
                    }
                    else
                    {
                        endreObjekt.kunde.id = endreBillett.kundeId;
                    }
                }
                if (endreObjekt.reise.id != endreBillett.reiseId)
                {
                    var sjekkReise = _billettDb.Reiser.Find(endreBillett.reiseId);
                    //Sjekke om reisen har blitt endret
                    if (endreObjekt.reise.id != endreBillett.reiseId)
                    {
                        var reiseRad = new Reiser();
                        reiseRad.reiseFra = endreBillett.reiseFra;
                        reiseRad.reiseTil = endreBillett.reiseTil;
                        reiseRad.reisePris = endreBillett.reisePris;
                        reiseRad.datoAnkomst = endreBillett.datoAnkomst;
                        reiseRad.datoAvreise = endreBillett.datoAvreise;
                        reiseRad.tidspunktFra = endreBillett.tidspunktFra;
                        reiseRad.tidspunktTil = endreBillett.tidspunktTil;
                        endreObjekt.reise = reiseRad;
                    }
                    else
                    {
                        endreObjekt.reise.id = endreBillett.reiseId;
                    }

                }

                endreObjekt.antallBarn = endreBillett.antallBarn;
                endreObjekt.antallVoksne = endreBillett.antallVoksne;
                endreObjekt.totalPris = endreBillett.totalPris;
                await _billettDb.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
            return true;
        }



        //LagHash, LagSalt og LoggInn er hentet fra fagstoff.
        public static byte[] LagHash(string passord, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: passord,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
                                numBytesRequested: 32);
        }

        public static byte[] LagSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }


        public async Task<bool> LoggInn(Bruker bruker)
        {
            try
            {
                Brukere funnetBruker = await _billettDb.Brukere.FirstOrDefaultAsync(b => b.Brukernavn == bruker.Brukernavn);

                //sjekk passordet
                byte[] hash = LagHash(bruker.Passord, funnetBruker.Salt);
                bool ok = hash.SequenceEqual(funnetBruker.Passord);
                if (ok)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }
    }
}
