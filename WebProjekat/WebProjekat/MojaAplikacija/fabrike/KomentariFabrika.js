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
    return factory;

});