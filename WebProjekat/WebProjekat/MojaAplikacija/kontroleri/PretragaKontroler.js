forum.controller('PretragaKontroler', function ($scope, $rootScope, PretragaFabrika) {

    //inicijalizacija 
    function inicijalizacija() {
        $scope.searched = false;

        $scope.rezultatiPretragePodforumi = null;
        $scope.rezultatiPretrageTeme = null;
        $scope.rezultatiPretrageKorisnika = null;

        console.log('Pretraga aktivirana');
    }

    inicijalizacija();


    $scope.pretraziPodforume = function (pretragaPodforuma) {
        if (pretragaPodforuma.naziv == "") {
            pretragaPodforuma.naziv = null;
        }
        if (pretragaPodforuma.opis == "") {
            pretragaPodforuma.opis = null;
        }
        if (pretragaPodforuma.moderator == "") {
            pretragaPodforuma.moderator = null;
        }

        PretragaFabrika.pretraziPodforume(pretragaPodforuma).then(function (odgovor) {
            console.log(odgovor.data);
            $scope.rezultatiPretragePodforumi = odgovor.data;
        });
    }

    $scope.pretraziTeme = function (pretragaTeme) {

        if (pretragaTeme.podforum == "") {
            pretragaTeme.podforum = null;
        }
        if (pretragaTeme.naslov == "") {
            pretragaTeme.naslov = null;
        }
        if (pretragaTeme.sadrzaj == "") {
            pretragaTeme.sadrzaj = null;
        }
        if (pretragaTeme.autor == "") {
            pretragaTeme.autor = null;
        }

        PretragaFabrika.pretraziTeme(pretragaTeme).then(function (odgovor) {
            console.log(odgovor.data);
            $scope.rezultatiPretrageTeme = odgovor.data;
        });
    }

    $scope.pretraziKorisnike = function (username) {

        if (username == "") {
            username = null;
        }

        PretragaFabrika.pretraziKorisnike(username).then(function (odgovor) {
            console.log(odgovor.data);
            $scope.rezultatiPretrageKorisnika = odgovor.data;
        });
    }


})