using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KundeApp2.DAL;
using KundeApp2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KundeApp2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class KundeController : ControllerBase
    {
        private IKundeRepository _billettDb;

        private ILogger<KundeController> _log;

        private const string _loggetInn = "loggetInn";

        public KundeController(IKundeRepository billettDb, ILogger<KundeController> log)
        {
            _billettDb = billettDb;
            _log = log;
        }

        [HttpGet]
        public async Task<ActionResult> HentAlleKunder()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke logget inn");
            }

            List<Kunde> alleKunder = await _billettDb.HentAlleKunder();
            if (alleKunder == null)
            {
                return NotFound(false);
            }
            return Ok(alleKunder);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> HentEnKunde(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke logget inn");
            }

            Kunde hentetKunde = await _billettDb.HentEnKunde(id);

            if (hentetKunde == null)
            {
                _log.LogInformation("Fant ikke reisen i databasen");
                return NotFound(false);
            }
            return Ok(hentetKunde);
        }

        [HttpPut]
        public async Task<ActionResult> EndreKunde(Kunde endreKunde)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke logget inn");
            }
            if (ModelState.IsValid)
            {
                bool endreOK = await _billettDb.EndreKunde(endreKunde);
                if (!endreOK)
                {
                    _log.LogInformation("Det skjedde noe feil under endringen");
                    return NotFound(false);
                }
                _log.LogInformation("Kunde har blitt endret");
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest(false);
        }
        [HttpPost]
        public async Task<ActionResult> LagreKunde(Kunde innKunde)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke logget inn");
            }
            if (ModelState.IsValid)
            {
                bool lagreOK = await _billettDb.LagreKunde(innKunde);
                if (!lagreOK)
                {
                    _log.LogInformation("Det skjedde noe feil under lagringen");
                    return BadRequest(false);
                }
                _log.LogInformation("Kunde har blitt lagret");
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest(false);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> SlettKunde(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke logget inn");
            }
            bool slettOk = await _billettDb.SlettKunde(id);
            if (!slettOk)
            {
                _log.LogInformation("Kunden ble ikke slettet");
                return NotFound(false);
            }
            _log.LogInformation("Kunde har blitt slettet");
            return Ok(true);
        }
    }
}
