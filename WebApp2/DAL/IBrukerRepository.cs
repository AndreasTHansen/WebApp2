using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kunde_SPA.Model;

namespace KundeApp2.DAL
{
    public interface IBrukerRepository
    {
        Task<bool> LoggInn(Bruker bruker);
    }
}