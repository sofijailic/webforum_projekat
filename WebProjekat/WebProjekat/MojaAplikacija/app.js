﻿var forum = angular.module('forum', ['ngRoute']); // inicijalizovana angular aplikacja

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
    })

});