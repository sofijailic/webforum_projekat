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

    return factory;
})