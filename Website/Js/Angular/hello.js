var hello = function (e) {
    return hello.use(e)
};
hello.utils = {
    extend: function (e) {
        for (var t = Array.prototype.slice.call(arguments, 1), n = 0; n < t.length; n++) {
            var o = t[n];
            if (e instanceof Object && o instanceof Object && e !== o)
                for (var i in o) e[i] = hello.utils.extend(e[i], o[i]);
            else e = o
        }
        return e
    }
}, hello.utils.extend(hello, {
    settings: {
        redirect_uri: window.location.href.split("#")[0] + "redirect.html",//window.location.href.split("#")[0],
        response_type: "token",
        display: "popup",
        state: "",
        oauth_proxy: "https://auth-server.herokuapp.com/proxy",
        timeout: 2e4,
        default_service: null,
        force: !0
    },
    service: function (e) {
        return "undefined" != typeof e ? this.utils.store("sync_service", e) : this.utils.store("sync_service")
    },
    services: {},
    use: function (e) {
        var t = this.utils.objectCreate(this);
        return t.settings = this.utils.objectCreate(this.settings), e && (t.settings.default_service = e), t.utils.Event.call(t), t
    },
    init: function (e, t) {
        var n = this.utils;
        if (!e) return this.services;
        for (var o in e) e.hasOwnProperty(o) && "object" != typeof e[o] && (e[o] = {
            id: e[o]
        });
        n.extend(this.services, e);
        for (o in this.services) this.services.hasOwnProperty(o) && (this.services[o].scope = this.services[o].scope || {});
        return t && (n.extend(this.settings, t), "redirect_uri" in t && (this.settings.redirect_uri = n.url(t.redirect_uri).href)), this
    },
    login: function () {
        var e = this.use(),
            t = e.utils,
            n = t.args({
                network: "s",
                options: "o",
                callback: "f"
            }, arguments);
        e.args = n;
        var o, i = n.options = t.merge(e.settings, n.options || {});
        if (n.network = e.settings.default_service = n.network || e.settings.default_service, e.on("complete", n.callback), "string" != typeof n.network || !(n.network in e.services)) return e.emitAfter("error complete", {
            error: {
                code: "invalid_network",
                message: "The provided network was not recognized"
            }
        }), e;
        var a = e.services[n.network],
            r = !1,
            s = t.globalEvent(function (n) {
                var o;
                n ? (o = JSON.parse(n), hello.utils.store(o.network, o)) : o = {
                    error: {
                        code: "cancelled",
                        message: "The authentication was not completed"
                    }
                }, r = !0, o.error ? e.emit("complete error failed auth.failed", {
                    error: o.error
                }) : (t.store(o.network, o), e.emit("complete success login auth.login auth", {
                    network: o.network,
                    authResponse: o
                }))
            }),
            l = t.url(i.redirect_uri).href;
        n.qs = {
            client_id: a.id,
            response_type: a.oauth.response_type || i.response_type,
            redirect_uri: l,
            display: i.display,
            scope: "basic",
            state: {
                client_id: a.id,
                network: n.network,
                display: i.display,
                callback: s,
                state: i.state,
                redirect_uri: l,
                oauth_proxy: i.oauth_proxy
            }
        };
        var c = t.store(n.network),
            u = (i.scope || "").toString();
        if (u = (u ? u + "," : "") + n.qs.scope, c && "scope" in c && c.scope instanceof String && (u += "," + c.scope), n.qs.state.scope = hello.utils.unique(u.split(/[,\s]+/)).join(","), n.qs.scope = u.replace(/[^,\s]+/gi, function (t) {
            if (t in a.scope) return a.scope[t];
            for (var n in e.services) {
                var o = e.services[n].scope;
                if (o && t in o) return ""
        }
            return t
        }).replace(/[,\s]+/gi, ","), n.qs.scope = t.unique(n.qs.scope.split(/,+/)).join(a.scope_delim || ","), i.force === !1 && c && "access_token" in c && c.access_token && "expires" in c && c.expires > (new Date).getTime() / 1e3) {
            var f = t.diff(c.scope || [], n.qs.state.scope || []);
            if (0 === f.length) return e.emit("notice", "User already has a valid access_token"), e.emitAfter("complete success login", {
                network: n.network,
                authResponse: c
            }), e
        }
        if (n.qs.state.oauth = a.oauth, n.qs.state = JSON.stringify(n.qs.state), "login" in a && "function" == typeof a.login && a.login(n), 1 === parseInt(a.oauth.version, 10) ? o = t.qs(i.oauth_proxy, n.qs) : "none" === i.display && a.oauth.grant && c && c.refresh_token ? (n.qs.refresh_token = c.refresh_token, o = t.qs(i.oauth_proxy, n.qs)) : o = t.qs(a.oauth.auth, n.qs), e.emit("notice", "Authorization URL " + o), "none" === i.display) t.iframe(o);
        else if ("popup" === i.display) var d = hello.utils.popup(o, l, i.window_width || 800, i.window_height || 900),
        p = setInterval(function () {
            if ((!d || d.closed) && (clearInterval(p), !r)) {
                var t = {
                    code: "cancelled",
                    message: "Login has been cancelled"
                };
                d || (t = {
                    code: "blocked",
                    message: "Popup was blocked"
                }), e.emit("complete failed error", {
                    error: t,
                    network: n.network
                })
            }
        }, 100);
        else window.location = o;
        return e
    },
    logout: function () {
        var e = this.use(),
            t = e.utils,
            n = t.args({
                name: "s",
                options: "o",
                callback: "f"
            }, arguments);
        if (n.options = n.options || {}, e.on("complete", n.callback), n.name = n.name || e.settings.default_service, !n.name || n.name in e.services)
            if (n.name && t.store(n.name)) {
                var o = function (t) {
                    e.utils.store(n.name, ""), e.emitAfter("complete logout success auth.logout auth", hello.utils.merge({
                        network: n.name
                    }, t || {}))
                }, i = {};
                if (n.options.force) {
                    var a = e.services[n.name].logout;
                    if (a)
                        if ("function" == typeof a && (a = a(o)), "string" == typeof a) t.iframe(a), i.force = null, i.message = "Logout success on providers site was indeterminate";
                        else if (void 0 === a) return e
                }
                o(i)
            } else if (n.name) e.emitAfter("complete error", {
                error: {
                    code: "invalid_session",
                    message: "There was no session to remove"
                }
            });
            else {
                for (var r in e.services) e.services.hasOwnProperty(r) && e.logout(r);
                e.service(!1)
            } else e.emitAfter("complete error", {
                error: {
                    code: "invalid_network",
                    message: "The network was unrecognized"
                }
            });
        return e
    },
    getAuthResponse: function (e) {
        return e = e || this.settings.default_service, e && e in this.services ? this.utils.store(e) || null : (this.emit("complete error", {
            error: {
                code: "invalid_network",
                message: "The network was unrecognized"
            }
        }), null)
    },
    events: {}
}), hello.utils.extend(hello.utils, {
    qs: function (e, t) {
        if (t) {
            var n;
            for (var o in t)
                if (e.indexOf(o) > -1) {
                    var i = "[\\?\\&]" + o + "=[^\\&]*";
                    n = new RegExp(i), e = e.replace(n, "")
                }
        }
        return e + (this.isEmpty(t) ? "" : (e.indexOf("?") > -1 ? "&" : "?") + this.param(t))
    },
    param: function (e) {
        var t, n, o = {};
        if ("string" == typeof e) {
            if (n = e.replace(/^[\#\?]/, "").match(/([^=\/\&]+)=([^\&]+)/g))
                for (var i = 0; i < n.length; i++) t = n[i].match(/([^=]+)=(.*)/), o[t[1]] = decodeURIComponent(t[2]);
            return o
        }
        var a = e;
        o = [];
        for (var r in a) a.hasOwnProperty(r) && a.hasOwnProperty(r) && o.push([r, "?" === a[r] ? "?" : encodeURIComponent(a[r])].join("="));
        return o.join("&")
    },
    store: function (e) {
        var t = [e, window.sessionStorage],
            n = 0;
        for (e = t[n++]; e;) try {
            e.setItem(n, n), e.removeItem(n);
            break
        } catch (o) {
            e = t[n++]
        }
        return e || (e = {
            getItem: function (e) {
                e += "=";
                for (var t = document.cookie.split(";"), n = 0; n < t.length; n++) {
                    var o = t[n].replace(/(^\s+|\s+$)/, "");
                    if (o && 0 === o.indexOf(e)) return o.substr(e.length)
                }
                return null
            },
            setItem: function (e, t) {
                document.cookie = e + "=" + t
            }
        }),
        function (t, n) {
            var o = JSON.parse(e.getItem("hello")) || {};
            if (t && void 0 === n) return o[t] || null;
            if (t && null === n) try {
                delete o[t]
            } catch (i) {
                o[t] = null
            } else {
                if (!t) return o;
                o[t] = n
            }
            return e.setItem("hello", JSON.stringify(o)), o || null
        }
    }(window.localStorage),
    append: function (e, t, n) {
        var o = "string" == typeof e ? document.createElement(e) : e;
        if ("object" == typeof t)
            if ("tagName" in t) n = t;
            else
                for (var i in t)
                    if (t.hasOwnProperty(i))
                        if ("object" == typeof t[i])
                            for (var a in t[i]) t[i].hasOwnProperty(a) && (o[i][a] = t[i][a]);
                        else "html" === i ? o.innerHTML = t[i] : /^on/.test(i) ? o[i] = t[i] : o.setAttribute(i, t[i]);
        return "body" === n ? ! function r() {
            document.body ? document.body.appendChild(o) : setTimeout(r, 16)
        }() : "object" == typeof n ? n.appendChild(o) : "string" == typeof n && document.getElementsByTagName(n)[0].appendChild(o), o
    },
    iframe: function (e) {
        this.append("iframe", {
            src: e,
            style: {
                position: "absolute",
                left: "-1000px",
                bottom: 0,
                height: "1px",
                width: "1px"
            }
        }, "body")
    },
    merge: function () {
        var e = Array.prototype.slice.call(arguments);
        return e.unshift({}), this.extend.apply(null, e)
    },
    args: function (e, t) {
        var n = {}, o = 0,
            i = null,
            a = null;
        for (a in e)
            if (e.hasOwnProperty(a)) break;
        if (1 === t.length && "object" == typeof t[0] && "o!" != e[a])
            for (a in t[0])
                if (e.hasOwnProperty(a) && a in e) return t[0];
        for (a in e)
            if (e.hasOwnProperty(a))
                if (i = typeof t[o], "function" == typeof e[a] && e[a].test(t[o]) || "string" == typeof e[a] && (e[a].indexOf("s") > -1 && "string" === i || e[a].indexOf("o") > -1 && "object" === i || e[a].indexOf("i") > -1 && "number" === i || e[a].indexOf("a") > -1 && "object" === i || e[a].indexOf("f") > -1 && "function" === i)) n[a] = t[o++];
                else if ("string" == typeof e[a] && e[a].indexOf("!") > -1) return !1;
        return n
    },
    url: function (e) {
        if (e) {
            if (window.URL && URL instanceof Function && 0 !== URL.length) return new URL(e, window.location);
            var t = document.createElement("a");
            return t.href = e, t
        }
        return window.location
    },
    diff: function (e, t) {
        for (var n = [], o = 0; o < t.length; o++) -1 === this.indexOf(e, t[o]) && n.push(t[o]);
        return n
    },
    indexOf: function (e, t) {
        if (e.indexOf) return e.indexOf(t);
        for (var n = 0; n < e.length; n++)
            if (e[n] === t) return n;
        return -1
    },
    unique: function (e) {
        if ("object" != typeof e) return [];
        for (var t = [], n = 0; n < e.length; n++) e[n] && 0 !== e[n].length && -1 === this.indexOf(t, e[n]) && t.push(e[n]);
        return t
    },
    isEmpty: function (e) {
        if (!e) return !0;
        if (e && e.length > 0) return !1;
        if (e && 0 === e.length) return !0;
        for (var t in e)
            if (e.hasOwnProperty(t)) return !1;
        return !0
    },
    objectCreate: function () {
        function e() { }
        return Object.create ? Object.create : function (t) {
            if (1 != arguments.length) throw new Error("Object.create implementation only accepts one parameter.");
            return e.prototype = t, new e
        }
    }(),
    Event: function () {
        var e = /[\s\,]+/;
        return this.parent = {
            events: this.events,
            findEvents: this.findEvents,
            parent: this.parent,
            utils: this.utils
        }, this.events = {}, this.on = function (t, n) {
            if (n && "function" == typeof n)
                for (var o = t.split(e), i = 0; i < o.length; i++) this.events[o[i]] = [n].concat(this.events[o[i]] || []);
            return this
        }, this.off = function (e, t) {
            return this.findEvents(e, function (e, n) {
                t && this.events[e][n] !== t || (this.events[e][n] = null)
            }), this
        }, this.emit = function (e) {
            var t = Array.prototype.slice.call(arguments, 1);
            t.push(e);
            for (var n = function (n, o) {
                t[t.length - 1] = "*" === n ? e : n, this.events[n][o].apply(this, t)
            }, o = this; o && o.findEvents;) o.findEvents(e + ",*", n), o = o.parent;
            return this
        }, this.emitAfter = function () {
            var e = this,
                t = arguments;
            return setTimeout(function () {
                e.emit.apply(e, t)
            }, 0), this
        }, this.findEvents = function (t, n) {
            var o = t.split(e);
            for (var i in this.events)
                if (this.events.hasOwnProperty(i) && hello.utils.indexOf(o, i) > -1)
                    for (var a = 0; a < this.events[i].length; a++) this.events[i][a] && n.call(this, i, a)
        }, this
    },
    globalEvent: function (e, t) {
        return t = t || "_hellojs_" + parseInt(1e12 * Math.random(), 10).toString(36), window[t] = function () {
            try {
                bool = e.apply(this, arguments)
            } catch (n) {
                console.error(n)
            }
            if (bool) try {
                delete window[t]
            } catch (n) { }
        }, t
    },
    popup: function (e, t, n, o) {
        var i = document.documentElement,
            a = void 0 !== window.screenLeft ? window.screenLeft : screen.left,
            r = void 0 !== window.screenTop ? window.screenTop : screen.top,
            s = window.innerWidth || i.clientWidth || screen.width,
            l = window.innerHeight || i.clientHeight || screen.height,
            c = (s - n) / 2 + a,
            u = (l - o) / 2 + r,
            f = function (e) {
                var i = window.open(e, "_blank", "resizeable=true,height=" + o + ",width=" + n + ",left=" + c + ",top=" + u);
                if (i && i.addEventListener) {
                    var a = hello.utils.url(t),
                        r = a.origin || a.protocol + "//" + a.hostname;
                    i.addEventListener("loadstart", function (e) {
                        var t = e.url;
                        if (0 === t.indexOf(r)) {
                            var n = hello.utils.url(t),
                                o = {
                                    location: {
                                        assign: function (e) {
                                            i.addEventListener("exit", function () {
                                                setTimeout(function () {
                                                    f(e)
                                                }, 1e3)
                                            })
                                        },
                                        search: n.search,
                                        hash: n.hash,
                                        href: n.href
                                    },
                                    close: function () {
                                        i.close && i.close()
                                    }
                                };
                            hello.utils.responseHandler(o, window), o.close()
                        }
                    })
                }
                return i && i.focus && i.focus(), i
            };
        return -1 !== navigator.userAgent.indexOf("Safari") && -1 === navigator.userAgent.indexOf("Chrome") && (e = t + "#oauth_redirect=" + encodeURIComponent(encodeURIComponent(e))), f(e)
    },
    responseHandler: function (e, t) {
        function n(e, t, n) {
            if (a.store(e.network, e), !("display" in e && "page" === e.display)) {
                if (n) {
                    var i = e.callback;
                    try {
                        delete e.callback
                    } catch (r) { }
                    if (a.store(e.network, e), i in n) {
                        var s = JSON.stringify(e);
                        try {
                            n[i](s)
                        } catch (r) { }
                    }
                }
                o()
            }
        }

        function o() {
            try {
                e.close()
            } catch (t) { }
            e.addEventListener && e.addEventListener("load", function () {
                e.close()
            })
        }
        var i, a = this,
            r = e.location,
            s = function (t) {
                r.assign ? r.assign(t) : e.location = t
            };
        if (i = a.param(r.search), i && (i.code && i.state || i.oauth_token && i.proxy_url)) {
            var l = JSON.parse(i.state);
            i.redirect_uri = l.redirect_uri || r.href.replace(/[\?\#].*$/, "");
            var c = (l.oauth_proxy || i.proxy_url) + "?" + a.param(i);
            return void s(c)
        }
        if (i = a.merge(a.param(r.search || ""), a.param(r.hash || "")), i && "state" in i) {
            try {
                var u = JSON.parse(i.state);
                a.extend(i, u)
            } catch (f) {
                console.error("Could not decode state parameter")
            }
            if ("access_token" in i && i.access_token && i.network) i.expires_in && 0 !== parseInt(i.expires_in, 10) || (i.expires_in = 0), i.expires_in = parseInt(i.expires_in, 10), i.expires = (new Date).getTime() / 1e3 + (i.expires_in || 31536e3), n(i, e, t);
            else if ("error" in i && i.error && i.network) i.error = {
                code: i.error,
                message: i.error_message || i.error_description
            }, n(i, e, t);
            else if (i.callback && i.callback in t) {
                var d = "result" in i && i.result ? JSON.parse(i.result) : !1;
                t[i.callback](d), o()
            }
        } else if ("oauth_redirect" in i) return void s(decodeURIComponent(i.oauth_redirect))
    }
}), hello.utils.Event.call(hello), hello.subscribe = hello.on, hello.trigger = hello.emit, hello.unsubscribe = hello.off, hello.utils.responseHandler(window, window.opener || window.parent),
function (e) {
    var t = {}, n = {};
    e.on("auth.login, auth.logout", function (n) {
        n && "object" == typeof n && n.network && (t[n.network] = e.utils.store(n.network) || {})
    }),
    function o() {
        var i = (new Date).getTime() / 1e3,
            a = function (t) {
                e.emit("auth." + t, {
                    network: r,
                    authResponse: s
                })
            };
        for (var r in e.services)
            if (e.services.hasOwnProperty(r)) {
                if (!e.services[r].id) continue;
                var s = e.utils.store(r) || {}, l = e.services[r],
                    c = t[r] || {};
                if (s && "callback" in s) {
                    var u = s.callback;
                    try {
                        delete s.callback
                    } catch (f) { }
                    e.utils.store(r, s);
                    try {
                        window[u](s)
                    } catch (f) { }
                }
                if (s && "expires" in s && s.expires < i) {
                    var d = l.refresh || s.refresh_token;
                    !d || r in n && !(n[r] < i) ? d || r in n || (a("expired"), n[r] = !0) : (e.emit("notice", r + " has expired trying to resignin"), e.login(r, {
                        display: "none",
                        force: !1
                    }), n[r] = i + 600);
                    continue
                }
                if (c.access_token === s.access_token && c.expires === s.expires) continue;
                !s.access_token && c.access_token ? a("logout") : s.access_token && !c.access_token ? a("login") : s.expires !== c.expires && a("update"), t[r] = s, r in n && delete n[r]
            }
        setTimeout(o, 1e3)
    }()
}(hello), hello.api = function () {
    function e(e) {
        if (n.data = i.clone(a), "get" === n.method) {
            for (var r, s = /[\?\&]([^=&]+)(=([^&]+))?/gi; r = s.exec(e) ;) n.data[r[1]] = r[3];
            e = e.replace(/\?.*/, "")
        }
        var f = l[{
            "delete": "del"
        }[n.method] || n.method] || {}, d = f[e] || f["default"] || e,
            p = function (e) {
                e = e.replace(/\@\{([a-z\_\-]+)(\|.+?)?\}/gi, function (e, t, i) {
                    var a = i ? i.replace(/^\|/, "") : "";
                    return t in n.data ? (a = n.data[t], delete n.data[t]) : "undefined" == typeof i && o.emitAfter("error", {
                        error: {
                            code: "missing_attribute_" + t,
                            message: "The attribute " + t + " is missing from the request"
                        }
                    }), a
                }), e.match(/^https?:\/\//) || (e = l.base + e);
                var a = {}, r = function (r, s) {
                    r && ("function" == typeof r ? r(a) : i.extend(a, r));
                    var c = i.qs(e, a || {});
                    o.emit("notice", "Request " + c), t(n.network, c, n.method, n.data, l.querystring, s)
                };
                if (!i.isEmpty(n.data) && !("FileList" in window) && i.hasBinary(n.data)) return i.post(r, n.data, "form" in l ? l.form(n) : null, c), o;
                if ("delete" === n.method) {
                    var s = c;
                    c = function (e, t) {
                        s(!e || i.isEmpty(e) ? {
                            success: !0
                        } : e, t)
                    }
                }
                if ("withCredentials" in new XMLHttpRequest && (!("xhr" in l) || l.xhr && l.xhr(n, a))) {
                    var f = i.xhr(n.method, r, n.headers, n.data, c);
                    f.onprogress = function (e) {
                        o.emit("progress", e)
                    }, f.upload && (f.upload.onprogress = function (e) {
                        o.emit("uploadprogress", e)
                    })
                } else {
                    if (n.callbackID = i.globalEvent(), "jsonp" in l && l.jsonp(n, a), "api" in l && l.api(e, n, u && u.access_token ? {
                        access_token: u.access_token
                    } : {}, c)) return;
                    "post" === n.method ? (a.redirect_uri = o.settings.redirect_uri, a.state = JSON.stringify({
                        callback: n.callbackID
                    }), i.post(r, n.data, "form" in l ? l.form(n) : null, c, n.callbackID, o.settings.timeout)) : (i.extend(a, n.data), a.callback = n.callbackID, i.jsonp(r, c, n.callbackID, o.settings.timeout))
                }
            };
        "function" == typeof d ? d(n, p) : p(d)
    }

    function t(e, t, n, a, r, s) {
        var l = o.services[e],
            c = u ? u.access_token : null,
            f = l.oauth && 1 === parseInt(l.oauth.version, 10) ? o.settings.oauth_proxy : null;
        if (f) return void s(i.qs(f, {
            path: t,
            access_token: c || "",
            then: "get" === n.toLowerCase() ? "redirect" : "proxy",
            method: n,
            suppress_response_codes: !0
        }));
        var d = {
            access_token: c || ""
        };
        r && r(d), s(i.qs(t, d))
    }
    var n = this.utils.args({
        path: "s!",
        method: "s",
        data: "o",
        timeout: "i",
        callback: "f"
    }, arguments),
        o = this.use(),
        i = o.utils;
    o.args = n, n.method = (n.method || "get").toLowerCase();
    var a = n.data = n.data || {};
    o.on("complete", n.callback), n.path = n.path.replace(/^\/+/, "");
    var r = (n.path.split(/[\/\:]/, 2) || [])[0].toLowerCase();
    if (r in o.services) {
        n.network = r;
        var s = new RegExp("^" + r + ":?/?");
        n.path = n.path.replace(s, "")
    }
    n.network = o.settings.default_service = n.network || o.settings.default_service;
    var l = o.services[n.network];
    if (!l) return o.emitAfter("complete error", {
        error: {
            code: "invalid_network",
            message: "Could not match the service requested: " + n.network
        }
    }), o;
    n.timeout && (o.settings.timeout = n.timeout), o.emit("notice", "API request " + n.method.toUpperCase() + " '" + n.path + "' (request)", n);
    var c = function (t, i) {
        if (l.wrap && (n.path in l.wrap || "default" in l.wrap)) {
            var a = n.path in l.wrap ? n.path : "default",
                r = (new Date).getTime(),
                s = l.wrap[a](t, i, n);
            s && (t = s), o.emit("notice", "Processing took" + ((new Date).getTime() - r))
        }
        o.emit("notice", "API: " + n.method.toUpperCase() + " '" + n.path + "' (response)", t);
        var c = null;
        t && "paging" in t && t.paging.next && (c = function () {
            e((t.paging.next.match(/^\?/) ? n.path : "") + t.paging.next)
        }), o.emit("complete " + (!t || "error" in t ? "error" : "success"), t, c)
    };
    if (n.method in l && n.path in l[n.method] && l[n.method][n.path] === !1) return o.emitAfter("complete error", {
        error: {
            code: "invalid_path",
            message: "The provided path is not available on the selected network"
        }
    });
    var u = o.getAuthResponse(n.network);
    return e(n.path), o
}, hello.utils.extend(hello.utils, {
    isArray: function (e) {
        return "[object Array]" === Object.prototype.toString.call(e)
    },
    domInstance: function (e, t) {
        var n = "HTML" + (e || "").replace(/^[a-z]/, function (e) {
            return e.toUpperCase()
        }) + "Element";
        return t ? window[n] ? t instanceof window[n] : window.Element ? t instanceof window.Element && (!e || t.tagName && t.tagName.toLowerCase() === e) : !(t instanceof Object || t instanceof Array || t instanceof String || t instanceof Number) && t.tagName && t.tagName.toLowerCase() === e : !1
    },
    clone: function (e) {
        if ("nodeName" in e || this.isBinary(e)) return e;
        var t, n = {};
        for (t in e) n[t] = "object" == typeof e[t] ? this.clone(e[t]) : e[t];
        return n
    },
    xhr: function (e, t, n, o, i) {
        function a(e) {
            for (var t, n = {}, o = /([a-z\-]+):\s?(.*);?/gi; t = o.exec(e) ;) n[t[1]] = t[2];
            return n
        }
        var r = this;
        if ("function" != typeof t) {
            var s = t;
            t = function (e, t) {
                t(r.qs(s, e))
            }
        }
        var l = new XMLHttpRequest,
            c = !1;
        "blob" === e && (c = e, e = "GET"), e = e.toUpperCase(), l.onload = function () {
            var t = l.response;
            try {
                t = JSON.parse(l.responseText)
            } catch (n) {
                401 === l.status && (t = {
                    error: {
                        code: "access_denied",
                        message: l.statusText
                    }
                })
            }
            var o = a(l.getAllResponseHeaders());
            o.statusCode = l.status, i(t || ("DELETE" !== e ? {
                error: {
                    message: "Could not get resource"
                }
            } : {}), o)
        }, l.onerror = function () {
            var e = l.responseText;
            try {
                e = JSON.parse(l.responseText)
            } catch (t) { }
            i(e || {
                error: {
                    code: "access_denied",
                    message: "Could not get resource"
                }
            })
        };
        var u, f = {};
        if ("GET" === e || "DELETE" === e) r.isEmpty(o) || r.extend(f, o), o = null;
        else if (!(!o || "string" == typeof o || o instanceof FormData || o instanceof File || o instanceof Blob)) {
            var d = new FormData;
            for (u in o) o.hasOwnProperty(u) && (o[u] instanceof HTMLInputElement ? "files" in o[u] && o[u].files.length > 0 && d.append(u, o[u].files[0]) : o[u] instanceof Blob ? d.append(u, o[u], o.name) : d.append(u, o[u]));
            o = d
        }
        return t(f, function (t) {
            if (l.open(e, t, !0), c && ("responseType" in l ? l.responseType = c : l.overrideMimeType("text/plain; charset=x-user-defined")), n)
                for (var i in n) l.setRequestHeader(i, n[i]);
            l.send(o)
        }), l
    },
    jsonp: function (e, t, n, o) {
        var i, a, r = this,
            s = 0,
            l = document.getElementsByTagName("head")[0],
            c = {
                error: {
                    message: "server_error",
                    code: "server_error"
                }
            }, u = function () {
                s++ || window.setTimeout(function () {
                    t(c), l.removeChild(a)
                }, 0)
            }, f = r.globalEvent(function (e) {
                return c = e, !0
            }, n);
        if ("function" != typeof e) {
            var d = e;
            d = d.replace(new RegExp("=\\?(&|$)"), "=" + f + "$1"), e = function (e, t) {
                t(r.qs(d, e))
            }
        }
        e(function (e) {
            for (var t in e) e.hasOwnProperty(t) && "?" === e[t] && (e[t] = f)
        }, function (e) {
            a = r.append("script", {
                id: f,
                name: f,
                src: e,
                async: !0,
                onload: u,
                onerror: u,
                onreadystatechange: function () {
                    /loaded|complete/i.test(this.readyState) && u()
                }
            }), window.navigator.userAgent.toLowerCase().indexOf("opera") > -1 && (i = r.append("script", {
                text: "document.getElementById('" + f + "').onerror();"
            }), a.async = !1), o && window.setTimeout(function () {
                c = {
                    error: {
                        message: "timeout",
                        code: "timeout"
                    }
                }, u()
            }, o), l.appendChild(a), i && l.appendChild(i)
        })
    },
    post: function (e, t, n, o, i, a) {
        var r = this,
            s = document;
        if ("function" != typeof e) {
            var l = e;
            e = function (e, t) {
                t(r.qs(l, e))
            }
        }
        var c, u = null,
            f = [],
            d = 0,
            p = null,
            m = 0,
            h = function (e) {
                m++ || o(e)
            };
        r.globalEvent(h, i);
        var g;
        try {
            g = s.createElement('<iframe name="' + i + '">')
        } catch (v) {
            g = s.createElement("iframe")
        }
        if (g.name = i, g.id = i, g.style.display = "none", n && n.callbackonload && (g.onload = function () {
            h({
            response: "posted",
            message: "Content was posted"
        })
        }), a && setTimeout(function () {
            h({
            error: {
            code: "timeout",
            message: "The post operation timed out"
        }
        })
        }, a), s.body.appendChild(g), r.domInstance("form", t)) {
            for (u = t.form, d = 0; d < u.elements.length; d++) u.elements[d] !== t && u.elements[d].setAttribute("disabled", !0);
            t = u
        }
        if (r.domInstance("form", t))
            for (u = t, d = 0; d < u.elements.length; d++) u.elements[d].disabled || "file" !== u.elements[d].type || (u.encoding = u.enctype = "multipart/form-data", u.elements[d].setAttribute("name", "file"));
        else {
            for (p in t) t.hasOwnProperty(p) && r.domInstance("input", t[p]) && "file" === t[p].type && (u = t[p].form, u.encoding = u.enctype = "multipart/form-data");
            u || (u = s.createElement("form"), s.body.appendChild(u), c = u);
            var w;
            for (p in t)
                if (t.hasOwnProperty(p)) {
                    var y = r.domInstance("input", t[p]) || r.domInstance("textArea", t[p]) || r.domInstance("select", t[p]);
                    if (y && t[p].form === u) y && t[p].name !== p && (t[p].setAttribute("name", p), t[p].name = p);
                    else {
                        var b = u.elements[p];
                        if (w)
                            for (b instanceof NodeList || (b = [b]), d = 0; d < b.length; d++) b[d].parentNode.removeChild(b[d]);
                        w = s.createElement("input"), w.setAttribute("type", "hidden"), w.setAttribute("name", p), w.value = y ? t[p].value : r.domInstance(null, t[p]) ? t[p].innerHTML || t[p].innerText : t[p], u.appendChild(w)
                    }
                }
            for (d = 0; d < u.elements.length; d++) w = u.elements[d], w.name in t || w.getAttribute("disabled") === !0 || (w.setAttribute("disabled", !0), f.push(w))
        }
        u.setAttribute("method", "POST"), u.setAttribute("target", i), u.target = i, e({}, function (e) {
            u.setAttribute("action", e), setTimeout(function () {
                u.submit(), setTimeout(function () {
                    try {
                        c && c.parentNode.removeChild(c)
                    } catch (e) {
                        try {
                            console.error("HelloJS: could not remove iframe")
                        } catch (t) { }
                    }
                    for (var n = 0; n < f.length; n++) f[n] && (f[n].setAttribute("disabled", !1), f[n].disabled = !1)
                }, 0)
            }, 100)
        })
    },
    hasBinary: function (e) {
        for (var t in e)
            if (e.hasOwnProperty(t) && this.isBinary(e[t])) return !0;
        return !1
    },
    isBinary: function (e) {
        return e instanceof Object && (this.domInstance("input", e) && "file" === e.type || "FileList" in window && e instanceof window.FileList || "File" in window && e instanceof window.File || "Blob" in window && e instanceof window.Blob)
    },
    toBlob: function (e) {
        var t = /^data\:([^;,]+(\;charset=[^;,]+)?)(\;base64)?,/i,
            n = e.match(t);
        if (!n) return e;
        for (var o = atob(e.replace(t, "")), i = [], a = 0; a < o.length; a++) i.push(o.charCodeAt(a));
        return new Blob([new Uint8Array(i)], {
            type: n[1]
        })
    }
}),
function (e) {
    var t = e.api,
        n = e.utils;
    n.extend(n, {
        dataToJSON: function (e) {
            var t = this,
                n = window,
                o = e.data;
            if (t.domInstance("form", o) ? o = t.nodeListToJSON(o.elements) : "NodeList" in n && o instanceof NodeList ? o = t.nodeListToJSON(o) : t.domInstance("input", o) && (o = t.nodeListToJSON([o])), ("File" in n && o instanceof n.File || "Blob" in n && o instanceof n.Blob || "FileList" in n && o instanceof n.FileList) && (o = {
                file: o
            }), !("FormData" in n && o instanceof n.FormData))
                for (var i in o)
                    if (o.hasOwnProperty(i))
                        if ("FileList" in n && o[i] instanceof n.FileList) 1 === o[i].length && (o[i] = o[i][0]);
                        else {
                            if (t.domInstance("input", o[i]) && "file" === o[i].type) continue;
                            t.domInstance("input", o[i]) || t.domInstance("select", o[i]) || t.domInstance("textArea", o[i]) ? o[i] = o[i].value : t.domInstance(null, o[i]) && (o[i] = o[i].innerHTML || o[i].innerText)
                        }
            return e.data = o, o
        },
        nodeListToJSON: function (e) {
            for (var t = {}, n = 0; n < e.length; n++) {
                var o = e[n];
                !o.disabled && o.name && (t[o.name] = "file" === o.type ? o : o.value || o.innerHTML)
            }
            return t
        }
    }), e.api = function () {
        var e = n.args({
            path: "s!",
            method: "s",
            data: "o",
            timeout: "i",
            callback: "f"
        }, arguments);
        return e.data && n.dataToJSON(e), t.call(this, e)
    }
}(hello),
function () {
    function e(e) {
        return function () {
            var n = e.apply(this, arguments),
                o = t(function (e, t) {
                    n.on("success", e).on("error", t).on("*", function () {
                        var e = Array.prototype.slice.call(arguments);
                        e.unshift(e.pop()), i.emit.apply(i, e)
                    })
                }),
                i = hello.utils.Event.call(o.proxy);
            return i
        }
    }
    Function.prototype.bind || (Function.prototype.bind = function (e) {
        function t() { }
        if ("function" != typeof this) throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");
        var n = [].slice,
            o = n.call(arguments, 1),
            i = this,
            a = function () {
                return i.apply(this instanceof t ? this : e || window, o.concat(n.call(arguments)))
            };
        return t.prototype = this.prototype, a.prototype = new t, a
    });
    var t = function () {
        var e = 0,
            t = 1,
            n = 2,
            o = function (t) {
                return this instanceof o ? (this.id = "Thenable/1.0.6", this.state = e, this.fulfillValue = void 0, this.rejectReason = void 0, this.onFulfilled = [], this.onRejected = [], this.proxy = {
                    then: this.then.bind(this)
                }, void ("function" == typeof t && t.call(this, this.fulfill.bind(this), this.reject.bind(this)))) : new o(t)
            };
        o.prototype = {
            fulfill: function (e) {
                return i(this, t, "fulfillValue", e)
            },
            reject: function (e) {
                return i(this, n, "rejectReason", e)
            },
            then: function (e, t) {
                var n = this,
                    i = new o;
                return n.onFulfilled.push(s(e, i, "fulfill")), n.onRejected.push(s(t, i, "reject")), a(n), i.proxy
            }
        };
        var i = function (t, n, o, i) {
            return t.state === e && (t.state = n, t[o] = i, a(t)), t
        }, a = function (e) {
            e.state === t ? r(e, "onFulfilled", e.fulfillValue) : e.state === n && r(e, "onRejected", e.rejectReason)
        }, r = function (e, t, n) {
            if (0 !== e[t].length) {
                var o = e[t];
                e[t] = [];
                var i = function () {
                    for (var e = 0; e < o.length; e++) o[e](n)
                };
                "object" == typeof process && "function" == typeof process.nextTick ? process.nextTick(i) : "function" == typeof setImmediate ? setImmediate(i) : setTimeout(i, 0)
            }
        }, s = function (e, t, n) {
            return function (o) {
                if ("function" != typeof e) t[n].call(t, o);
                else {
                    var i;
                    try {
                        i = e(o)
                    } catch (a) {
                        return void t.reject(a)
                    }
                    l(t, i)
                }
            }
        }, l = function (e, t) {
            if (e === t || e.proxy === t) return void e.reject(new TypeError("cannot resolve promise with itself"));
            var n;
            if ("object" == typeof t && null !== t || "function" == typeof t) try {
                n = t.then
            } catch (o) {
                return void e.reject(o)
            }
            if ("function" != typeof n) e.fulfill(t);
            else {
                var i = !1;
                try {
                    n.call(t, function (n) {
                        i || (i = !0, n === t ? e.reject(new TypeError("circular thenable chain")) : l(e, n))
                    }, function (t) {
                        i || (i = !0, e.reject(t))
                    })
                } catch (o) {
                    i || e.reject(o)
                }
            }
        };
        return o
    }();
    hello.login = e(hello.login), hello.api = e(hello.api), hello.logout = e(hello.logout)
}(hello),
function (e) {
    function t(e) {
        e && "error" in e && (e.error = {
            code: "server_error",
            message: e.error.message || e.error
        })
    }

    function n(t) {
        if (!("object" != typeof t || "Blob" in window && t instanceof Blob || "ArrayBuffer" in window && t instanceof ArrayBuffer || "error" in t)) {
            var n = t.root + t.path.replace(/\&/g, "%26");
            t.thumb_exists && (t.thumbnail = e.settings.oauth_proxy + "?path=" + encodeURIComponent("https://api-content.dropbox.com/1/thumbnails/" + n + "?format=jpeg&size=m") + "&access_token=" + e.getAuthResponse("dropbox").access_token), t.type = t.is_dir ? "folder" : t.mime_type, t.name = t.path.replace(/.*\//g, ""), t.is_dir ? t.files = "metadata/" + n : (t.downloadLink = e.settings.oauth_proxy + "?path=" + encodeURIComponent("https://api-content.dropbox.com/1/files/" + n) + "&access_token=" + e.getAuthResponse("dropbox").access_token, t.file = "https://api-content.dropbox.com/1/files/" + n), t.id || (t.id = t.path.replace(/^\//, ""))
        }
    }

    function o(e) {
        return function (t, n) {
            delete t.data.limit, n(e)
        }
    }
    e.init({
        dropbox: {
            login: function (e) {
                e.options.window_width = 1e3, e.options.window_height = 1e3
            },
            oauth: {
                version: "1.0",
                auth: "https://www.dropbox.com/1/oauth/authorize",
                request: "https://api.dropbox.com/1/oauth/request_token",
                token: "https://api.dropbox.com/1/oauth/access_token"
            },
            base: "https://api.dropbox.com/1/",
            root: "sandbox",
            get: {
                me: "account/info",
                "me/files": o("metadata/@{root|sandbox}/@{parent}"),
                "me/folder": o("metadata/@{root|sandbox}/@{id}"),
                "me/folders": o("metadata/@{root|sandbox}/"),
                "default": function (e, t) {
                    e.path.match("https://api-content.dropbox.com/1/files/") && (e.method = "blob"), t(e.path)
                }
            },
            post: {
                "me/files": function (t, n) {
                    var o = t.data.parent,
                        i = t.data.name;
                    t.data = {
                        file: t.data.file
                    }, "string" == typeof t.data.file && (t.data.file = e.utils.toBlob(t.data.file)), n("https://api-content.dropbox.com/1/files_put/@{root|sandbox}/" + o + "/" + i)
                },
                "me/folders": function (t, n) {
                    var o = t.data.name;
                    t.data = {}, n("fileops/create_folder?root=@{root|sandbox}&" + e.utils.param({
                        path: o
                    }))
                }
            },
            del: {
                "me/files": "fileops/delete?root=@{root|sandbox}&path=@{id}",
                "me/folder": "fileops/delete?root=@{root|sandbox}&path=@{id}"
            },
            wrap: {
                me: function (e) {
                    return t(e), e.uid ? (e.name = e.display_name, e.first_name = e.name.split(" ")[0], e.last_name = e.name.split(" ")[1], e.id = e.uid, delete e.uid, delete e.display_name, e) : e
                },
                "default": function (e) {
                    if (t(e), e.is_dir && e.contents) {
                        e.data = e.contents, delete e.contents;
                        for (var o = 0; o < e.data.length; o++) e.data[o].root = e.root, n(e.data[o])
                    }
                    return n(e), e.is_deleted && (e.success = !0), e
                }
            },
            xhr: function (e) {
                if (e.data && e.data.file) {
                    var t = e.data.file;
                    t && (e.data = t.files ? t.files[0] : t)
                }
                return "delete" === e.method && (e.method = "post"), !0
            }
        }
    })
}(hello),
function (e) {
    function t(e) {
        return e.id && (e.thumbnail = e.picture = "http://graph.facebook.com/" + e.id + "/picture"), e
    }

    function n(e) {
        if ("data" in e)
            for (var n = 0; n < e.data.length; n++) t(e.data[n]);
        return e
    }

    function o(t) {
        if ("data" in t)
            for (var n = e.getAuthResponse("facebook").access_token, o = 0; o < t.data.length; o++) {
                var a = t.data[o];
                a.picture && (a.thumbnail = a.picture), a.cover_photo && (a.thumbnail = i + a.cover_photo + "/picture?access_token=" + n), "album" === a.type && (a.files = a.photos = i + a.id + "/photos"), a.can_upload && (a.upload_location = i + a.id + "/photos")
            }
        return t
    }
    var i = "https://graph.facebook.com/";
    e.init({
        facebook: {
            name: "Facebook",
            login: function (e) {
                e.options.window_width = 580, e.options.window_height = 400
            },
            oauth: {
                version: 2,
                auth: "https://www.facebook.com/dialog/oauth/"
            },
            refresh: !0,
            logout: function (t) {
                var n = e.utils.globalEvent(t),
                    o = encodeURIComponent(e.settings.redirect_uri + "?" + e.utils.param({
                        callback: n,
                        result: JSON.stringify({
                            force: !0
                        }),
                        state: "{}"
                    })),
                    i = (e.utils.store("facebook") || {}).access_token;
                return e.utils.iframe("https://www.facebook.com/logout.php?next=" + o + "&access_token=" + i), i ? void 0 : !1
            },
            scope: {
                basic: "public_profile",
                email: "email",
                birthday: "user_birthday",
                events: "user_events",
                photos: "user_photos,user_videos",
                videos: "user_photos,user_videos",
                friends: "user_friends",
                files: "user_photos,user_videos",
                publish_files: "user_photos,user_videos,publish_actions",
                publish: "publish_actions",
                offline_access: "offline_access"
            },
            base: "https://graph.facebook.com/",
            get: {
                me: "me",
                "me/friends": "me/friends",
                "me/following": "me/friends",
                "me/followers": "me/friends",
                "me/share": "me/feed",
                "me/files": "me/albums",
                "me/albums": "me/albums",
                "me/album": "@{id}/photos",
                "me/photos": "me/photos",
                "me/photo": "@{id}"
            },
            post: {
                "me/share": "me/feed",
                "me/albums": "me/albums",
                "me/album": "@{id}/photos"
            },
            del: {
                "me/photo": "@{id}"
            },
            wrap: {
                me: t,
                "me/friends": n,
                "me/following": n,
                "me/followers": n,
                "me/albums": o,
                "me/files": o,
                "default": o
            },
            xhr: function (t, n) {
                return ("get" === t.method || "post" === t.method) && (n.suppress_response_codes = !0), "post" === t.method && t.data && "string" == typeof t.data.file && (t.data.file = e.utils.toBlob(t.data.file)), !0
            },
            jsonp: function (t, n) {
                var o = t.method.toLowerCase();
                "get" === o || e.utils.hasBinary(t.data) ? "delete" === t.method && (n.method = "delete", t.method = "post") : (t.data.method = o, t.method = "get")
            },
            form: function () {
                return {
                    callbackonload: !0
                }
            }
        }
    })
}(hello),
function (e) {
    function t(t, n, o) {
        var i = (o ? "" : "flickr:") + "?method=" + t + "&api_key=" + e.init().flickr.id + "&format=json";
        for (var a in n) n.hasOwnProperty(a) && (i += "&" + a + "=" + n[a]);
        return i
    }

    function n(n) {
        var o = e.getAuthResponse("flickr");
        o && o.user_nsid ? n(o.user_nsid) : e.api(t("flickr.test.login"), function (e) {
            n(l(e, "user").id)
        })
    }

    function o(e, o) {
        return o || (o = {}),
        function (i, a) {
            n(function (n) {
                o.user_id = n, a(t(e, o, !0))
            })
        }
    }

    function i(e, t) {
        var n = "https://www.flickr.com/images/buddyicon.gif";
        return e.nsid && e.iconserver && e.iconfarm && (n = "https://farm" + e.iconfarm + ".staticflickr.com/" + e.iconserver + "/buddyicons/" + e.nsid + (t ? "_" + t : "") + ".jpg"), n
    }

    function a(e, t, n, o, i) {
        return i = i ? "_" + i : "", "https://farm" + t + ".staticflickr.com/" + n + "/" + e + "_" + o + i + ".jpg"
    }

    function r(e) {
        e && e.stat && "ok" != e.stat.toLowerCase() && (e.error = {
            code: "invalid_request",
            message: e.message
        })
    }

    function s(e) {
        if (e.photoset || e.photos) {
            var t = "photoset" in e ? "photoset" : "photos";
            e = l(e, t), u(e), e.data = e.photo, delete e.photo;
            for (var n = 0; n < e.data.length; n++) {
                var o = e.data[n];
                o.name = o.title, o.picture = a(o.id, o.farm, o.server, o.secret, ""), o.source = a(o.id, o.farm, o.server, o.secret, "b"), o.thumbnail = a(o.id, o.farm, o.server, o.secret, "m")
            }
        }
        return e
    }

    function l(e, t) {
        return t in e ? e = e[t] : "error" in e || (e.error = {
            code: "invalid_request",
            message: e.message || "Failed to get data from Flickr"
        }), e
    }

    function c(e) {
        if (r(e), e.contacts) {
            e = l(e, "contacts"), u(e), e.data = e.contact, delete e.contact;
            for (var t = 0; t < e.data.length; t++) {
                var n = e.data[t];
                n.id = n.nsid, n.name = n.realname || n.username, n.thumbnail = i(n, "m")
            }
        }
        return e
    }

    function u(e) {
        e.page && e.pages && e.page !== e.pages && (e.paging = {
            next: "?page=" + ++e.page
        })
    }
    e.init({
        flickr: {
            name: "Flickr",
            oauth: {
                version: "1.0a",
                auth: "https://www.flickr.com/services/oauth/authorize?perms=read",
                request: "https://www.flickr.com/services/oauth/request_token",
                token: "https://www.flickr.com/services/oauth/access_token"
            },
            base: "https://api.flickr.com/services/rest",
            get: {
                me: o("flickr.people.getInfo"),
                "me/friends": o("flickr.contacts.getList", {
                    per_page: "@{limit|50}"
                }),
                "me/following": o("flickr.contacts.getList", {
                    per_page: "@{limit|50}"
                }),
                "me/followers": o("flickr.contacts.getList", {
                    per_page: "@{limit|50}"
                }),
                "me/albums": o("flickr.photosets.getList", {
                    per_page: "@{limit|50}"
                }),
                "me/photos": o("flickr.people.getPhotos", {
                    per_page: "@{limit|50}"
                })
            },
            wrap: {
                me: function (e) {
                    if (r(e), e = l(e, "person"), e.id) {
                        if (e.realname) {
                            e.name = e.realname._content;
                            var t = e.name.split(" ");
                            e.first_name = t[0], e.last_name = t[1]
                        }
                        e.thumbnail = i(e, "l"), e.picture = i(e, "l")
                    }
                    return e
                },
                "me/friends": c,
                "me/followers": c,
                "me/following": c,
                "me/albums": function (e) {
                    if (r(e), e = l(e, "photosets"), u(e), e.photoset) {
                        e.data = e.photoset, delete e.photoset;
                        for (var n = 0; n < e.data.length; n++) {
                            var o = e.data[n];
                            o.name = o.title._content, o.photos = "https://api.flickr.com/services/rest" + t("flickr.photosets.getPhotos", {
                                photoset_id: o.id
                            }, !0)
                        }
                    }
                    return e
                },
                "me/photos": function (e) {
                    return r(e), s(e)
                },
                "default": function (e) {
                    return r(e), s(e)
                }
            },
            xhr: !1,
            jsonp: function (e, t) {
                "get" == e.method.toLowerCase() && (delete t.callback, t.jsoncallback = e.callbackID)
            }
        }
    })
}(hello),
function (e) {
    function t(e) {
        e.meta && 400 === e.meta.code && (e.error = {
            code: "access_denied",
            message: e.meta.errorDetail
        })
    }

    function n(e) {
        e && e.id && (e.thumbnail = e.photo.prefix + "100x100" + e.photo.suffix, e.name = e.firstName + " " + e.lastName, e.first_name = e.firstName, e.last_name = e.lastName, e.contact && e.contact.email && (e.email = e.contact.email))
    }
    e.init({
        foursquare: {
            name: "FourSquare",
            oauth: {
                version: 2,
                auth: "https://foursquare.com/oauth2/authenticate"
            },
            refresh: !0,
            querystring: function (e) {
                var t = e.access_token;
                delete e.access_token, e.oauth_token = t, e.v = 20121125
            },
            base: "https://api.foursquare.com/v2/",
            get: {
                me: "users/self",
                "me/friends": "users/self/friends",
                "me/followers": "users/self/friends",
                "me/following": "users/self/friends"
            },
            wrap: {
                me: function (e) {
                    return t(e), e && e.response && (e = e.response.user, n(e)), e
                },
                "default": function (e) {
                    if (t(e), e && "response" in e && "friends" in e.response && "items" in e.response.friends) {
                        e.data = e.response.friends.items, delete e.response;
                        for (var o = 0; o < e.data.length; o++) n(e.data[o])
                    }
                    return e
                }
            }
        }
    })
}(hello),
function (e) {
    function t(e, t) {
        var n = t ? t.statusCode : e && "meta" in e && "status" in e.meta && e.meta.status;
        (401 === n || 403 === n) && (e.error = {
            code: "access_denied",
            message: e.message || (e.data ? e.data.message : "Could not get response")
        }, delete e.message)
    }

    function n(e) {
        e.id && (e.thumbnail = e.picture = e.avatar_url, e.name = e.login)
    }

    function o(e, t) {
        if (e.data && e.data.length && t && t.Link) {
            var n = t.Link.match(/&page=([0-9]+)/);
            n && (e.paging = {
                next: "?page=" + n[1]
            })
        }
    }
    e.init({
        github: {
            name: "GitHub",
            oauth: {
                version: 2,
                auth: "https://github.com/login/oauth/authorize",
                grant: "https://github.com/login/oauth/access_token",
                response_type: "code"
            },
            scope: {
                basic: "",
                email: "user:email"
            },
            base: "https://api.github.com/",
            get: {
                me: "user",
                "me/friends": "user/following?per_page=@{limit|100}",
                "me/following": "user/following?per_page=@{limit|100}",
                "me/followers": "user/followers?per_page=@{limit|100}"
            },
            wrap: {
                me: function (e, o) {
                    return t(e, o), n(e), e
                },
                "default": function (e, i, a) {
                    if (t(e, i), "[object Array]" === Object.prototype.toString.call(e)) {
                        e = {
                            data: e
                        }, o(e, i, a);
                        for (var r = 0; r < e.data.length; r++) n(e.data[r])
                    }
                    return e
                }
            }
        }
    })
}(hello),
function (e, t) {
    "use strict";

    function n(e) {
        return parseInt(e, 10)
    }

    function o(e) {
        e.error || (e.name || (e.name = e.title || e.message), e.picture || (e.picture = e.thumbnailLink), e.thumbnail || (e.thumbnail = e.thumbnailLink), "application/vnd.google-apps.folder" === e.mimeType && (e.type = "folder", e.files = "https://www.googleapis.com/drive/v2/files?q=%22" + e.id + "%22+in+parents"))
    }

    function i(e) {
        s(e);
        var t = function (e) {
            var t, n = e.media$group.media$content.length ? e.media$group.media$content[0] : {}, o = 0,
                i = {
                    id: e.id.$t,
                    name: e.title.$t,
                    description: e.summary.$t,
                    updated_time: e.updated.$t,
                    created_time: e.published.$t,
                    picture: n ? n.url : null,
                    thumbnail: n ? n.url : null,
                    width: n.width,
                    height: n.height
                };
            if ("link" in e)
                for (o = 0; o < e.link.length; o++) {
                    var a = e.link[o];
                    if (a.rel.match(/\#feed$/)) {
                        i.upload_location = i.files = i.photos = a.href;
                        break
                    }
                }
            if ("category" in e && e.category.length)
                for (t = e.category, o = 0; o < t.length; o++) t[o].scheme && t[o].scheme.match(/\#kind$/) && (i.type = t[o].term.replace(/^.*?\#/, ""));
            if ("media$thumbnail" in e.media$group && e.media$group.media$thumbnail.length) {
                for (t = e.media$group.media$thumbnail, i.thumbnail = e.media$group.media$thumbnail[0].url, i.images = [], o = 0; o < t.length; o++) i.images.push({
                    source: t[o].url,
                    width: t[o].width,
                    height: t[o].height
                });
                t = e.media$group.media$content.length ? e.media$group.media$content[0] : null, t && i.images.push({
                    source: t.url,
                    width: t.width,
                    height: t.height
                })
            }
            return i
        }, n = [];
        if ("feed" in e && "entry" in e.feed) {
            for (i = 0; i < e.feed.entry.length; i++) n.push(t(e.feed.entry[i]));
            e.data = n, delete e.feed
        } else {
            if ("entry" in e) return t(e.entry);
            if ("items" in e) {
                for (var i = 0; i < e.items.length; i++) o(e.items[i]);
                e.data = e.items, delete e.items
            } else o(e)
        }
        return e
    }

    function a(e) {
        e.name = e.displayName || e.name, e.picture = e.picture || (e.image ? e.image.url : null), e.thumbnail = e.picture
    }

    function r(t) {
        s(t);
        var n = [];
        if ("feed" in t && "entry" in t.feed) {
            for (var o = e.getAuthResponse("google").access_token, i = 0; i < t.feed.entry.length; i++) {
                var a = t.feed.entry[i],
                    r = a.link && a.link.length > 0 ? a.link[0].href + "?access_token=" + o : null;
                n.push({
                    id: a.id.$t,
                    name: a.title.$t,
                    email: a.gd$email && a.gd$email.length > 0 ? a.gd$email[0].address : null,
                    updated_time: a.updated.$t,
                    picture: r,
                    thumbnail: r
                })
            }
            t.data = n, delete t.feed
        }
        return t
    }

    function s(e) {
        if ("feed" in e && e.feed.openSearch$itemsPerPage) {
            var t = n(e.feed.openSearch$itemsPerPage.$t),
                o = n(e.feed.openSearch$startIndex.$t),
                i = n(e.feed.openSearch$totalResults.$t);
            i > o + t && (e.paging = {
                next: "?start=" + (o + t)
            })
        } else "nextPageToken" in e && (e.paging = {
            next: "?pageToken=" + e.nextPageToken
        })
    }

    function l() {
        function e(e) {
            var t = new FileReader;
            t.onload = function (t) {
                n(btoa(t.target.result), e.type + r + "Content-Transfer-Encoding: base64")
            }, t.readAsBinaryString(e)
        }

        function n(e, t) {
            o.push(r + "Content-Type: " + t + r + r + e), a--, l()
        }
        var o = [],
            i = (1e10 * Math.random()).toString(32),
            a = 0,
            r = "\r\n",
            s = r + "--" + i,
            l = function () { }, c = /^data\:([^;,]+(\;charset=[^;,]+)?)(\;base64)?,/i;
        this.append = function (o, i) {
            "string" != typeof o && "length" in Object(o) || (o = [o]);
            for (var s = 0; s < o.length; s++) {
                a++;
                var l = o[s];
                if (l instanceof t.File || l instanceof t.Blob) e(l);
                else if ("string" == typeof l && l.match(c)) {
                    var u = l.match(c);
                    n(l.replace(c, ""), u[1] + r + "Content-Transfer-Encoding: base64")
                } else n(l, i)
            }
        }, this.onready = function (e) {
            (l = function () {
                0 === a && (o.unshift(""), o.push("--"), e(o.join(s), i), o = [])
            })()
        }
    }

    function c(e, n, o, i, a) {
        var r = "https://content.googleapis.com";
        m || (g = String(parseInt(1e8 * Math.random(), 10)), m = p.append("iframe", {
            src: r + "/static/proxy.html?jsh=m%3B%2F_%2Fscs%2Fapps-static%2F_%2Fjs%2Fk%3Doz.gapi.en.mMZgig4ibk0.O%2Fm%3D__features__%2Fam%3DEQ%2Frt%3Dj%2Fd%3D1%2Frs%3DAItRSTNZBJcXGialq7mfSUkqsE3kvYwkpQ#parent=" + t.location.origin + "&rpctoken=" + g,
            style: {
                position: "absolute",
                left: "-1000px",
                bottom: 0,
                height: "1px",
                width: "1px"
            }
        }, "body"), f(t, "message", function (e) {
            if (e.origin === r) {
                var t;
                try {
                    t = JSON.parse(e.data)
                } catch (n) {
                    return
                }
                if (t && t.s && t.s === "ready:" + g) {
                    h = !0, v = 0;
                    for (var o = 0; o < w.length; o++) w[o]()
                }
            }
        }));
        var s = function () {
            var s = t.navigator,
                l = ++v,
                c = p.param(n.match(/\?.+/)[0]),
                u = c.access_token;
            delete c.access_token;
            var h = JSON.stringify({
                s: "makeHttpRequests",
                f: "..",
                c: l,
                a: [
                    [{
                        key: "gapiRequest",
                        params: {
                            url: n.replace(/(^https?\:\/\/[^\/]+|\?.+$)/, ""),
                            httpMethod: e.toUpperCase(),
                            body: i,
                            headers: {
                                Authorization: ":Bearer " + u,
                                "Content-Type": o["content-type"],
                                "X-Origin": t.location.origin,
                                "X-ClientDetails": "appVersion=" + s.appVersion + "&platform=" + s.platform + "&userAgent=" + s.userAgent
                            },
                            urlParams: c,
                            clientName: "google-api-javascript-client",
                            clientVersion: "1.1.0-beta"
                        }
                    }]
                ],
                t: g,
                l: !1,
                g: !0,
                r: ".."
            });
            f(t, "message", function w(e) {
                if (e.origin === r) try {
                    var n = JSON.parse(e.data);
                    n.t === g && n.a[0] === l && (d(t, "message", w), a(JSON.parse(JSON.parse(n.a[1]).gapiRequest.data.body)))
                } catch (o) {
                    a({
                        error: {
                            code: "request_error",
                            message: "Failed to post to Google"
                        }
                    })
                }
            }), m.contentWindow.postMessage(h, "*")
        };
        h ? s() : w.push(s)
    }

    function u(e, n) {
        var o = {};
        e.data && e.data instanceof t.HTMLInputElement && (e.data = {
            file: e.data
        }), !e.data.name && Object(Object(e.data.file).files).length && "post" === e.method && (e.data.name = e.data.file.files[0].name), "post" === e.method ? e.data = {
            title: e.data.name,
            parents: [{
                id: e.data.parent || "root"
            }],
            file: e.data.file
        } : (o = e.data, e.data = {}, o.parent && (e.data.parents = [{
            id: e.data.parent || "root"
        }]), o.file && (e.data.file = o.file), o.name && (e.data.title = o.name)), n("upload/drive/v2/files" + (o.id ? "/" + o.id : "") + "?uploadType=multipart")
    }
    var f, d, p = e.utils;
    document.removeEventListener ? (f = function (e, t, n) {
        e.addEventListener(t, n)
    }, d = function (e, t, n) {
        e.removeEventListener(t, n)
    }) : document.detachEvent && (d = function (e, t, n) {
        e.detachEvent("on" + t, n)
    }, f = function (e, t, n) {
        e.attachEvent("on" + t, n)
    });
    var m, h, g, v, w = [],
        y = "https://www.google.com/m8/feeds/contacts/default/full?alt=json&max-results=@{limit|1000}&start-index=@{start|1}";
    e.init({
        google: {
            name: "Google Plus",
            login: function (e) {
                "none" === e.qs.display && (e.qs.display = ""), "code" === e.qs.response_type && (e.qs.access_type = "offline")
            },
            oauth: {
                version: 2,
                auth: "https://accounts.google.com/o/oauth2/auth",
                grant: "https://accounts.google.com/o/oauth2/token"
            },
            scope: {
                basic: "https://www.googleapis.com/auth/plus.me profile",
                email: "email",
                birthday: "",
                events: "",
                photos: "https://picasaweb.google.com/data/",
                videos: "http://gdata.youtube.com",
                friends: "https://www.google.com/m8/feeds, https://www.googleapis.com/auth/plus.login",
                files: "https://www.googleapis.com/auth/drive.readonly",
                publish: "",
                publish_files: "https://www.googleapis.com/auth/drive",
                create_event: "",
                offline_access: ""
            },
            scope_delim: " ",
            base: "https://www.googleapis.com/",
            get: {
                me: "plus/v1/people/me",
                "me/friends": "plus/v1/people/me/people/visible?maxResults=@{limit|100}",
                "me/following": y,
                "me/followers": y,
                "me/contacts": y,
                "me/share": "plus/v1/people/me/activities/public?maxResults=@{limit|100}",
                "me/feed": "plus/v1/people/me/activities/public?maxResults=@{limit|100}",
                "me/albums": "https://picasaweb.google.com/data/feed/api/user/default?alt=json&max-results=@{limit|100}&start-index=@{start|1}",
                "me/album": function (e, t) {
                    var n = e.data.id;
                    delete e.data.id, t(n.replace("/entry/", "/feed/"))
                },
                "me/photos": "https://picasaweb.google.com/data/feed/api/user/default?alt=json&kind=photo&max-results=@{limit|100}&start-index=@{start|1}",
                "me/files": "drive/v2/files?q=%22@{parent|root}%22+in+parents+and+trashed=false&maxResults=@{limit|100}",
                "me/folders": "drive/v2/files?q=%22@{id|root}%22+in+parents+and+mimeType+=+%22application/vnd.google-apps.folder%22+and+trashed=false&maxResults=@{limit|100}",
                "me/folder": "drive/v2/files?q=%22@{id|root}%22+in+parents+and+trashed=false&maxResults=@{limit|100}"
            },
            post: {
                "me/files": u,
                "me/folders": function (e, t) {
                    e.data = {
                        title: e.data.name,
                        parents: [{
                            id: e.data.parent || "root"
                        }],
                        mimeType: "application/vnd.google-apps.folder"
                    }, t("drive/v2/files")
                }
            },
            put: {
                "me/files": u
            },
            del: {
                "me/files": "drive/v2/files/@{id}",
                "me/folder": "drive/v2/files/@{id}"
            },
            wrap: {
                me: function (e) {
                    return e.id && (e.last_name = e.family_name || (e.name ? e.name.familyName : null), e.first_name = e.given_name || (e.name ? e.name.givenName : null), e.emails && e.emails.length && (e.email = e.emails[0].value), a(e)), e
                },
                "me/friends": function (e) {
                    if (e.items) {
                        s(e), e.data = e.items, delete e.items;
                        for (var t = 0; t < e.data.length; t++) a(e.data[t])
                    }
                    return e
                },
                "me/contacts": r,
                "me/followers": r,
                "me/following": r,
                "me/share": function (e) {
                    return s(e), e.data = e.items, delete e.items, e
                },
                "me/feed": function (e) {
                    return s(e), e.data = e.items, delete e.items, e
                },
                "me/albums": i,
                "me/photos": i,
                "default": i
            },
            xhr: function (e) {
                if ("post" === e.method || "put" === e.method) {
                    if (e.data && p.hasBinary(e.data) || e.data.file) return e.cors_support = e.cors_support || !0, !1;
                    e.data = JSON.stringify(e.data), e.headers = {
                        "content-type": "application/json"
                    }
                }
                return !0
            },
            api: function (e, t, n, o) {
                if ("get" !== t.method) {
                    "file" in t.data && p.domInstance("input", t.data.file) && !("files" in t.data.file) && o({
                        error: {
                            code: "request_invalid",
                            message: "Sorry, can't upload your files to Google Drive in this browser"
                        }
                    });
                    var i;
                    if ("file" in t.data && (i = t.data.file, delete t.data.file, "object" == typeof i && "files" in i && (i = i.files), !i || !i.length)) return void o({
                        error: {
                            code: "request_invalid",
                            message: "There were no files attached with this request to upload"
                        }
                    });
                    var a = new l;
                    return a.append(JSON.stringify(t.data), "application/json"), i && a.append(i), a.onready(function (i, a) {
                        t.cors_support ? p.xhr(t.method, p.qs(e, n), {
                            "content-type": 'multipart/related; boundary="' + a + '"'
                        }, i, o) : c(t.method, p.qs(e, n), {
                            "content-type": 'multipart/related; boundary="' + a + '"'
                        }, i, o)
                    }), !0
                }
            }
        }
    })
}(hello, window),
function (e) {
    function t(e) {
        e && "meta" in e && "error_type" in e.meta && (e.error = {
            code: e.meta.error_type,
            message: e.meta.error_message
        })
    }

    function n(e) {
        if (i(e), e && "data" in e)
            for (var t = 0; t < e.data.length; t++) o(e.data[t]);
        return e
    }

    function o(e) {
        e.id && (e.thumbnail = e.profile_picture, e.name = e.full_name || e.username)
    }

    function i(e) {
        "pagination" in e && (e.paging = {
            next: e.pagination.next_url
        }, delete e.pagination)
    }
    e.init({
        instagram: {
            name: "Instagram",
            login: function (e) {
                e.qs.display = ""
            },
            oauth: {
                version: 2,
                auth: "https://instagram.com/oauth/authorize/"
            },
            refresh: !0,
            scope: {
                basic: "basic",
                friends: "relationships",
                photos: ""
            },
            scope_delim: " ",
            base: "https://api.instagram.com/v1/",
            get: {
                me: "users/self",
                "me/feed": "users/self/feed?count=@{limit|100}",
                "me/photos": "users/self/media/recent?min_id=0&count=@{limit|100}",
                "me/friends": "users/self/follows?count=@{limit|100}",
                "me/following": "users/self/follows?count=@{limit|100}",
                "me/followers": "users/self/followed-by?count=@{limit|100}"
            },
            wrap: {
                me: function (e) {
                    return t(e), "data" in e && (e.id = e.data.id, e.thumbnail = e.data.profile_picture, e.name = e.data.full_name || e.data.username), e
                },
                "me/friends": n,
                "me/following": n,
                "me/followers": n,
                "me/photos": function (e) {
                    if (t(e), i(e), "data" in e)
                        for (var n = 0; n < e.data.length; n++) {
                            var o = e.data[n];
                            "image" === o.type ? (o.thumbnail = o.images.thumbnail.url, o.picture = o.images.standard_resolution.url, o.name = o.caption ? o.caption.text : null) : (e.data.splice(n, 1), n--)
                        }
                    return e
                },
                "default": function (e) {
                    return i(e), e
                }
            },
            xhr: !1
        }
    })
}(hello),
function (e) {
    function t(e) {
        e && "errorCode" in e && (e.error = {
            code: e.status,
            message: e.message
        })
    }

    function n(e) {
        e.error || (e.first_name = e.firstName, e.last_name = e.lastName, e.name = e.formattedName || e.first_name + " " + e.last_name, e.thumbnail = e.pictureUrl)
    }

    function o(e) {
        if (t(e), i(e), e.values) {
            e.data = e.values;
            for (var o = 0; o < e.data.length; o++) n(e.data[o]);
            delete e.values
        }
        return e
    }

    function i(e) {
        "_count" in e && "_start" in e && e._count + e._start < e._total && (e.paging = {
            next: "?start=" + (e._start + e._count) + "&count=" + e._count
        })
    }
    e.init({
        linkedin: {
            oauth: {
                version: 2,
                response_type: "code",
                auth: "https://www.linkedin.com/uas/oauth2/authorization",
                grant: "https://www.linkedin.com/uas/oauth2/accessToken"
            },
            refresh: !0,
            scope: {
                basic: "r_fullprofile",
                email: "r_emailaddress",
                friends: "r_network",
                publish: "rw_nus"
            },
            scope_delim: " ",
            querystring: function (e) {
                e.oauth2_access_token = e.access_token, delete e.access_token
            },
            base: "https://api.linkedin.com/v1/",
            get: {
                me: "people/~:(picture-url,first-name,last-name,id,formatted-name)",
                "me/friends": "people/~/connections?count=@{limit|500}",
                "me/followers": "people/~/connections?count=@{limit|500}",
                "me/following": "people/~/connections?count=@{limit|500}",
                "me/share": "people/~/network/updates?count=@{limit|250}"
            },
            post: {},
            wrap: {
                me: function (e) {
                    return t(e), n(e), e
                },
                "me/friends": o,
                "me/following": o,
                "me/followers": o,
                "me/share": function (e) {
                    if (t(e), i(e), e.values) {
                        e.data = e.values, delete e.values;
                        for (var o = 0; o < e.data.length; o++) {
                            var a = e.data[o];
                            n(a), a.message = a.headline
                        }
                    }
                    return e
                },
                "default": function (e) {
                    t(e), i(e)
                }
            },
            jsonp: function (e, t) {
                t.format = "jsonp", "get" === e.method && (t["error-callback"] = "?")
            },
            xhr: !1
        }
    })
}(hello),
function (e) {
    function t(e) {
        e.id && (e.picture = e.avatar_url, e.thumbnail = e.avatar_url, e.name = e.username || e.full_name)
    }

    function n(e) {
        "next_href" in e && (e.paging = {
            next: e.next_href
        })
    }
    e.init({
        soundcloud: {
            name: "SoundCloud",
            oauth: {
                version: 2,
                auth: "https://soundcloud.com/connect"
            },
            querystring: function (e) {
                var t = e.access_token;
                delete e.access_token, e.oauth_token = t, e["_status_code_map[302]"] = 200
            },
            base: "https://api.soundcloud.com/",
            get: {
                me: "me.json",
                "me/friends": "me/followings.json",
                "me/followers": "me/followers.json",
                "me/following": "me/followings.json",
                "default": function (e, t) {
                    t(e.path + ".json")
                }
            },
            wrap: {
                me: function (e) {
                    return t(e), e
                },
                "default": function (e) {
                    if (e instanceof Array) {
                        e = {
                            data: e
                        };
                        for (var o = 0; o < e.data.length; o++) t(e.data[o])
                    }
                    return n(e), e
                }
            }
        }
    })
}(hello),
function (e) {
    function t(e) {
        if (e.id) {
            if (e.name) {
                var t = e.name.split(" ");
                e.first_name = t[0], e.last_name = t[1]
            }
            e.thumbnail = e.profile_image_url
        }
    }

    function n(e) {
        if (o(e), i(e), e.users) {
            e.data = e.users;
            for (var n = 0; n < e.data.length; n++) t(e.data[n]);
            delete e.users
        }
        return e
    }

    function o(e) {
        if (e.errors) {
            var t = e.errors[0];
            e.error = {
                code: "request_failed",
                message: t.message
            }
        }
    }

    function i(e) {
        "next_cursor_str" in e && (e.paging = {
            next: "?cursor=" + e.next_cursor_str
        })
    }
    e.init({
        twitter: {
            oauth: {
                version: "1.0a",
                auth: "https://twitter.com/oauth/authorize",
                request: "https://twitter.com/oauth/request_token",
                token: "https://twitter.com/oauth/access_token"
            },
            base: "https://api.twitter.com/1.1/",
            get: {
                me: "account/verify_credentials.json",
                "me/friends": "friends/list.json?count=@{limit|200}",
                "me/following": "friends/list.json?count=@{limit|200}",
                "me/followers": "followers/list.json?count=@{limit|200}",
                "me/share": "statuses/user_timeline.json?count=@{limit|200}"
            },
            post: {
                "me/share": function (e, t) {
                    var n = e.data;
                    e.data = null, t("statuses/update.json?include_entities=1&status=" + n.message)
                }
            },
            wrap: {
                me: function (e) {
                    return o(e), t(e), e
                },
                "me/friends": n,
                "me/followers": n,
                "me/following": n,
                "me/share": function (e) {
                    return o(e), i(e), !e.error && "length" in e ? {
                        data: e
                    } : e
                },
                "default": function (e) {
                    return i(e), e
                }
            },
            xhr: function (e) {
                return "get" !== e.method
            }
        }
    })
}(hello),
function (e) {
    function t(t) {
        if (t.id) {
            var n = e.getAuthResponse("windows").access_token;
            if (t.emails && (t.email = t.emails.preferred), t.is_friend !== !1) {
                var o = t.user_id || t.id;
                t.thumbnail = t.picture = "https://apis.live.net/v5.0/" + o + "/picture?access_token=" + n
            }
        }
    }

    function n(e) {
        if ("data" in e)
            for (var n = 0; n < e.data.length; n++) t(e.data[n]);
        return e
    }
    e.init({
        windows: {
            name: "windows",
            oauth: {
                version: 2,
                auth: "https://login.live.com/oauth20_authorize.srf"
            },
            refresh: !0,
            logout: function () {
                return "http://login.live.com/oauth20_logout.srf?ts=" + (new Date).getTime()
            },
            id: _authConfig.live.client_id,
            scope: {
                basic: "wl.signin,wl.basic",
                email: "wl.emails",
                birthday: "wl.birthday",
                events: "wl.calendars",
                photos: "wl.photos",
                videos: "wl.photos",
                friends: "wl.contacts_emails",
                files: "wl.skydrive",
                publish: "wl.share",
                publish_files: "wl.skydrive_update",
                create_event: "wl.calendars_update,wl.events_create",
                offline_access: "wl.offline_access"
            },
            base: "https://apis.live.net/v5.0/",
            get: {
                me: "me",
                "me/friends": "me/friends",
                "me/following": "me/contacts",
                "me/followers": "me/friends",
                "me/contacts": "me/contacts",
                "me/albums": "me/albums",
                "me/album": "@{id}/files",
                "me/photo": "@{id}",
                "me/files": "@{parent|me/skydrive}/files",
                "me/folders": "@{id|me/skydrive}/files",
                "me/folder": "@{id|me/skydrive}/files"
            },
            post: {
                "me/albums": "me/albums",
                "me/album": "@{id}/files/",
                "me/folders": "@{id|me/skydrive/}",
                "me/files": "@{parent|me/skydrive/}/files"
            },
            del: {
                "me/album": "@{id}",
                "me/photo": "@{id}",
                "me/folder": "@{id}",
                "me/files": "@{id}"
            },
            wrap: {
                me: function (e) {
                    return t(e), e
                },
                "me/friends": n,
                "me/contacts": n,
                "me/followers": n,
                "me/following": n,
                "me/albums": function (e) {
                    if ("data" in e)
                        for (var t = 0; t < e.data.length; t++) {
                            var n = e.data[t];
                            n.photos = n.files = "https://apis.live.net/v5.0/" + n.id + "/photos"
                        }
                    return e
                },
                "default": function (e) {
                    if ("data" in e)
                        for (var t = 0; t < e.data.length; t++) {
                            var n = e.data[t];
                            n.picture && (n.thumbnail = n.picture)
                        }
                    return e
                }
            },
            xhr: function (t) {
                return "get" === t.method || "delete" === t.method || e.utils.hasBinary(t.data) || ("string" == typeof t.data.file ? t.data.file = e.utils.toBlob(t.data.file) : (t.data = JSON.stringify(t.data), t.headers = {
                    "Content-Type": "application/json"
                })), !0
            },
            jsonp: function (t) {
                "get" === t.method.toLowerCase() || e.utils.hasBinary(t.data) || (t.data.method = t.method.toLowerCase(), t.method = "get")
            }
        }
    })
}(hello),
function (e) {
    function t(e) {
        e && "meta" in e && "error_type" in e.meta && (e.error = {
            code: e.meta.error_type,
            message: e.meta.error_message
        })
    }

    function n(e) {
        t(e), o(e);
        var n, i;
        if (e.query && e.query.results && e.query.results.contact) {
            e.data = e.query.results.contact, delete e.query, e.data instanceof Array || (e.data = [e.data]);
            for (var a = 0; a < e.data.length; a++) {
                n = e.data[a], n.id = null;
                for (var r = 0; r < n.fields.length; r++) i = n.fields[r], "email" === i.type && (n.email = i.value), "name" === i.type && (n.first_name = i.value.givenName, n.last_name = i.value.familyName, n.name = i.value.givenName + " " + i.value.familyName), "yahooid" === i.type && (n.id = i.value)
            }
        }
        return e
    }

    function o(e) {
        e.query && e.query.count && (e.paging = {
            next: "?start=" + e.query.count
        })
    }
    var i = function (e) {
        return "https://query.yahooapis.com/v1/yql?q=" + (e + " limit @{limit|100} offset @{start|0}").replace(/\s/g, "%20") + "&format=json"
    };
    e.init({
        yahoo: {
            oauth: {
                version: "1.0a",
                auth: "https://api.login.yahoo.com/oauth/v2/request_auth",
                request: "https://api.login.yahoo.com/oauth/v2/get_request_token",
                token: "https://api.login.yahoo.com/oauth/v2/get_token"
            },
            redirect_uri: 'redirect.html',
            login: function (e) {
                e.options.window_width = 560
            },
            id: _authConfig.yahoo.consumer_key,//'dj0yJmk9ZXpFRTdiZ3lzakl5JmQ9WVdrOVZrUnNVRE5KTkdVbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD1kYQ--',
            base: "https://social.yahooapis.com/v1/",
            get: {
                me: i("select * from social.profile(0) where guid=me"),
                "me/friends": i("select * from social.contacts(0) where guid=me"),
                "me/following": i("select * from social.contacts(0) where guid=me")
            },
            wrap: {
                me: function (e) {
                    if (t(e), e.query && e.query.results && e.query.results.profile) {
                        e = e.query.results.profile, e.id = e.guid, e.last_name = e.familyName, e.first_name = e.givenName || e.nickname;
                        var n = [];
                        e.first_name && n.push(e.first_name), e.last_name && n.push(e.last_name), e.name = n.join(" "), e.email = e.emails ? e.emails.handle : null, e.thumbnail = e.image ? e.image.imageUrl : null
                    }
                    return e
                },
                "me/friends": n,
                "me/following": n,
                "default": function (e) {
                    return o(e), e
                }
            },
            xhr: !1
        }
    })
}(hello), "function" == typeof define && define.amd && define(function () {
    return hello
});
