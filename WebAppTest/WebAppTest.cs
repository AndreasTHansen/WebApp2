using Kunde_SPA.Controllers;
using Kunde_SPA.DAL;
using Kunde_SPA.Model;
using KundeApp2.Controllers;
using KundeApp2.DAL;
using KundeApp2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace WebAppTest
{
    public class WebAppTest
    {
        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";


        //for billett
        private readonly Mock<IBillettRepository> mockRep = new Mock<IBillettRepository>();
        private readonly Mock<ILogger<BillettController>> mockLog = new Mock<ILogger<BillettController>>();

        //for kunde
        private readonly Mock<IKundeRepository> mockRepK = new Mock<IKundeRepository>();
        private readonly Mock<ILogger<KundeController>> mockLogK = new Mock<ILogger<KundeController>>();

        //for reise
        private readonly Mock<IReiseRepository> mockRepR = new Mock<IReiseRepository>();
        private readonly Mock<ILogger<ReiseController>> mockLogR = new Mock<ILogger<ReiseController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();


        //Billett controller

        [Fact]
        public async Task LagreLoggetInnOK()
        {
            mockRep.Setup(k => k.Lagre(It.IsAny<Billett>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.Lagre(It.IsAny<Billett>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Billett lagret", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnFeilOK()
        {
            mockRep.Setup(k => k.Lagre(It.IsAny<Billett>())).ReturnsAsync(false);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await billettController.Lagre(It.IsAny<Billett>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Billetten kunne ikke lagres", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnFeilModellOK()
        {
            // Arrange
            mockRep.Setup(k => k.Lagre(It.IsAny<Billett>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            billettController.ModelState.AddModelError("ReiseID", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.Lagre(It.IsAny<Billett>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task LagreIkkeLoggetInnOK()
        {
            //Arrange
            mockRep.Setup(k => k.Lagre(It.IsAny<Billett>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await billettController.Lagre(It.IsAny<Billett>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task HentAlleLoggetInnOK()
        {
            //Arrange
            var billett1 = new Billett
            {
                id = 1,
                antallBarn = 0,
                antallVoksne = 1,
                totalPris = 3000,
                kundeId = 1,
                fornavn = "Test",
                etternavn = "Tester",
                epost = "test.tester@gmail.com",
                mobilnummer = "12345678",
                kortnummer = "1234123412341234",
                utlopsdato = "10/21",
                cvc = 123,
                reiseId = 1,
                reiseFra = "Oslo",
                reiseTil = "Kiel",
                datoAnkomst = "10/01/2021",
                datoAvreise = "09/01/2021",
                tidspunktFra = "19:00",
                tidspunktTil = "14:00",
                reisePris = 3000
            };
            var billett2 = new Billett
            {
                id = 2,
                antallBarn = 1,
                antallVoksne = 0,
                totalPris = 1000,
                kundeId = 2,
                fornavn = "Tester",
                etternavn = "Testerson",
                epost = "tester.testerson@gmail.com",
                mobilnummer = "87654321",
                kortnummer = "4321432143214321",
                utlopsdato = "05/22",
                cvc = 321,
                reiseId = 2,
                reiseFra = "Oslo",
                reiseTil = "København",
                datoAnkomst = "13/12/2021",
                datoAvreise = "12/12/2021",
                tidspunktFra = "12:00",
                tidspunktTil = "09:00",
                reisePris = 1000
            };

            var billettListe = new List<Billett>();
            billettListe.Add(billett1);
            billettListe.Add(billett2);

            mockRep.Setup(k => k.HentAlle()).ReturnsAsync(billettListe);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await billettController.HentAlle() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<Billett>>((List<Billett>)resultat.Value, billettListe);
        }

        [Fact]
        public async Task HentAlleLoggetInnFeilOK()
        {
            //Arrange
            var billettListe = new List<Billett>();

            mockRep.Setup(k => k.HentAlle()).ReturnsAsync(() => null);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await billettController.HentAlle() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Null(resultat.Value);
        }

        [Fact]
        public async Task HentAlleIkkeLoggetInn()
        {
            //Arrange
            mockRep.Setup(k => k.HentAlle()).ReturnsAsync(It.IsAny<List<Billett>>());

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await billettController.HentAlle() as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task SlettLoggetInnOK()
        {
            mockRep.Setup(k => k.Slett(It.IsAny<int>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.Slett(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Billett slettet", resultat.Value);
        }

        [Fact]
        public async Task SlettLoggetInnFeilOK()
        {
            mockRep.Setup(k => k.Slett(It.IsAny<int>())).ReturnsAsync(false);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await billettController.Slett(It.IsAny<int>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Billetten ble ikke slettet", resultat.Value);
        }

        [Fact]
        public async Task SlettIkkeLoggetInnOK()
        {
            //Arrange
            mockRep.Setup(k => k.Slett(It.IsAny<int>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await billettController.Slett(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task EndreBillettLoggetInnOK()
        {
            mockRep.Setup(k => k.EndreBillett(It.IsAny<Billett>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await billettController.EndreBillett(It.IsAny<Billett>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Billetten ble endret", resultat.Value);
        }

        [Fact]
        public async Task EndreBillettLoggetInnFeilOK()
        {
            mockRep.Setup(k => k.EndreBillett(It.IsAny<Billett>())).ReturnsAsync(false);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await billettController.EndreBillett(It.IsAny<Billett>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Billetten ble ikke endret", resultat.Value);
        }

        [Fact]
        public async Task EndreBillettLoggetInnFeilModellOK()
        {
            // Arrange
            mockRep.Setup(k => k.EndreBillett(It.IsAny<Billett>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            billettController.ModelState.AddModelError("ReiseID", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.EndreBillett(It.IsAny<Billett>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task EndreBillettIkkeLoggetInnOK()
        {
            //Arrange
            mockRep.Setup(k => k.EndreBillett(It.IsAny<Billett>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await billettController.EndreBillett(It.IsAny<Billett>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task HentEnReiseLoggetInnOK()
        {
            //Arrange
            var reise = new Reise
            {
                id = 1,
                reiseFra = "Oslo",
                reiseTil = "Kiel",
                tidspunktFra = "09:00",
                tidspunktTil = "15:00",
                datoAnkomst = "01/01/2022",
                datoAvreise = "31/12/2021",
                reisePris = 1000
            };

            mockRepR.Setup(k => k.HentEnReise(It.IsAny<int>())).ReturnsAsync(reise);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await reiseController.HentEnReise(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<Reise>((Reise)resultat.Value, reise);
        }

        [Fact]
        public async Task HentEnReiseLoggetInnFeilOK()
        {
            //Arrange
            mockRepR.Setup(k => k.HentEnReise(It.IsAny<int>())).ReturnsAsync(() => null);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await reiseController.HentEnReise(It.IsAny<int>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Fant ikke reisen i databasen", resultat.Value);
        }

        [Fact]
        public async Task HentEnReiseIkkeLoggetInnOK()
        {
            //Arrange
            mockRepR.Setup(k => k.HentEnReise(It.IsAny<int>())).ReturnsAsync(It.IsAny<Reise>);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await reiseController.HentEnReise(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task HentAlleReiserLoggetInnOK()
        {
            //Arrange
            var reise1 = new Reise
            {
                id = 1,
                reiseFra = "Oslo",
                reiseTil = "Kiel",
                tidspunktFra = "09:00",
                tidspunktTil = "15:00",
                datoAnkomst = "01/01/2022",
                datoAvreise = "31/12/2021",
                reisePris = 1000
            };

            var reise2 = new Reise
            {
                id = 2,
                reiseFra = "Oslo",
                reiseTil = "København",
                tidspunktFra = "09:00",
                tidspunktTil = "18:00",
                datoAnkomst = "01/01/2022",
                datoAvreise = "31/12/2021",
                reisePris = 3000
            };

            var reiseListe = new List<Reise>();
            reiseListe.Add(reise1);
            reiseListe.Add(reise2);

            mockRepR.Setup(k => k.HentAlleReiser()).ReturnsAsync(reiseListe);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await reiseController.HentAlleReiser() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<Reise>>((List<Reise>)resultat.Value, reiseListe);
        }

        [Fact]
        public async Task HentAlleReiserLoggetInnFeilOK()
        {
            //Arrange
            var reiseListe = new List<Reise>();

            mockRepR.Setup(k => k.HentAlleReiser()).ReturnsAsync(() => null);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await reiseController.HentAlleReiser() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Null(resultat.Value);
        }

        [Fact]
        public async Task HentAlleReiserIkkeLoggetInnOK()
        {
            //Arrange
            mockRepR.Setup(k => k.HentAlleReiser()).ReturnsAsync(It.IsAny<List<Reise>>());

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await reiseController.HentAlleReiser() as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task LoggInnOK()
        {
            mockRep.Setup(k => k.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task LoggInnFeilOK()
        {
            mockRep.Setup(k => k.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(false);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.False((bool)resultat.Value);
        }

        [Fact]
        public async Task LoggInnInputFeilOK()
        {
            mockRep.Setup(k => k.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var billettController = new BillettController(mockRep.Object, mockLog.Object);

            billettController.ModelState.AddModelError("Brukernavn", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.LoggInn(It.IsAny<Bruker>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }


        //Kunde controller

        [Fact]
        public async Task EndreKundeLoggetInnOK()
        {
            mockRepK.Setup(k => k.EndreKunde(It.IsAny<Kunde>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.EndreKunde(It.IsAny<Kunde>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Kunden ble endret", resultat.Value);
        }

        [Fact]
        public async Task EndreKundeLoggetInnFeilOK()
        {
            mockRepK.Setup(k => k.EndreKunde(It.IsAny<Kunde>())).ReturnsAsync(false);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.EndreKunde(It.IsAny<Kunde>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Kunden kunne ikke endres", resultat.Value);
        }

        [Fact]
        public async Task EndreKundeLoggetInnFeilModelOK()
        {
            // Arrange
            mockRepK.Setup(k => k.EndreKunde(It.IsAny<Kunde>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            kundeController.ModelState.AddModelError("Fornavn", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.EndreKunde(It.IsAny<Kunde>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task EndreKundeIkkeLoggetInnOK()
        {
            //Arrange
            mockRepK.Setup(k => k.EndreKunde(It.IsAny<Kunde>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.EndreKunde(It.IsAny<Kunde>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }
    }
}
