using Kunde_SPA.DAL;
using Kunde_SPA.Model;
using KundeApp2.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kunde_SPA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class BrukerController : ControllerBase
    {

        private readonly IBillettRepository _billettDb;

        private ILogger<BrukerController> _log;

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        public BrukerController(IBillettRepository billettDb, ILogger<BrukerController> log)
        {
            _billettDb = billettDb;
            _log = log;
        }

        //Hentet fra fagstoff
        [HttpPost]
        public async Task<ActionResult> LoggInn(Bruker bruker)
        {

            if (ModelState.IsValid)
            {
                bool returnOK = await _billettDb.LoggInn(bruker);
                if (!returnOK)
                {
                    _log.LogInformation("Feil brukernavn eller passord");
                    HttpContext.Session.SetString(_loggetInn, _ikkeLoggetInn);
                    return BadRequest(false);
                }
                HttpContext.Session.SetString(_loggetInn, _loggetInn);
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest(false);
        }

        [HttpGet]
        public async Task<ActionResult> LoggUt()
        {

            if (ModelState.IsValid)
            {
                bool returnOK = await _billettDb.LoggUt();
                if (!returnOK)
                {
                    _log.LogInformation("Kunne ikke logge ut");
                    return BadRequest(false);
                }
                HttpContext.Session.SetString(_ikkeLoggetInn, _ikkeLoggetInn);
                return Ok(true);
            }
            return BadRequest("Kunne ikke logge ut");
        }





    }

}
