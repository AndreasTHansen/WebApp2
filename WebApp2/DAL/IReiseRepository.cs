using Kunde_SPA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kunde_SPA.DAL
{
    public interface IReiseRepository
    {
        Task<List<Reise>> HentAlleReiser();
        Task<Reise> HentEnReise(int id);
        Task<bool> LagreReise(Reise innReise);
        Task<bool> EndreReise(Reise endreReise);
        Task<bool> SlettReise(int id);
    }
}
