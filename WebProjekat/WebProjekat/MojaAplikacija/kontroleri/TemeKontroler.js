forum.controller('TemeKontroler', function ($scope, $routeParams, TemeFabrika, KomentariFabrika, $window, $rootScope) {

    $scope.podforumKomeTemaPripada = $routeParams.naziv;
    $scope.nazivTeme = $routeParams.naslovTeme;

    function inicijalizacija() {
        console.log('Inicijalizovana teme controller');

        TemeFabrika.UzmiTemuPoImenu($scope.nazivTeme, $scope.podforumKomeTemaPripada).then(function (odgovor) {
            console.log(odgovor.data);
            $scope.tema = odgovor.data;


            KomentariFabrika.uzmiKomentareZaTemu($scope.podforumKomeTemaPripada, $scope.nazivTeme).then(function (odgovor) {

                console.log(odgovor.data);
                $scope.komentari = odgovor.data;

            });
        });
    }

    inicijalizacija();


    $scope.dodavanjeTeme = function (tema) {
        tema.podforumKomePripada = $scope.podforumKomeTemaPripada;
        tema.autor = sessionStorage.getItem("username");
        TemeFabrika.dodajTemu(tema).then(function (odgovor) {

            console.log(odgovor.data);
            $window.location.href = "#!/podforumi/"+tema.podforumKomePripada+"/"+tema.naslov;
        });
    }

    $scope.DodajKomentarNaTemu = function (naslovTeme, podforumTeme, tekstKomentara) {

        var korisnickoIme = sessionStorage.getItem("username");
        KomentariFabrika.OstaviKomentar(naslovTeme, podforumTeme, tekstKomentara, korisnickoIme).then(function (odgovor) {
            console.log(odgovor.data);
            $scope.tekstKomentara = "";
            inicijalizacija();

        });

    }

    $scope.DodajPodkomentar = function (IdRoditelja, tekstPodkomentara, podforum, tema) {
        var autor = sessionStorage.getItem("username");
        var temaKojojPripada = podforum + '-' + tema;
        KomentariFabrika.dodajPodkomentar(IdRoditelja, tekstPodkomentara, autor, temaKojojPripada).then(function (odgovor) {
            inicijalizacija();
        });
    }


    $scope.lajkujTemu = function (tema) {
        if (!$rootScope.ulogovan) {
            alert('Ulogujte se da bi dali glas temi!');
            return;
        }
        TemeFabrika.lajkujTemu(tema, sessionStorage.getItem("username")).then(function (odgovor) {
            console.log(odgovor.data);
            if (odgovor.data == false) {
                alert('Vec ste dali pozitivan glas ovoj temi');
            } else {
                inicijalizacija();
            }
        });
    }

    $scope.dislajkujTemu = function (tema) {
        if (!$rootScope.ulogovan) {
            alert('Ulogujte se da bi dali glas temi!');
            return;
        }
        TemeFabrika.dislajkujTemu(tema, sessionStorage.getItem("username")).then(function (odgovor) {
            console.log(odgovor.data);
            if (odgovor.data == false) {
                alert('Vec ste dali negativan glas ovoj temi');
            } else {
                inicijalizacija();
            }
        });
    }

})