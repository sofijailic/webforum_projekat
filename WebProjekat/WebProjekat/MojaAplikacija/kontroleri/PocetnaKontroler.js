forum.controller('PocetnaKontroler', function ($scope, $rootScope, TemeFabrika, $window) {

    if (!$rootScope.ulogovan) {
        $window.location.href = "#!/login";
    }


    function inicijalizacija() {
        TemeFabrika.uzmiSacuvaneTeme(sessionStorage.getItem("username")).then(function (odgovor) {
            $scope.sacuvaneTeme = odgovor.data;
            console.log(odgovor.data);
        });
    }

    inicijalizacija();

});