forum.factory('PodforumiFabrika', function ($http) {
    var factory = {};

    factory.DodajJedanPodforum = function (podforum) {
        return $http.post('/api/Podforumi/DodavanjePodforuma', {
            Naziv: podforum.naziv,
            Opis: podforum.opis,
            SpisakPravila: podforum.spisakPravila,
            Moderator: podforum.moderator

        });
    }

    factory.UzmiSvePodforume = function () {
        return $http.get('/api/Podforumi/UzmiSvePodforume');
    }

    return factory;
});