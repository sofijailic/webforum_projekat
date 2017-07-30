forum.factory('PretragaFabrika', function ($http) {


    var factory = {};

    factory.pretraziPodforume = function (pretragaPodforuma) {
        return $http.post('/api/Pretraga/PretraziPodforume', {
            Naziv: pretragaPodforuma.naziv,
            Opis: pretragaPodforuma.opis,
            OdgovorniModerator: pretragaPodforuma.moderator
        });
    }

    factory.pretraziTeme = function (pretragaTeme) {
        return $http.post('/api/Pretraga/PretraziTeme', {
            Naslov: pretragaTeme.naslov,
            Sadrzaj: pretragaTeme.sadrzaj,
            Autor: pretragaTeme.autor,
            PodforumKomePripada: pretragaTeme.podforum
        });
    }

    factory.pretraziKorisnike = function (username) {
        return $http.post('/api/Pretraga/PretraziKorisnike', {
            KorisnickoIme: username
        });
    }

    return factory;
})