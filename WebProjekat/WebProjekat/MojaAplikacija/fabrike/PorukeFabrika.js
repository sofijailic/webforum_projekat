forum.factory('PorukeFabrika', function ($http) {

    var factory = {};

    factory.posaljiPoruku = function (poruka) {

        return $http.post('/api/Poruke/PosaljiPoruku',{
            Posiljalac:poruka.posiljalac,
            Primalac:poruka.primalac,
            Sadrzaj:poruka.tekst,
            Procitana:poruka.procitana
        });
    }

    factory.uzmiPorukeZaKorisnika = function (username) {

        return $http.get('/api/Poruke/UzmiSvePoruke?username='+username);

    }

    factory.OznaciKaoProcitano = function (id) {
        return $http.post('/api/Poruke/OznaciKaoProcitano?id=' + id);
    }
    return factory;
});