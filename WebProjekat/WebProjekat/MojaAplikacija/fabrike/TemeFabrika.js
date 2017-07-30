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

    factory.lajkujTemu = function (tema, username) {
        return $http.post('/api/Teme/LajkujTemu', {
            PunNazivTeme: tema.PodforumKomePripada + '-' + tema.Naslov,
            KoVrsiAkciju: username
        })
    }

    factory.dislajkujTemu = function (tema, username) {
        return $http.post('/api/Teme/DislajkujTemu', {
            PunNazivTeme: tema.PodforumKomePripada + '-' + tema.Naslov,
            KoVrsiAkciju: username
        })

    }


    factory.sacuvajTemu = function (naslovTeme, username) {
        return $http.post('/api/Teme/SacuvajTemu', {
            NaslovTeme: naslovTeme,
            KorisnikKojiPrati: username
        })
    }

    factory.uzmiSacuvaneTeme = function (username) {
        return $http.get('/api/Teme/UzmiSacuvaneTeme?username=' + username);

    }
    
    factory.uzmiLajkovaneTeme = function (username) {
        return $http.get('/api/Teme/UzmiLajkovaneTeme?username=' + username);
    }

    factory.uzmiDislajkovaneTeme = function (username) {
        return $http.get('/api/Teme/UzmiDislajkovaneTeme?username=' + username);
    }

    factory.obrisiTemu = function (tema) {
        return $http.post('/api/Teme/ObrisiTemu', {
            PodforumKomePripada: tema.PodforumKomePripada,
            Naslov: tema.Naslov
        });
    }

    return factory;
})