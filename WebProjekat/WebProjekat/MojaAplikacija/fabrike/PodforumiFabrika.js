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

    factory.UzmiPodforumPoImenu = function (nazivPodforuma) {
        return $http.get('/api/Podforumi/UzmiPodforumPoImenu?nazivPodforuma=' + nazivPodforuma);

    }

    factory.sacuvajPodforum = function (nazivPodforuma, username) {
        return $http.post('/api/Podforumi/SacuvajPodforum', {
            NazivPodforuma: nazivPodforuma,
            KorisnikKojiCuva: username
        });
    }

    factory.uzmiSacuvanePodforume=function(username){
        return $http.get('/api/Podforumi/UzmiSacuvanePodforume?username=' + username);
    
    }
    return factory;
});