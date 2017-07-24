forum.controller('AutentifikacijaKontroler', function ($scope,AutentifikacijaFabrika) {
    
    function inicijalizacija() {
        console.log('Inicijalizovan autentifikacija kontroler');
    }

    inicijalizacija();

    $scope.korisnik = {};

    $scope.Registracija = function (korisnik) {
        //ovde validacije: da li je prazno,telefon,email i potvrda lozinke

        AutentifikacijaFabrika.RegistrujKorisnika(korisnik).then(function (odgovor) {
            console.log(odgovor.data);
        });
    }

    $scope.login = function (korisnik) {

    }

});