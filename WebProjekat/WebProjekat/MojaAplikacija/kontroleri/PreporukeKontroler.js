forum.controller('PreporukeKontroler', function ($scope, PreporukeFabrika) {

    function inicijalizacija() {

       
        PreporukeFabrika.uzmiPreporukeZaKorisnika(sessionStorage.getItem("username")).then(function (odgovor) {
            console.log(odgovor.data);
            $scope.preporuceneTeme = odgovor.data;
        });
    }

    inicijalizacija();


});

