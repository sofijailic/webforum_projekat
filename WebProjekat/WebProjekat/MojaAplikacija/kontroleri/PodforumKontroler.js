forum.controller('PodforumKontroler', function ($scope, $routeParams, $rootScope,PodforumiFabrika, $window) {
    
    $scope.nazivPodforuma = $routeParams.naziv;

    function inicijalizacija() {
        console.log('Inicijalizovan Podforum kontroler');
        PodforumiFabrika.UzmiPodforumPoImenu($scope.nazivPodforuma).then(function (odgovor) {
            console.log(odgovor.data);
            $scope.podforum = odgovor.data;
        });
    }

    inicijalizacija();

    $scope.OtvoriStranicuZaDodavanjeTeme = function () {
        $window.location.href = "#!/podforumi/"+$scope.nazivPodforuma+'/dodajTemu';
    }
})