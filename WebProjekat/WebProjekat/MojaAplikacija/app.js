var forum = angular.module('forum', ['ngRoute']); // inicijalizovana angular aplikacja

forum.config(function ($routeProvider) { // konfiguracija potrebnih stvari koje ce se koristiti u aplikaciji
    $routeProvider.when('/',
	{
	    redirectTo: '/pocetna'

	}).when('/login',
    {
        controller: 'AutentifikacijaKontroler',
        templateUrl: 'MojaAplikacija/stranice/login.html'
    }).when('/registracija', {

        controller: 'AutentifikacijaKontroler',
        templateUrl: 'MojaAplikacija/stranice/registracija.html'
    }).when('/podforumi', {

        controller: 'PodforumiKontroler',
        templateUrl: 'MojaAplikacija/stranice/podforumi.html'
    }).when('/dodajPodforum', {

        controller: 'PodforumiKontroler',
        templateUrl: 'MojaAplikacija/stranice/dodajPodforum.html'
    }).when('/podforumi/:naziv', {

        controller: 'PodforumKontroler',
        templateUrl: 'MojaAplikacija/stranice/prikazPodforuma.html'
    }).when('/podforumi/:naziv/dodajTemu', {

        controller: 'TemeKontroler',
        templateUrl: 'MojaAplikacija/stranice/dodajNovuTemu.html'
    }).when('/podforumi/:naziv/:naslovTeme', {

        controller: 'TemeKontroler',
        templateUrl: 'MojaAplikacija/stranice/prikazTeme.html'
    }).when('/profil/:username', {

        controller: 'ProfilKontroler',
        templateUrl: 'MojaAplikacija/stranice/profil.html',

    }).when('/pocetna', {
        controller: 'PocetnaKontroler',
        templateUrl: 'MojaAplikacija/stranice/pocetna.html',

    }).when('/pretraga', {

        controller: 'PretragaKontroler',
        templateUrl: 'MojaAplikacija/stranice/pretraga.html',

    }).when('/promenaTipa', {

        controller: 'PromenaTipaKontroler',
        templateUrl: 'MojaAplikacija/stranice/promenaTipa.html',

    }).when('/posaljiPoruku', {

        controller:'PorukeKontroler',
        templateUrl: 'MojaAplikacija/stranice/slanjePoruke.html',

    }).when('/zalbaNaPodforum', {

        controller: 'ZalbeKontroler',
        templateUrl: 'MojaAplikacija/stranice/zalbaNaPodforum.html'

    }).when('/zalbaNaTemu', {

        controller: 'ZalbeKontroler',
        templateUrl: 'MojaAplikacija/stranice/zalbaNaTemu.html'

    }).when('/zalbaNaKomentar', {

        controller: 'ZalbeKontroler',
        templateUrl: 'MojaAplikacija/stranice/zalbaNaKomentar.html'

    }).when('/zalbaNaPodkomentar', {

        controller: 'ZalbeKontroler',
        templateUrl: 'MojaAplikacija/stranice/zalbaNaPodkomentar.html'

    }).when('/zalbe', {

        controller: 'PrikazZalbiKontroler',
        templateUrl: 'MojaAplikacija/stranice/zalbe.html'

    }).when('/izmenaTeme', {

        controller: 'TemeKontroler',
        templateUrl: 'MojaAplikacija/stranice/izmenaTeme.html'

    }).when('/izmeniKomentar', {

        controller: 'TemeKontroler',
        templateUrl: 'MojaAplikacija/stranice/izmeniKomentar.html'

    }).when('/izmeniPodkomentar', {

        controller: 'TemeKontroler',
        templateUrl: 'MojaAplikacija/stranice/izmeniPodkomentar.html'

    }).when('/preporuke', {

        controller: 'PreporukeKontroler',
        templateUrl: 'MojaAplikacija/stranice/preporuke.html'
    })

});