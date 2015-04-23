'use strict';

/* Directives */
var blurFocusDirective = function () {
    return {
        restrict: 'E',
        require: '?ngModel',
        link: function (scope, elm, attr, ctrl) {
            if (!ctrl) {
                return;
            }

            elm.on('focus', function () {
                elm.addClass('has-focus');

                scope.$apply(function () {
                    ctrl.hasFocus = true;
                });
            });

            elm.on('blur', function () {
                elm.removeClass('has-focus');
                elm.addClass('has-visited');

                scope.$apply(function () {
                    ctrl.hasFocus = false;
                    ctrl.hasVisited = true;
                });
            });

            elm.closest('form').on('submit', function () {
                elm.addClass('has-visited');

                scope.$apply(function () {
                    ctrl.hasFocus = false;
                    ctrl.hasVisited = true;
                });
            });

        }
    };
};

var LoadBehaviorCaller = [];
angular.module('App.directives', [])
    .directive('input', blurFocusDirective)
    .directive('select', blurFocusDirective)

    .directive('appVersion', ['version', function (version) {
        return function (scope, elm, attrs) {
            elm.text(version);
        };
    }])
    .directive('dashboxes', ['$timeout', function ($timeout) { // Diretiva que é executada após ser carregado cada box
        return {
            restrict: 'A',
            link: function ($scope, element, attrs) {
                $scope.$on('dataloaded', function () {
                    
                    // Verifica se já executou esta função no elemento do scope atual
                    $timeout(function () { // You might need this timeout to be sure its run after DOM render.
                        // Carrega as funções de data-fn no elemento
                        // Realinha box
                        if (LoadBehaviorCaller['dataload-' + attrs.widget] == undefined) {
                            LoadBehaviorCaller['dataload-' + attrs.widget] = setTimeout(function () {
                                //$('body').trigger('realignBoxes');
                                NB.LoadBehavior(element);
                            }, 700);
                        }
                    }, 100, false);
                });
            }
        };
    }])
    .directive('filterDash', ['filter', function (filter) {
        return {
            restrict: 'A',
            controller: function ($scope, $attrs, filter) {
                $scope.$on('handleBroadcast', function () {
                    $scope.filter.key = filter;
                });
            }
        };
    }])
    .directive('loadbehavior', ['$timeout', function ($timeout) {
        // Runs during compile
        return {
            restrict: 'A',
            link: function ($scope, element, attrs) {
                $scope.$on('dataloaded', function () {
                    // Verifica se já executou esta função no elemento do scope atual
                    $timeout(function () { // You might need this timeout to be sure its run after DOM render.
                        // Carrega as funções de data-fn no elemento
                        NB.LoadBehavior(element);
                    }, 0, false);
                });
            }
        };
    }])
    .directive('resizecolorbox', ['$timeout', function ($timeout) {
        // Runs during compile
        return {
            restrict: 'A',
            link: function ($scope, element, attrs) {
                $scope.$on('dataloaded', function () {
                    // Verifica se já executou esta função no elemento do scope atual
                    $timeout(function () { // You might need this timeout to be sure its run after DOM render.
                        // Resize no colorbox
                        if (attrs.resizecolorbox === 'false') {
                            $.colorbox.resize();
                        } else {
                            var objWidth = (element.innerWidth() > 0) ? { width: element.innerWidth() + 10 } : {};
                            $.colorbox.resize(objWidth);
                        }
                    }, 200, false);
                });
            }
        };
    }])
    .directive('ngopenpop', ['$http', '$compile', '$parse', '$rootScope', '$timeout', function ($http, $compile, $parse, $rootScope, $timeout) {
        return {
            restrict: 'A',
            link: function ($scope, element, attrs, ngModel) {
                element.off('click.ngopenpop').on('click.ngopenpop', function () {
                    if ($rootScope.widgets != undefined) {
                        $rootScope.widgets.attrsClick = attrs;
                    }
                    document.body.scrollTop = document.documentElement.scrollTop = 0;

                    $http.get(attrs.ngopenpop + '?m='+ ((new Date()).getMilliseconds()), { cache: false })
                        .success(function (data, status, headers, config) {
                            $('#modalSliderAjax').html($compile(data)($scope));
                            $scope.$watch('$compile', function () {
                                $timeout(function () {
                                    NB.LoadBehavior($('#modalSliderAjax')); 
                                    $.colorbox.resize();
                                }, 10, false);
                            });
                        })
                        .error(function (data, status, headers, config) {

                        });
                });
            }
        };
    }])
    .directive('nglogged', [function () {
        return {
            restrict: 'A',
            link: function ($scope, element, attrs) {
                $scope.$watch('reloadHome', function () {
                    if ($scope.reloadHome)
                        window.location.href = '/';
                });
            }
        };
    }])
    .directive('alignboxes', ['$timeout', function ($timeout) {
        return {
            restrict: 'A',
            link: function ($scope, element, attrs) {
                $scope.$on('dataloaded', function () {
                    // Verifica se já executou esta função no elemento do scope atual
                    $timeout(function () { // You might need this timeout to be sure its run after DOM render.
                        var $els = element.find(attrs.alignboxes);
                        var _maxHeight = 0;
                        $els.height('auto').each(function () {
                            var _thisHeight = $(this).height();

                            if (_thisHeight === 0) {
                                _thisHeight = $(this).find('label').height();
                            }

                            if (_maxHeight < _thisHeight)
                                _maxHeight = _thisHeight;
                        });

                        $els.height(_maxHeight);
                    }, 200, false);
                });
            }
        };
    }])
    .directive('checkUrl', [function () {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function ($scope, element, attrs, ngModelCtrl) {
                if (!ngModelCtrl) {
                    return;
                }

                var pattern = /((ftp|http|https):\/\/)/;
                element.on({
                    blur: function () {
                        regUrl($(this));
                    },
                    'keydown keypress': function () {
                        try{
                            if (event.which === 13) {
                                regUrl($(this));
                            }
                        } catch (e) { }
                    }
                });

                var regUrl = function (el) {
                    var val = el.val();
                    if (val !== '' && !pattern.test(val)) {
                        el.val('http://' + val);
                        $scope.$apply(function () {
                            ngModelCtrl.$setViewValue('http://' + val);
                        });
                    }
                };
            }
        };
    }])
    .directive('loading', [function () {
        return {
            restrict: 'A',
            replace: true,
            templateUrl: '/Templates/Load.html',
            link: function ($scope, element, attrs) {
                var parent = element.parent();
                var parentPosition = parent.css('position') || parent.addClass('pos-relative');

                $scope.$on('dataloaded', function () {
                    var parent = element.parent();
                    var parentPosition = parent.css('position') || 'static';

                    if (parentPosition === 'static')
                        parent.addClass('pos-relative');
                });
            }
        };
    }])
    // Direcitve for enter action in elements are not tags form
    .directive('ngEnter', [function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.find('input, textarea, select').bind("keydown keypress", function (event) {
                    if (event.which === 13) {
                        scope.$apply(function () {
                            scope.$eval(attrs.ngEnter);
                        });

                        event.preventDefault();
                    }
                });
            }
        };
    }])
    // Direcitve for close buttons of colorbox
    .directive('colorboxClose', [function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.on('click', function (e) {
                    e.preventDefault();
                    $.colorbox.close();
                    //alert('directive close colorbox');
                });
            }
        };
    }])
    // Direcitve for close colorbox action
    .directive('closeColorbox', [function () {
        return {

            restrict: 'A',
            link: function ($scope, element, attrs) {
                $scope.$on('closeColorbox', function () {
                    $.colorbox.close();
                });
            }
        };
    }])
    // Directive for decide if use target = blank or open in lightbox
    .directive('ngTarget', ['$rootScope', '$timeout', function ($rootScope, $timeout) {
        return {
            restrict: 'A',
            link: function ($scope, element, attrs) {
                element.on('click.blank', '.openIframe', function () {
                    var $window = $(window);
                    if ($rootScope.currentExternalWindows == undefined)
                        $rootScope.currentExternalWindows = [];

                    var top;
                    var myNav = navigator.userAgent.toLowerCase();
                    if ((myNav.indexOf('msie') != -1)){
                        top = window.screenY + 480; //IE
                    }else{
                        top = window.screenY + 180
                    }

                    var new_window = window.open($(this).attr('href'), '', 'menubar=yes,toolbar=yes,scrollbars=yes,width=' + window.innerWidth + ',height=' + (window.innerHeight * 0.75) + ',top=' + top + ',left=' + window.screenX, '');
                    $rootScope.currentExternalWindows.push(new_window);
                    waitUser();
                    return false;
                });
            }
        };
    }])
    // Directive for update time and display in page of minute for minute
    .directive('currentTime', ['$timeout', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope) {
                var dateTime = $timeout(function refreshDate() {
                    scope.currentTime = moment().format('dddd, MMMM D, YYYY h:mm A');
                    dateTime = $timeout(refreshDate, 30000);
                }, 0, false);
            }
        };
    }])


    .directive('pwCheck', [function () {
        return {
            require: 'ngModel',
            link: function (scope, elem, attrs, ctrl) {
                var firstPassword = '#' + attrs.pwCheck;
                elem.add(firstPassword).on('keyup', function () {
                    scope.$apply(function () {
                        ctrl.$setValidity('pwmatch', elem.val() === $(firstPassword).val());
                    });
                });
            }
        }
    }])

    .directive("passwordMatch", function () {
        return {
            require: "ngModel",
            restrict: "A",
            scope: true,
            link: function (scope, element, attrs, ctrl) {
                var checker = function () {
                    var e1 = scope.$eval(attrs.ngModel);
                    var e2 = scope.$eval(attrs.passwordMatch);
                    return e1 == e2;
                };
                scope.$watch(checker, function (n) {
                    ctrl.$setValidity("passwordMatch", n);
                });
            }
        };
    })

    .directive('ensureUniqueEmail', ['$http', function ($http) {
        return {
            strict: 'A',
            require: 'ngModel',
            scope: {
                unique: '=unique',
                requiredEmail: '='
            },
            link: function ($scope, ele, attrs, ngModel) {
                $scope.unique = {};
                ngModel.$setValidity('unique', true);
                if (ngModel.$modelValue !== '') {
                    $http.post('/REST/User/Check', { email: ngModel.$modelValue })
                        .success(function (data, status, headers, cfg) {
                            $scope.unique.status = data.status;
                            if (data.status) {
                                ngModel.$setValidity('unique', true);
                            } else {
                                ngModel.$setValidity('unique', false);
                            }
                        })
                        .error(function (data, status, headers, cfg) {
                            $scope.unique = data;
                            ngModel.$setValidity('unique', false);
                        });
                }
                ele.on({
                    blur: function () {
                        if (!ngModel || !ele.val()) return;
                        if (ngModel.$modelValue && ngModel.$pristine) {
                            $scope.unique = {
                                response: '',
                                status: true
                            }
                            ngModel.$setValidity('unique', true);
                            return;
                        }

                        if (!ngModel.$error.required && !ngModel.$error.$email) {
                            $scope.unique = {                                
                                response: 'Verifying email...',
                                status: true
                            };
                            $.colorbox.resize();
                            $scope.model = ngModel.$modelValue;
                            $http.post('/REST/User/Check', { email: ngModel.$modelValue })
                                .success(function (data, status, headers, cfg) {
                                    $scope.unique.status = data.status;

                                    if (data.status) {
                                        if (data.response == "SAME") {
                                            ngModel.$setValidity('unique', true);
                                            $scope.unique.response = '';
                                        } else {
                                            ngModel.$setValidity('unique', true);
                                            $scope.unique.response = 'Email available';
                                        }
                                    } else {
                                        ngModel.$setValidity('unique', false);
                                        $scope.unique.response = 'Email already registered';
                                    }
                                    $.colorbox.resize();
                                })
                                .error(function (data, status, headers, cfg) {
                                    $scope.unique = data;
                                    ngModel.$setValidity('unique', false);
                                });
                        } else {
                            ngModel.$setValidity('unique', false);
                        }
                    },
                    focus: function () {
                        if ($scope.unique) $scope.unique.response = '';
                    }
                });
            }
        };
    }])

    .directive('ngEqual', ['$http', function ($http) {
        return {
            require: 'ngModel',
            scope: {
                inputEqual: '=inputEqual'
            },
            link: function ($scope, ele, attrs, c) {
                c.$setValidity('equal', false);
                ele.on('blur keydown keyup', function () {
                    verifyEqual();
                });
                var verifyEqual = function () {
                    if (c.$modelValue !== $scope.inputEqual)
                        c.$setValidity('equal', false);
                    else
                        c.$setValidity('equal', true);
                };
            }
        };
    }])

    .directive('colorboxCloseReload', [function () {
        return {
            restrict: 'A',
            link: function () {
                $(document).on('cbox_closed', function () {
                    window.location.href = '/';
                });
            }
        };
    }])

    .directive('colorboxCloseClean', ['$rootScope', function ($rootScope) {
        return {
            restrict: 'A',
            link: function ($scope, ele, attrs) {
                $(document).on('cbox_closed.clean', function () {
                    $rootScope[attrs.colorboxCloseClean] = undefined;
                    $(this).off('cbox_closed.clean');
                });
            }
        };
    }])

    // Animate tab for open and close. Best pratice for animations
    .directive('animateTab', [function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                //console.log(attrs);

                var el = $(attrs.elAnimate),
                    action = {};

                action.open = function () {
                    el.removeClass('close');//.animate({ left: attrs.elOpen });

                    //if (el.hasClass('aside-box'))
                        //el.find('.btn-close-aside-box').html('Close X');

                };
                action.close = function () {
                    el.addClass('close');//.animate({ left: attrs.elClose });
                    //if (el.hasClass('aside-box'))
                        //el.find('.btn-close-aside-box').html('Open');
                };

                //if(el.hasClass('close'))
                //    action.close();
                //else
                //    action.open();

                element.on('click', function (e) {
                    e.preventDefault();

                    if (el.hasClass('close'))
                        action.open();
                    else
                        action.close();
                });

                //$(window).scroll(function () {
                //    action.close();
                //});

                $('.wrap, footer.main').on('click', function (e) {
                    if (e.target != undefined && e.target.attributes["class"] != undefined && e.target.attributes["class"].nodeValue == 'icon icon-remove-red')
                        e.stopPropagation();
                    else
                        action.close();
                }).on('click', '.aside-box', function (e) {
                    e.stopPropagation();
                });
            }
        };
    }])

     .directive('placeholder', function($timeout){
         if (!$.browser.msie || $.browser.version >= 10) {
             return {};
         }
         return {
             link: function(scope, elm, attrs){
                 if (attrs.type === 'password') {
                     elm.attr('placeholder', '');
                     return;
                 }
                 $timeout(function () {
                     if (elm.val() != '' && elm.val() != null) return;
                     elm.val(attrs.placeholder).focus(function(){
                         if ($(this).val() == $(this).attr('placeholder')) {
                             $(this).val('');
                         }
                     }).blur(function(){
                         if ($(this).val() == '') {
                             $(this).val($(this).attr('placeholder'));
                         }
                     });
                 });
             }
         }
     })


    .directive('ngRepeatFinished', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                if (scope.$last === true) {
                    $timeout(function () {
                        scope.$emit('ngRepeatFinished');
                    });
                }
            }
        }
    })

    // Carousel
    .directive('ngCarousel', ['$compile', function ($compile) {
        return {
            restrict: 'A',
            link: function ($scope, element, attrs) {

                // callCarousel
                $scope.$on('callCarousel', function () {
                    $scope.$watch('$compile', function () {
                        element.bxSlider({
                            slideWidth: 280,
                            pager: false,
                            minSlides: 2,
                            maxSlides: 3,
                            moveSlides: 1,
                            slideMargin: 14,
                            infiniteLoop: false,
                            hideControlOnEnd: true//,
                            //onSliderLoad: function () {                                
                            //$('.bx-prev').addClass('icon-arrow-left-blue');
                            //$('.bx-next').addClass('icon-arrow-right-blue');
                            //}                            
                        });
                        // ie nao addclass no onSliderLoad
                        $('.bx-prev').addClass('icon-arrow-left-blue');
                        $('.bx-next').addClass('icon-arrow-right-blue');
                    });
                });


                // callCarouselFbPhoto
                $scope.$on('callCarouselFbPhoto', function () {
                    $scope.$watch('$compile', function () {
                        //element.lemmonSlider({
                        //    infinite: true
                        //});
                        element.cycle();
                    });
                });

                // callCarouselFbPhotoDetail
                $scope.$on('callCarouselFbPhotoDetail', function () {
                    $scope.$watch('$compile', function () {
                        element.bxSlider({
                            //slideWidth: 140,
                            pager: false,
                            //minSlides: 4,
                            //maxSlides: 7,
                            //moveSlides: 3,
                            infiniteLoop: false,
                            hideControlOnEnd: true//,
                            //onSliderLoad: function () {                                
                            //$('.bx-prev').addClass('icon-arrow-left-blue');
                            //$('.bx-next').addClass('icon-arrow-right-blue');
                            //}                            
                        });
                        // ie nao addclass no onSliderLoad
                        $('.bx-prev').addClass('icon-arrow-left-blue'); 
                        $('.bx-next').addClass('icon-arrow-right-blue');
                    });
                });

            }
        };
    }])
;