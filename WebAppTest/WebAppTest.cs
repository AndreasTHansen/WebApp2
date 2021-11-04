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

        //for bruker
        private readonly Mock<IBillettRepository> mockRepB = new Mock<IBillettRepository>();
        private readonly Mock<ILogger<BrukerController>> mockLogB = new Mock<ILogger<BrukerController>>();

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
            Assert.Equal(true, resultat.Value);
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
            Assert.Equal(false, resultat.Value);
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
            Assert.Equal(false, resultat.Value);
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
            Assert.Equal(true, resultat.Value);
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
            Assert.Equal(false, resultat.Value);
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
            Assert.Equal(true, resultat.Value);
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
            Assert.Equal(false, resultat.Value);
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
            Assert.Equal(false, resultat.Value);
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


        //Kunde controller

        [Fact]
        public async Task HentAlleKunderLoggetInnOK()
        {
            var kunde1 = new Kunde
            {
                id = 1,
                fornavn = "test",
                etternavn = "person",
                epost = "test.person@testmail.com",
                mobilnummer = "12345678",
                kortnummer = "1234123412341234",
                utlopsdato = "12/21",
                cvc = 321
            };

            var kundeListe = new List<Kunde>();
            kundeListe.Add(kunde1);

            mockRepK.Setup(k => k.HentAlleKunder()).ReturnsAsync(kundeListe);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.HentAlleKunder() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<Kunde>>((List<Kunde>)resultat.Value, kundeListe);
        }

        [Fact]
        public async Task HentAllekunderLoggetInnFeilOK()
        {
            //Arrange

            mockRepK.Setup(k => k.HentAlleKunder()).ReturnsAsync(() => null);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.HentAlleKunder() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Null(resultat.Value);
        }

        [Fact]
        public async Task HentAlleKunderIkkeLoggetInnOK()
        {
            //Arrange
            mockRepK.Setup(k => k.HentAlleKunder()).ReturnsAsync(It.IsAny<List<Kunde>>());

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.HentAlleKunder() as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task HentEnKundeLoggetInnOK()
        {
            //Arrange

            var kunde1 = new Kunde
            {
                id = 1,
                fornavn = "test",
                etternavn = "person",
                epost = "test.person@testmail.com",
                mobilnummer = "12345678",
                kortnummer = "1234123412341234",
                utlopsdato = "12/21",
                cvc = 321
            };

            mockRepK.Setup(k => k.HentEnKunde(It.IsAny<int>())).ReturnsAsync(kunde1);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.HentEnKunde(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<Kunde>((Kunde)resultat.Value, kunde1);
        }

        [Fact]
        public async Task HentEnKundeLoggetInnFeilOK()
        {
            //Arrange
            mockRepK.Setup(k => k.HentEnKunde(It.IsAny<int>())).ReturnsAsync(() => null);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.HentEnKunde(It.IsAny<int>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal(false, resultat.Value);
        }

        [Fact]
        public async Task HentEnKundeIkkeLoggetInnOK()
        {
            //Arrange
            mockRepK.Setup(k => k.HentEnKunde(It.IsAny<int>())).ReturnsAsync(It.IsAny<Kunde>);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.HentEnKunde(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task SlettKundeLoggetInnOK()
        {
            mockRepK.Setup(k => k.SlettKunde(It.IsAny<int>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.SlettKunde(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task SlettKundeLoggetInnFeilOK()
        {
            mockRepK.Setup(k => k.SlettKunde(It.IsAny<int>())).ReturnsAsync(false);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.SlettKunde(It.IsAny<int>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal(false, resultat.Value);
        }

        [Fact]
        public async Task SlettKundeIkkeLoggetInnOK()
        {
            //Arrange
            mockRepK.Setup(k => k.SlettKunde(It.IsAny<int>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.SlettKunde(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task LagreKundeLoggetInnOK()
        {
            mockRepK.Setup(k => k.LagreKunde(It.IsAny<Kunde>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.LagreKunde(It.IsAny<Kunde>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task LagreKundeLoggetInnFeilModellOK()
        {
            // Arrange
            mockRepK.Setup(k => k.LagreKunde(It.IsAny<Kunde>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            kundeController.ModelState.AddModelError("KundeID", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.LagreKunde(It.IsAny<Kunde>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal(false, resultat.Value);
        }

        [Fact]
        public async Task LagreKundeLoggetInnFeilOK()
        {
            mockRepK.Setup(k => k.LagreKunde(It.IsAny<Kunde>())).ReturnsAsync(false);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.LagreKunde(It.IsAny<Kunde>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal(false, resultat.Value);
        }

        [Fact]
        public async Task LagreKundeIkkeLoggetInnOK()
        {
            //Arrange
            mockRepK.Setup(k => k.LagreKunde(It.IsAny<Kunde>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.LagreKunde(It.IsAny<Kunde>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }
        
        [Fact]
        public async Task HentAlleKunderLoggetInnFeilOK()
        {
            //Arrange
            var kundeListe = new List<Kunde>();

            mockRepK.Setup(k => k.HentAlleKunder()).ReturnsAsync(() => null);

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.HentAlleKunder() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Null(resultat.Value);
        }

        [Fact]
        public async Task HentAlleKunderIkkeLoggetInn()
        {
            //Arrange
            mockRepK.Setup(k => k.HentAlleKunder()).ReturnsAsync(It.IsAny<List<Kunde>>());

            var kundeController = new KundeController(mockRepK.Object, mockLogK.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await kundeController.HentAlleKunder() as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }
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
            Assert.Equal(true, resultat.Value);
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
            Assert.Equal(false, resultat.Value);
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
            Assert.Equal(false, resultat.Value);
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

        //Reise controller

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
            Assert.Equal(false, resultat.Value);
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
        public async Task EndreReiseLoggetInnOK()
        {
            mockRepR.Setup(k => k.EndreReise(It.IsAny<Reise>())).ReturnsAsync(true);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await reiseController.EndreReise(It.IsAny<Reise>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task EndreReiseLoggetInnFeilOK()
        {
            mockRepR.Setup(k => k.EndreReise(It.IsAny<Reise>())).ReturnsAsync(false);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await reiseController.EndreReise(It.IsAny<Reise>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal(false, resultat.Value);
        }

        [Fact]
        public async Task EndreReiseLoggetInnFeilModellOK()
        {
            // Arrange
            mockRepR.Setup(k => k.EndreReise(It.IsAny<Reise>())).ReturnsAsync(true);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            reiseController.ModelState.AddModelError("ReiseID", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await reiseController.EndreReise(It.IsAny<Reise>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal(false, resultat.Value);
        }

        [Fact]
        public async Task EndreReiseIkkeLoggetInnOK()
        {
            //Arrange
            mockRepR.Setup(k => k.EndreReise(It.IsAny<Reise>())).ReturnsAsync(true);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await reiseController.EndreReise(It.IsAny<Reise>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }
        [Fact]
        public async Task SlettReiseLoggetInnOK()
        {
            mockRepR.Setup(k => k.SlettReise(It.IsAny<int>())).ReturnsAsync(true);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await reiseController.SlettReise(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }
        [Fact]
        public async Task SlettReiseLoggetInnFeilOK()
        {
            mockRepR.Setup(k => k.SlettReise(It.IsAny<int>())).ReturnsAsync(false);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await reiseController.SlettReise(It.IsAny<int>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal(false, resultat.Value);
        }
        [Fact]
        public async Task SlettReiseIkkeLoggetInnOK()
        {
            //Arrange
            mockRepR.Setup(k => k.SlettReise(It.IsAny<int>())).ReturnsAsync(true);

            var reiseController = new ReiseController(mockRepR.Object, mockLogR.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            reiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await reiseController.SlettReise(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }


        //Bruker controller
        [Fact]
        public async Task LoggInnOK()
        {
            mockRepB.Setup(k => k.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var brukerController = new BrukerController(mockRepB.Object, mockLogB.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            brukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await brukerController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task LoggInnFeilModellOK()
        {
            // Arrange
            mockRepB.Setup(k => k.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var brukerController = new BrukerController(mockRepB.Object, mockLogB.Object);

            brukerController.ModelState.AddModelError("BrukerID", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            brukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await brukerController.LoggInn(It.IsAny<Bruker>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal(false, resultat.Value);
        }

        [Fact]
        public async Task LoggInnFeilOK()
        {
            mockRepB.Setup(k => k.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(false);

            var brukerController = new BrukerController(mockRepB.Object, mockLogB.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            brukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await brukerController.LoggInn(It.IsAny<Bruker>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal(false, resultat.Value);
        }

        [Fact]
        public async Task LoggUtOK()
        {
            mockRepB.Setup(k => k.LoggUt()).ReturnsAsync(true);

            var brukerController = new BrukerController(mockRepB.Object, mockLogB.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            brukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await brukerController.LoggUt() as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task LoggUtFeilOK()
        {
            mockRepB.Setup(k => k.LoggUt()).ReturnsAsync(false);

            var brukerController = new BrukerController(mockRepB.Object, mockLogB.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            brukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await brukerController.LoggUt() as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal(false, resultat.Value);
        }
    }
}
