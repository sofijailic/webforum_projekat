forum.controller('ZalbeKontroler', function ($scope, $rootScope, $window, ZalbeFabrika) {

    function inicijalizacija() {
        console.log('Zalbe kontroler inicijalizovan');
    }

    inicijalizacija();

    $scope.posaljiZalbuNaPodforum = function (zalba) {
        if (zalba.tekst == "" || zalba.tekst == null) {
            alert('Popunite tekst zalbe!');
            return;
        }

        ZalbeFabrika.posaljiZalbuNaPodforum(zalba).then(function (response) {
            console.log(response.data);
            if (response.data == true) {
                alert('Uspesno poslata zalba!');
                $window.location.href = "#!/podforumi/" + zalba.entitet;
            }
        });
    }

    $scope.posaljiZalbuNaTemu = function (zalba) {
        if (zalba.tekst == "" || zalba.tekst == null) {
            alert('Popunite tekst zalbe!');
            return;
        }

        ZalbeFabrika.posaljiZalbuNaTemu(zalba).then(function (response) {
            console.log(response.data);
            if (response.data == true) {
                alert('Uspesno poslata zalba!');
                var ruta = zalba.entitet.replace('-', '/');
                $window.location.href = "#!/podforumi/" + ruta;
            }
        });
    }

    $scope.posaljiZalbuNaKomentar = function (zalba) {
        if (zalba.tekst == "" || zalba.tekst == null) {
            alert('Popunite tekst zalbe!');
            return;
        }

        ZalbeFabrika.posaljiZalbuNaKomentar(zalba).then(function (response) {
            console.log(response.data);
            if (response.data == true) {
                alert('Uspesno poslata zalba!');
                $window.location.href = "#!/podforumi";
            }
        });
    }

    $scope.posaljiZalbuNaPodkomentar = function (zalba) {
        if (zalba.tekst == "" || zalba.tekst == null) {
            alert('Popunite tekst zalbe!');
            return;
        }

        ZalbeFabrika.posaljiZalbuNaPodkomentar(zalba).then(function (response) {
            console.log(response.data);
            if (response.data == true) {
                alert('Uspesno poslata zalba!');
                $window.location.href = "#!/podforumi";
            }
        });
    }

});