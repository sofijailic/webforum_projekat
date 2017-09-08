forum.controller('PorukeKontroler', function ($scope, PorukeFabrika, $routeParams, $window, $rootScope) {

    function inicijalizacija() {
        console.log('Inicijalizovan poruke kontroler');
    }

    inicijalizacija();
    
    $scope.posaljiPoruku = function (tekst,primalac) {

        var porukaZaSlanje = {
            posiljalac: sessionStorage.getItem('username'),
            primalac: primalac,
            tekst: tekst,
            procitana:false
        };
        PorukeFabrika.posaljiPoruku(porukaZaSlanje).then(function (odgovor) {
            alert('Uspesno poslato');
            $window.location.href = '#!/profil/' + $rootScope.primalacPoruke;

        });
    }
});

