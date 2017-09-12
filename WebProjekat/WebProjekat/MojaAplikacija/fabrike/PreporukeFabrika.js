forum.factory('PreporukeFabrika', function ($http) {

    var factory = {};

    factory.uzmiPreporukeZaKorisnika = function (username) {
        return $http.get('/api/Preporuke/UzmiPreporukeZaKorisnika?username=' + username);
    }

    return factory;
});