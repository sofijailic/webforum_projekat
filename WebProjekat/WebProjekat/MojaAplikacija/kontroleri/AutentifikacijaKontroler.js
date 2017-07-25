forum.controller('AutentifikacijaKontroler', function ($scope,AutentifikacijaFabrika,$window) {
    
    function inicijalizacija() {
        console.log('Inicijalizovan autentifikacija kontroler');
    }

    inicijalizacija();

    $scope.korisnik = {};

    $scope.Registracija = function (korisnik) {
        //ovde validacije: da li je prazno,telefon,email i potvrda lozinke

        AutentifikacijaFabrika.RegistrujKorisnika(korisnik).then(function (odgovor) {
            if (odgovor.data == true) {
                $window.location.href = "#!/login";

            }
        });
    }

    $scope.Login = function (korisnik) {

        AutentifikacijaFabrika.LogovanjeKorisnika(korisnik).then(function (odgovor) {
            console.log(odgovor.data);

        })
    }

});