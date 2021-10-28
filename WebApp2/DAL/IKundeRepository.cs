using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KundeApp2.Model;

namespace KundeApp2.DAL
{
    public interface IKundeRepository
    {
        Task<List<Kunde>> HentAlleKunder();
        Task<Kunde> HentEnKunde(int id);
        Task<bool> EndreKunde(Kunde endreKunde);
        Task<bool> SlettKunde(int id);
        Task<bool> LagreKunde(Kunde innKunde);
    }
}
