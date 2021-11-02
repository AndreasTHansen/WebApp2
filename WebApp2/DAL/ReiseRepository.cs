using Kunde_SPA.Model;
using KundeApp2.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kunde_SPA.DAL
{
    public class ReiseRepository : IReiseRepository
    {
        private readonly DBContext _billettDb;

        private ILogger<ReiseRepository> _log;

        public ReiseRepository(DBContext billettDb, ILogger<ReiseRepository> log)
        {
            _billettDb = billettDb;
            _log = log;

        }

        public async Task<List<Reise>> HentAlleReiser()
        {
            try
            {
                List<Reise> alleReiser = await _billettDb.Reiser.Select(k => new Reise
                {
                    id = k.id,
                    reiseFra = k.reiseFra,
                    reiseTil = k.reiseTil,
                    tidspunktFra = k.tidspunktFra,
                    tidspunktTil = k.tidspunktTil,
                    datoAnkomst = k.datoAnkomst,
                    datoAvreise = k.datoAvreise,
                    reisePris = k.reisePris
                }).ToListAsync();

                return alleReiser;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Reise> HentEnReise(int id)
        {
            try
            {
                Reiser enReise = await _billettDb.Reiser.FindAsync(id);
                var hentetReise = new Reise()
                {
                    id = enReise.id,
                    reiseFra = enReise.reiseFra,
                    reiseTil = enReise.reiseTil,
                    tidspunktFra = enReise.tidspunktFra,
                    tidspunktTil = enReise.tidspunktTil,
                    datoAnkomst = enReise.datoAnkomst,
                    datoAvreise = enReise.datoAvreise,
                    reisePris = enReise.reisePris
                };

                return hentetReise;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> EndreReise(Reise endreReise)
        {
            try
            {
                var endreObjekt = await _billettDb.Reiser.FindAsync(endreReise.id);
                endreObjekt.reiseFra = endreReise.reiseFra;
                endreObjekt.reiseTil = endreReise.reiseFra;
                endreObjekt.tidspunktFra = endreReise.tidspunktFra;
                endreObjekt.tidspunktTil = endreReise.tidspunktTil;
                endreObjekt.datoAnkomst = endreReise.datoAnkomst;
                endreObjekt.datoAvreise = endreReise.datoAvreise;
                endreObjekt.reisePris = endreReise.reisePris;
                await _billettDb.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
            return true;
        }

        public async Task<bool> SlettReise(int id)
        {
            try
            {
                Reiser enReise = await _billettDb.Reiser.FindAsync(id);
                _billettDb.Remove(enReise);
                await _billettDb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LagreReise(Reise innReise)
        {
            try
            {
                Reiser nyReise = new Reiser();

                nyReise.reiseFra = innReise.reiseFra;
                nyReise.reiseTil = innReise.reiseTil;
                nyReise.datoAnkomst = innReise.datoAnkomst;
                nyReise.datoAvreise = innReise.datoAvreise;
                nyReise.tidspunktFra = innReise.tidspunktFra;
                nyReise.tidspunktTil = innReise.tidspunktTil;
                nyReise.reisePris = innReise.reisePris;

                _billettDb.Reiser.Add(nyReise);
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
