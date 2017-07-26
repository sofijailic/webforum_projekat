forum.controller('TemeKontroler', function ($scope, $routeParams, TemeFabrika) {

    $scope.podforumKomeTemaPripada = $routeParams.naziv;
    $scope.nazivTeme = $routeParams.naslovTeme;

    function inicijalizacija() {
        console.log('Inicijalizovana teme controller');

        TemeFabrika.UzmiTemuPoImenu($scope.nazivTeme, $scope.podforumKomeTemaPripada).then(function (odgovor) {
            console.log(odgovor.data);
            $scope.tema = odgovor.data;

        });
    }

    inicijalizacija();


    $scope.dodavanjeTeme = function (tema) {
        tema.podforumKomePripada = $scope.podforumKomeTemaPripada;
        tema.autor = sessionStorage.getItem("username");
        TemeFabrika.dodajTemu(tema).then(function (odgovor) {

            console.log(odgovor.data);
            $window.location.href = "#!/podforumi";
        })
    }
})