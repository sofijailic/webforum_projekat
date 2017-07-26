forum.factory('TemeFabrika', function ($http) {
    var factory = {};

    factory.dodajTemu = function (tema) {
        return $http.post('/api/Teme/DodajJednuTemu', {
            PodforumKomePripada: tema.podforumKomePripada,
            Naslov: tema.naslov,
            Tip: tema.tip,
            Autor:tema.autor,
            Sadrzaj: tema.sadrzaj

        });

    }

    factory.uzmiTemeZaPodforum = function (nazivPodforuma) {
        return $http.get('/api/Teme/UzmiSveTemeIzPodforuma?podforum=' + nazivPodforuma);
    }

    factory.UzmiTemuPoImenu = function (nazivTeme, nazivPodforuma) {
        return $http.get('/api/Teme/UzmiTemuPoImenu?podforum=' + nazivPodforuma+'&tema='+nazivTeme);

    }
    return factory;
})