forum.factory('KomentariFabrika', function ($http) {

    var factory = {};

    factory.OstaviKomentar = function (naslovTeme,podforum,tekstKomentara,autor) {
        return $http.post('/api/Komentari/DodajKomentar', {
            TemaKojojPripada: podforum + '-' + naslovTeme,
            Autor:autor,
            Tekst:tekstKomentara

        });

    }

    factory.dodajPodkomentar = function (IdRoditelja, tekstPodkomentara, autor, temaKojojPripada) {
        return $http.post('/api/Komentari/DodajPodkomentar', {
            RoditeljskiKomentar: IdRoditelja,
            TemaKojojPripada: temaKojojPripada,
            Tekst: tekstPodkomentara,
            Autor: autor
        })
    }

    factory.uzmiKomentareZaTemu = function (podforum,tema) {
        return $http.get('/api/Komentari/UzmiKomentare/?idTeme=' + podforum + '-' + tema);
    }



    factory.lajkujKomentar = function (komentar, username) {

        return $http.post('/api/Komentari/LajkujKomentar', {
            IdKomentara: komentar.Id,
            KoVrsiAkciju: username
        })

    }

    factory.dislajkujKomentar = function (komentar, username) {

        return $http.post('/api/Komentari/DislajkujKomentar', {
            IdKomentara: komentar.Id,
            KoVrsiAkciju: username
        })

    }

    factory.sacuvajKomentar = function (idKomentara, username) {
        return $http.post('/api/Komentari/SacuvajKomentar', {
            IdKomentara: idKomentara,
            KoCuva: username
        });
    }

    factory.uzmiSacuvaneKomentare = function (username) {
        return $http.get('/api/Komentari/UzmiSacuvaneKomentare?username=' + username);
    }

    factory.uzmiSacuvanePodkomentare = function (username) {
        return $http.get('/api/Komentari/UzmiSacuvanePodkomentare?username=' + username);
    }

    factory.uzmiLajkovaniKomentari = function (username) {
        return $http.get('/api/Komentari/UzmiLajkovaniKomentari?username=' + username);
    }

    factory.uzmiDislajkovaniKomentari = function (username) {
        return $http.get('/api/Komentari/UzmiDislajkovaniKomentari?username=' + username);
    }

    factory.obrisiPodkomentar = function (podkomentar) {
        return $http.post('/api/Komentari/ObrisiPodkomentar', {
            Id: podkomentar.Id
        });
    }

    factory.obrisiKomentar = function (komentar) {
        return $http.post('/api/Komentari/ObrisiKomentar', {
            Id: komentar.Id
        });
    }

    factory.izmeniKomentar = function (komentarZaIzmenu) {
        return $http.post('/api/Komentari/IzmeniKomentar', {
            Id: komentarZaIzmenu.Id,
            Tekst: komentarZaIzmenu.Tekst,
            Izmenjen: true
        })
    }

    factory.izmeniPodkomentar = function (podkomentarZaIzmenu) {
        return $http.post('/api/Komentari/IzmeniPodkomentar', {
            Id: podkomentarZaIzmenu.Id,
            Tekst: podkomentarZaIzmenu.Tekst,
            Izmenjen: true
        });
    }

    return factory;

    

});