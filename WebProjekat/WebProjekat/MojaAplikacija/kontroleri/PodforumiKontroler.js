forum.controller('PodforumiKontroler', function ($scope, PodforumiFabrika, $window, $rootScope) {

    function inicijalizacija() {
        PodforumiFabrika.UzmiSvePodforume().then(function (odgovor) {
            console.log(odgovor.data);
            $scope.podforumi = odgovor.data;
        });
    }

    inicijalizacija();

    $scope.OtvoriDodavanjePodforuma = function () {

        $window.location.href = "#!/dodajPodforum";
    }


    $scope.dodajPodforum = function (podforum) {

        podforum.moderator = sessionStorage.getItem("username");
        
        

        PodforumiFabrika.DodajJedanPodforum(podforum).then(function (odgovor) { 

            console.log(odgovor.data);
            $window.location.href = "#!/podforumi";
        });
    }

});