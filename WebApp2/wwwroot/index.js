let valgtReise = null;
var lastet = false;
var kielArr = [];
var kobenhavnArr = [];
var osloArr = [];
var alleArr = [];


hentAlleReiser();

//Sortering av avreisedatoer
function compareDatoAvreise(a, b) {
    const aDatoTemp = a.datoAvreise.split('/');
    const bDatoTemp = b.datoAvreise.split('/');

    const aDato = aDatoTemp[2] + aDatoTemp[1] + aDatoTemp[0];
    const bDato = bDatoTemp[2] + bDatoTemp[1] + bDatoTemp[0];

    if (aDato < bDato) {
        return -1;
    }
    if (aDato > bDato) {
        return 1;
    }
    return 0;
}

function hentAlleReiser() {
    $.get("billett/hentAlleReiser", function (reiser) {
        init(reiser);
    });
}

function adminLogin() {
    const bruker = { Brukernavn: $("#usernameInput").val(), Passord: $("#passwordInput").val() };

    $.get("billett/LoggInn", bruker, function (finnes) {
        if (finnes) {
            location.href = "admin.html";
        } else {
            alert("Feil brukernavn eller passord");
            $("#usernameInput").val("");
            $("#passwordInput").val("");
        }
    });
}

function init(reiser) {
    let i = 0;
    for (let reise of reiser) {
        alleArr[i] = reise;
        i++;
    }

    alleArr.sort(compareDatoAvreise);

    if ($("#land").html() == "Tyskland") {

        kielArr = alleArr.filter(function (reise) {
            return reise.reiseTil == "Kiel";
        });

        visReiser(kielArr);
    }

    if ($("#land").html() == "Danmark") {

        kobenhavnArr = alleArr.filter(function (reise) {
            return reise.reiseTil == "København";
        });

        visReiser(kobenhavnArr);
    }

    if ($("#land").html() == "Norge") {

        osloArr = alleArr.filter(function (reise) {
            return reise.reiseTil == "Oslo";
        });

        visReiser(osloArr);
    }
}

function sokDato(innArr) {
    let dag = $("#dagInn").val();
    let mon = $("#manedInn").val();
    let ar = $("#arInn").val();

    if (dag < 10) {
        dag = "0" + dag;
    }

    if (mon < 10) {
        mon = "0" + mon;
    }

    const dato = dag + "/" + mon + "/" + ar;

    let sokArr = [];

    sokArr = innArr.filter(function (datoer) {
        return datoer.datoAvreise == dato;
    })

    if (sokArr.length > 0) {
        visReiser(sokArr);
        $("#sokeMsg").html("");
    }
    else {
        $("#sokeMsg").html("Det finnes ingen reiser for denne datoen");
    }
}

function visReiser(reiseArr) {
    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Kjøp billett</th><th>Reiser fra</th><th>Destinasjon</th><th>Avgang</th><th>Ankomst</th><th>Pris</th>" +
    "</tr>";

    for (let i = 0; i < reiseArr.length; i++) {
        ut += "<tr>" +
            "<td><button id=" + reiseArr[i].id + " class='btn btn-primary'>Kjøp her</button></td>" +
            "<td>" + reiseArr[i].reiseFra + "</td>" +
            "<td>" + reiseArr[i].reiseTil + "</td>" +
            "<td>" + reiseArr[i].datoAvreise + ", " + reiseArr[i].tidspunktFra + "</td>" +
            "<td>" + reiseArr[i].datoAnkomst + ", " + reiseArr[i].tidspunktTil + "</td>" +
            "<td>" + reiseArr[i].reisePris + " kr,-" + "</td>" +
        "</tr>";
    }

    ut += "</table>";
    $("#reisene").html(ut);

    // Utifra hvilken knapp som trykkes skjer ulike ting.
    $("button").click(function (event) {
        
        if (!event.detail || event.detail == 1) { //Skal hjelpe mot double clicks
            if (this.id == "knapp")
            {
                validerBillett();
            }
            else if (this.id == "resetTabell") {
                $("#sokeMsg").html("")
                return;
            }
            else if (this.id == "sokDato") {
                return;
            }

            else {
                id = this.id
                HentEnReise(id);

                //Animasjon som scroller til bunn av skjermen
                $('html, body').animate({
                    scrollTop: $(document).height()
                },
                    1000);
            }
        }
    });

    $("#antallBarn").change(function () {
        oppdaterPris();
    });
    $("#antallVoksne").change(function () {
        oppdaterPris();
    });
}

function oppdaterPris() {
    console.log(valgtReise.reisePris)
    let pris = antallVoksne.value * valgtReise.reisePris + antallBarn.value * (valgtReise.reisePris*0.5);
    $("#pris").html(pris + "kr");
}


//Viser utfyllingsfeltet for kjøp av billett, og viser bruker hvilken reise som er valgt.
function velgReise(reise) {
    $("#kjopForm").css("display", "block");
    $("#knapp").css("display", "block");
    $("#utDestinasjon").html(reise.reiseTil);
    $("#fraDestinasjon").html(reise.reiseFra);
    $("#fraTid").html(reise.tidspunktFra + ", " + reise.datoAvreise);
    $("#tilTid").html(reise.tidspunktTil + ", " + reise.datoAnkomst)
    valgtReise = reise;
}

function HentEnReise(reiseId) {
    $.get("billett/HentEnReise", { id: reiseId }, function (reise) {
        velgReise(reise);
    });
};

function validerBillett() {
    const fornavnOK = validerFornavn($("#fornavn").val());
    const etternavnOK = validerEtternavn($("#etternavn").val());
    const epostOK = validerEpost($("#epost").val());
    const mobilOK = validerMobilnummer($("#mobilnummer").val());
    const kortOK = validerKortnummer($("#kortnummer").val());
    const cvcOK = validerCvc($("#cvc").val());
    const månedOK = validerMåned($("#måned").val());
    const årOK = validerÅr($("#år").val());
    const antallOK = validerAntall($("#antallVoksne").val());

    if (fornavnOK && etternavnOK && epostOK && mobilOK && kortOK && cvcOK && månedOK && årOK && antallOK) {
        lagreBillett();
    }
}

function lagreBillett() {
    var pris = antallVoksne.value * valgtReise.reisePris + antallBarn.value * (valgtReise.reisePris * 0.5);
    var dato = $("#måned").val() + "/" + $("#år").val();
    const billett = {
        fornavn: $("#fornavn").val(),
        etternavn: $("#etternavn").val(),
        epost: $("#epost").val(),
        mobilnummer: $("#mobilnummer").val(),
        antallVoksne: $("#antallVoksne").val(),
        antallBarn: $("#antallBarn").val(),
        kortnummer: $("#kortnummer").val(),
        cvc: $("#cvc").val(),
        utlopsdato: dato,
        totalPris: pris,
        reiseId: valgtReise.id
    };

    const url = "billett/Lagre";

    $.post(url, billett, function () {
        window.location.href = 'kvittering.html';
    })
        .fail(function () {
            console.log("Noe feil skjedde under lagringen")
        });
};