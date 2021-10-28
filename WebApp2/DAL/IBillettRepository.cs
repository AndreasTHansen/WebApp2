using Kunde_SPA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kunde_SPA.DAL
{
    public interface IBillettRepository
    {
        Task<List<Billett>> HentAlle();
        Task<Billett> HentEnBillett(int id);
        Task<bool> Lagre(Billett innBillett);
        Task<bool> Slett(int id);
        Task<bool> EndreBillett(Billett endreBillett);
        Task<List<Reise>> HentAlleReiser();
        Task<Reise> HentEnReise(int id);
        Task<bool> LagreReise(Reise innReise);
        Task<bool> EndreReise(Reise endreReise);
        Task<bool> SlettReise(int id);
        Task<bool> LoggInn(Bruker bruker);
    }
}
