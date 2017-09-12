forum.controller('PrikazZalbiKontroler', function ($scope, $rootScope, $window, ZalbeFabrika, PorukeFabrika, PodforumiFabrika, TemeFabrika, KomentariFabrika) {

    if (sessionStorage.getItem("uloga") != 'Moderator') {
        if (sessionStorage.getItem("uloga") != 'Administrator') {
            alert('Niste autorizovani da pregledate ovu stranicu.');
            $window.location.href = "#!/podforumi";
        }
    }

    function inicijalizacija() {
        ZalbeFabrika.uzmiZalbeZaKorisnika(sessionStorage.getItem("username")).then(function (response) {
            console.log(response.data);
            $scope.zalbe = response.data;
        });
    }

    inicijalizacija();

    $scope.odbijZalbu = function (zalba) {
        var tekstPoruke = "";
        if (zalba.TipEntiteta == 'Podforum') {
            tekstPoruke = "Vasa zalba na podforum " + zalba.Entitet + " je odbijena."
        }
        else if (zalba.TipEntiteta == 'Tema') {
            tekstPoruke = "Vasa zalba na temu " + zalba.Entitet + " je odbijena."
        }
        else if (zalba.TipEntiteta == 'Komentar') {
            tekstPoruke = "Vasa zalba na komentar " + zalba.Entitet + " je odbijena."
        }
        else if (zalba.TipEntiteta == 'Podkomentar') {
            tekstPoruke = "Vasa zalba na komentar " + zalba.Entitet + " je odbijena."
        }

        var poruka = {
            posiljalac: sessionStorage.getItem("username"),
            primalac: zalba.KorisnikKojiJeUlozio,
            tekst: tekstPoruke
        }
        PorukeFabrika.posaljiPoruku(poruka).then(function (response) {
            console.log(response.data);
            ZalbeFabrika.obrisiZalbu(zalba).then(function (response) {
                console.log(response.data);
                alert('Zalba uspesno odbijena');
                inicijalizacija();
            });
        });
    }

    $scope.upozoriAutoraEntiteta = function (zalba) {
        var tekstPorukeZaAutoraZalbe = "";
        if (zalba.TipEntiteta == 'Podforum') {
            tekstPorukeZaAutoraZalbe = "Postovani, upozorili smo korisnika " + zalba.AutorZaljenogEntiteta + " zbog vase zalbe na podforum " + zalba.Entitet;
        }
        else if (zalba.TipEntiteta == 'Tema') {
            tekstPorukeZaAutoraZalbe = "Postovani, upozorili smo korisnika " + zalba.AutorZaljenogEntiteta + " zbog vase zalbe na temu " + zalba.Entitet;
        }
        else if (zalba.TipEntiteta == 'Komentar') {
            tekstPorukeZaAutoraZalbe = "Postovani, upozorili smo korisnika " + zalba.AutorZaljenogEntiteta + " zbog vase zalbe na komentar " + zalba.Entitet;
        }
        else if (zalba.TipEntiteta == 'Podkomentar') {
            tekstPorukeZaAutoraZalbe = "Postovani, upozorili smo korisnika " + zalba.AutorZaljenogEntiteta + " zbog vase zalbe na komentar " + zalba.Entitet;
        }

        var tekstPorukeZaAutoraZaljenogEntiteta = "";
        if (zalba.TipEntiteta == 'Podforum') {
            tekstPorukeZaAutoraZaljenogEntiteta = "Upozoravamo vas da je vas podforum " + zalba.Entitet + " bio prijavljen u zalbi.";
        }
        else if (zalba.TipEntiteta == 'Tema') {
            tekstPorukeZaAutoraZaljenogEntiteta = "Upozoravamo vas da je vasa tema " + zalba.Entitet + " bila prijavljena u zalbi.";
        }
        else if (zalba.TipEntiteta == 'Komentar') {
            tekstPorukeZaAutoraZaljenogEntiteta = "Upozoravamo vas da je vas komentar " + zalba.Entitet + " bio prijavljen u zalbi.";
        }
        else if (zalba.TipEntiteta == 'Podkomentar') {
            tekstPorukeZaAutoraZaljenogEntiteta = "Upozoravamo vas da je vas komentar " + zalba.Entitet + " bio prijavljen u zalbi.";
        }

        var porukaZaAutoraZalbe = {
            posiljalac: sessionStorage.getItem("username"),
            primalac: zalba.KorisnikKojiJeUlozio,
            tekst: tekstPorukeZaAutoraZalbe
        };

        var porukaZaAutoraZaljenogEntiteta = {
            posiljalac: sessionStorage.getItem("username"),
            primalac: zalba.AutorZaljenogEntiteta,
            tekst: tekstPorukeZaAutoraZaljenogEntiteta
        };

        PorukeFabrika.posaljiPoruku(porukaZaAutoraZalbe).then(function (response) {
            console.log(response.data);

            PorukeFabrika.posaljiPoruku(porukaZaAutoraZaljenogEntiteta).then(function (response) {
                ZalbeFabrika.obrisiZalbu(zalba).then(function (response) {
                    console.log(response.data);
                    alert('Korisnici uspesno obavesteni');
                    inicijalizacija();
                });
            });
        });
    }

    $scope.obrisiEntitet = function (zalba) {
        var tekstPorukeZaAutoraZalbe = "";
        if (zalba.TipEntiteta == 'Podforum') {
            tekstPorukeZaAutoraZalbe = "Postovani, obrisali smo podforum " + zalba.Entitet + " na koji ste se zalili.";
        }
        else if (zalba.TipEntiteta == 'Tema') {
            tekstPorukeZaAutoraZalbe = "Postovani, obrisali smo temu " + zalba.Entitet + " na koju ste se zalili.";
        }
        else if (zalba.TipEntiteta == 'Komentar') {
            tekstPorukeZaAutoraZalbe = "Postovani, obrisali smo komentar " + zalba.Entitet + " na koji ste se zalili sa tekstom zalbe: '" + zalba.Tekst + "'";
        }
        else if (zalba.TipEntiteta == 'Podkomentar') {
            tekstPorukeZaAutoraZalbe = "Postovani, obrisali smo komentar " + zalba.Entitet + " na koji ste se zalili sa tekstom zalbe: '" + zalba.Tekst + "'";
        }

        var tekstPorukeZaAutoraZaljenogEntiteta = "";
        if (zalba.TipEntiteta == 'Podforum') {
            tekstPorukeZaAutoraZaljenogEntiteta = "Obavestavamo vas da je vas podforum " + zalba.Entitet + " obrisan zbog zalbi.";
        }
        else if (zalba.TipEntiteta == 'Tema') {
            tekstPorukeZaAutoraZaljenogEntiteta = "Obavestavamo vas da je vasa tema " + zalba.Entitet + " obrisana zbog zalbi.";
        }
        else if (zalba.TipEntiteta == 'Komentar') {
            tekstPorukeZaAutoraZaljenogEntiteta = "Obavestavamo vas da je vas komentar sa id-em " + zalba.Entitet + " obrisan zbog zalbi.";
        }
        else if (zalba.TipEntiteta == 'Podkomentar') {
            tekstPorukeZaAutoraZaljenogEntiteta = "Obavestavamo vas da je vas komentar sa id-em " + zalba.Entitet + " obrisan zbog zalbi.";
        }

        var porukaZaAutoraZalbe = {
            posiljalac: sessionStorage.getItem("username"),
            primalac: zalba.KorisnikKojiJeUlozio,
            tekst: tekstPorukeZaAutoraZalbe
        };

        var porukaZaAutoraZaljenogEntiteta = {
            posiljalac: sessionStorage.getItem("username"),
            primalac: zalba.AutorZaljenogEntiteta,
            tekst: tekstPorukeZaAutoraZaljenogEntiteta
        };

        PorukeFabrika.posaljiPoruku(porukaZaAutoraZalbe).then(function (response) {
            console.log(response.data);

            if (zalba.TipEntiteta == 'Podforum') {
                var podforum = {
                    Naziv: zalba.Entitet
                }
                PodforumiFabrika.obrisiPodforum(podforum).then(function (response) {
                    if (response.data == true) {
                        PorukeFabrika.posaljiPoruku(porukaZaAutoraZaljenogEntiteta).then(function (response) {
                            ZalbeFabrika.obrisiZalbu(zalba).then(function (response) {
                                console.log(response.data);
                                alert('Entitet obrisan, korisnici obavesteni');
                                inicijalizacija();
                            });
                        });
                    }
                });
            }
            else if (zalba.TipEntiteta == 'Tema') {
                var podforumTema = zalba.Entitet.split('-');
                var tema = {
                    PodforumKomePripada: podforumTema[0],
                    Naslov: podforumTema[1]
                }

                TemeFabrika.obrisiTemu(tema).then(function (response) {
                    if (response.data == true) {
                        PorukeFabrika.posaljiPoruku(porukaZaAutoraZaljenogEntiteta).then(function (response) {
                            ZalbeFabrika.obrisiZalbu(zalba).then(function (response) {
                                console.log(response.data);
                                alert('Entitet obrisan, korisnici obavesteni');
                                inicijalizacija();
                            });
                        });
                    }
                });

            }
            else if (zalba.TipEntiteta == 'Komentar') {

                var komentar = {
                    Id: zalba.Entitet
                }

                KomentariFabrika.obrisiKomentar(komentar).then(function (response) {
                    if (response.data == true) {
                        PorukeFabrika.posaljiPoruku(porukaZaAutoraZaljenogEntiteta).then(function (response) {
                            ZalbeFabrika.obrisiZalbu(zalba).then(function (response) {
                                console.log(response.data);
                                alert('Entitet obrisan, korisnici obavesteni');
                                inicijalizacija();
                            });
                        });
                    }
                });

            }

            else if (zalba.TipEntiteta == 'Podkomentar') {

                var podkomentar = {
                    Id: zalba.Entitet
                }

                KomentariFabrika.obrisiPodkomentar(podkomentar).then(function (response) {
                    if (response.data == true) {
                        PorukeFabrika.posaljiPoruku(porukaZaAutoraZaljenogEntiteta).then(function (response) {
                            ZalbeFabrika.obrisiZalbu(zalba).then(function (response) {
                                console.log(response.data);
                                alert('Entitet obrisan, korisnici obavesteni');
                                inicijalizacija();
                            });
                        });
                    }
                });

            }


        });

    }

});