forum.factory('ZalbeFabrika', function ($http) {

    var factory = {};

    factory.posaljiZalbuNaPodforum = function (zalba) {
        return $http.post('/api/Zalbe/ZalbaNaPodforum', {
            Tekst: zalba.tekst,
            Entitet: zalba.entitet,
            KorisnikKojiJeUlozio: zalba.korisnikKojiSeZali
        })
    }

    factory.posaljiZalbuNaTemu = function (zalba) {
        return $http.post('/api/Zalbe/ZalbaNaTemu', {
            Tekst: zalba.tekst,
            Entitet: zalba.entitet,
            KorisnikKojiJeUlozio: zalba.korisnikKojiSeZali
        })
    }

    factory.posaljiZalbuNaKomentar = function (zalba) {
        return $http.post('/api/Zalbe/ZalbaNaKomentar', {
            Tekst: zalba.tekst,
            Entitet: zalba.entitet,
            KorisnikKojiJeUlozio: zalba.korisnikKojiSeZali
        })
    }

    factory.posaljiZalbuNaPodkomentar = function (zalba) {
        return $http.post('/api/Zalbe/ZalbaNaPodkomentar', {
            Tekst: zalba.tekst,
            Entitet: zalba.entitet,
            KorisnikKojiJeUlozio: zalba.korisnikKojiSeZali
        })
    }

    factory.uzmiZalbeZaKorisnika = function (username) {
        return $http.get('/api/Zalbe/UzmiZalbeZaKorisnika?username=' + username);
    }

    factory.obrisiZalbu = function (zalba) {
        return $http.post('/api/Zalbe/ObrisiZalbu', {
            Id: zalba.Id
        });
    }

    return factory;
});