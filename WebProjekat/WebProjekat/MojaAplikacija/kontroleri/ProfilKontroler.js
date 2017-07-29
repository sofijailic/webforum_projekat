forum.controller('ProfilKontroler', function ($scope, AutentifikacijaFabrika, PodforumiFabrika, TemeFabrika, KomentariFabrika, $routeParams) {

    function inicijalizacija() {
        console.log('Profil kontroler inicijalizovan');


        //uzmi korisnika-level 0
        AutentifikacijaFabrika.uzmiKorisnikaNaOsnovuImena($routeParams.username).then(function (odgovor) {
            console.log(odgovor.data);

            $scope.korisnickiProfil = odgovor.data;

            //uzmi podforume-level 1
            $scope.korisnickiProfil.DatumRegistracije = new Date($scope.korisnickiProfil.DatumRegistracije).toLocaleDateString();

            PodforumiFabrika.uzmiSacuvanePodforume($routeParams.username).then(function (odgovor) {
                $scope.sacuvaniPodforumi = odgovor.data;
                console.log(odgovor.data);

                //uzmi teme-level 2
                TemeFabrika.uzmiSacuvaneTeme($routeParams.username).then(function (odgovor) {
                    $scope.sacuvaneTeme = odgovor.data;
                    console.log(odgovor.data);
                
                    //uzmi komentare- level 3        
                    KomentariFabrika.uzmiSacuvaneKomentare($routeParams.username).then(function (odgovor) {
                        $scope.sacuvaniKomentari = odgovor.data;
                        $scope.sacuvaniKomentari.forEach(function (komentar) {
                            komentar.TemaKojojPripada = komentar.TemaKojojPripada.replace("-", "/");
                        });
                        console.log(odgovor.data);

                        //uzmi podkomentare-level 4

                        KomentariFabrika.uzmiSacuvanePodkomentare($routeParams.username).then(function (odgovor) {
                            var listaPodkomentara = odgovor.data;
                            listaPodkomentara.forEach(function (podkomentar) {
                                podkomentar.TemaKojojPripada = podkomentar.TemaKojojPripada.replace("-", "/");
                                $scope.sacuvaniKomentari.push(podkomentar);
                            })
                            console.log(odgovor.data);

                            TemeFabrika.uzmiLajkovaneTeme($routeParams.username).then(function (odgovor) {
                                var listaLajkovanih = odgovor.data;
                                $scope.listaLajkovanihTema = [];
                                listaLajkovanih.forEach(function (tema) {
                                    tema = tema.replace("-", "/");
                                    $scope.listaLajkovanihTema.push(tema);
                                });
                                console.log(odgovor.data);
                           
                                TemeFabrika.uzmiDislajkovaneTeme($routeParams.username).then(function (odgovor) {
                                    var listaDislajkovanih = odgovor.data;
                                    $scope.listaDislajkovanihTema = [];
                                    listaDislajkovanih.forEach(function (tema) {
                                        tema = tema.replace("-", "/");
                                        $scope.listaDislajkovanihTema.push(tema);
                                    });
                                    console.log(odgovor.data);


                                    KomentariFabrika.uzmiLajkovaniKomentari($routeParams.username).then(function (odgovor) {
                                        $scope.listaLajkovanihKomentara = odgovor.data;
                                        $scope.listaLajkovanihKomentara.forEach(function (komentar) {
                                            komentar.TemaKojojPripada = komentar.TemaKojojPripada.replace("-", "/");
                                        });
                                        console.log(odgovor.data);

                                        KomentariFabrika.uzmiDislajkovaniKomentari($routeParams.username).then(function (odgovor) {
                                            $scope.listaDislajkovanihKomentara = odgovor.data;
                                            $scope.listaDislajkovanihKomentara.forEach(function (komentar) {
                                                komentar.TemaKojojPripada = komentar.TemaKojojPripada.replace("-", "/");
                                            });
                                            console.log(odgovor.data);

                                        })
                                    })
                                })

                            })
                        })

                    })
                })

            })
        })
     }
   inicijalizacija();

});