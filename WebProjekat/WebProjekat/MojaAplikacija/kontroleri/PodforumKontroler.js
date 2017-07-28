﻿forum.controller('PodforumKontroler', function ($scope, $routeParams, $rootScope,PodforumiFabrika, $window, TemeFabrika) {
    
    $scope.nazivPodforuma = $routeParams.naziv;

    function inicijalizacija() {
        console.log('Inicijalizovan Podforum kontroler');
        PodforumiFabrika.UzmiPodforumPoImenu($scope.nazivPodforuma).then(function (odgovor) {
            console.log(odgovor.data);
            $scope.podforum = odgovor.data;

            TemeFabrika.uzmiTemeZaPodforum($scope.nazivPodforuma).then(function (odgovor) {
                console.log(odgovor.data);
                $scope.temePodforuma = odgovor.data;
            })
        });
    }

    inicijalizacija();

    $scope.OtvoriStranicuZaDodavanjeTeme = function () {
        $window.location.href = "#!/podforumi/"+$scope.nazivPodforuma+'/dodajTemu';
    }

    $scope.sacuvajPodforum = function (nazivPodforuma, username) {
        PodforumiFabrika.sacuvajPodforum(nazivPodforuma, username).then(function (odgovor) {
            if (odgovor.data == false) {
                alert('Vec pratite ovaj podforum!');
            }
            else alert('Podforum dodat u listu pracenih');
        });
    }
})