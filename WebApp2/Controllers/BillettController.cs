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


        public BillettController(IBillettRepository billettDb, ILogger<BillettController> log)
        {
            _billettDb = billettDb;
            _log = log;
        }

        [HttpGet]
        public async Task<ActionResult> HentAlle()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                bool returOK = await _billettDb.Lagre(innBillett);
                if (!returOK)
                {
                    _log.LogInformation("Billetten kunne ikke lagres!");
                    return BadRequest();
                }
                _log.LogInformation("Billett ble lagret");
                return Ok();
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Slett(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            bool returOK = await _billettDb.Slett(id);
            if (!returOK)
            {
                _log.LogInformation("Billetten ble ikke slettet");
                return NotFound("Billetten ble ikke slettet");
            }
            _log.LogInformation("Billetten ble slettet");
            return Ok("Billetten ble slettet");
        }
        [HttpPut]
        public async Task<ActionResult> EndreBillett(Billett endreBillett)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                bool endreOk = await _billettDb.EndreBillett(endreBillett);
                if (!endreOk)
                {
                    _log.LogInformation("Billetten kunne ikke bli endret");
                    return NotFound();
                }
                _log.LogInformation("Billetten har blitt endret");
                return Ok();
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> HentEnBillett(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            Billett hentetBillett = await _billettDb.HentEnBillett(id);

            if(hentetBillett == null)
            {
                _log.LogInformation("Fant ikke billetten i databasen");
                return NotFound();
            }
            return Ok(hentetBillett);
        }
    }
}
