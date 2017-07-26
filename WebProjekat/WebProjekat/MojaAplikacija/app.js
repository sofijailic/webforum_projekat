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
    })

});