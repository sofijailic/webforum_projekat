forum.controller('PromenaTipaKontroler', function ($scope, $rootScope, $window, AutentifikacijaFabrika) {

    if (sessionStorage.getItem("uloga") != 'Administrator') {
        alert('Niste autorizovani da pregledate ovu stranicu.');
        $window.location.href = "#!/podforumi";
    }

    function inicijalizacija() {
        console.log('Promena tipa inicijalizovana');

        // Uzmi sve korisnike, ali nemoj uzeti ovog korisnika
        AutentifikacijaFabrika.uzmiSveKorisnikeOsimMene(sessionStorage.getItem("username")).then(function (odgovor) {
            console.log(odgovor.data);
            $scope.korisnici = odgovor.data;
        });
    }

    inicijalizacija();

    $scope.promeniTipKorisniku = function (username, tip) {
        AutentifikacijaFabrika.promeniTipKorisniku(username, tip).then(function (odgovor) {
            console.log(odgovor.data);
            alert('Uspesno promenjen tip korisniku');
        });
    }

});