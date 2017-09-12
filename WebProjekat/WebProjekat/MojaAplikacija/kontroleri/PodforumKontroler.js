forum.controller('PodforumKontroler', function ($scope, $routeParams, $rootScope,PodforumiFabrika, $window, TemeFabrika) {
    
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
    
    $scope.OtvoriStranicuZaSlanjeZalbeNaPodforum = function () {
        $rootScope.zalbaPodforuma = {
            entitet: $scope.nazivPodforuma,
            tip: 'Podforum',
            korisnikKojiSeZali: sessionStorage.getItem('username')
        }
        $window.location.href = "#!/zalbaNaPodforum";
    }

    $scope.sacuvajPodforum = function (nazivPodforuma, username) {
        PodforumiFabrika.sacuvajPodforum(nazivPodforuma, username).then(function (odgovor) {
            if (odgovor.data == false) {
                alert('Vec pratite ovaj podforum!');
            }
            else alert('Podforum dodat u listu pracenih');
        });
    }

    $scope.obrisiPodforum = function (podforum) {

        PodforumiFabrika.obrisiPodforum(podforum).then(function (odgovor) {

            console.log(odgovor.data);
            alert('Uspesno obrisan podforum');
            $window.location.href = "#!/podforumi";

        });

    }
})