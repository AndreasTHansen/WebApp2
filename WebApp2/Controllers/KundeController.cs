﻿using System;
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
                return Unauthorized();
            }

            List<Kunde> alleKunder = await _billettDb.HentAlleKunder();
            if (alleKunder == null)
            {
                return NotFound();
            }
            return Ok(alleKunder);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> HentEnKunde(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            Kunde hentetKunde = await _billettDb.HentEnKunde(id);

            if (hentetKunde == null)
            {
                _log.LogInformation("Fant ikke reisen i databasen");
                return NotFound();
            }
            return Ok(hentetKunde);
        }

        [HttpPut]
        public async Task<ActionResult> EndreKunde(Kunde endreKunde)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                bool endreOK = await _billettDb.EndreKunde(endreKunde);
                if (!endreOK)
                {
                    _log.LogInformation("Det skjedde noe feil under endringen");
                    return NotFound();
                }
                _log.LogInformation("Kunde har blitt endret");
                return Ok();
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }
        [HttpPost]
        public async Task<ActionResult> LagreKunde(Kunde innKunde)
        {
            bool lagreOK = await _billettDb.LagreKunde(innKunde);
            if (!lagreOK)
            {
                _log.LogInformation("Det skjedde noe feil under lagringen");
                return BadRequest();
            }
            _log.LogInformation("Kunde har blitt lagret");
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> SlettKunde(int id)
        {
            bool slettOk = await _billettDb.SlettKunde(id);
            if (!slettOk)
            {
                _log.LogInformation("Kunden ble ikke slettet");
                return NotFound();
            }
            _log.LogInformation("Kunde har blitt slettet");
            return Ok();
        }
    }
}
