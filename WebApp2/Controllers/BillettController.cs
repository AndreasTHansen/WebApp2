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

    public class BillettController : ControllerBase
    {
        private readonly IBillettRepository _billettDb;

        private ILogger<BillettController> _log;

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        public BillettController(IBillettRepository billettDb, ILogger<BillettController> log)
        {
            _billettDb = billettDb;
            _log = log;
        }

        [HttpGet]
        public async Task<ActionResult> HentAlle()
        {

            List<Billett> alleBilletter = await _billettDb.HentAlle();
            if (alleBilletter == null)
            {
                return NotFound();
            }
            return Ok(alleBilletter);
        }

        [HttpPost]
        public async Task<ActionResult> Lagre(Billett innBillett)
        {
            if (ModelState.IsValid)
            {
                bool returOK = await _billettDb.Lagre(innBillett);
                if (!returOK)
                {
                    _log.LogInformation("Billetten kunne ikke lagres!");
                    return BadRequest();
                }
                return Ok();
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Slett(int id)
        {
            bool returOK = await _billettDb.Slett(id);
            if (!returOK)
            {
                _log.LogInformation("Billetten ble ikke slettet");
                return NotFound();
            }
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> EndreBillett(Billett endreBillett)
        {
            if (ModelState.IsValid)
            {
                bool endreOk = await _billettDb.EndreBillett(endreBillett);
                if (!endreOk)
                {
                    _log.LogInformation("Billetten kunne ikke bli endret");
                    return NotFound();
                }
                return Ok("Billetten ble endret");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        public async Task<ActionResult> HentEnBillett(int id)
        {
            Billett hentetBillett = await _billettDb.HentEnBillett(id);

            if(hentetBillett == null)
            {
                _log.LogInformation("Fant ikke billetten i databasen");
                return NotFound();
            }
            return Ok(hentetBillett);
        }
        public async Task<ActionResult> HentAlleReiser()
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            //{
            //    return Unauthorized("Ikke logget inn");
            //}
            List<Reise> reiseListe = await _billettDb.HentAlleReiser();
            return Ok(reiseListe);
        }

        public async Task<ActionResult> HentEnReise(int id)
        {
            Reise hentetReise = await _billettDb.HentEnReise(id);

            if (hentetReise == null)
            {
                _log.LogInformation("Fant ikke reisen i databasen");
                return NotFound();
            }
            return Ok(hentetReise);
        }

        public async Task<ActionResult> LagreReise(Reise innReise)
        {
            bool lagreOk = await _billettDb.LagreReise(innReise);
            if (!lagreOk)
            {
                _log.LogInformation("Reisen kunne ikke lagres!");
                return BadRequest();
            }
            return Ok();
        }

        //EndreReise
        public async Task<ActionResult> EndreReise(Reise endreReise)
        {
            if (ModelState.IsValid)
            {
                bool endreOk = await _billettDb.EndreReise(endreReise);
                if (!endreOk)
                {
                    _log.LogInformation("Reisen kunne ikke bli endret");
                    return NotFound();
                }
                return Ok();
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        public async Task<ActionResult> SlettReise(int id)
        {
            bool returOK = await _billettDb.SlettReise(id);
            if (!returOK)
            {
                _log.LogInformation("Reisen ble ikke slettet");
                return NotFound();
            }
            return Ok();
        }

        //Hentet fra fagstoff
        public async Task<ActionResult> LoggInn(Bruker bruker)
        {
            if (ModelState.IsValid)
            {
                bool returnOK = await _billettDb.LoggInn(bruker);
                if (!returnOK)
                {
                    _log.LogInformation("Innloggingen feilet for bruker");
                    HttpContext.Session.SetString(_loggetInn, _ikkeLoggetInn);
                    return Ok(false);
                }
                HttpContext.Session.SetString(_loggetInn, _loggetInn);
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }
    }
}
