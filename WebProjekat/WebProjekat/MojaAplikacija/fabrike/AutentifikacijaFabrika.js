forum.factory('AutentifikacijaFabrika', function ($http) {
    var factory = {};

    factory.RegistrujKorisnika = function (korisnik) {
        return $http.post('/api/Autentifikacija/Registracija', {
            KorisnickoIme: korisnik.username,
            Lozinka: korisnik.password,
            Ime: korisnik.ime,
            Prezime: korisnik.prezime,
            BrTelefona: korisnik.telefon,
            Email: korisnik.email
        });
    }

    factory.LogovanjeKorisnika = function (korisnik) {
        return $http.post('/api/Autentifikacija/Logovanje', {
            KorisnickoIme: korisnik.username,
            Lozinka: korisnik.password

        });

    }

    factory.uzmiKorisnikaNaOsnovuImena = function (username) {
        return $http.get('/api/Autentifikacija/uzmiKorisnikaNaOsnovuImena?username=' + username);

        
    }
    return factory;

});