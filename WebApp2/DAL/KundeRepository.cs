﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KundeApp2.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KundeApp2.DAL
{
    public class KundeRepository : IKundeRepository
    {
        private readonly DBContext _billettDb;

        private ILogger<KundeRepository> _log;

        public KundeRepository(DBContext billettDb, ILogger<KundeRepository> log)
        {
            _billettDb = billettDb;
            _log = log;

        }

        public async Task<List<Kunde>> HentAlleKunder()
        {
            try
            {
                List<Kunde> alleKunder = await _billettDb.Kunder.Select(k => new Kunde
                {
                    id = k.id,
                    fornavn = k.fornavn,
                    etternavn = k.etternavn,
                    epost = k.epost,
                    mobilnummer = k.mobilnummer,
                    kortnummer = k.kort.kortnummer,
                    utlopsdato = k.kort.utlopsdato,
                    cvc = k.kort.cvc
                }).ToListAsync();

                return alleKunder;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<Kunde> HentEnKunde(int id)
        {
            try
            {
                Kunder enKunde = await _billettDb.Kunder.FindAsync(id);
                var hentetKunde = new Kunde()
                {
                    id = enKunde.id,
                    fornavn = enKunde.fornavn,
                    etternavn = enKunde.etternavn,
                    epost = enKunde.epost,
                    mobilnummer = enKunde.mobilnummer,
                    kortnummer = enKunde.kort.kortnummer,
                    utlopsdato = enKunde.kort.utlopsdato,
                    cvc = enKunde.kort.cvc
                };

                return hentetKunde;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> EndreKunde(Kunde endreKunde)
        {
            try
            {
                var endreObjekt = await _billettDb.Kunder.FindAsync(endreKunde.id);
                //Sjekke om kortet finnes fra før
                if (endreObjekt.kort.kortnummer != endreKunde.kortnummer)
                {
                    var kortRad = new Kort();
                    kortRad.kortnummer = endreKunde.kortnummer;
                    kortRad.cvc = endreKunde.cvc;
                    kortRad.utlopsdato = endreKunde.utlopsdato;
                    endreObjekt.kort = kortRad;
                }
                else
                {
                    endreObjekt.kort.kortnummer = endreKunde.kortnummer;
                }
                endreObjekt.fornavn = endreKunde.fornavn;
                endreObjekt.etternavn = endreKunde.etternavn;
                endreObjekt.epost = endreKunde.epost;
                endreObjekt.mobilnummer = endreKunde.mobilnummer;
                await _billettDb.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
            return true;
        }

        public async Task<bool> SlettKunde(int id)
        {


            try
            {
                Kunder enKunde = await _billettDb.Kunder.FindAsync(id);
                _billettDb.Remove(enKunde);
                await _billettDb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LagreKunde(Kunde innKunde)
        {
            try
            {
                var nyKunde = new Kunder();
                nyKunde.fornavn = innKunde.fornavn;
                nyKunde.etternavn = innKunde.etternavn;
                nyKunde.epost = innKunde.epost;
                nyKunde.mobilnummer = innKunde.mobilnummer;


                var sjekkKort = await _billettDb.Kort.FindAsync(innKunde.kortnummer);
                if (sjekkKort == null)
                {
                    Kort kortRad = new Kort();
                    kortRad.kortnummer = innKunde.kortnummer;
                    kortRad.cvc = innKunde.cvc;
                    kortRad.utlopsdato = innKunde.utlopsdato;
                    nyKunde.kort = kortRad;
                }
                else
                {
                    nyKunde.kort = sjekkKort;
                }

                _billettDb.Add(nyKunde);
                await _billettDb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}