'use strict';

/* Controllers */
/*
    Controllers:        
        DashCtrl
        DeleteWidgetCtrl
        FeedCtrl
        HeaderCtrl
        WeatherCtrl
        FacebookCtrl
        LoginCtrl
        MoreFeedCtrl
        MoreWebsiteCtrl
        MyProfileCtrl
        RegisterCtrl
        EmailCtrl
        EmailAccountCtrl
        MyWebsiteCtrl
        AddNewWidgetCtrl
        AddNewWidgetCtrl.FeedCtrl
        AddNewWidgetCtrl.MyWebsiteCtrl
        FacebookPhotosCtrl
        FacebookPhotoDetailCtrl
        FacebookPhotosManageFriendsCtrl
        InviteFriends
        CustomizeHomepageCtrl
        SuggestionCtrl
        ChatCtrl
        SupportCtrl
        FAQsCtrl
        AddAsHomePageCtrl
*/
//debugger;

//hello.init({
//    //windows: CLIENT_IDS_ALL.windows,
//    //google: CLIENT_IDS_ALL.google,
//    //facebook: CLIENT_IDS_ALL.facebook,
//    yahoo: 'dj0yJmk9YTg0TXhJMjJKTXM3JmQ9WVdrOWFHRTNPRzFaTm1zbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD0wNw--'
//}
//	, {
//	    redirect_uri: '',
//	    oauth_proxy: 'https://auth-server.herokuapp.com',
//	    scope: "friends"
//	}
//);

//var CLIENT_IDS = {
//    google: '656984324806-sr0q9vq78tlna4hvhlmcgp2bs2ut8uj8.apps.googleusercontent.com',
//    windows: {
//        'adodson.com': '00000000400D8578',
//        'local.knarly.com': '000000004405FD31',
//        'auth-server.herokuapp.com': '000000004C0DFA39'
//    }[window.location.hostname],
//    facebook: {
//        'adodson.com': '160981280706879',
//        'local.knarly.com': '285836944766385',
//        'auth-server.herokuapp.com': '115601335281241'
//    }[window.location.hostname],
//    yahoo: {
//        'local.knarly.com': 'dj0yJmk9cjVDdHlDaGtrbldJJmQ9WVdrOVYyZFhSWE4yTm04bWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD1jOA--',
//        'adodson.com': 'dj0yJmk9TWtNN0ppYTBGSW1vJmQ9WVdrOVIxSnhUVVJsTlRJbWNHbzlOamMxT1RVM01UWXkmcz1jb25zdW1lcnNlY3JldCZ4PWNk',
//        'auth-server.herokuapp.com': 'dj0yJmk9M1JuUWFaRHl5U01nJmQ9WVdrOWMzZHBVRFJsTXpJbWNHbzlNVEExTURVeE5qYzJNZy0tJnM9Y29uc3VtZXJzZWNyZXQmeD0wNg--'
//    }[window.location.hostname]
//};

var REDIRECT_URI = {
    'local.knarly.com': '/hello.js/redirect.html'
}[window.location.host] || './redirect.html';


function AppCtrl($scope) {
    angular.element(document).ready(function () {
        //document.getElementById('msg').innerHTML = 'anirudh';
        alert("hello");
    });
}

$(document).ready(function () {
    if (typeof (Storage) !== "undefined") {
        if (localStorage != undefined || localStorage != "undefined") {
            if (localStorage.FirstUser == undefined) {
                localStorage.FirstUser = "trues";
                $('#joinSite').click();
            }
        }
    }
});

angular.module('App.controllers', [])
.constant('gcfg', {
    clientId: _googleAuthConfig.clientId,
    state: _googleAuthConfig.appState,
    scopes: {
        email: _googleAuthConfig.scopes.email,
        profile: _googleAuthConfig.scopes.profile,
        plus: _googleAuthConfig.scopes.plus,
        me: _googleAuthConfig.scopes.me
    }
})

    .constant('authcfg', {
        live: {
            clientId: _authConfig.live.client_id,
            clientSecret: _authConfig.live.client_secret,
            redirectUrl: _authConfig.live.redirect_url
        },
        yahoo: {
            consumerKey: _authConfig.yahoo.consumer_key,
            consumerSecret: _authConfig.yahoo.consumer_secret,
            redirectUrl: _authConfig.yahoo.redirect_url
        }
    })
    // Main Controller
    .controller('AppCtrl', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
        $scope.overlayClose = false;
        $scope.dash = $rootScope.dash = {};
        $rootScope.facebookToken = '';
        $rootScope.firstAccess = false;
        if ($location.$$absUrl.indexOf('?InviteYourFriends') != -1) {
            $rootScope.firstAccess = true;
            $location.$$absUrl = $location.$$absUrl.replace('?InviteYourFriends', '');
            setTimeout(function () {
                $("#lnkMenuInvite").trigger("click");
            }, 1000);
        }
        $scope.cboxClose.click = function () {
            alert("sasa");
        };
        $scope.returningUser = function () {
            $rootScope.returningUser();
        };

    }])

    // Controller for DashCtrl in /Default.aspx
    .controller('DashCtrl', ['$timeout', '$scope', '$http', '$rootScope', '$injector', 'filterDash', function ($timeout, $scope, $http, $rootScope, $injector, filterDash) {
        $scope.userFriendsSearch = $rootScope.userFriendsSearch = [];
        $scope.facebookPhoto = { friendName: '', viewDetail: false };
        $scope.userEmailFeed = $rootScope.userEmailFeed = [];
        $scope.userHasEmailAccount = $rootScope.userHasEmailAccount = false;
        $rootScope.$gridster = null;

        $rootScope.loadDragWidgets = function (id) {
            //$(function () {
            var iOS = /(iPad|iPhone|iPod)/g.test(navigator.userAgent);
            if (iOS || homepage == 'True') {
                $(function () {
                    $rootScope.$gridster = $('.gridster .list-gridster').gridster({
                        widget_margins: [5, 5],
                        widget_base_dimensions: [310, 340],
                    }).data('gridster').disable();
                });
            }
                //else if ($(window).width() >= 1024) {
            else {
                if ($rootScope.$gridster == null) {
                    $(function () {
                        console.log('remake grid');
                        $rootScope.$gridster = $('.gridster .list-gridster').gridster({
                            widget_margins: [5, 5],
                            widget_base_dimensions: [310, 340],
                            shift_larger_widgets_down: false,
                            //draggable: {
                            //handle: '.list-gridster header.nv-1'
                            //},
                            serialize_params: function ($w, wgd) {
                                return {
                                    id: $w.attr('data-widget'),
                                    col: wgd.col,
                                    row: wgd.row,
                                    size_x: wgd.size_x,
                                    size_y: wgd.size_y
                                }
                            },
                            draggable: {
                                stop: function () {
                                    var gridsterItens = $rootScope.$gridster.serialize();
                                    $scope.updatePosition(gridsterItens);
                                    //console.log(gridsterItens);
                                }
                            }
                        }).data('gridster');
                    });
                } else {
                    $timeout(function () {
                        $('.list-gridster .box-column').each(function (i, e) {
                            if (!$(e).hasClass('gs_w')) {
                                $rootScope.$gridster.add_widget(e, $(e).attr('data-sizex'), 1, $(e).attr('data-col'), $(e).attr('data-row'));
                            }

                        });

                        $rootScope.$gridster = null;
                        $rootScope.loadDragWidgets();
                    }, 200);
                }
            }
            //});
        };


        $rootScope.loadDragWidgets();

        var _widgets;
        $scope.$inject = ['$scope', 'filterDash'];


        $scope.$on('handleBroadcast', function () {
            $scope.category = filterDash.key;
        });
        /*
        $http.get('/REST/UserWidget?m=' + (new Date()).getMilliseconds(), { cache: false })
            .success(function (data, status, headers, config) {

                $scope.widgets = $rootScope.widgets = _widgets = data;
                $scope.$parent.dash.nWidgets = $scope.widgets.length;

                if (NB.searchGoogle && $scope.$parent.dash.nWidgets < 1)
                    $scope.$parent.dash.nWidgets = 1;
            })
            .error(function (data, status, headers, config) {

            });
            */

        $scope.widgets = $rootScope.widgets = _widgets = _page.widgets;
        $scope.$parent.dash.nWidgets = $scope.widgets.length;

        if (NB.searchGoogle && $scope.$parent.dash.nWidgets < 1)
            $scope.$parent.dash.nWidgets = 1;


        $scope.delSelected = function (id, index) {
            $rootScope.deleteItem = { id: id, i: index };
        };

        /* Used for Facebook Photos */
        $scope.findFriend = function () {
            if ($scope.facebookPhoto.friendName != '') {
                $scope.userFriendsSearch = $rootScope.userFriends.slice(0);
                $scope.userFriendsSearch = _.filter($scope.userFriends, function (v) {
                    return (v.name.toLowerCase().slice(0, $scope.facebookPhoto.friendName.length) == $scope.facebookPhoto.friendName.toLowerCase());
                });
            } else {
                $scope.userFriendsSearch = [];
            }
        };

        /* Used for Facebook Photos */

        $rootScope.returningUser = $scope.returningUser = function (obj) {
            if ($rootScope.currentExternalWindows != undefined) {
                for (var i in $rootScope.currentExternalWindows) {
                    if ($rootScope.currentExternalWindows[i] != null) {
                        $rootScope.currentExternalWindows[i].close();
                    }
                }
            }

            if (!$(".take-me-home").hasClass("hide")) {
                $(".take-me-home").addClass("hide");
                $(".take-me-home-background").addClass("hide");
            }
        };

        window.focus = $rootScope.returningUser;


        $rootScope.removeWidget = $scope.removeWidget = function (obj) {
            LoadBehaviorCaller = [];
            $scope.widgets.splice(obj.i, 1);
            $scope.$parent.dash.nWidgets--;
            $scope.$broadcast('dataloaded');
        };


        $rootScope.addWidgetFirst = $scope.addWidgetFirst = function (obj) {
            $scope.widgets.unshift($.parseJSON(obj));
            $scope.$parent.dash.nWidgets++;
            $scope.$broadcast('dataloaded');
            $rootScope.loadDragWidgets();
        };

        $rootScope.addWidgetLast = $scope.addWidgetLast = function (obj) {
            //$scope.widgets.push($.parseJSON(obj));
            var fixedItems = 3;
            var widget = $.parseJSON(obj);
            $scope.widgets.splice($scope.widgets.length - fixedItems, 0, $.parseJSON(obj));
            $scope.$parent.dash.nWidgets++;
            $scope.$broadcast('dataloaded');
            $rootScope.loadDragWidgets();
            /*
            console.log(widget);
            $('ul.list-gridster li.box-column').each(function(i, e){
                console.log($(e).attr("data-widget"));
                if ($(e).attr("data-widget") == widget.id){
                    console.log($(e).html());
                    $rootScope.$gridster.add_widget($(e).html());
                }
            });*/
        };

        $rootScope.repositionWidgets = $scope.repositionWidgets = function () {

            var noOfPos = 1;
            if ($rootScope.$gridster != null) {
                var cell = $rootScope.$gridster.get_highest_occupied_cell();
                var max_row = cell.row;

                var move_to_col = 0;
                var move_to_row = 0;

                for (var r = 1; r <= max_row; r++) {
                    for (var c = 1; c <= 3; c++) {
                        if ($rootScope.$gridster.is_empty(c, r)) {
                            var nextCol = c + 1;
                            var nextRow = r;
                            if (nextCol == 4) {
                                nextCol = 1;
                                nextRow = r + 1;
                            }

                            var nextEmpty = $rootScope.$gridster.is_empty(nextCol, nextRow);

                            move_to_col = move_to_col == 0 ? c : move_to_col;
                            move_to_row = move_to_row == 0 ? r : move_to_row;


                            if (nextEmpty)
                                noOfPos++;
                            else {
                                for (var n = 1; n <= noOfPos; n++) {
                                    var nextWdgt = $rootScope.$gridster.is_widget(nextCol, nextRow);
                                    if (nextWdgt) {
                                        var colSize = nextWdgt.context.attributes["data-sizex"].value;

                                        if (colSize == 3 && move_to_col > 1) {
                                            var isFound = false;
                                            for (var nr = nextRow + 1; nr <= max_row; nr++) {
                                                for (var nc = 1; nc <= 3; nc++) {
                                                    nextWdgt = $rootScope.$gridster.is_widget(nc, nr);
                                                    if (nextWdgt) {
                                                        colSize = nextWdgt.context.attributes["data-sizex"].value;
                                                        if (colSize == 1) {
                                                            isFound = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                                if (isFound)
                                                    break;
                                            }
                                        }

                                        if (colSize == 1 || (colSize == 3 && move_to_col == 1)) {
                                            $rootScope.$gridster.new_move_widget_to(nextWdgt, move_to_col, move_to_row);
                                            move_to_col++;
                                            if (move_to_col > 3) {
                                                move_to_col = 1;
                                                move_to_row++;
                                            }
                                        }
                                    }
                                    nextCol++;
                                    if (nextCol > 3) {
                                        nextCol = 1;
                                        nextRow++;
                                    }
                                    move_to_col = (n == noOfPos) ? 0 : move_to_col;
                                    move_to_row = (n == noOfPos) ? 0 : move_to_row;
                                    noOfPos = (n == noOfPos) ? 1 : noOfPos;
                                }
                            }
                        }
                    }
                }
            }
        };

        //$rootScope.repositionWidgets = $scope.repositionWidgets = function () {

        //    var isMoved = true;
        //    var noOfPos = 1;

        //    var cell = $rootScope.$gridster.get_highest_occupied_cell();
        //    var max_col = cell.col;
        //    var max_row = cell.row;

        //    var move_to_col = 0;
        //    var move_to_row = 0;

        //    for (var r = 1; r <= max_row; r++) {
        //        for(var c = 1; c <= 3; c++ )
        //        {
        //            if ($rootScope.$gridster.is_empty(c,r))
        //            {                        
        //                var nextCol = c + 1;
        //                var nextRow = r;
        //                if (nextCol == 4)
        //                {
        //                    nextCol = 1;
        //                    nextRow = r + 1;
        //                }

        //                var nextWdgt = $rootScope.$gridster.is_widget(nextCol, nextRow);

        //                if (isMoved) {
        //                    move_to_col = c;
        //                    move_to_row = r;
        //                    noOfPos = 1;
        //                }
        //                else
        //                    noOfPos++;

        //                if (nextWdgt)
        //                {
        //                    isMoved = false;
        //                    for(var  n = 1; n <= noOfPos; n++)
        //                    {
        //                        var colSize = nextWdgt.context.attributes["data-sizex"].value;
        //                        if (colSize == 1 || (colSize == 3 && move_to_col == 1)) {
        //                            $rootScope.$gridster.new_move_widget_to(nextWdgt, move_to_col, move_to_row);
        //                            isMoved = true;
        //                            move_to_col++;
        //                            if (move_to_col > 3) {
        //                                move_to_col = 1;
        //                                move_to_row++;
        //                            }
        //                        }                                
        //                    }
        //                }
        //                else
        //                    isMoved = false;                            
        //            }
        //        }
        //    }
        //};

        $rootScope.addWidgetForIndex = $scope.addWidgetForIndex = function (i, obj) {
            /*
            for (var w = 0; w < $scope.widgets.length; w++) {
                if ($scope.widgets[w].id == $.parseJSON(obj).id) {
                    $scope.widgets[w].title = $.parseJSON(obj).title;
                }
            }*/
            //$scope.widgets = (JSON.parse(JSON.stringify($scope.widgets)));
            //quando reset (splice) ele chama o UserWidgetBookmark de novo


            //$scope.widgets.splice(i, 1, (JSON.parse(JSON.stringify($scope.widgets[i]))));
            //$scope.$parent.dash.nWidgets++;
            $scope.widgets.splice(i, 1, $.parseJSON(obj));
            $scope.$parent.dash.nWidgets++;
            setTimeout(function () {
                $scope.$broadcast('dataloaded');
            }, 300);
        };


        $scope.viewUserPhotos = function (user) {
            $scope.userFriendsSearch = false;
            $rootScope.viewUserPhotos(user);
        };

        $scope.updateFacebookFriendsList = function () {
            $scope.facebookPhoto.viewDetail = false;
            $scope.facebookPhoto.friendName = '';
            $rootScope.showFriendsList();
        };

        $rootScope.attWidget = $scope.attWidget = function (id, obj) {
            var objSelected = {},
                objIndex = 0;
            for (var i in $scope.widgets) {
                if ($scope.widgets[i].id === id) {
                    objSelected = $scope.widgets[i];
                    objSelected.i = objIndex = i;
                }
            }
            $scope.addWidgetForIndex(objIndex, obj);
        };

        $scope.showEditTitle = function (i) {
            var widget = $scope.widgets[i];
            widget.editTitle = true;
            widget.newTitle = widget.title;
        };


        $rootScope.updatePosition = $scope.updatePosition = function (widgets) {
            var widget = widgets,
                url = '/REST/UserWidget/SetPosition';


            $http.post(url, widgets)
                .success(function (data, status, headers, config) {
                    if (data.status)
                        console.log(data);
                    else
                        console.log(data);
                })
                .error(function (data, status, headers, config) {
                    console.log(data);
                });

        };

        $scope.submitTitle = function (i) {
            var widget = $scope.widgets[i],
                url = '/REST/UserWidget/UpdateTitle',
                title = '';
            widget.editTitle = false;
            title = widget.title;
            widget.title = 'Saving title...';

            $http.post(url, { id: widget.id, title: widget.newTitle })
                .success(function (data, status, headers, config) {
                    if (data.status)
                        widget.title = widget.newTitle;
                    else
                        widget.title = title;
                })
                .error(function (data, status, headers, config) {
                    widget.title = title;
                });
        };

        $scope.edit = function (widget) {
            $rootScope.edit = {
                id: widget.id,
                title: widget.title
            };
        };

        $scope.loadedWidget = 0;
        $rootScope.loadWidget = $scope.loadWidget = function () {
            $scope.loadedWidget++;
            if ($scope.widgets.length <= $scope.loadedWidget) {
                $scope.wasLoaded = true;
            }
        };
    }])


    // Controller for GoogleSearchCtrl in /GoogleSearch.aspx
    .controller('GoogleSearchCtrl', ['$scope', '$rootScope', '$http', '$sce', '$location', function ($scope, $rootScope, $http, $sce, $location) {

        $rootScope.forceNoWidgetsHide = true;
        $scope.searchResult = [];

        var qsSearch = 'q';
        var urlSegs = $location.$$absUrl.toString();

        qsSearch = qsSearch.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regex = new RegExp("[\\?&]" + qsSearch + "=([^&#]*)"),
            results = regex.exec(urlSegs);

        results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));


        $http.get('/REST/GoogleSearch/' + results[1] + '?m=' + (new Date()).getMilliseconds(), { cache: false })
            .success(function (data, status, headers, config) {

                var searchResult = $.parseJSON(data.response);
                for (var ic = 0; ic < searchResult.responseData.results.length; ic++) {
                    searchResult.responseData.results[ic].content = $sce.trustAsHtml(searchResult.responseData.results[ic].content);
                }

                $scope.searchResult = searchResult.responseData.results;
            })
            .error(function (data, status, headers, config) {

            });
    }])

    .controller('DeleteWidgetCtrl', ['$scope', '$rootScope', '$http', function ($scope, $rootScope, $http) {
        $scope.feedbackDelete = {};
        $scope.deleteConfirm = function () {
            $scope.feedbackDelete.msg = 'Deleting frame...';
            $scope.feedbackDelete.status = true;
            $('ul.list-gridster li.box-column').each(function (i, e) {
                if ($(e).attr("data-widget") == $rootScope.deleteItem.id) {
                    if ($rootScope.$gridster != null) {
                        $rootScope.$gridster.remove_widget(e);
                        $rootScope.repositionWidgets();
                        var gridsterItems = $rootScope.$gridster.serialize();
                        $rootScope.updatePosition(gridsterItems);
                    }
                }
            });

            $http.post('/REST/UserWidget/Delete/' + $rootScope.deleteItem.id)
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        $scope.feedbackDelete.msg = 'Frame deleted. Updating page...';
                        $scope.feedbackDelete.status = true;
                        $rootScope.removeWidget($rootScope.deleteItem);
                        $scope.$broadcast('closeColorbox');
                        $('#closeModal').trigger('click');
                    } else {
                        $scope.feedbackDelete.msg = data.response;
                        $scope.feedbackDelete.status = false;
                    }
                })
                .error(function (data, status, headers, config) {
                    $scope.feedbackDelete.msg = data.response;
                    $scope.feedbackDelete.status = false;
                });
        };
    }])

    // Controller for HeaderCtrl in /Default.aspx
    .controller('HeaderCtrl', ['$window', '$scope', '$http', '$rootScope', '$injector', 'filterDash', function ($window, $scope, $http, $rootScope, $injector, filterDash) {
        $scope.$inject = ['$scope', 'filterDash'];
        $scope.filterCategory = 0;

        $scope.logout = function () {
            $http.get('/REST/User/Logout?m=' + (new Date()).getMilliseconds(), { cache: false }).
                success(function (data, status, headers, config) {
                    if (data.status)
                        window.location.href = '/';
                })
                .error(function () {

                });
        };


        $scope.getTrusteds = function () {
            /*
            $http.get('/REST/UserWidgetCategory?m=' + (new Date()).getMilliseconds(), { cache: false })
                .success(function (data, status, headers, config) {
                    $scope.trustedSource = data;
                })
                .error(function (data, status, headers, config) {

                });
                */
            $scope.trustedSource = _page.trustedSource;

        };

        $scope.getTrusteds();

        $scope.changeCategory = function (val) {
            $scope.filterCategory = val;
            filterDash.prepForBroadcast(val);
        };

        $scope.googleSearch = function () {
            $("#btnGoogleSearch").trigger("click");
        };

        $scope.addBookmark = function (e) {
            // Mozilla Firefox Bookmark
            if ('sidebar' in $window && 'addPanel' in $window.sidebar) {
                $window.sidebar.addPanel(location.href, document.title, "");
            } else if ( /*@cc_on!@*/false) { // IE Favorite
                $window.external.AddFavorite(location.href, document.title);
            } else { // webkit - safari/chrome
                alert('Press ' + (navigator.userAgent.toLowerCase().indexOf('mac') != -1 ? 'Command/Cmd' : 'CTRL') + ' + D to bookmark this page.');
            }
        };
    }])

    // Controller for WeatherCtrl in /Templates/Weather.html
    .controller('WeatherCtrl', ['$scope', '$http', '$sce', '$rootScope', function ($scope, $http, $sce, $rootScope) {
        $scope.weather = { address: ($scope.$parent.widget.zipCode != null) ? $scope.$parent.widget.zipCode : '10001', lastUpdate: new Date(), location: '', currentTemp: 0, forecast: [], firstLoad: true };

        $scope.loadWeather = function () {

            $http.get('/REST/UserWidgetWeather/CurrentWeather/' + $scope.weather.address + '?WidgetId=' + $scope.$parent.widget.id + '&m=' + (new Date()).getMilliseconds())
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        var geoweather = $.parseJSON(data.response);
                        $scope.weather.forecast = geoweather.weather;
                        $scope.weather.location = geoweather.location;
                        $scope.weather.currentTemp = $scope.weather.forecast[0].currentTemp;
                        $scope.weather.lastUpdate = $scope.weather.forecast[0].dateWeatherLabel;


                        if ($scope.weather.firstLoad) {
                            $scope.weather.firstLoad = false;
                            $rootScope.loadWidget();
                        }
                    } else {
                        alert(data.response);
                    }
                })
                .error(function (data, status, headers, config) {
                    //alert('weather error');
                });
        }

        if ($scope.weather.firstLoad) {
            $scope.loadWeather();
        }


    }])

    // Controller for FacebookCtrl in /Templates/Facebook.html
    .controller('FacebookCtrl', ['$scope', '$http', '$sce', '$rootScope', function ($scope, $http, $sce, $rootScope) {
        $scope.facebookUserLogged = false;
        $scope.facebookFeed = [];

        (function (d) {
            var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement('script'); js.id = id; js.async = true;
            js.src = "//connect.facebook.net/en_US/all.js";
            ref.parentNode.insertBefore(js, ref);
        }(document));

        window.fbAsyncInit = function () {
            $scope.$apply(function () {
                FB.init({
                    appId: _facebookAppId, // App ID
                    status: true, // check login status
                    cookie: true, // enable cookies to allow the server to access the session
                    xfbml: true  // parse XFBML
                });
                $scope.loadFacebookNewsFeed();
            });

        };

        $scope.facebookLogin = function () {
            FB.login(function (response) {

                if (response.authResponse) {
                    $scope.loadFacebookNewsFeed();
                } else {
                    alert('Not Authorized');
                }

            }, { scope: 'read_stream' });
        }


        $scope.processItemFeed = function (data) {
            var oResponse = data;
            var ltItems = [];
            for (var i = 0; i < oResponse.length; i++) {
                var item = { nameFrom: '', url: '#', photo: '', message: '', id: 0 };


                if (oResponse[i].message != undefined) {
                    if (oResponse[i].id != 0) {
                        item.id = oResponse[i].id;
                        if (oResponse[i].type == "link") {
                            item.url = "https://www.facebook.com/" + oResponse[i].id;
                        } else if (oResponse[i].type == "status") {
                            item.url = "https://www.facebook.com/" + oResponse[i].id;
                        } else if (oResponse[i].type == "photo") {
                            item.url = oResponse[i].link;
                        } else {
                            item.url = "https://www.facebook.com/" + oResponse[i].id;
                        }
                        item.photo = "https://graph.facebook.com/" + oResponse[i].from.id + "/picture";
                    } else {
                        item.photo = oResponse[i].from.photo;
                    }
                    item.nameFrom = oResponse[i].from.name;
                    item.message = oResponse[i].message;
                    ltItems.push(item);
                }
            }
            return ltItems;
        };

        $scope.loadFacebookNewsFeed = function () {

            if ($scope.widget.isDefault) {
                if ($scope.widget.token != '') {
                    $scope.wasLoaded = false;
                    $scope.facebookUserLogged = true;

                    var data = [];
                    data.push({ message: 'Check your local weather in real time, just type your zip code or address into the Weather Frame.', id: 0, from: { id: 0, name: 'Take 65', photo: '/Img/Default/take65-icon-34x34.png' } });
                    data.push({ message: 'Take 65 is your trusted source for security and simplicity on the web.', id: 0, from: { id: 0, name: 'Take 65', photo: '/Img/Default/take65-icon-34x34.png' } });
                    data.push({ message: 'With the Social Media Photo feature you can access your family & friends\' photos directly from your Take 65 dashboard.', id: 0, from: { id: 0, name: 'Take 65', photo: '/Img/Default/take65-icon-34x34.png' } });
                    data.push({ message: 'Join Take 65 right now clicking on the top menu \'Join TAKE 65\'', id: 0, from: { id: 0, name: 'Take 65', photo: '/Img/Default/take65-icon-34x34.png' } });

                    $scope.facebookFeed = $scope.processItemFeed(data);

                    $scope.wasLoaded = true;
                    $rootScope.$broadcast('dataloaded');


                }
            } else {

                FB.getLoginStatus(function (response) {
                    if (response.status === 'connected') {
                        $scope.wasLoaded = false;
                        $scope.facebookUserLogged = true;

                        FB.api('/me/home', 'get', function (response) {
                            $scope.$apply(function () {
                                if (!response || response.error) {
                                    $scope.facebookUserLogged = false;
                                    $scope.$broadcast('dataloaded');
                                } else {
                                    $scope.facebookFeed = $scope.processItemFeed(response.data);
                                    $scope.wasLoaded = true;
                                    $rootScope.$broadcast('dataloaded');

                                }
                            });
                        });

                    } else if (response.status === 'not_authorized') {
                        alert('Not Authorized');
                        // not_authorized
                    } else {
                        //alert('Not logged');
                        // not_logged_in
                    }


                });

            }
        };

        $rootScope.loadWidget();
    }])

    // Controller for FeedCtrl in /Templates/Feed.html
    .controller('FeedCtrl', ['$scope', '$http', '$sce', '$rootScope', function ($scope, $http, $sce, $rootScope) {
        $scope.wasLoaded = false;
        if (_page.userWidgetFeed[('items_' + $scope.widget.id)] == undefined) {
            $http.get('/REST/UserWidgetFeedContent/' + $scope.widget.id + '/?m=' + (new Date()).getMilliseconds(), { cache: false }).
                success(function (data, status, headers, config) {
                    if (data.length > 0) {
                        $scope.highlight = data.shift();
                        $scope.highlight.description = $sce.trustAsHtml($scope.highlight.description);

                        $scope.feeds = data;

                        //$rootScope.$broadcast('dataloaded');
                        //$scope.$broadcast('callCarousel');
                        $rootScope.loadWidget();
                    }
                    $scope.wasLoaded = true;
                })
                .error(function (data, status, headers, config) {

                });
        } else {

            $scope.highlight = _page.userWidgetFeed[('items_' + $scope.widget.id)].shift();
            $scope.highlight.description = $sce.trustAsHtml($scope.highlight.description);
            $scope.feeds = _page.userWidgetFeed[('items_' + $scope.widget.id)];
            $rootScope.loadWidget();

            $scope.wasLoaded = true;
        }

        $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
            $rootScope.$broadcast('dataloaded');
            $scope.$broadcast('callCarousel');
        });

    }])

    // Controller for MoreFeedCtrl in /Templates/MoreFeed.html
    .controller('MoreFeedCtrl', ['$scope', '$http', '$sce', '$rootScope', function ($scope, $http, $sce, $rootScope) {
        $scope.widget = {}; // cria o objeto widget
        $scope.widget.id = $rootScope.widgets.attrsClick.id; // id do widget
        $scope.widget.count = 10; // de quantos em quantos itens deve ser feito o carregamento
        $scope.widget.skip = 0; // Quantos itens deve pular para trazer no ajax
        $scope.loading = false; // status se está carregando conteúdo ou não
        $scope.hasMore = false; // Se tem ou não botão de view more
        $scope.statusLoad = true; // True adiciona classe 'success' na mensagem de load e false adiciona classe 'error'
        $scope.msgfeedback = ''; // Mensagem de feedback enquanto carrega
        $scope.txtBtnMoreNews = 'More News'; // texto do botão de carregar mais
        $scope.category = 0;
        $scope.qFilter = '';

        $http.get('/REST/UserWidgetFeed/' + $scope.widget.id + '?m=' + (new Date()).getMilliseconds(), { cache: false })
            .success(function (data, status, headers, config) {
                $scope.widget.title = data.userWidget.title; // Recebe o titulo do widget
                if (data.trustedSource.length > 0) {
                    delete $scope.trustedSource;
                    $scope.trustedSource = data.trustedSource;
                }
                $scope.widget.wasLoaded = true;

                $rootScope.$broadcast('dataloaded');
            })
            .error(function (data, status, headers, config) {
            });

        $scope.getFeeds = function (more) {
            $scope.loading = true;
            $scope.txtBtnMoreNews = 'Loading...';
            $scope.widget.wasLoaded = false;

            $http.get('/REST/UserWidgetFeedContent/' + $scope.widget.id + '/' + $scope.widget.count + '/' + $scope.widget.skip + '/' + $scope.category + '/' + (($scope.qFilter !== '') ? $scope.qFilter : '') + '?m=' + (new Date()).getMilliseconds(), { cache: false })
                .success(function (data, status, headers, config) {

                    $scope.hasMore = (data.length < $scope.widget.count) ? false : true;
                    $scope.feeds = (more) ? $scope.feeds.concat(data) : $scope.feeds = data;


                    $rootScope.$broadcast('dataloaded');
                    $scope.loading = false;
                    $scope.txtBtnMoreNews = 'More News';
                    $scope.widget.wasLoaded = true;
                })
                .error(function (data, status, headers, config) {
                    $scope.loading = false;
                    $scope.msgfeedback = 'Error, try again.';
                });
        };

        // Carrega mais itens
        $scope.loadMore = function () {
            if (!$scope.loading) {
                $scope.widget.skip += $scope.widget.count;
                $scope.getFeeds(true);
            }
        };

        $scope.setTrusted = function (filter) {
            $scope.widget.skip = 0;
            $scope.category = filter;
            $scope.getFeeds();
        };

        $scope.searchNews = function () {
            $scope.getFeeds();
        };

        $scope.getFeeds(false);
    }])

    // Controller for MyWebsiteCtrl in /Templates/MyWebsite.html

    .controller('MyWebsiteCtrl', ['$scope', '$http', '$sce', '$rootScope', function ($scope, $http, $sce, $rootScope) {
        if (_page.userWidgetBookmark[('items_' + $scope.widget.id)] == undefined) {
            $http.get('/REST/UserWidgetBookmark/' + $scope.widget.id + '/?m=' + (new Date()).getMilliseconds(), { cache: false }).
                success(function (data, status, headers, config) {
                    $scope.source = []
                        .concat(_.map(data.trustedSource, function (item) {
                            item.trusted = true;
                            return item;
                        }))
                        .concat(_.map(data.source, function (item) {
                            item.trusted = false;
                            return item;
                        }))
                        .slice(0, 8);

                    $rootScope.$broadcast('dataloaded');
                    $rootScope.loadWidget();

                    $scope.widget.sourcesLength = data.trustedSource.length + data.source.length;
                })
                .error(function (data, status, headers, config) {

                });
        } else {
            $scope.source = []
                        .concat(_.map(_page.userWidgetBookmark[('items_' + $scope.widget.id)].trustedSource, function (item) {
                            item.trusted = true;
                            return item;
                        }))
                        .concat(_.map(_page.userWidgetBookmark[('items_' + $scope.widget.id)].source, function (item) {
                            item.trusted = false;
                            return item;
                        }))
                        .slice(0, 8);

            $rootScope.$broadcast('dataloaded');
            $rootScope.loadWidget();

            $scope.widget.sourcesLength = _page.userWidgetBookmark[('items_' + $scope.widget.id)].trustedSource.length + _page.userWidgetBookmark[('items_' + $scope.widget.id)].source.length;
        }
    }])

    // Controller for MoreWebsiteCtrl in /Templates/MoreMyWebsite.html
    .controller('MoreWebsiteCtrl', ['$scope', '$http', '$sce', '$rootScope', function ($scope, $http, $sce, $rootScope) {
        $scope.widget = {}; // cria o objeto widget
        $scope.widget.id = $rootScope.widgets.attrsClick.id; // id do widget
        $scope.widget.title = $rootScope.widgets[$rootScope.widgets.attrsClick.indexWidget].title; // Pega o titulo do widget já armazenado em $rootScope de acordo com o index de array que está no botão

        $http.get('/REST/UserWidgetBookmark/' + $scope.widget.id + '?m=' + (new Date()).getMilliseconds(), { cache: false })
            .success(function (data, status, headers, config) {
                if (data.trustedSource.length > 0) {
                    $scope.trustedSource = data.trustedSource;
                }

                if (data.source.length > 0) {
                    $scope.source = data.source;
                }

                $rootScope.$broadcast('dataloaded');
            })
            .error(function (data, status, headers, config) {
            });
    }])


    .controller('MyProfileCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {
        $scope.changePassword = false;
        $scope.step = 1;
        //$scope.user = { email: '', name: '', facebookId: '', yearofbirth: '', gender: '', password: '', preference: [] };
        $scope.userPrefences = [];

        $http.get('/REST/User/?m=' + (new Date()).getMilliseconds())
            .success(function (data, status, headers, config) {
                if (data.id != undefined) {
                    $scope.user = data;
                } else {
                    window.location.href = '/?SessionExpired';
                }
            })
            .error(function (data, status, headers, config) {
                $scope.feedbackRegister.msg = data.response;
                $scope.feedbackRegister.status = false;
            });


        $scope.setStep = function (i) {
            var retStep = true;
            switch (i) {
                case 2:
                    retStep = $scope.step2();
                    break;
            }

            if (!retStep)
                return false;

            $scope.step = i;
            $rootScope.$broadcast('dataloaded');
        };

        $scope.getChkId = function (list) {
            var chkList = [];

            for (var i in list) {
                if (list[i].chk)
                    chkList.push({ id: list[i].id });
            }

            return chkList;
        };

        $scope.step2 = function () {
            if ($scope.register.$invalid) {
                $scope.register.showMessage = true;
                return false;
            }

            $http.get('/REST/UserPreference/?m=' + (new Date()).getMilliseconds())
                .success(function (data, status, headers, config) {
                    if (data.length > 0) {
                        $scope.userPrefences = data;
                    }
                    var list = $scope.userPrefences;
                    for (var i in list) {
                        for (var n in $scope.user.preference) {
                            if (list[i].id == $scope.user.preference[n].id) {
                                list[i].chk = true;
                            }
                        }
                    }
                    $scope.wasLoaded = true;
                    $rootScope.$broadcast('dataloaded');
                })
                .error(function (data, status, headers, config) {
                });

            return true;
        };

        $scope.userChangePassword = function () {
            $scope.changePassword = !$scope.changePassword;
            $.colorbox.resize();
        };
        $scope.submit = function () {
            var deafultUser = {
                facebookId: '',
                login: '',
                password: '',
                name: '',
                email: '',
                birthdate: '',
                yearofbirth: '',
                gender: '',
                state: '',
                city: '',
                postalCode: '',
                preference: []
            };

            $scope.user.preference = $scope.getChkId($scope.userPrefences);
            $scope.user.categories = $scope.getChkId($scope.categories);
            $scope.user.birthdate = '';

            $scope.feedbackRegister = {
                msg: 'Updating user...',
                status: true
            };

            var user = angular.extend(deafultUser, $scope.user);

            $http.post('/REST/User/Update/', user)
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        $.colorbox.close();
                        $scope.$broadcast('closeColorbox');
                        $('#closeModal').trigger('click');
                    } else {
                        $scope.feedbackRegister.msg = data.response;
                        $scope.feedbackRegister.status = false;
                    }
                })
                .error(function (data, status, headers, config) {
                    $scope.feedbackRegister.msg = data.response;
                    $scope.feedbackRegister.status = false;
                });
        };
    }])


    // Controller for RegisterCtrl in /Templates/Register.html
    .controller('RegisterCtrl', ['$scope', '$http', '$rootScope', 'gcfg', function ($scope, $http, $rootScope, gcfg) {
        $scope.overlayClose = false;
        $scope.step = 1;
        $scope.facebook = { register: false };
        console.log(gcfg);
        $scope.user = { email: $scope.newUseremail };
        if ($scope.newUseremail != "") {
            $scope.step = 2;
        }

        //$scope.user = $scope.user || {};
        //$scope.user.email = 'asda@asas.com';
        var yr = new Date().getFullYear();
        var minyear = yr - 100; // at most 100year old 
        //var maxyear = yr - 18; //at lease 18yr older
        var years = [];
        for (var i = parseInt(minyear) ; i <= parseInt(yr) ; i++) {
            //conole.log(i);
            years.push(i);
        }
        $scope.byears = years;
        $scope.user.yearofbirth = yr - 65;
        //console.log(years);

        //clear feedback errors to start clean
        $scope.clearFeedback = function () {
            $scope.google.feedback.msg = '';
            $scope.facebook.feedback.msg = '';
        };

        $scope.setStep = function (i) {
            var retStep = true;
            switch (i) {
                case 2:
                    retStep = $scope.step2();
                    break;
                case 3:
                    retStep = $scope.step3();
                    break;
                case 4:
                    retStep = $scope.step4();
                    break;
            }

            if (!retStep)
                return false;

            $scope.step = i;
            $rootScope.$broadcast('dataloaded');
        };

        $scope.getChkId = function (list) {
            var chkList = [];

            for (var i in list) {
                if (list[i].chk)
                    chkList.push({ id: list[i].id });
            }

            return chkList;
        };

        $scope.step2 = function () {
            return true;
        };

        $scope.step3 = function () {
            if ($scope.register.$invalid && !$scope.facebook.register && !$scope.google.register) {
                $scope.register.showMessage = true;
                return false;
            }

            $http.get('/REST/UserPreference/?m=' + (new Date()).getMilliseconds())
                .success(function (data, status, headers, config) {
                    if (data.length > 0) {
                        $scope.userPrefences = data;
                    }
                    $scope.wasLoaded = true;
                    $rootScope.$broadcast('dataloaded');
                })
                .error(function (data, status, headers, config) {
                });

            return true;
        };

        $scope.step4 = function () {
            $http.get('/REST/TrustedSourceCategory/?m=' + (new Date()).getMilliseconds(), { cache: false })
                .success(function (data, status, headers, config) {
                    $scope.wasLoadedCategories = true;
                    if (data.length > 0) {
                        $scope.categories = data;
                    }

                    $rootScope.$broadcast('dataloaded');
                })
                .error(function (data, status, headers, config) {
                });

            return true;
        };

        $scope.submit = function () {
            var deafultUser = {
                facebookId: '',
                googleId: '',
                login: '',
                password: '',
                name: '',
                email: '',
                birthdate: '',
                yearofbirth: '',
                gender: '',
                state: '',
                city: '',
                postalCode: '',
                preference: []
            };

            $scope.user.preference = $scope.getChkId($scope.userPrefences);
            $scope.user.categories = $scope.getChkId($scope.categories);
            $scope.user.email = $scope.register.email.$modelValue;
            $scope.feedbackRegister = {
                msg: 'Registering user...',
                status: true
            };

            var user = angular.extend(deafultUser, $scope.user);

            $http.post('/REST/User/', user)
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        //$scope.feedbackRegister.msg = 'Register OK.';
                        //window.location.href = '/';
                        $('#openWizard').trigger('click');
                        //$scope.$parent.nextCustomStep();
                    } else {
                        $scope.feedbackRegister.msg = data.response;
                        $scope.feedbackRegister.status = false;
                    }
                })
                .error(function (data, status, headers, config) {
                    $scope.feedbackRegister.msg = data.response;
                    $scope.feedbackRegister.status = false;
                });
        };
        //gplus register
        $scope.registerGoogle = function () {
            console.log("register with google called...");
            //initlaize local variables
            $scope.google = {};
            $scope.google.feedback = {};
            $scope.google.feedback.status = true;
            //$scope.clearFeedback();
            // $scope.facebook.feedback.msg = '';
            $scope.google.feedback.msg = 'Authenticating on Google...';
            $rootScope.$broadcast('dataloaded');
            //flag to ensure only one callback
            var calbackflag = false;
            console.log("calling gapi.auth.singIn....");
            gapi.auth.signIn({
                'clientid': gcfg.clientId,
                'state': gcfg.state,
                'cookiepolicy': 'single_host_origin',
                'accesstype': 'offline',
                'responsetype': 'code',
                'approvalprompt': 'auto',
                'includegrantedscopes': 'true',
                'redirecturi': 'postmessage',
                'scope': gcfg.scopes.plus + " " + gcfg.scopes.me + " " + gcfg.scopes.email + " " + gcfg.scopes.profile,
                'callback': function (authresult) {
                    if (!calbackflag) {
                        if (typeof authresult["code"] != 'undefined') {
                            calbackflag = true;
                        }
                        console.log("Aplication state :   ", gcfg.state);
                        console.log("in google singn callback on gapi.auth.signIn -- authresult...");
                        console.log("authresult --- ", authresult);
                        console.log("authresult[status][method]----", authresult["status"]["method"]);
                        console.log("autherization code ....", authresult["code"]);
                        console.log("access tocken ....", authresult["access_token"]);
                        console.log("post to /REST/User/RegisterGoogleAccount  params... state:   ", gcfg.state, "authcode:  ", authresult["code"]);
                        $http.post('/REST/User/RegisterGoogleAccount', { state: gcfg.state, authcode: authresult["code"] }).
                            success(function (data) {
                                console.log("response received :  ", data.response);
                                if (data.status) {
                                    //console.log(data.response);
                                    //take it though customizing home page process
                                    $scope.setStep(2);

                                    $scope.user.googleId = gcfg.state;
                                    $scope.google.register = true;
                                    $scope.google.feedback.status = true;
                                    $scope.google.feedback.msg = '';
                                }
                                else {
                                    //console.log(data.response);
                                    //inform the user to use login
                                    //$scope.google.feedback = 'Google Account Already Exists';
                                    console.log("Google account already registered...  REST response:   " + data.response);
                                    if (data.response.length > 0) {
                                        $scope.google.feedback.msg = data.response;
                                    }
                                    else {
                                        $scope.google.feedback.msg = 'Google account already registered';
                                    }

                                    $scope.google.feedback.status = false;
                                    //window.location.href = '/';
                                }
                            })
                            .error(function () {

                                console.log("FATAL ERROR -- google user could not be registered...");
                                $scope.google.feedback.msg = 'Google account is not available';
                                $scope.google.feedback.status = false;
                            });
                    }
                }
            })
        }

        $scope.registerFacebook = function () {
            $scope.facebook = {};
            $scope.facebook.feedback = {};
            $scope.facebook.feedback.status = true;
            //$scope.clearFeedback();
            //$scope.google.feedback.msg = '';
            $scope.facebook.feedback.msg = 'Authenticating on facebook...';
            $rootScope.$broadcast('dataloaded');
            console.log("registerFacebook called....");

            FB.login(function (response) {
                if (response.authResponse) {
                    var authResponse = response.authResponse;

                    // Get user facebook data
                    FB.api('/me', function (response) {
                        // Check if user already has a registry in our site
                        $scope.facebook.response = response;
                        //if(response.email.length > 0) {
                        //check if email registered in take65 
                        $http.post('/REST/User/Check', { email: response.email }).
                            success(function (res) {
                                console.log("/REST/User/Check .. response - ", res.response);
                                if (!(res.status)) {
                                    var errmsg = $scope.facebook.response.email + "  is already registered with take65";
                                    console.log(errmsg);
                                    $scope.facebook.feedback.status = false;
                                    $scope.facebook.feedback.msg = errmsg;
                                }
                                else {
                                    $http.post('/REST/User/CheckFacebook', { facebookId: response.id }).
                                        success(function (data) {
                                            if (data.status) {
                                                //$scope.$apply(function () {
                                                $scope.setStep(2);
                                                $scope.user.email = $scope.facebook.response.email;
                                                $scope.user.facebookId = $scope.facebook.response.id;
                                                $scope.user.name = $scope.facebook.response.name;
                                                if ($scope.facebook.response.location != undefined) {
                                                    $scope.user.city = $scope.facebook.response.location.name;
                                                }
                                                $scope.user.gender = $scope.facebook.response.gender;
                                                $scope.facebook.register = true;
                                                //});
                                            }
                                            else {
                                                $http.post('/REST/User/Login', { facebookId: response.id, facebookToken: authResponse.accessToken })
                                                    .success(function (data, status, headers, config) {
                                                        if (data.status) {
                                                            //$scope.facebook.feedback = 'Facebook Account Already Exists';
                                                            $scope.facebook.feedback.msg = 'Facebook Account Already Exists';
                                                            window.location.href = '/';
                                                        }
                                                        else {
                                                            $scope.facebook.feedback.status = false;
                                                            // User did not authorized our application to access his data in Facebook
                                                            $scope.facebook.feedback.msg = 'Your Facebook account is not available at the moment.';
                                                        }
                                                    })
                                                    .error(function (data) {
                                                        $scope.facebook.feedback.status = false;
                                                        // User did not authorized our application to access his data in Facebook
                                                        $scope.facebook.feedback.msg = 'Your Facebook account is not available at the moment.';
                                                    });
                                            }
                                        })
                                        .error(function () {
                                            alert('check facebook error');
                                            $scope.facebook.feedback.status = false;
                                            // User did not authorized our application to access his data in Facebook
                                            $scope.facebook.feedback.msg = 'Your Facebook account is not available at the moment.';
                                        });
                                }
                            }).
                            error(function () {
                                var errmsg = "Error in checking user email take65";
                                console.log(errmsg);
                                $scope.facebook.feedback.status = false;
                                $scope.facebook.feedback.msg = errmsg;
                                return;
                            });
                    });
                }
                else {
                    $scope.facebook.feedback.status = false;
                    // User did not authorized our application to access his data in Facebook
                    $scope.facebook.feedback.msg = 'You have canceled Facebook connection.';
                }
            }, { scope: 'email' });
        };
    }])

    // Controller for SupportCtrl in /Templates/Support.html
    .controller('SupportCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {
        $scope.feedbackStatus = 'error';
        $scope.isSending = false;
        $scope.isFeedback = false;
        $scope.feedbackMessage = '';
        $scope.title = 'Support Request';
        $scope.userExists = false;

        $http.get('/REST/User/?m=' + (new Date()).getMilliseconds())
           .success(function (data, status, headers, config) {
               if (data.id != undefined) {
                   $scope.name = data.name;
                   $scope.email = data.email;
                   $scope.userExists = true;
               } else {
                   $scope.name = '';
                   $scope.email = '';
                   $scope.userExists = false;
               }
           })
           .error(function (data, status, headers, config) {
               $scope.msgError(data.response);
               $rootScope.$broadcast('dataloaded');
           });

        $scope.submit = function () {
            $scope.isFeedback = false;
            $scope.classStatus = '';
            $scope.supportMessage = 'Sending mail...';
            $scope.isSending = true;
            $rootScope.$broadcast('dataloaded');

            var url = '/REST/User/SendSupportRequestEmail/' + $scope.name + '/' + $scope.email + '/' + $scope.subject + '/' + $scope.issue;

            var objData = {
                name: $scope.name,
                email: $scope.email,
                subject: $scope.subject,
                issue: $scope.issue
            };

            $http.post(url, objData)
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        if (data.response == 'Issue submitted.') {
                            $scope.feedbackStatus = 'success';
                            $scope.supportMessage = 'sent.';
                            $scope.reloadHome = true;
                        }
                    } else {
                        $scope.msgError(data.response);
                    }
                    $rootScope.$broadcast('dataloaded');
                })
                .error(function (data) {
                    $scope.msgError(data.response);
                    $rootScope.$broadcast('dataloaded');
                });
        };
    }])
    .controller('GetToKnowAboutSiteCtrl', ['$scope', '$http', '$rootScope', 'gcfg', function ($scope, $http, $rootScope, gcfg) {
        $scope.overlayClose = false;
        $scope.title = 'Join Take 65 for Free';
        $scope.newUseremail = '';
        $scope.login = $scope.newUseremail;
        $scope.regsterEmail = function () {
            $http.post('/REST/User/RegisterNewUserEmail', { email: $scope.newUseremail })
               .success(function (data, status, headers, config) {
                   $rootScope.$broadcast('dataloaded');
                   //$rootScope.$broadcast('nglogged');
               })
               .error(function (data) {
                   $scope.msgError(data.response);
                   $rootScope.$broadcast('dataloaded');
               });
        }
    }])
     // Controller for LoginCtrl in /Templates/Login.html
   .controller('LoginCtrl', ['$scope', '$http', '$rootScope', 'gcfg', function ($scope, $http, $rootScope, gcfg) {
       $scope.feedbackStatus = 'error';
       $scope.isSending = false;
       $scope.isFeedback = false;
       $scope.feedbackMessage = '';
       $scope.forgotPass = false;
       $scope.forgotPasswordValid = true;
       $scope.title = 'Login TAKE 65';

       console.log("LoginCtrl ctr.......   ");
       $scope.login = $scope.newUseremail;
       $scope.setForgotPass = function (v) {
           $scope.forgotPass = v;
           $rootScope.$broadcast('dataloaded');

           if (v)
               $scope.title = 'Resend your password';
           else
               $scope.title = 'Login Take 65';
       };

       $scope.forgotPassword = function () {
           $scope.isFeedback = false;
           $scope.isSendingForgot = true;
           $scope.feedbackStatusForgot = 'success';
           $scope.forgotMessage = 'Sending data...';

           $http.post('REST/User/ForgotPassword', { email: $scope.loginForgot })
               .success(function (data, status, headers, config) {

                   if (data.status) {
                       $scope.forgotMessage = '';
                       $scope.feedbackMessage = 'Your new password was sent to ' + $scope.loginForgot + '. Please, check your email.';
                       $scope.isFeedback = true;
                       $scope.setForgotPass(false);
                   } else {
                       $scope.feedbackStatusForgot = 'error';
                       $scope.forgotMessage = data.response;
                   }

                   $rootScope.$broadcast('dataloaded');
                   //$rootScope.$broadcast('nglogged');
               })
               .error(function (data) {
                   $scope.feedbackStatusForgot = 'error';
                   $scope.forgotMessage = data.response;
                   $rootScope.$broadcast('dataloaded');
               });
       };

       // Função para logar com a conta do facebook
       $scope.loginFacebook = function () {
           $scope.isFeedback = false;
           $scope.facebookSending = true;
           $scope.facebookMessage = 'Sending data...';
           $scope.feedbackFacebook = 'success';
           $rootScope.$broadcast('dataloaded');

           FB.login(function (response) {
               if (response.authResponse) {
                   $scope.facebookMessage = 'Authenticated user.';
                   var authResponse = response.authResponse;
                   $rootScope.$broadcast('dataloaded');
                   // Get user facebook data
                   FB.api('/me', function (response) {
                       $scope.facebookMessage = 'Loading user information...';
                       // Check if user already has a registry in our site
                       $http.post('/REST/User/Login', { facebookId: response.id, facebookToken: authResponse.accessToken })
                           .success(function (data, status, headers, config) {
                               if (data.status) {
                                   $scope.facebookMessage = 'Logged';
                                   window.location.href = '/';
                                   $scope.reloadHome = true;
                               } else {
                                   $scope.feedbackFacebook = 'error';
                                   $scope.facebookMessage = data.response;
                               }

                               $rootScope.$broadcast('dataloaded');

                           })
                           .error(function (data) {
                               $scope.feedbackFacebook = 'error';
                               $scope.facebookMessage = data.response;
                               $rootScope.$broadcast('dataloaded');
                           });
                   });
               } else {
                   // User did not authorized our application to access his data in Facebook
                   $scope.feedbackFacebook = 'error';
                   $scope.facebookMessage = 'You have canceled Facebook connection.';
                   $rootScope.$broadcast('dataloaded');
               }
           }, { scope: 'email,read_friendlists,user_photos,friends_photos' });
       };

       //login google
       $scope.loginGoogle = function () {
           $scope.isFeedback = false;
           $scope.googleSending = true;
           $scope.googleMessage = 'Sending data...';
           $scope.feedbackGoogle = 'success';
           $rootScope.$broadcast('dataloaded');

           var calbackflag = false;
           console.log("calling gapi.auth.singIn....");
           gapi.auth.signIn({
               'clientid': gcfg.clientId,
               'state': gcfg.state,
               'cookiepolicy': 'single_host_origin',
               'accesstype': 'offline',
               'responsetype': 'code',
               'approvalprompt': 'auto',
               'includegrantedscopes': 'true',
               'redirecturi': 'postmessage',
               'scope': gcfg.scopes.plus + " " + gcfg.scopes.me + " " + gcfg.scopes.email + " " + gcfg.scopes.profile,
               'callback': function (authresult) {
                   if (!calbackflag) {
                       if (typeof authresult["code"] != 'undefined') {
                           calbackflag = true;
                       }
                       console.log("Aplication state :   ", gcfg.state);
                       console.log("in google singn callback on gapi.auth.signIn -- authresult...");
                       console.log("authresult --- ", authresult);
                       console.log("authresult[status][method]----", authresult["status"]["method"]);
                       console.log("autherization code ....", authresult["code"]);
                       console.log("access tocken ....", authresult["access_token"]);
                       //get google user id
                       if (authresult["access_token"]) {
                           gapi.client.load('plus', 'v1', function () {
                               gapi.client.plus.people.get({ 'userId': 'me' })
                                   .execute(function (resp) {
                                       // Shows profile information
                                       console.log("response received  : ", resp);
                                       console.log("google userid :    ", resp.id);

                                       $http.post('/REST/User/Login', { googleId: resp.id })
                                           .success(function (data, status, headers, config) {
                                               if (data.status) {
                                                   $scope.googleMessage = 'Logged, loading preferences...';
                                                   window.location.href = '/';
                                                   $scope.reloadHome = true;
                                               }
                                               else {
                                                   $scope.feedbackGoogle = 'error';
                                                   $scope.googleMessage = data.response;
                                               }
                                               $rootScope.$broadcast('dataloaded');
                                           })
                                           .error(function (data) {
                                               $scope.feedbackGoogle = 'error';
                                               $scope.googleMessage = data.response;
                                               $rootScope.$broadcast('dataloaded');
                                           });
                                   })
                           });

                           //gapi.client.load("oauth2", "v1", function () {
                           //    var req = gapi.client.oauth2.userinfo.get();
                           //    req.execute(function (res) {
                           //        console.log(res["useremail"]);
                           //    })
                           //})
                       }



                       //    console.log("post to /REST/User/RegisterGoogleAccount  params... state:   ", gcfg.state, "authcode:  ", authresult["code"]);
                       //    $http.post('/REST/User/RegisterGoogleAccount', { state: gcfg.state, authcode: authresult["code"] }).
                       //        success(function (data) {
                       //            console.log("response received :  ", data.response);
                       //            if (data.status) {
                       //                //console.log(data.response);
                       //                //take it though customizing home page process
                       //                $scope.setStep(2);

                       //                $scope.user.googleId = gcfg.state;
                       //                $scope.google.register = true;
                       //            }
                       //            else {
                       //                //console.log(data.response);
                       //                //inform the user to use login
                       //                //$scope.google.feedback = 'Google Account Already Exists';
                       //                console.log("Google account already registered...  REST response:   " + data.response);
                       //                $scope.google.feedback.msg = 'Google account already registered';
                       //                $scope.google.feedback.status = false;
                       //                //window.location.href = '/';
                       //            }
                       //        })
                       //        .error(function () {

                       //            console.log("FATAL ERROR -- google user could not be registered...");
                       //            $scope.google.feedback.msg = 'Google account is not available';
                       //            $scope.google.feedback.status = false;
                       //        });
                       //}
                   }
               }
           })
       }





       // Função para logar com login e password
       $scope.submit = function () {
           //var a = window.open('https://api.login.yahoo.com/oauth/v2/get_request_token?oauth_nonce=123456789&oauth_timestamp=1257965367&oauth_consumer_key=dj0yJmk9YTg0TXhJMjJKTXM3JmQ9WVdrOWFHRTNPRzFaTm1zbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD0wNw--&oauth_signature_method=plaintext&oauth_signature=4b9a36b6f819efba691a669622088cf0ba3a1af5%26&oauth_version=1.0&xoauth_lang_pref=en-us&oauth_callback=http://www.take65.com​', '_blank', 'location=yes,height=570,width=520,scrollbars=yes,status=yes');

           $scope.isFeedback = false;
           $scope.classStatus = '';
           $scope.loginMessage = 'Sending data...';
           $scope.isSending = true;
           $rootScope.$broadcast('dataloaded');

           $http.post('/REST/User/Login', { email: $scope.login, password: $scope.password })
               .success(function (data, status, headers, config) {
                   if (data.status) {
                       if (data.response == 'Change Password') {
                           $scope.password = '';
                           $('#openChangePassword').trigger('click');
                       } else {
                           $scope.feedbackStatus = 'success';
                           $scope.loginMessage = 'OK.';
                           $scope.reloadHome = true;
                       }
                   } else {
                       $scope.msgError(data.response);
                   }

                   $rootScope.$broadcast('dataloaded');
                   //$rootScope.$broadcast('nglogged');
               })
               .error(function (data) {
                   $scope.msgError(data.response);
                   $rootScope.$broadcast('dataloaded');
               });
       };

       $scope.changePassword = function () {
           $scope.isFeedback = false;
           if ($scope.password != '') {
               if ($scope.password != $scope.password2) {
                   $scope.loginMessage = 'Your password does not match';
                   $scope.isSending = false;
               } else {
                   $scope.classStatus = '';
                   $scope.loginMessage = 'Sending data...';
                   $scope.isSending = true;

                   $http.post('/REST/User/UpdatePassword', { password: $scope.password })
                   .success(function (data, status, headers, config) {
                       if (data.status) {
                           $scope.feedbackStatus = 'success';
                           $scope.loginMessage = 'OK.';
                           $scope.reloadHome = true;
                       } else {
                           $scope.msgError(data.response);
                       }
                       $rootScope.$broadcast('dataloaded');
                   })
                   .error(function (data) {
                       $scope.msgError(data.response);
                       $rootScope.$broadcast('dataloaded');
                   });
               }
           }
       };

       // Mensagem de erro
       $scope.msgError = function (msg) {
           $scope.feedbackStatus = 'error';
           $scope.loginMessage = msg;
       };
   }])


    // Controller for AddNewWidgetCtrl in /Templates/AddNewWidget.html
    .controller('AddNewWidgetCtrl', ['$timeout', '$scope', '$http', '$rootScope', function ($timeout, $scope, $http, $rootScope) {
        $scope.hasLoad = false;
        $rootScope.edit = undefined;
        $scope.hasValidFacebookToken = false;
        $scope.isSocialMediaExists = false;

        $http.get('/REST/UserWidgetFacebookPhotos/GetToken?m=' + (new Date()).getMilliseconds())
        .success(function (data, status, headers, config) {
            $scope.hasValidFacebookToken = data.status;
            if (data.status) {
                $rootScope.facebookToken = data.response;
            }
        })
        .error(function (data, status, headers, config) {
            $scope.hasValidFacebookToken = false;
        });

        $scope.VerifyForSocialMediaFrame = function () {
            $scope.isSocialMediaExists = true;
            $timeout(function () {
                $.colorbox.resize({ width: $(".hold-modal").width() + 50 });
            }, 100);
        };

        $scope.addTravel = function () {
            $scope.hasLoad = true;
            $rootScope.$broadcast('dataloaded');
            $scope.loadMsg = 'Creating a frame...';

            $http.post('/REST/UserWidgetBookmark/Travel/')
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        $scope.loadMsg = 'Frame created, please wait...';
                        //window.location.href = '/';
                        $rootScope.addWidgetLast(data.response);
                        $scope.$broadcast('closeColorbox');
                        $('#closeModal').trigger('click');
                    } else {
                        $scope.loadMsg = data.response;
                    }
                })
                .error(function (data, status, headers, config) {
                });
        };

        $scope.addFacebook = function () {
            $scope.hasLoad = true;
            $rootScope.$broadcast('dataloaded');
            $scope.loadMsg = 'Creating a frame...';

            $http.post('/REST/UserWidgetFacebook/')
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        $scope.loadMsg = 'Frame created, please wait...';
                        //window.location.href = '/';
                        $rootScope.addWidgetLast(data.response);
                        $scope.$broadcast('closeColorbox');
                        $('#closeModal').trigger('click');
                    } else {
                        $scope.loadMsg = data.response;
                    }
                })
                .error(function (data, status, headers, config) {
                });
        };


        $scope.addFacebookPhotos = function () {
            $scope.hasLoad = true;
            $scope.loadMsg = 'Creating a frame...';
            $rootScope.$broadcast('dataloaded');



            $http.post('/REST/UserWidgetFacebookPhotos/')
                .success(function (data, status, headers, config) {

                    if (data.status) {
                        $rootScope.addWidgetFirst(data.response);
                        $rootScope.$broadcast('dataloaded');
                        $scope.loadMsg = 'Frame created, please wait...';
                        //window.location.href = '/';
                        $scope.$broadcast('closeColorbox');
                        $('#closeModal').trigger('click');

                    } else {
                        //$scope.loadMsg = data.response;
                        $scope.$broadcast('closeColorbox');
                        $('#closeModal').trigger('click');
                    }

                })
                .error(function (data, status, headers, config) {
                });
        };

        $scope.addWeather = function () {
            $scope.hasLoad = true;
            $rootScope.$broadcast('dataloaded');
            $scope.loadMsg = 'Creating a frame...';

            $http.post('/REST/UserWidgetWeather/')
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        $scope.loadMsg = 'Frame created, please wait...';
                        //window.location.href = '/';
                        $rootScope.addWidgetLast(data.response);
                        $scope.$broadcast('closeColorbox');
                        $('#closeModal').trigger('click');
                    } else {
                        $scope.loadMsg = data.response;
                    }
                })
                .error(function (data, status, headers, config) {
                });
        };
    }])

    // Controller Facebook Login
    .controller('FacebookLoginCtrl', function ($scope, $http, $rootScope) {
        //$rootScope.$broadcast('dataloaded');
    })

    .controller('FacebookPhotosCtrl', ['$scope', '$http', '$sce', '$rootScope', function ($scope, $http, $sce, $rootScope) {
        $scope.hasLoad = false;
        $scope.hasValidFacebookToken = false;
        $scope.photos = [];
        $scope.userFriends = [];
        $scope.friendsSearch = [];
        $scope.highlightFriend = { id: 0, photoCount: 0, name: '' };

        $scope.currentPage = 0;
        $scope.pageSize = 10;


        $rootScope.userFriends = [];
        $rootScope.userFriendsSearch = [];

        $scope.checkFacebookToken = function (forceManageFriends) {
            $http.get('/REST/UserWidgetFacebookPhotos/GetToken?m=' + (new Date()).getMilliseconds())
            .success(function (data, status, headers, config) {
                $scope.hasValidFacebookToken = data.status;
                if (data.status) {
                    $rootScope.facebookToken = data.response;
                    $scope.loadUserFriends(forceManageFriends);
                }
            })
                .error(function (data, status, headers, config) {
                    $scope.hasValidFacebookToken = false;
                });
        }

        $scope.loadUserFriends = function (forceManageFriends) {
            $http.get('/REST/UserWidgetFacebookPhotos/UserFriends?m=' + (new Date()).getMilliseconds())
                .success(function (data, status, headers, config) {
                    if (data.length > 0) {
                        $scope.photos = [];
                        $rootScope.userFriends = data.slice(0);
                        $scope.userFriends = data;
                        /*
                        if (data.length > 5) {
                            var itemHighlight = Math.floor(Math.random() * data.length);
                            $scope.highlightFriend = data[itemHighlight];
                            $scope.userFriends.splice(itemHighlight, 1);
                        } else {
                            $scope.highlightFriend = { id: 0, photoCount: 0, name: '' };
                        }*/
                        $scope.highlightFriend = { id: 0, photoCount: 0, name: '' };

                        $rootScope.$broadcast('dataloaded');
                        //setTimeout(function () {
                        //$scope.$broadcast('callCarouselFbPhoto');
                        //}, 500);

                        //$scope.$broadcast('dataloaded');


                    } else {
                        $scope.userFriends = [];
                        $scope.highlightFriend = { id: 0, photoCount: 0, name: '' };
                        if (forceManageFriends) {
                            setTimeout(function () {
                                $("#lnk-edit-facebook-photo").trigger("click");
                            }, 1000);
                        }
                    }
                })
                .error(function (data, status, headers, config) {

                });
        }

        $rootScope.viewUserPhotos = $scope.viewUserPhotos = function (user) {
            $scope.currentPage = 0;
            if (user.photos == null) {
                $scope.hasLoad = true;
                $http.get('/REST/UserWidgetFacebookPhotos/FacebookPhotos/' + user.id + '?m=' + (new Date()).getMilliseconds())
                .success(function (data, status, headers, config) {
                    $scope.hasLoad = false;
                    if (data.length > 0) {

                        $scope.currentUser = user;
                        $scope.photos = data;

                        user.photos = data;
                        var blnUpdateCount = (user.photoCount != data.length);

                        user.photoCount = data.length;

                        $scope.facebookPhoto.friendName = user.name;
                        $scope.facebookPhoto.viewDetail = true;

                        /* Update User Reference to not call the next time */
                        for (var u in $scope.userFriends) {
                            if ($scope.userFriends[u].id == user.id) {
                                $scope.userFriends[u].photos = user.photos;
                                $scope.userFriends[u].photoCount = user.photos.length;
                                if (blnUpdateCount) {
                                    $scope.saveFriends();
                                }
                            }
                        }
                        /* Update User Reference to not call the next time */

                        $rootScope.$broadcast('dataloaded');
                    }
                })
                .error(function (data, status, headers, config) {
                    alert('facebook photos error');
                });
            } else {
                $scope.currentUser = user;
                $scope.photos = user.photos;

                $scope.facebookPhoto.friendName = user.name;
                $scope.facebookPhoto.viewDetail = true;
                $rootScope.$broadcast('dataloaded');
            }
        };

        $scope.saveFriends = function () {
            $http.post('/REST/UserWidgetFacebookPhotos/SaveFriends', $scope.userFriends)
                        .success(function (data, status, headers, config) {
                        });
        };

        $rootScope.rebind = function () {
            $rootScope.$broadcast('dataloaded');
        };

        $rootScope.updateFacebookFriendsList = $scope.updateFacebookFriendsList = function () {
            $scope.loadUserFriends(false);
        };

        $rootScope.showFriendsList = $scope.showFriendsList = function () {
            $scope.photos = [];
        };

        $scope.facebookLogin = function () {

            FB.login(function (response) {
                if (response.authResponse) {
                    $http.post('/REST/UserWidgetFacebookPhotos/SetToken', { facebookToken: response.authResponse.accessToken })
                        .success(function (data, status, headers, config) {
                            if (data.status) {
                                $scope.checkFacebookToken(true);
                            }
                            $rootScope.$broadcast('dataloaded');

                        })
                        .error(function (data) {

                        });


                } else {
                    // cancelled
                }
            }, { scope: 'email,read_friendlists,user_photos,friends_photos' });

        };

        $rootScope.loadWidget();
        $scope.checkFacebookToken(false);
        $rootScope.$broadcast('dataloaded');

    }])


    // Controller for FacebookPhotoDetail in /Templates/FacebookPhotoDetail.html
    .controller('FacebookPhotoDetailCtrl', function ($scope, $http, $rootScope) {
        //$scope.facebookImage = $scope.photos[$scope.$index].photo;
        //$scope.facebookImageWidth = $scope.photos[$scope.$index].width + 'px';
        //$scope.facebookImageHeight = $scope.photos[$scope.$index].height + 'px';
        //$scope.facebookImageCaption = $scope.photos[$scope.$index].name;
        //$rootScope.$broadcast('dataloaded');

        /*
        setTimeout(function () {
            $scope.$broadcast('callCarouselFbPhotoDetail');
        }, 200);
        */
    })

    // Controller for FacebookPhotosManageFriends in /Templates/FacebookPhotosManageFriends.html
    .controller('FacebookPhotosManageFriendsCtrl', function ($scope, $http, $rootScope) {
        // Swapna Changes
        $scope.facebookPhoto = { friendName: '', viewDetail: false };

        $scope.userFriends = $rootScope.userFriends.slice(0);
        $scope.facebookFriends = [];
        $scope.facebookFriendsFilter = [];
        $scope.friendSearch = { keywork: '' };


        $scope.loadFacebookFriends = function () {
            $http.get('/REST/UserWidgetFacebookPhotos/FacebookFriends?m=' + (new Date()).getMilliseconds())
                .success(function (data, status, headers, config) {
                    if (data.length > 0) {

                        $scope.facebookFriends = data;
                        for (var i in $scope.facebookFriends) {
                            for (var j in $scope.userFriends) {
                                if ($scope.facebookFriends[i].id == $scope.userFriends[j].id) {
                                    $scope.facebookFriends[i].chk = true;
                                }
                            }
                        }

                        $scope.facebookFriendsFilter = $scope.facebookFriends.slice(0);

                    } else {
                        $scope.facebookFriends = [];
                    }
                })
                .error(function (data, status, headers, config) {

                });
        }

        $scope.loadFacebookPhotos = function (user) {

            if (user.photoCount == -1) {
                user.verifing = true;
                $http.get('/REST/UserWidgetFacebookPhotos/FacebookPhotos/' + user.id + '?m=' + (new Date()).getMilliseconds())
            .success(function (data, status, headers, config) {
                user.verifing = false;
                if (data.length > 0) {

                    $scope.userFriends.push(user);

                    for (var o in $scope.userFriends) {
                        if ($scope.userFriends[o].id == user.id) {
                            $scope.userFriends[o].photos = data;
                            $scope.userFriends[o].photoCount = data.length;
                        }
                    }

                    for (var o in $scope.facebookFriends) {
                        if ($scope.facebookFriends[o].id == user.id) {
                            $scope.facebookFriends[o].photos = data;
                            $scope.facebookFriends[o].photoCount = data.length;
                        }
                    }


                    //$.colorbox.resize({ width: "910px" });

                } else {
                    alert('This user selected to not share their Facebook photos. Please, select a different user.');
                    user.photoCount = 0;
                    user.chk = false;
                }
            })
                    .error(function (data, status, headers, config) {

                    });
            } else {
                if (user.photoCount > 0) {
                    $scope.userFriends.push(o);
                } else {
                    alert('This user selected to not share their Facebook photos. Please, select a different user.');
                    user.chk = false;
                }
            }
        };

        $scope.findFriend = function () {
            $scope.facebookFriendsFilter = $scope.facebookFriends.slice(0);
            $scope.facebookFriendsFilter = _.filter($scope.facebookFriendsFilter, function (v) {
                return (v.name.toLowerCase().slice(0, $scope.friendSearch.keywork.length) == $scope.friendSearch.keywork.toLowerCase());
            });

        }

        $scope.removeFriend = function (i, id) {
            if (i != -1) {
                $scope.userFriends.splice(i, 1);
            } else {
                for (var o in $scope.userFriends) {
                    if ($scope.userFriends[o].id == id) {
                        $scope.userFriends.splice(o, 1);
                    }
                }
            }

            for (var o in $scope.facebookFriends) {
                if ($scope.facebookFriends[o].id == id) {
                    $scope.facebookFriends[o].chk = false;
                }
            }
        };

        $scope.addFriend = function (o) {
            if (o.chk) {
                //$scope.userFriends.push(o);
                $scope.loadFacebookPhotos(o);
            } else {
                $scope.removeFriend(-1, o.id);
            }
        };

        $scope.loadFacebookFriends();

        $scope.saveFriends = function () {
            $http.post('/REST/UserWidgetFacebookPhotos/SaveFriends', $scope.userFriends)
                        .success(function (data, status, headers, config) {
                            if (data.status) {
                                //$scope.$parent.LoadTake65FacebookFriends();
                                $scope.facebookPhoto.viewDetail = false;
                                $scope.facebookPhoto.friendName = '';
                                $rootScope.updateFacebookFriendsList()
                                $.colorbox.close();

                                if (data.response != 'User already have Facebook Photos Frame') {
                                    $rootScope.addWidgetFirst(data.response);
                                }

                                $rootScope.$broadcast('dataloaded');
                                $scope.$broadcast('closeColorbox');
                                $('#closeModal').trigger('click');
                            } else {
                                $scope.$broadcast('closeColorbox');
                                $('#closeModal').trigger('click');
                            }
                        })
                        .error(function (data, status, headers, config) {
                        });
        };
    })

    // Controller for AddNewWidget.FeedCtrl in /Templates/AddNewWidget/Feed.html
    .controller('AddNewWidget.FeedCtrl', ['$scope', '$http', '$rootScope', '$element', '$compile', function ($scope, $http, $rootScope, $element, $compile) {
        var edit = ($rootScope.edit !== undefined) ? true : false;
        $scope.typeWidget = $element.data('typewidget');
        $scope.clickSubmit = true;
        $scope.step = 1; // Variavel com o passo que esta
        $scope.checkeds = {}; // Cria objeto de checkeds
        $scope.checkeds.categories = []; // Cria array para categorias selecionadas
        $scope.checkeds.trustedSource = []; // Cria array para trusteds selecionados
        $scope.feedback = { status: 'error', msg: '' }; // Mensagem e status de feedback
        $scope.validation = { msg: '' };


        var url = (edit) ? '/REST/TrustedSourceCategory/EditUserWidgetNews/' + $rootScope.edit.id : '/REST/TrustedSourceCategory/' + $scope.typeWidget + '?m=' + (new Date()).getMilliseconds();
        $http.get(url, { cache: false })
            .success(function (data, status, headers, config) {
                if (data.length > 0) {
                    $scope.categories = data;

                    $scope.changeAll = {
                        id: 0,
                        title: 'Select All',
                        image: '',
                        chk: false
                    };

                }

                // Se for na edição, checka os itens que já foram selecionados pelo cliente
                if (edit)
                    $scope.checkItens();

                $scope.$broadcast('dataloaded');

                $scope.$watch('$compile', function () {
                    $scope.$broadcast('dataloaded');
                });
            })
            .error(function (data, status, headers, config) {
            });


        $scope.checkItens = function () {
            for (var iCat in $scope.categories) {
                for (var iCatSource = 0; iCatSource < $scope.categories[iCat].trustedSource.length; iCatSource++) {
                    if ($scope.categories[iCat].trustedSource[iCatSource].chk)
                        $scope.categories[iCat].chk = true;
                }
            }
        };

        // Seta mensagem e status de feedback
        var setFeedback = function (status, msg) {
            $scope.feedback.status = status;
            $scope.feedback.msg = msg;
            $rootScope.$broadcast('dataloaded');
        };

        $scope.postCategories = function () {
            $scope.clickSubmit = false;
            if (!edit) {
                if ($scope.widget == undefined) {
                    $scope.validation.msg = 'Please type a frame name to continue';
                    $scope.clickSubmit = true;
                    return false;
                } else {
                    if ($scope.widget.title == '') {
                        $scope.clickSubmit = true;
                        $scope.validation.msg = 'Please type a frame name to continue';
                        return false;
                    } else {
                        $scope.validation.msg = '';
                    }
                }
            }

            console.log($scope.getCheckeds($scope.categories));
            $scope.postTrusteds();
            /*
            setFeedback('success', 'Sending data...');
            var serviceUrl = ($rootScope.edit !== undefined) ? ("/REST/TrustedSource/Edit/" + $rootScope.edit.id) : "/REST/TrustedSource/New";

            $http.post(serviceUrl, { category: $scope.getCheckeds($scope.categories) })
                .success(function (data, status, headers, config) {
                    setFeedback('', '');

                    if (data.length > 0) {
                        $scope.trustedSource = data;
                        if ($rootScope.edit === undefined) {
                            for (var iSource in $scope.trustedSource) {
                                $scope.trustedSource[iSource].chk = true;
                            }
                        }
                    }

                    $scope.$broadcast('closeColorbox');
                    $('#closeModal').trigger('click');
                })
                .error(function (data, status, headers, config) {
                });
                */
        };

        $scope.postTrusteds = function () {
            $scope.clickSubmit = false;
            if (!edit) {
                if ($scope.widget == undefined) {
                    $scope.validation.msg = 'Please type a frame name to continue';
                    $scope.clickSubmit = true;
                    return false;
                } else {
                    if ($scope.widget.title == '') {
                        $scope.clickSubmit = true;
                        $scope.validation.msg = 'Please type a frame name to continue';
                        return false;
                    } else {
                        $scope.validation.msg = '';
                    }
                }
            }

            setFeedback('success', 'Sending data...');
            if (!edit) {
                $rootScope.repositionWidgets();
                var gridsterItems = $rootScope.$gridster.serialize();
                $rootScope.updatePosition(gridsterItems);
            };

            //, trustedSource: $scope.getCheckeds($scope.trustedSource)
            $http.post('/REST/UserWidgetFeed/' + ((edit) ? $rootScope.edit.id : ''), { title: $scope.widget.title, category: $scope.getCheckeds($scope.categories) })
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        if (edit) {

                            $('.list-gridster .box-column').each(function (i, e) {
                                if ($(e).attr('data-widget') == $rootScope.edit.id) {
                                    $rootScope.$gridster.remove_widget(e);
                                }
                            });

                            LoadBehaviorCaller['dataload-' + $rootScope.edit.id] = undefined;
                            _page.userWidgetFeed[('items_' + $rootScope.edit.id)] = undefined;
                            $rootScope.attWidget($rootScope.edit.id, data.response);
                            $scope.$broadcast('closeColorbox');
                            $('#closeModal').trigger('click');
                            $rootScope.loadDragWidgets($rootScope.edit.id);

                        } else {
                            $rootScope.addWidgetFirst(data.response);
                            $scope.$broadcast('closeColorbox');
                            $('#closeModal').trigger('click');
                        }
                    } else {
                        $scope.messageFeedback = 'Error, try again.';
                    }
                })
                .error(function (data, status, headers, config) {
                });
        };

        $scope.changeChecked = function (item) {
            item.chk = !item.chk;
            $scope.changeAll.chk = $scope.getCheckeds($scope.categories).length === $scope.categories.length;
        };

        $scope.getCheckeds = function (list) {
            var checkeds = [];
            for (var iEl in list) {
                var el = list[iEl];
                if (el.chk)
                    checkeds.push({ id: el.id });
            }

            return checkeds;
        };

        $scope.changeAllCategories = function (item) {
            $scope.checkeds.categories = [];
            item.chk = !item.chk;

            for (var el in $scope.categories) {
                $scope.categories[el].chk = item.chk;
                if (item.chk)
                    $scope.checkeds.categories.push({ id: $scope.categories[el].id });
            }
        };

        $scope.changeStep = function (step) {
            if (step === 1) {
                $scope.step = 1;
                $rootScope.$broadcast('dataloaded');
                return;
            } else if (step === 2) {
                if ($scope.clickStep2)
                    return false;

                $scope.clickStep2 = true;
                $scope.checkeds.categories = $scope.getCheckeds($scope.categories);
                if ($scope.checkeds.categories.length > 0) {
                    $scope.postCategories();
                } else {
                    setFeedback('error', 'Please select at least one category.');
                    $scope.clickStep2 = false;
                }

                return;
            }
        };

        $scope.submitAdd = function () {
            setFeedback('', '');
            $scope.checkeds.categories = $scope.getCheckeds($scope.categories);

            if ($scope.checkeds.categories.length > 0) {
                $scope.postCategories();
            } else {
                setFeedback('error', 'Please select at least one category.');
            }
        };

        $scope.submit = function () {
            setFeedback('', '');
            $scope.checkeds.trustedSource = $scope.getCheckeds($scope.trustedSource);
            if ($scope.checkeds.trustedSource.length > 0) {
                $scope.postTrusteds();
            } else {
                setFeedback('error', 'Please select at least one resources.');
                $scope.clickSubmit = true;
            }
        };

        $scope.submitInRegister = function () {
            setFeedback('', '');
            $scope.checkeds.trustedSource = $scope.getCheckeds($scope.trustedSource);
            $scope.postTrustedsInRegister();
        };

        $scope.postTrustedsInRegister = function () {
            setFeedback('success', 'Sending data, please wait...');

            $scope.ajaxController = $scope.getCheckeds($scope.categories).length;

            if ($scope.ajaxController > 0) {

                var allTrustedSource = [];
                var selectedCats = $scope.getCheckeds($scope.categories);

                for (var catSel = 0; catSel < selectedCats.length; catSel++) {
                    for (var cats = 0; cats < $scope.categories.length; cats++) {
                        if (selectedCats[catSel].id == $scope.categories[cats].id) {
                            for (var ts = 0; ts < $scope.categories[cats].trustedSource.length; ts++) {
                                var blnExisits = false;
                                for (var exists = 0; exists < allTrustedSource.length; exists++) {
                                    if (allTrustedSource[exists].id == $scope.categories[cats].trustedSource[ts].id) {
                                        blnExisits = true;
                                    }
                                }
                                if (!blnExisits) {
                                    allTrustedSource.push({ id: $scope.categories[cats].trustedSource[ts].id });
                                }

                            }
                        }
                    }
                }
                $http.post('/REST/UserWidgetFeed', { title: 'News', category: $scope.getCheckeds($scope.categories), trustedSource: allTrustedSource })
                    .success(function (data, status, headers, config) {
                        if (data.status) {
                            $scope.$parent.nextCustomStep();
                        } else {
                            $scope.messageFeedback = 'Error, try again.';
                            $rootScope.$broadcast('dataloaded');
                        }
                    })
                    .error(function (data, status, headers, config) {
                    });
            } else {
                $scope.$parent.nextCustomStep();
            }
            /*
            if ($scope.ajaxController > 0) {
                for (var requestAjax in $scope.getCheckeds($scope.categories)) {
                    
                    $http.post('/REST/UserWidgetFeed', { category: [$scope.categories[requestAjax]], trustedSource: $scope.categories[requestAjax].trustedSource })
                    .success(function (data, status, headers, config) {
                        if (data.status) {
                            $scope.ajaxController--;
                            if ($scope.ajaxController <= 0) {
                                $scope.$parent.nextCustomStep();
                            }
                        } else {
                            $scope.messageFeedback = 'Error, try again.';
                            $rootScope.$broadcast('dataloaded');
                        }
                    })
                    .error(function (data, status, headers, config) {
                    });
                }
            } else {
                $scope.$parent.nextCustomStep();
            }
            */
        };
    }])

    // Controller for AddNewWidget.MyWebsiteCtrl in /Templates/AddNewWidget/Feed.html
    .controller('AddNewWidget.MyWebsiteCtrl', ['$timeout', '$scope', '$http', '$rootScope', '$element', function ($timeout, $scope, $http, $rootScope, $element) {
        var edit = ($rootScope.edit !== undefined) ? true : false;
        $scope.websites = [];
        $scope.trustedWebsites = {};
        $scope.trusted = {};
        $scope.trusted.trustedSourceSelected = [];
        $scope.feedback = { status: 'error', msg: '' };
        $scope.validation = { msg: '' };
        $scope.clickSubmit = true;
        $scope.confirmTrusted = false;
        $scope.listTrustedCustom = {};
        $rootScope.websitesCategories = [];
        $scope.$broadcast('dataloaded');

        // Get Trusteds and Categories in REST/TrustedSourceCategory/
        $scope.getTrusteds = function () {
            var url = '/REST/TrustedSourceCategory/?m=' + (new Date()).getMilliseconds();
            $scope.listTrusted = [];
            $scope.listSources = [];
            $scope.category = {};

            $http.get(url, { cache: false })
                .success(function (data, status, headers, config) {
                    if (data.length > 0) {
                        $scope.listTrusted = data;
                        $rootScope.websitesCategories = data;
                        $scope.$parent.cListTrusted = data.length;
                        $scope.category.selected = $scope.listTrusted[0].id;

                        for (var i in $scope.listTrusted) {
                            $scope.trustedWebsites[i] = [];
                        }
                    }
                    //$.colorbox.resize({ width: "75%", height: "90%" });
                    //$rootScope.$broadcast('dataloaded');                    
                    $timeout(function () {
                        $.colorbox.resize({ width: "85%" });
                    }, 500);
                })
                .error(function (data, status, headers, config) {
                });
        };

        // Get Edit Trusteds and Categories in REST/TrustedSourceCategory/EditUserWidgetBookmark
        $scope.getEditTrusteds = function () {
            var url = '/REST/TrustedSourceCategory/EditUserWidgetBookmark/' + $rootScope.edit.id + '?m=' + (new Date()).getMilliseconds();
            $scope.listTrusted = [];
            $scope.listSources = [];
            $scope.category = {};
            $scope.widget = $rootScope.edit;
            $rootScope.deleteItem = { id: $scope.widget.id };

            $http.get(url, { cache: false })
                .success(function (data, status, headers, config) {
                    if (data.length > 0) {

                        var dataTrustedUrls = [];
                        var initialCategory = -1;

                        for (var iCat = 0; iCat < data.length; iCat++) {

                            if (data[iCat].id != 0) {
                                dataTrustedUrls.push(data[iCat]);
                                if (initialCategory == -1) {
                                    for (var icatcheck = 0; icatcheck < data[iCat].trustedSource.length; icatcheck++) {
                                        if (data[iCat].trustedSource[icatcheck].chk)
                                            initialCategory = iCat;
                                    }
                                }
                            } else {
                                for (var iCatSources = 0; iCatSources < data[iCat].trustedSource.length; iCatSources++) {
                                    $scope.websites.push({
                                        title: data[iCat].trustedSource[iCatSources].title,
                                        link: data[iCat].trustedSource[iCatSources].link
                                    });
                                }
                            }
                        }

                        $scope.listTrusted = dataTrustedUrls;
                        $scope.$parent.cListTrusted = dataTrustedUrls.length;


                        initialCategory = (initialCategory == -1) ? 0 : initialCategory;

                        $scope.category.selected = $scope.listTrusted[initialCategory].id;

                        for (var i in $scope.listTrusted) {
                            $scope.trustedWebsites[i] = [];
                        }
                        $scope.setTrustedsChecked();
                    }
                    //$.colorbox.resize({ width: "75%", height: "90%" });
                    //$rootScope.$broadcast('dataloaded');
                    $timeout(function () {
                        $.colorbox.resize({ width: "85%" });
                    }, 500);
                })
                .error(function (data, status, headers, config) {
                });
        };

        // Add new websites
        $scope.addWebsite = function () {
            $scope.websites.push({
                title: $scope.newWebsite.title,
                link: $scope.newWebsite.link
            });

            $scope.newWebsite.title = '';
            $scope.newWebsite.link = '';

            $scope.setConfirmTrusted(false);
            //$rootScope.$broadcast('dataloaded');
            //$.colorbox.resize({ width: "75%", height: "90%" });
            $timeout(function () {
                $.colorbox.resize({ width: "85%" });
            }, 500);
        };
        $scope.addWebsiteInCategory = function (iArray) {

            $scope.trustedWebsites[iArray].push({
                title: $scope.newWebsite.title,
                link: $scope.newWebsite.link
            });

            $scope.newWebsite.title = '';
            $scope.newWebsite.link = '';

            $scope.setConfirmTrusted(false);
            $rootScope.$broadcast('dataloaded');

            $(".list-dashboard .content").animate({ scrollTop: $('.list-dashboard .content')[0].scrollHeight }, 400);
        };

        $scope.setConfirmTrusted = function (val) {
            if (val) {
                $http.post('/REST/SafeWebsite', { url: $scope.newWebsite.link })
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        $scope.addWebsite();
                        $scope.confirmTrusted = false;
                    } else {
                        $scope.confirmTrusted = val;
                    }
                })
                .error(function (data, status, headers, config) {
                    $scope.confirmTrusted = val;
                });
            } else {
                $scope.confirmTrusted = val;
            }
            //$scope.confirmTrusted = val;
            //$rootScope.$broadcast('dataloaded');
        };

        $scope.setConfirmTrustedInCategory = function (val, cat) {
            if (val) {
                $http.post('/REST/SafeWebsite', { url: $scope.newWebsite.link })
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        $scope.addWebsiteInCategory(cat);
                        $scope.confirmTrusted = false;
                    } else {
                        $scope.confirmTrusted = val;
                    }
                })
                .error(function (data, status, headers, config) {
                    $scope.confirmTrusted = val;
                });
            } else {
                $scope.confirmTrusted = val;
            }
            //$scope.confirmTrusted = val;
            //$rootScope.$broadcast('dataloaded');
        };

        $scope.selectCategory = function (cat, itemID) {
            cat.selected = itemID;
            $timeout(function () {
                $.colorbox.resize({ height: $(".hold-modal").height() + 75 });
            }, 200);
        }
        // Change source and manipulate the $scope.trusted.trustedSourceSelected's array
        $scope.changeSource = function (src) {
            src.chk = !src.chk;
            $(".list-dashboard .content").animate({ scrollTop: $('.list-dashboard .content')[0].scrollHeight }, 400);

            if (src.chk) {
                $scope.trusted.trustedSourceSelected.push(src);
            } else {
                $scope.trusted.trustedSourceSelected.splice($scope.trusted.trustedSourceSelected.indexOf(src), 1);
            }
            //$.colorbox.resize({ width: "75%", height: "90%" });
            //$rootScope.$broadcast('dataloaded');
            $timeout(function () {
                $.colorbox.resize({ width: "85%" });
            }, 500);
        };
        $scope.setTrustedsChecked = function () {
            for (var i in $scope.listTrusted) {
                for (var j in $scope.listTrusted[i].trustedSource) {
                    if ($scope.listTrusted[i].trustedSource[j].chk)
                        $scope.trusted.trustedSourceSelected.push($scope.listTrusted[i].trustedSource[j]);
                }
            }
        };

        $scope.changeSourceInCategory = function (index, src) {
            $scope.trusted.trustedSourceSelected[index] = $scope.trusted.trustedSourceSelected[index] || [];
            src.chk = !src.chk;
            $(".list-dashboard .content").animate({ scrollTop: $('.list-dashboard .content')[0].scrollHeight }, 400);

            if (src.chk) {
                $scope.trusted.trustedSourceSelected[index].push(src);
            } else {
                $scope.trusted.trustedSourceSelected[index].splice(index, 1);
            }
            $rootScope.$broadcast('dataloaded');
        };

        $scope.deleteWebsite = function (index) {
            $scope.websites.splice(index, 1);
        };
        $scope.deleteWebsiteInCategory = function (index, cat) {
            $scope.trustedWebsites[cat].splice(index, 1);
        };

        $scope.toggleCustomWebsite = function () {
            $scope.customWebsiteDisplay = !$scope.customWebsiteDisplay;
            $rootScope.$broadcast('dataloaded');
            //$.colorbox.resize({ width: "75%", height: "90%" });
        };

        $scope.closeModal = function () {
            $scope.$broadcast('closeColorbox');
            $('#closeModal').trigger('click');
        };

        // Send data for MyWebsite to /REST/UserWidgetBookmark/
        $scope.submit = function () {
            if (!edit) {
                if ($scope.widget == undefined) {
                    $scope.validation.msg = 'Please type a frame name to continue';
                    return false;
                } else {
                    if ($scope.widget.title == '') {
                        $scope.validation.msg = 'Please type a frame name to continue';
                        return false;
                    } else {
                        $scope.validation.msg = '';
                    }
                }
            }


            var url = '/REST/UserWidgetBookmark/' + ((edit) ? 'Edit/' + $rootScope.edit.id : '');

            $scope.clickSubmit = false;
            $scope.feedback.status = 'success';
            $scope.feedback.msg = 'Sending data...';
            var objData = {
                title: (edit) ? $rootScope.edit.title : $scope.widget.title,
                trustedSource: $scope.trusted.trustedSourceSelected.concat($scope.websites)
            };

            if (!edit) {
                if ($rootScope.repositionWidgets() != null)
                    $rootScope.repositionWidgets();
                if ($rootScope.$gridster != null) {
                    var gridsterItems = $rootScope.$gridster.serialize();
                    $rootScope.updatePosition(gridsterItems);
                }
            };

            $http.post(url, objData)
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        $scope.feedback.msg = 'OK';
                        if (edit) {
                            $('.list-gridster .box-column').each(function (i, e) {
                                if ($(e).attr('data-widget') == $rootScope.edit.id) {
                                    if ($rootScope.$gridster != null)
                                        $rootScope.$gridster.remove_widget(e);

                                }
                            });

                            LoadBehaviorCaller['dataload-' + $rootScope.edit.id] = undefined;
                            _page.userWidgetBookmark[('items_' + $rootScope.edit.id)] = undefined;
                            $rootScope.attWidget($rootScope.edit.id, data.response);
                            $rootScope.loadDragWidgets($rootScope.edit.id);
                        } else {
                            $rootScope.addWidgetLast(data.response);
                        }

                        $scope.$broadcast('closeColorbox');
                        $('#closeModal').trigger('click');
                        location.reload();
                    } else {
                        $scope.feedback.msg = data.response;
                        $scope.clickSubmit = true;
                    }
                })
                .error(function (data, status, headers, config) {
                    $scope.feedback.msg = data.response;
                    $scope.clickSubmit = true;
                });
        };

        // Send data for MyWebsite to /REST/UserWidgetBookmark/
        $scope.submitInCategory = function () {
            $scope.cSelecteds = 0;
            $scope.clickSubmit = false;
            $scope.feedback.status = 'success';
            $scope.selectedsCategories = [];


            for (var count in $scope.trusted.trustedSourceSelected) {
                $scope.selectedsCategories[count] = [];

                $scope.selectedsCategories[count] = $scope.trusted.trustedSourceSelected[count];
            }
            for (var count2 in $scope.trustedWebsites) {
                if ($scope.trustedWebsites[count2].length > 0) {
                    $scope.selectedsCategories[count2] = $scope.selectedsCategories[count2] || [];
                    $scope.selectedsCategories[count2] = $scope.selectedsCategories[count2].concat($scope.trustedWebsites[count2]);
                }
            }
            for (var i in $scope.selectedsCategories) {
                var frameName = "Websites";
                for (var cats = 0; cats < $scope.listTrusted.length; cats++) {
                    if ($scope.selectedsCategories[i][0].categoryId == $scope.listTrusted[cats].id) {
                        frameName = $scope.listTrusted[cats].title;
                    }
                }

                $scope.cSelecteds++;
                var objData = {
                    title: frameName,
                    trustedSource: $scope.selectedsCategories[i]
                };

                $http.post('/REST/UserWidgetBookmark/', objData)
                    .success(function (data, status, headers, config) {
                        if (data.status) {
                            $scope.feedback.msg = 'Creating frames...';
                        } else {
                            $scope.feedback.msg = data.response;
                            $scope.clickSubmit = true;
                        }
                        $rootScope.$broadcast('dataloaded');

                        $scope.cSelecteds--;

                        if ($scope.cSelecteds === 0) {
                            window.location.href = '/?InviteYourFriends';
                        }
                    })
                    .error(function (data, status, headers, config) {
                        $scope.feedback.msg = data.response;
                        $scope.clickSubmit = true;

                        $scope.cSelecteds--;

                        if ($scope.cSelecteds === 0) {
                            window.location.href = '/?InviteYourFriends';
                        }
                    });
            }

            if ($scope.selectedsCategories.length === 0)
                window.location.href = '/?InviteYourFriends';
            //$scope.clickSubmit = true;
        };


        if (!edit)
            $scope.getTrusteds();
        else
            $scope.getEditTrusteds();
    }])
    // Controller for InviteFriends
    .controller('InviteFriends', ['$scope', '$http', '$rootScope', '$element', '$timeout', '$window', 'authcfg', function ($scope, $http, $rootScope, $element, $timeout, $window, authcfg) {
        $scope.title = 'Invite your Friends';
        $scope.step = 1;
        $scope.listEmails = [];
        $scope.firstAccess = false;
        // $rootScope.$broadcast('dataloaded');
        console.log("live yahoo auth details :    ", authcfg);

        if ($rootScope.firstAccess) {
            $scope.firstAccess = true;
            $rootScope.firstAccess = false;
        }

        $scope.goStep = function (i) {
            $scope.step = i;
            // $rootScope.$broadcast('dataloaded');
        };

        $scope.finishRegistration = function () {
            $.colorbox.close();
            $scope.$broadcast('closeColorbox');
            $('#closeModal').trigger('click');
        };

        $scope.addIndividual = function () {
            if ($scope.addIndividualEmail.$valid) {
                var friendsInvited = $scope.emailIndividual.split(',');
                var objFriends = [];

                for (var friend in friendsInvited)
                    objFriends.push({ email: friendsInvited[friend] });

                var invite = $scope.invite(objFriends, function (data) {
                    if (data.status) {
                        //cboxLoadingGraphic
                        $scope.classIndividualFeedback = 'success';
                        $scope.msgIndividualFeedback = 'Your invite was sent successfully.';
                    } else {
                        $scope.classIndividualFeedback = 'error';
                        $scope.msgIndividualFeedback = data.response;
                    }
                });

                $scope.emailIndividual = '';
                $scope.classIndividualFeedback = 'success';
                $scope.msgIndividualFeedback = 'Inviting your friend...';
                // $rootScope.$broadcast('dataloaded');
            }
        };

        $scope.loginGmail = function () {

            $scope.feedbackStatus = 'success';
            $scope.loginMessage = 'Sending data...';
            $rootScope.$broadcast('dataloaded');

            $http.get('/REST/Invite/Gmail/' + $scope.login + '/' + $scope.password + '?m=' + (new Date()).getMilliseconds())
                .success(function (data, status, headers, config) {
                    if (data.status || data.length > 0) {
                        $scope.loginMessage = 'OK.';
                        $scope.title = 'Select your Friends';

                        $scope.friends = data;
                        $scope.friendsFilter = $scope.friends;
                        $scope.goStep(3);
                    } else {
                        $scope.feedbackStatus = 'error';
                        $scope.loginMessage = data.response;
                    }
                    $rootScope.$broadcast('dataloaded');
                })
                .error(function (data, status, headers, config) {
                    $scope.feedbackStatus = 'error';
                    $scope.loginMessage = data;
                });
        };

        $scope.handleGmailAuthResult = function (authResult) {
            if (authResult && !authResult.error) {
                $http.jsonp('https://www.google.com/m8/feeds/contacts/default/full', { params: { 'max-results': '999', access_token: authResult.access_token, alt: 'json', callback: 'JSON_CALLBACK' } })
                    .success(function (response) {
                        // $scope.$apply(function () {
                        if (response.feed && response.feed.entry && response.feed.entry.length) {

                            $scope.friends = _.compact(_.map(response.feed.entry, function (item) {
                                var emails = []
                                for (en in item.gd$email) {
                                    entry = item.gd$email[en];
                                    if (entry.primary == "true") {
                                        return {
                                            email: entry.address,
                                            name: item.title.$t || entry.address
                                        }
                                    }
                                }
                            }));
                            $scope.friendsFilter = $scope.friends;

                            $scope.loginMessage = 'OK.';
                            $scope.$parent.title = 'Select your Friends';

                            $scope.goStep(3);
                        } else {
                            $scope.friends = [];
                            $scope.friendsFilter = $scope.friends;

                            $scope.loginMessage = 'OK.';
                            $scope.title = "Not found";
                            $scope.inviteMessage = "You don't have friends on this account";
                            $scope.goStep(4);
                        }

                        $timeout($.colorbox.resize, 500);

                        $scope.hasLoad = false;
                    });
            }
        };

        $scope.getGmailContacts = function (callback) {
            $scope.hasLoad = true;
            gapi.auth.authorize({ client_id: _googleConfig.clientId, scope: [_googleConfig.scopes.contacts, _googleConfig.scopes.plus], immediate: false }, $scope.handleGmailAuthResult);
        };

        $scope.findFriend = function () {
            if ($scope.friendSearch.keywork.length > 2) {
                $scope.friendsFilter = $scope.friends.slice(0);
                $scope.friendsFilter = _.filter($scope.friendsFilter, function (v) {
                    if ((v.name != undefined) && (v.name.length > $scope.friendSearch.keywork.length)) {
                        return (v.name.toLowerCase().slice(0, $scope.friendSearch.keywork.length) == $scope.friendSearch.keywork.toLowerCase())
                    }
                });
            } else {
                $scope.friendsFilter = $scope.friends;
            }

        }


        $scope.getOutlookContacts = function () {
            console.log("getOutlookContacts called...");
            $scope.hasLoad = true;
            var network = 'windows';
            var path = 'me/contacts';
            var list = document.getElementById('list');
            hello.login(network, { scope: 'friends', force: true }, function (auth) {
                if (!auth || auth.error) {
                    console.log("Signin aborted");
                    $scope.hasLoad = false;
                    return;
                }
                hello(network).api(path, { limit: 1000 }, function responseHandler(r) {
                    if (r.data != undefined) {
                        $scope.friends = _.compact(_.map(r.data, function (item) {
                            var emails = []
                            if (item.email != undefined) {
                                return {
                                    email: item.email,
                                    name: item.name
                                }
                            }
                        }));
                        $scope.friendsFilter = $scope.friends;
                        $scope.loginMessage = 'OK.';
                        $scope.$parent.title = 'Select your Friends';
                        $scope.goStep(3);
                        $scope.hasLoad = false;
                    }
                    else {
                        $scope.friends = [];
                        $scope.friendsFilter = $scope.friends;

                        $scope.loginMessage = 'OK.';
                        $scope.title = "Not found";
                        $scope.inviteMessage = "You don't have friends on this account";
                        $scope.goStep(4);
                        $timeout($.colorbox.resize, 500);
                        $scope.hasLoad = false;
                    }
                });
            });
        };

        $scope.invite = function (friends, callback) {
            var friendsInvited = friends;
            callback = callback || function () { };

            $http.post('/REST/Invite/', friendsInvited)
                .success(function (data, status, headers, config) {
                    callback(data);

                    $rootScope.$broadcast('dataloaded');
                })
                .error(function (data, status, headers, config) {
                    callback(data);
                });
        };

        $scope.inviteAllFriends = function () {
            for (var i in $scope.friends) {
                $scope.friends[i].chk = true; //#-TEMP TO NOT ENABLE FOR TESTING
            }
            $scope.inviteFriends();
        };

        $scope.inviteFriends = function () {
            var friendsInvited = $scope.getCheckeds();
            $scope.msgSendInvite = 'Inviting your friends...';
            $scope.feedbackSendStatus = 'success';
            $rootScope.$broadcast('dataloaded');

            $scope.invite(friendsInvited, function (data) {
                if (data.status) {
                    $scope.title = "Invite your Friends";
                    $scope.inviteMessage = "Your friends were invited successfully.";

                    if ($scope.$parent.goStep)
                        $scope.$parent.goStep(4);
                    else if ($scope.goStep)
                        $scope.goStep(4);
                } else {
                    $scope.feedbackSendStatus = 'error';
                    $scope.msgSendInvite = data.response;
                }

                $rootScope.$broadcast('dataloaded');
            });
        };

        $scope.getCheckeds = function () {
            return _.filter($scope.friends, function (item) {
                return item.chk
            });
        };

        $scope.getauthnonce = function () {
            var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
            var result = "";
            for (var i = 0; i < 10; ++i) {
                var rnum = Math.floor(Math.random() * chars.length);
                result += chars.substring(rnum, rnum + 1);
            }
            return result;
        };
        $scope.getauthtimestamp = function () {
            var t = (new Date()).getTime();
            return Math.floor(t / 1000);
        };

        //yahoo contacts implementation
        $scope.getYahooContacts = function () {
            console.log("getyahoocontacts called....");
            $scope.hasLoad = true;

            var gdata = { token: 'abc', email: 'a@a.com' };
            $http.get('/rest/Yahoo', { params: gdata })
                               .success(function (data, status, headers, config) {

                                   if (data) {
                                       $scope.userEmailFeed = $rootScope.userEmailFeed = data;
                                       $rootScope.$broadcast('dataloaded');
                                       $scope.userHasEmailAccount = true;
                                       $scope.wasLoaded = true;
                                   } else {
                                       $scope.userHasEmailAccount = false;
                                   }
                               });


            var network = 'yahoo';
            var path = 'me/friends';
            var list = document.getElementById('list');
            hello.login(network, { scope: 'friends', force: true }, function (auth) {
                if (!auth || auth.error) {
                    console.log("Signin aborted");
                    $scope.hasLoad = false;
                    return;
                }
                hello(network).api(path, { limit: 1000 }, function responseHandler(r) {
                    if (r.data != undefined) {
                        $scope.friends = _.compact(_.map(r.data, function (item) {
                            var emails = []
                            if (item.email != undefined) {
                                return {
                                    email: item.email,
                                    name: item.name
                                }
                            }
                        }));
                        $scope.friendsFilter = $scope.friends;
                        $scope.loginMessage = 'OK.';
                        $scope.$parent.title = 'Select your Friends';
                        $scope.goStep(3);
                        $scope.hasLoad = false;
                    }
                    else {
                        $scope.friends = [];
                        $scope.friendsFilter = $scope.friends;

                        $scope.loginMessage = 'OK.';
                        $scope.title = "Not found";
                        $scope.inviteMessage = "You don't have friends on this account";
                        $scope.goStep(4);
                        $timeout($.colorbox.resize, 500);
                        $scope.hasLoad = false;
                    }
                });
            });
        };

    }])
    .directive('ngLastRepeat', function () {
        return function (scope, element, attrs) {
            if (scope.$last) {
                setTimeout($.colorbox.resize, 500);
            }
        };
    })

    // Controller for CustomizeHomepageCtrl in /Templates/CustomizeHomepage.html
    .controller('CustomizeHomepageCtrl', ['$scope', '$http', '$rootScope', '$timeout', function ($scope, $http, $rootScope, $timeout) {
        $scope.customStep = 0;
        $scope.custom = {};
        $scope.custom.mywebsite = {};
        $scope.custom.mywebsite.step = 0;
        $scope.currentCategory = 'Entertainment';
        $rootScope.currentCategoryIndex = 0;

        $scope.refreshPage = function () {
            window.window.location.href = '/?InviteYourFriends';
        };
        $scope.setCustomStep = function (i) {
            if (i == 2)
                $.colorbox.resize({ width: "75%" });

            $scope.customStep = i;
            $rootScope.$broadcast('dataloaded');
            $scope.$watch('$compile', function () {
                $rootScope.$broadcast('dataloaded');
            });
        };

        $scope.resizeModal = function () {
            $.colorbox.resize({ width: "910px" });
        };

        $scope.nextCustomStep = function () {
            $scope.setCustomStep($scope.customStep + 1);
        };
        $scope.prevCustomStep = function () {
            $scope.setCustomStep($scope.customStep - 1);
        };

        $scope.defaultHomepage = function () {
            $http.post('/REST/UserWidget/CreateDefault', { cache: false })
                .success(function (data, status, headers, config) {
                    $scope.refreshPage();
                })
                .error(function (data, status, headers, config) {
                });
        };

        // Custom events for myWebsite
        $scope.custom.mywebsite.selectCategory = function (i, id) {
            $scope.currentCategory = $rootScope.websitesCategories[i].title;
            $rootScope.currentCategoryIndex = i;
            $scope.custom.mywebsite.step = i;
            $timeout(function () {
                $.colorbox.resize({ height: $(".hold-modal").height() + 75 });
            }, 200);
            //if ($scope.custom.mywebsite.step > i) {
            //    $scope.custom.mywebsite.step = i;
            //}
        };
        $scope.custom.mywebsite.nextStep = function () {
            $scope.custom.mywebsite.step++;
            $scope.currentCategory = $rootScope.websitesCategories[$scope.custom.mywebsite.step].title;
            $timeout(function () {
                $.colorbox.resize({ height: $(".hold-modal").height() + 75 });
            }, 200);
        };
        $scope.custom.mywebsite.prevStep = function () {
            $scope.custom.mywebsite.step--;
            $scope.currentCategory = $rootScope.websitesCategories[$scope.custom.mywebsite.step].title;
            $timeout(function () {
                $.colorbox.resize({ height: $(".hold-modal").height() + 75 });
            }, 200);
        };

        $scope.toggleCustomWebsite = function () {
            $scope.customWebsiteDisplay = !$scope.customWebsiteDisplay;
            $rootScope.$broadcast('dataloaded');
        };
    }])

    .controller('EmailCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {
        $rootScope.loadWidget();
    }])

    .controller('EmailFeedCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {
        $scope.wasLoaded = false;
        try {
            gapi.auth.init(function () { });
        } catch (e) { }
        $scope.userEmailFeed = $rootScope.userEmailFeed || [];
        $scope.userHasEmailAccount = true;

        $scope.providers = [
            { name: 'Gmail', image: 'gmail.gif', serverType: 'GMAIL' }
            //, { name: 'Live', image: 'hotmail.gif', serverType: 'HOTMAIL' }
            // , { name: 'YahooMail', image: 'yahoo.gif', serverType: 'YAHOO' }
        ];

        $rootScope.checkEmailAccount = $scope.checkEmailAccount = function () {
            $http.get('/REST/UserWidgetEmail/?m=' + (new Date()).getMilliseconds())
                .success(function (data, status, headers, config) {
                    if (data.length > 0) {
                        if (data.username != undefined) {
                            $scope.userHasEmailAccount = true;
                            $scope.$parent.widget.userHasEmailAccount = true;
                            $rootScope.serverType = data.serverType;
                            switch (data.serverType) {
                                case 'GMAIL':
                                    $scope.$parent.widget.webmailUrl = $scope.webmailUrl = 'http://www.gmail.com/';
                                    gapi.auth.authorize({ client_id: _googleConfig.clientId, scope: [_googleConfig.scopes.email, _googleConfig.scopes.feed], immediate: true }, $scope.handleGmailAuthResult);
                                    break;
                                case 'HOTMAIL':
                                    $scope.$parent.widget.webmailUrl = $scope.webmailUrl = 'http://www.outlook.com/';
                                    break;
                                case 'YAHOO':
                                    $scope.$parent.widget.webmailUrl = $scope.webmailUrl = 'http://mail.yahoo.com/';
                                    break;
                            }
                            //$scope.loadEmailFeed();
                        } else {
                            $scope.$parent.widget.webmailUrl = '';
                            $scope.$parent.widget.userHasEmailAccount = false;
                            $scope.userHasEmailAccount = false;
                            $rootScope.loadWidget();
                        }
                    } else {
                        $scope.userHasEmailAccount = false;
                        $scope.wasLoaded = true;
                        $rootScope.loadWidget();
                    }
                });
        }

        $scope.setServerType = function (serverType) {
            $rootScope.serverType = serverType;
        };

        $scope.handleGmailAuthResult = function (authResult) {
            if (authResult != null) {
                if (authResult && !authResult.error) {
                    gapi.client.load('oauth2', 'v2', function () {
                        var request = gapi.client.oauth2.userinfo.v2.me.get({
                            'fields': 'email'
                        });
                        request.execute(function (resp) {
                            var gdata = { token: authResult.access_token, email: resp.email };
                            $http.get('/rest/gmail', { params: gdata })
                                .success(function (data, status, headers, config) {
                                    if (data) {
                                        $scope.userEmailFeed = $rootScope.userEmailFeed = data;
                                        $rootScope.$broadcast('dataloaded');
                                        $scope.userHasEmailAccount = true;
                                        $scope.wasLoaded = true;
                                    } else {
                                        $scope.userHasEmailAccount = false;
                                    }
                                });
                        });
                    });
                } else {
                    // authorizeButton.style.visibility = '';
                    // authorizeButton.onclick = handleAuthClick;
                }
            } else {
                $scope.userHasEmailAccount = false;
                $scope.wasLoaded = true;
            }
        };

        $scope.setEmailAccount = function (serverType) {

            $scope.account = { username: 'oauth', password: '', serverType: serverType };
            $scope.wasLoaded = false;

            switch (serverType) {
                case 'GMAIL':
                    gapi.auth.authorize({
                        client_id: _googleConfig.clientId,
                        scope: [_googleConfig.scopes.email, _googleConfig.scopes.feed],
                        immediate: false
                    }, $scope.handleGmailAuthResult);

                    $http.post('/REST/UserWidgetEmail/Save', $scope.account)
                    .success(function (data, status, headers, config) {
                        if (!data.status) {
                            $scope.loginMessage = data.response;
                        }
                    });

                    return false;
                case 'HOTMAIL':
                    $scope.wasLoaded = true;
                    alert('Account Yahoo');
                    break;
                case 'YAHOO':
                    $scope.wasLoaded = true;
                    alert('Account Live');
                    break;
            }
        };

        $scope.loadEmailFeed = function () {
            $scope.userEmailFeed = [];
            $scope.wasLoaded = false;
            $http.get('/REST/UserWidgetEmail/Feed?m=' + (new Date()).getMilliseconds())
                .success(function (data, status, headers, config) {
                    if (data != null) {
                        if (data.status) {
                            $scope.userEmailFeed = $rootScope.userEmailFeed = $.parseJSON(data.response);
                            $rootScope.$broadcast('dataloaded');
                        } else {
                            $scope.userHasEmailAccount = false;
                            $scope.wasLoaded = true;
                        }
                    }
                    //$scope.wasLoaded = true;
                    // $rootScope.loadWidget();
                });
        };

        $scope.checkEmailAccount();
    }])


    .controller('EmailAccountCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {
        $scope.canDeleted = false;
        $scope.loginMessage = '';
        $scope.account = { username: '', password: '', serverType: $rootScope.serverType };

        switch ($rootScope.serverType) {
            case "GMAIL":
                $scope.provider = "Gmail";
                $scope.provider_image = "gmail.gif";
                break;
            case "HOTMAIL":
                $scope.provider = "Live";
                $scope.provider_image = "hotmail.gif";
                break;
            case "YAHOO":
                $scope.provider = "Yahoo Mail";
                $scope.provider_image = "yahoo.gif";
                break;
        }

        $http.get('/REST/UserWidgetEmail/?m=' + (new Date()).getMilliseconds())
            .success(function (data, status, headers, config) {
                if (data != undefined) {
                    if (data.username != undefined) {
                        $scope.canDeleted = true;
                        $scope.account = data;
                    }
                }
            })
            .error(function (data, status, headers, config) {

            });

        // $scope.login = function () {
        $scope.login = function () {
            $http.post('/REST/UserWidgetEmail/Save', $scope.account)
                .success(function (data, status, headers, config) {
                    if (data.status) {
                        $rootScope.userEmailFeed = $.parseJSON(data.response);
                        $rootScope.checkEmailAccount();
                        $.colorbox.close();

                    } else {
                        $scope.loginMessage = data.response;
                    }
                });
        }

        $scope.delete = function () {
            $http.post('/REST/UserWidgetEmail/Delete')
            .success(function (data, status, headers, config) {
                if (data.status) {
                    $rootScope.userEmailFeed = [];
                    $rootScope.userHasEmailAccount = false;
                    $rootScope.checkEmailAccount();
                    $.colorbox.close();
                } else {
                    $scope.loginMessage = data.response;
                }
            });
        }

    }])

    .controller('SuggestionCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {
        $scope.currentPage = 0;
        $scope.limitSuggestions = 5;
        $scope.suggestions = [];
        var remainingSuggestion = [];
        if (typeof (Storage) !== "undefined") {
            if (localStorage != undefined || localStorage != "undefined") {
                if (localStorage.remainingSuggestion != undefined) {
                    remainingSuggestion = JSON.parse(localStorage.remainingSuggestion);
                    localStorage.removeItem("remainingSuggestion");
                }
            }
        }
        /*
        $http.get('/REST/SuggestionBox?m=' + (new Date()).getMilliseconds())
            .success(function (data, status, headers, config) {
                //$scope.suggestions = data.splice(0, 3);
                $scope.suggestions = data;
                $rootScope.$broadcast('dataloaded');
            });
            */
        $scope.suggestions = _page.suggestions;
        if (remainingSuggestion != undefined && remainingSuggestion.length > 0)
            $scope.suggestions = remainingSuggestion;
        $rootScope.$broadcast('dataloaded');

        $scope.numberOfPages = function () {
            return Math.ceil($scope.suggestions.length / $scope.limitSuggestions);
        }

        $scope.$on('removeSuggestion', function (e) {
            $scope.suggestions = _.without($scope.suggestions, _.findWhere($scope.suggestions, { Id: $rootScope.selectedSuggestionBoxId }));
            $scope.$broadcast('closeColorbox');
            $('#closeModal').trigger('click');
        });

        $scope.setSelected = function (id) {
            $rootScope.selectedSuggestionBoxId = id;
        };


        $rootScope.reloadSuggestions = $scope.reloadSuggestions = function () {
            $http.get('/REST/SuggestionBox')
                .success(function (data, status, headers, config) {
                    _page.suggestions = data;
                    $scope.suggestions = _page.suggestions;
                    $rootScope.$broadcast('dataloaded');
                });
        }


        $scope.ignore = function (o) {
            $scope.suggestions = _.without($scope.suggestions, o);

            o.register = '';
            o.lastupdate = '';
            o.deleted = '';

            $http.post('/REST/SuggestionBox/Ignore', o)
            .success(function (data, status, headers, config) {
                //$scope.suggestions = data.splice(0, 3);
                //$rootScope.reloadSuggestions();

            });
            ////back to first page
            //if ($scope.suggestions.length <= 5) {
            //    $scope.currentPage = $scope.currentPage - 1;
            //}

        };
    }])

    .controller('AddSuggestionCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {
        $scope.feedbackSuggestion = { status: false, msg: '' };
        $scope.widgets = [];
        $scope.suggestionBoxWidgetId = 0;
        $scope.suggestionBoxWidgetVisible = false;
        $scope.suggestionBoxWidgetTitle = 'New Frame +';
        $scope.widgets.push({ id: 0, title: '<< New Frame >>' });
        $scope.setSuggestionBoxWidgetId = function (widgetId, widgetTitle) {
            if (widgetId)
                $scope.suggestionBoxWidgetTitle = widgetTitle;
            else
                $scope.suggestionBoxWidgetTitle = 'New Frame +';

            $scope.suggestionBoxWidgetId = widgetId;
            $scope.suggestionBoxWidgetVisible = false;
        }

        for (var iws = 0; iws < $rootScope.widgets.length; iws++) {
            if ($rootScope.widgets[iws].typeId == 2) {
                $scope.widgets.push($rootScope.widgets[iws]);
            }
        }

        $scope.submit = function () {
            var url = '/REST/UserWidgetBookmark/AddSuggestionBox/' + $rootScope.selectedSuggestionBoxId;

            $scope.clickSubmit = false;
            $scope.feedbackSuggestion.status = 'success';
            $scope.feedbackSuggestion.msg = 'Sending data...';
            $scope.$broadcast('dataloaded');
            var objData = {
                id: $scope.suggestionBoxWidgetId,
                title: 'Websites'
            };
            $http.post(url, objData)
               .success(function (data, status, headers, config) {
                   if (data.status) {
                       $scope.$emit('removeSuggestion');
                       $scope.feedbackSuggestion.msg = 'OK';
                       if ($scope.suggestionBoxWidgetId === 0) {
                           $rootScope.addWidgetLast(data.response);
                       } else {
                           $('.list-gridster .box-column').each(function (i, e) {
                               if ($(e).attr('data-widget') == $scope.suggestionBoxWidgetId) {
                                   if ($rootScope.$gridster != null)
                                       $rootScope.$gridster.remove_widget(e);

                               }
                           });

                           LoadBehaviorCaller['dataload-' + $scope.suggestionBoxWidgetId] = undefined;
                           _page.userWidgetBookmark[('items_' + $scope.suggestionBoxWidgetId)] = undefined;
                           $rootScope.attWidget($scope.suggestionBoxWidgetId, data.response);
                           $rootScope.loadDragWidgets($scope.suggestionBoxWidgetId);
                       }
                       if (typeof (Storage) !== "undefined") {

                           if (localStorage != undefined) {
                               localStorage.setItem("remainingSuggestion", JSON.stringify($scope.suggestions));
                           }
                       }

                       //         $rootScope.reloadSuggestions();
                       $scope.$broadcast('closeColorbox');
                       $('#closeModal').trigger('click');
                       location.reload();

                   } else {
                       $scope.feedbackSuggestion = false;
                       $scope.feedbackSuggestion.msg = data.response;
                   }
               })
                .error(function (data, status, headers, config) {
                    $scope.feedbackSuggestion = false;
                    $scope.feedbackSuggestion.msg = data.response;
                });
        };
    }])

    .controller('ChatCtrl', ['$scope', '$http', '$rootScope', 'xsocket', function ($scope, $http, $rootScope, xsocket) {
        $scope.users = [];

        if (typeof _userName === 'string') {
            //$scope.$apply(function () {
            //    Candy.Core.connect(_chatUrl, null, _userName);
            //});
        }
    }])

    // Controller for AddAsHomePageCtrl
    .controller('AddAsHomePageCtrl', ['$timeout', '$scope', function ($timeout, $scope) {
        $scope.browser;

        $.browser.chrome = $.browser.webkit && !!window.chrome;
        $.browser.safari = $.browser.webkit && !window.chrome;
        $scope.homepageImgWidth = 0;

        if ($.browser.mozilla) {
            $scope.browser = "mozilla";
        }

        else if ($.browser.chrome) {
            $scope.browser = "chrome";
        }

        else if ($.browser.safari) {
            $scope.browser = "safari";
        }

        else if ($.browser.msie) {
            $scope.browser = "msie";
        }
        //initiate an array to hold all active Questions
        $scope.activeHPageQs = [];

        $scope.currentHPageTab = 'AddAsHome.tpl.html';

        //check if the Question is active
        $scope.isOpenHPageQ = function (question) {
            //check if this question is already in the activeQs array
            if ($scope.activeHPageQs.indexOf(question) > -1) {
                $scope.homepageImgWidth = (($(".accord_container").width() - 20) / 2) * 60 / 100;
                //if so, return true                      
                return true;
            } else {
                //if not, return false
                return false;
            }
        }
        //function to 'open' a question
        $scope.openHPageQ = function (question) {
            //check if question is already open
            if ($scope.isOpenHPageQ(question)) {
                //if it is, remove it from the activeQs array
                $scope.activeHPageQs.splice($scope.activeHPageQs.indexOf(question), 1);
                $timeout(function () {
                    $.colorbox.resize({ height: $(".hold-modal").height() + 50 });
                }, 200);
            } else {
                //if it's not, add it!
                $scope.activeHPageQs.push(question);
                $timeout(function () {
                    $.colorbox.resize({ height: $(".hold-modal").height() + 50 });
                }, 200);
            }
        }
        $scope.$broadcast('dataloaded');
    }])

     .controller('FAQsCtrl', ['$timeout', '$scope', function ($timeout, $scope) {
         //initiate an array to hold all active Questions
         $scope.activeQs = [];
         $scope.faqImgWidth = 0;
         $scope.tabs = [{
             title: 'Registration',
             url: 'Registration.tpl.html'
         }, {
             title: 'Frames',
             url: 'Frames.tpl.html'
         }, {
             title: 'Misc',
             url: 'Misc.tpl.html'
         }];

         $scope.currentTab = 'Registration.tpl.html';

         $scope.onClickTab = function (tab) {
             $scope.activeQs = [];
             $scope.currentTab = tab.url;
             $timeout(function () {
                 $.colorbox.resize({ height: $(".hold-modal").height() + 90 });
             }, 200);
         }

         $scope.isActiveTab = function (tabUrl) {
             $scope.faqImgWidth = (($(".accordion_container").width() - 20) / 2) * 60 / 100;
             return tabUrl == $scope.currentTab;
         }

         //check if the Question is active
         $scope.isOpenQ = function (question) {
             //check if this question is already in the activeQs array
             if ($scope.activeQs.indexOf(question) > -1) {
                 //if so, return true
                 return true;
             } else {
                 //if not, return false
                 return false;
             }
         }
         //function to 'open' a question
         $scope.openQ = function (question) {
             //check if question is already open
             if ($scope.isOpenQ(question)) {
                 //if it is, remove it from the activeQs array
                 $scope.activeQs.splice($scope.activeQs.indexOf(question), 1);
                 $timeout(function () {
                     $.colorbox.resize({ height: $(".hold-modal").height() + 90 });
                 }, 200);
             } else {
                 //if it's not, add it!
                 $scope.activeQs.push(question);
                 $timeout(function () {
                     $.colorbox.resize({ height: $(".hold-modal").height() + 90 });
                 }, 200);
             }
         }
     }])
    // Controller for Welcome Modal
    .controller('WelcomeModal', ['$scope', function ($scope) {
        //$scope.step = 1;
    }]);