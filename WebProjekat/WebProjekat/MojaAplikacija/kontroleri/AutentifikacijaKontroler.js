forum.controller('AutentifikacijaKontroler', function ($scope,AutentifikacijaFabrika,$window,$rootScope) {
    
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
            if (odgovor.data == null) {

                alert('Korisnik sa tim korisnickim imenom i lozinkom ne postoji');
            } else {

                console.log(odgovor);
                document.cookie = "korisnik=" + JSON.stringify({
                    username: odgovor.data.KorisnickoIme,
                    uloga: odgovor.data.Uloga,
                    imePrezime: odgovor.data.Ime + " " + odgovor.data.Prezime
                }) + ";expires=Thu, 01 Jan 2019 00:00:01 GMT;";
                sessionStorage.setItem("korisnickoIme", odgovor.data.KorisnickoIme);
                sessionStorage.setItem("uloga", odgovor.data.Uloga);
                sessionStorage.setItem("imePrezime", odgovor.data.Ime + " " + odgovor.data.Prezime);

                $rootScope.ulogovan = true;
                $rootScope.korisnik = {
                    username: sessionStorage.getItem("username"),
                    uloga: sessionStorage.getItem("uloga"),
                    imePrezime: sessionStorage.getItem("imePrezime")
                };
                $window.location.href = "#!/";
            }

        })
    }

});