'use strict';

/* Animate */
angular.module('App.animate', [])
    .animation('animateSliderLeft', function () {
        return {
            setup: function (el) {
                el.width(0);
            },
            start: function (el) {
                el.animate({ width: 'auto' }, 500);
            }
        }
    })
;