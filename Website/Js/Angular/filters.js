'use strict';

/* Filters */

angular.module('App.filters', [])
    .filter('interpolate', function (version) {
        return function (text) {
            return String(text).replace(/\%VERSION\%/mg, version);
        };
    })
    .filter('pubdate', function () {
        return function (value) {
            var date = new Date(Date(value));
            return date.toISOString();
        };
    })
    .filter('formatDate', function () {
        return function (date) {
            return moment(date).format('MM/D/YYYY - hh:mm a');
        };
    })
    .filter('numberForString', function () {
        return function (value) {
            switch (value) {
                case 1:
                    return 'one';
                case 2:
                    return 'two';
                case 3:
                    return 'three';
            }
        };
    })
    .filter('categoryWidget', function () {
        return function (value) {
            switch (value) {
                case 1:
                    return 'Feed';
                case 2:
                    return 'Bookmark';
            }
        };
    })
    .filter('sizeLightbox', function () {
        return function (value) {
            switch (value) {
                case 1:
                    return '80%';
                case 2:
                    return '60%';//return '800';
                default:
                    return '80%';
            }
        };
    })
    .filter('linkService', function () {
        return function (value) {
            switch (value) {
                case 1:
                    return '/Templates/MoreFeed.html';
                case 2:
                    return '/Templates/MoreMyWebsite.html';
            }
        };
    })
    .filter('countCheckeds', function () {
        return function (inputs) {
            var cont = 0;
            angular.forEach(inputs, function (item) {
                if (item.chk)
                    cont++;
            });
            return cont;
        };
    })
    .filter('escape', function () {
        return window.escape;
    })
    .filter('startFrom', function () {
        return function (input, start) {
            start = +start; //parse to int
            try {
                return input.slice(start);
            } catch (e) { return null; }
        }
    });