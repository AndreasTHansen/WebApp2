using Kunde_SPA.DAL;
using Kunde_SPA.Model;
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
    public class ReiseController : ControllerBase
    {
        private IReiseRepository _billettDb;

        private ILogger<ReiseController> _log;

        private const string _loggetInn = "loggetInn";

        public ReiseController(IReiseRepository billettDb, ILogger<ReiseController> log)
        {
            _billettDb = billettDb;
            _log = log;
        }

        [HttpGet]
        public async Task<ActionResult> HentAlleReiser()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke logget inn");
            }

            List<Reise> reiseListe = await _billettDb.HentAlleReiser();
            return Ok(reiseListe);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> HentEnReise(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke logget inn");
            }

            Reise hentetReise = await _billettDb.HentEnReise(id);

            if (hentetReise == null)
            {
                _log.LogInformation("Fant ikke reisen i databasen");
                return NotFound(false);
            }
            return Ok(hentetReise);
        }
        [HttpPost]
        public async Task<ActionResult> LagreReise(Reise innReise)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke logget inn");
            }

            bool lagreOk = await _billettDb.LagreReise(innReise);
            if (!lagreOk)
            {
                _log.LogInformation("Reisen kunne ikke lagres!");
                return BadRequest(false);
            }
            _log.LogInformation("Reise har blitt lagret"); 
            return Ok(true);
        }

        [HttpPut]
        public async Task<ActionResult> EndreReise(Reise endreReise)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke logget inn");
            }

            if (ModelState.IsValid)
            {
                bool endreOk = await _billettDb.EndreReise(endreReise);
                if (!endreOk)
                {
                    _log.LogInformation("Reisen kunne ikke bli endret");
                    return NotFound(false);
                }
                _log.LogInformation("Reisen har blitt endret");
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest(false);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> SlettReise(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke logget inn");
            }

            bool returOK = await _billettDb.SlettReise(id);
            if (!returOK)
            {
                _log.LogInformation("Reisen ble ikke slettet");
                return NotFound(false);
            }
            _log.LogInformation("Reisen har blitt slettet");
            return Ok(true);
        }
    }
}
