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
    public class BrukerRepository : IBrukerRepository
    {

        private readonly DBContext _billettDb;

        private ILogger<BillettRepository> _log;

        public BrukerRepository(DBContext billettDb, ILogger<BillettRepository> log)
        {
            _billettDb = billettDb;
            _log = log;

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
