forum.controller('TemeKontroler', function ($scope, $routeParams, TemeFabrika) {

    $scope.podforumKomeTemaPripada = $routeParams.naziv;

    function inicijalizacija() {
        console.log('Inicijalizovana teme controller');
    }

    inicijalizacija();


    $scope.dodavanjeTeme = function (tema) {
        tema.podforumKomePripada = $scope.podforumKomeTemaPripada;
        tema.autor = sessionStorage.getItem("username");
        TemeFabrika.dodajTemu(tema).then(function (odgovor) {

            console.log(odgovor.data);

        })
        
        
        
    }
})