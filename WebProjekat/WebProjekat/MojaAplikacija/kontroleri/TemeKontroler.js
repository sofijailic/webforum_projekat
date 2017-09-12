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

                //PodforumiFabrika.uzmiOdgovornogModeratoraZaTemu($scope.podforumKomeTemaPripada).then(function (odgovor) {
                //    $scope.odgovorniModeratorTeme = odgovor.data;
                //})

            });
        });
    }

    inicijalizacija();

    //dodavanje teme ,komentara,podkomentara
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

    //lajkovanje i dislajkovanje teme

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

    //lajkovanje i dislajkovanje komentara
    $scope.lajkujKomentar = function (komentar) {
        if (!$rootScope.ulogovan) {
            alert('Ulogujte se da bi ste dali glas komentaru!');
            return;
        }
        KomentariFabrika.lajkujKomentar(komentar, sessionStorage.getItem("username")).then(function (odgovor) {
            console.log(odgovor.data);
            if (odgovor.data == false) {
                alert('Vec ste dali pozitivan glas ovom komentaru');
            }
            else inicijalizacija();
        });
    }

    $scope.dislajkujKomentar = function (komentar) {
        if (!$rootScope.ulogovan) {
            alert('Ulogujte se da bi ste dali glas komentaru!');
            return;
        }
        KomentariFabrika.dislajkujKomentar(komentar, sessionStorage.getItem("username")).then(function (odgovor) {
            console.log(odgovor.data);
            if (odgovor.data == false) {
                alert('Vec ste dali negativan glas ovom komentaru');
            }
            else inicijalizacija();
        });
    }

    $scope.sacuvajTemu = function (username) {

            var naslovTeme = $scope.podforumKomeTemaPripada + '-' + $scope.nazivTeme;
            TemeFabrika.sacuvajTemu(naslovTeme, username).then(function (odgovor) {
                if (odgovor.data == false) {
                    alert('Vec pratite ovu temu!');
                }
                else alert('Tema dodata u listu pracenih');
            });
       
    }

    $scope.sacuvajKomentar = function (idKomentara, username) {
        KomentariFabrika.sacuvajKomentar(idKomentara, username).then(function (odgovor) {
            if (odgovor.data == false) {
                alert('Vec ste sacuvali ovaj komentar!');
            } else alert('Komentar uspesno sacuvan!');
            console.log(odgovor.data);
        });
    }
    $scope.obrisiTemu = function (tema) {

        TemeFabrika.obrisiTemu(tema).then(function (odgovor) {
            console.log(odgovor.data);
            alert('Uspesno obrisana tema');
            $window.location.href = "#!/podforumi"
        });
    }

    $scope.obrisiPodkomentar = function (podkomentar) {
        KomentariFabrika.obrisiPodkomentar(podkomentar).then(function (odgovor) {
            console.log(odgovor.data);
            inicijalizacija();
        });
    }

    $scope.obrisiKomentar = function (komentar) {
        KomentariFabrika.obrisiKomentar(komentar).then(function (odgovor) {
            console.log(odgovor.data);
            inicijalizacija();
        });
    }

    $scope.OtvoriStranicuZaSlanjeZalbeNaTemu = function () {
        $rootScope.zalbaNaTemu = {
            entitet: $scope.podforumKomeTemaPripada + '-' + $scope.nazivTeme,
            tip: 'Tema',
            korisnikKojiSeZali: sessionStorage.getItem('username')
        }
        $window.location.href = "#!/zalbaNaTemu";
    }

    $scope.OtvoriSlanjeZalbeNaKomentar = function (id) {
        $rootScope.zalbaNaKomentar = {
            entitet: id,
            tip: 'Komentar',
            korisnikKojiSeZali: sessionStorage.getItem('username')
        }
        $window.location.href = "#!/zalbaNaKomentar";
    }

    $scope.OtvoriSlanjeZalbeNaPodkomentar = function (id) {
        $rootScope.zalbaNaPodkomentar = {
            entitet: id,
            tip: 'Podkomentar',
            korisnikKojiSeZali: sessionStorage.getItem('username')
        }
        $window.location.href = "#!/zalbaNaPodkomentar";
    }

    $scope.otvoriIzmenuTeme = function () {
        $rootScope.temaZaIzmenu = $scope.tema;
        $window.location.href = "#!/izmenaTeme";
    }

    $scope.izmeniTemu = function (izmenjenaTema) {
        TemeFabrika.izmeniTemu(izmenjenaTema).then(function (odgovor) {
            alert('Uspesno izmenjeno');
            $window.location.href = '#!/podforumi/' + $rootScope.temaZaIzmenu.PodforumKomePripada + '/' + $rootScope.temaZaIzmenu.Naslov;
        })
    }

    $scope.otvoriIzmenuKomentara = function (komentar) {
        $rootScope.komentarZaIzmenu = komentar;
        $window.location.href = '#!/izmeniKomentar';
    }

    $scope.izmeniKomentar = function (izmenjenKomentar) {
        KomentariFabrika.izmeniKomentar(izmenjenKomentar).then(function (odgovor) {
            alert('Uspesno izmenjeno');
            $window.location.href = '#!/podforumi';
        })
    }

    $scope.otvoriIzmenuPodkomentara = function (podkomentar) {
        $rootScope.podkomentarZaIzmenu = podkomentar;
        $window.location.href = '#!/izmeniPodkomentar';
    }

    $scope.izmeniPodkomentar = function (izmenjenPodkomentar) {
        KomentariFabrika.izmeniPodkomentar(izmenjenPodkomentar).then(function (odgovor) {
            alert('Uspesno izmenjeno');
            $window.location.href = '#!/podforumi';
        })
    }

})