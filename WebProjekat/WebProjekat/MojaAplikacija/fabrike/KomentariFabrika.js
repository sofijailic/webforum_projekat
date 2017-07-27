forum.factory('KomentariFabrika', function ($http) {

    var factory = {};

    factory.OstaviKomentar = function (naslovTeme,podforum,tekstKomentara,autor) {
        return $http.post('/api/Komentari/DodajKomentar', {
            TemaKojojPripada: podforum + '-' + naslovTeme,
            Autor:autor,
            Tekst:tekstKomentara

        });

    }

    factory.uzmiKomentareZaTemu = function (podforum,tema) {
        return $http.get('/api/Komentari/UzmiKomentare/?idTeme=' + podforum + '-' + tema);
    }

    return factory;

});