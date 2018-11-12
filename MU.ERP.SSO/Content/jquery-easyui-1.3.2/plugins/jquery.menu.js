/**
 * jQuery EasyUI 1.3.2
 * 
 * Copyright (c) 2009-2013 www.jeasyui.com. All rights reserved.
 *
 * Licensed under the GPL or commercial licenses
 * To use it on other terms please contact us: jeasyui@gmail.com
 * http://www.gnu.org/licenses/gpl.txt
 * http://www.jeasyui.com/license_commercial.php
 *
 */
(function ($) {
    function _1(_2) {
        $(_2).appendTo("body");
        $(_2).addClass("menu-top");

        $(document).unbind(".menu").bind("mousedown.menu", function (e) {
            var _3 = $("body>div.menu:visible");
            var m = $(e.target).closest("div.menu", _3);
            if (m.length) {
                return;
            }
            $("body>div.menu-top:visible").menu("hide");
        });
        var _4 = _5($(_2));
        for (var i = 0; i < _4.length; i++) {
            _6(_4[i]);
        }
        function _5(_7) {
            var _8 = [];
            _7.addClass("menu");
            if (!_7[0].style.width) {
                _7[0].autowidth = true;
            }
            _8.push(_7);
            if (!_7.hasClass("menu-content")) {
                _7.children("div").each(function () {
                    var _9 = $(this).children("div");
                    if (_9.length) {
                        _9.insertAfter(_2);
                        this.submenu = _9;
                        var mm = _5(_9);
                        _8 = _8.concat(mm);
                    }
                });
            }
            return _8;
        };
        function _6(_a) {
            if (!_a.hasClass("menu-content")) {
                _a.children("div").each(function () {
                    var _b = $(this);
                    if (_b.hasClass("menu-sep")) {
                        //console.log(_b);
                    } else {
                        //console.log(_b);
                        var _c=$.extend({},$.parser.parseOptions(this,["name","iconCls","href"]),{disabled:(_b.attr("disabled")?true:undefined)});
                        // 注释掉上面 3 行代码，并添加了下面 3 行代码，以实现获取 menu-item 的属性 hideOnClick，该参数表示是否在点击菜单项后菜单自动隐藏
                        //var _c = $.extend({ hideOnClick: true }, $.parser.parseOptions(this, ['name', 'iconCls', 'href', { hideOnClick: 'boolean', separator: 'boolean' }]), {
                        //    disabled: (_b.attr('disabled') ? true : undefined)
                        //});
                        _b.attr("name", _c.name || "").attr("href", _c.href || "");

                        // 添加了下一行代码，以实现将 menu-item 的 hideOnClick 绑定到菜单项上
                        _b.hideOnClick = (_c.hideOnClick == undefined || _c.hideOnClick == null ? true : !!_c.hideOnClick);
                        var _d = _b.addClass("menu-item").html();
                        _b.empty().append($("<div class=\"menu-text\"></div>").html(_d));
                        if (_c.iconCls) {
                            $("<div class=\"menu-icon\"></div>").addClass(_c.iconCls).appendTo(_b);
                        }
                        if (_c.disabled) {
                            _e(_2, _b[0], true);
                        }
                        if (_b[0].submenu) {
                            $("<div class=\"menu-rightarrow\"></div>").appendTo(_b);
                        }
                        _b._outerHeight(22);
                        _f(_2, _b);
                    }
                });
                $("<div class=\"menu-line\"></div>").prependTo(_a);
            }
            _10(_2, _a);
            _a.hide();
            _11(_2, _a);
        };
    };
    function _10(_12, _13) {
        var _14 = $.data(_12, "menu").options;
        var d = _13.css("display");
        _13.css({ display: "block", left: -10000 });
        var _15 = _13._outerWidth();
        var _16 = 0;
        _13.find("div.menu-text").each(function () {
            if (_16 < $(this)._outerWidth()) {
                _16 = $(this)._outerWidth();
            }
        });
        _16 += 65;
        _13._outerWidth(Math.max(_15, _16, _14.minWidth));
        _13.css("display", d);
    };
    function _11(_17, _18) {
        var _19 = $.data(_17, "menu");
        _18.unbind(".menu").bind("mouseenter.menu", function () {
            if (_19.timer) {
                clearTimeout(_19.timer);
                _19.timer = null;
            }
        }).bind("mouseleave.menu", function () {
            _19.timer = setTimeout(function () {
                _1a(_17);
            }, 100);
        });
    };
    function _f(_1b, _1c) {
        _1c.unbind(".menu");
        _1c.bind("click.menu", function () {
            if ($(this).hasClass("menu-item-disabled")) {
                return;
            }
            if (!this.submenu) {
                // 以下1行代码为 jquery-menu.js 源码原版
                //hideAll(target, $(target).hasClass('menu-inline'));
                // 注释掉上面1行代码，并添加下面 3 行代码，以实现当 menu-item 的属性 hideOnClick 为 false 的情况下，点击菜单项不自动隐藏菜单控件。
                console.log(this);
                this.hideOnClick = $(this).attr("hideOnClick") == 'true' ? true : false;
                if (this.hideOnClick == true) {
                    _1a(_1b);
                }

                //_1a(_1b);
                var _1d = $(this).attr("href");
                if (_1d) {
                    location.href = _1d;
                }
            }
            var _1e = $(_1b).menu("getItem", this);
            $.data(_1b, "menu").options.onClick.call(_1b, _1e);
        }).bind("mouseenter.menu", function (e) {
            _1c.siblings().each(function () {
                if (this.submenu) {
                    _21(this.submenu);
                }
                $(this).removeClass("menu-active");
            });
            _1c.addClass("menu-active");
            if ($(this).hasClass("menu-item-disabled")) {
                _1c.addClass("menu-active-disabled");
                return;
            }
            var _1f = _1c[0].submenu;
            if (_1f) {
                $(_1b).menu("show", { menu: _1f, parent: _1c });
            }
        }).bind("mouseleave.menu", function (e) {
            _1c.removeClass("menu-active menu-active-disabled");
            var _20 = _1c[0].submenu;
            if (_20) {
                if (e.pageX >= parseInt(_20.css("left"))) {
                    _1c.addClass("menu-active");
                } else {
                    _21(_20);
                }
            } else {
                _1c.removeClass("menu-active");
            }
        });
    };
    function _1a(_22) {
        var _23 = $.data(_22, "menu");
        if (_23) {
            if ($(_22).is(":visible")) {
                _21($(_22));
                _23.options.onHide.call(_22);
            }
        }
        return false;
    };
    function _24(_25, _26) {
        var _27, top;
        var _28 = $(_26.menu || _25);
        if (_28.hasClass("menu-top")) {
            var _29 = $.data(_25, "menu").options;
            _27 = _29.left;
            top = _29.top;
            if (_26.alignTo) {
                var at = $(_26.alignTo);
                _27 = at.offset().left;
                top = at.offset().top + at._outerHeight();
            }
            if (_26.left != undefined) {
                _27 = _26.left;
            }
            if (_26.top != undefined) {
                top = _26.top;
            }
            if (_27 + _28.outerWidth() > $(window)._outerWidth() + $(document)._scrollLeft()) {
                _27 = $(window)._outerWidth() + $(document).scrollLeft() - _28.outerWidth() - 5;
            }
            if (top + _28.outerHeight() > $(window)._outerHeight() + $(document).scrollTop()) {
                top -= _28.outerHeight();
            }
        } else {
            var _2a = _26.parent;
            _27 = _2a.offset().left + _2a.outerWidth() - 2;
            if (_27 + _28.outerWidth() + 5 > $(window)._outerWidth() + $(document).scrollLeft()) {
                _27 = _2a.offset().left - _28.outerWidth() + 2;
            }
            var top = _2a.offset().top - 3;
            if (top + _28.outerHeight() > $(window)._outerHeight() + $(document).scrollTop()) {
                top = $(window)._outerHeight() + $(document).scrollTop() - _28.outerHeight() - 5;
            }
        }
        _28.css({ left: _27, top: top });
        _28.show(0, function () {
            if (!_28[0].shadow) {
                _28[0].shadow = $("<div class=\"menu-shadow\"></div>").insertAfter(_28);
            }
            _28[0].shadow.css({ display: "block", zIndex: $.fn.menu.defaults.zIndex++, left: _28.css("left"), top: _28.css("top"), width: _28.outerWidth(), height: _28.outerHeight() });
            _28.css("z-index", $.fn.menu.defaults.zIndex++);
            if (_28.hasClass("menu-top")) {
                $.data(_28[0], "menu").options.onShow.call(_28[0]);
            }
        });
    };
    function _21(_2b) {
        if (!_2b) {
            return;
        }
        _2c(_2b);
        _2b.find("div.menu-item").each(function () {
            if (this.submenu) {
                _21(this.submenu);
            }
            $(this).removeClass("menu-active");
        });
        function _2c(m) {
            m.stop(true, true);
            if (m[0].shadow) {
                m[0].shadow.hide();
            }
            m.hide();
        };
    };
    function _2d(_2e, _2f) {
        var _30 = null;
        var tmp = $("<div></div>");
        function _31(_32) {
            _32.children("div.menu-item").each(function () {
                var _33 = $(_2e).menu("getItem", this);
                var s = tmp.empty().html(_33.text).text();
                if (_2f == $.trim(s)) {
                    _30 = _33;
                } else {
                    if (this.submenu && !_30) {
                        _31(this.submenu);
                    }
                }
            });
        };
        _31($(_2e));
        tmp.remove();
        return _30;
    };
    function _e(_34, _35, _36) {
        var t = $(_35);
        if (_36) {
            t.addClass("menu-item-disabled");
            if (_35.onclick) {
                _35.onclick1 = _35.onclick;
                _35.onclick = null;
            }
        } else {
            t.removeClass("menu-item-disabled");
            if (_35.onclick1) {
                _35.onclick = _35.onclick1;
                _35.onclick1 = null;
            }
        }
    };
    function _37(_38, _39) {
        var _3a = $(_38);
        if (_39.parent) {
            if (!_39.parent.submenu) {
                var _3b = $("<div class=\"menu\"><div class=\"menu-line\"></div></div>").appendTo("body");
                _3b[0].autowidth = true;
                _3b.hide();
                _39.parent.submenu = _3b;
                $("<div class=\"menu-rightarrow\"></div>").appendTo(_39.parent);
            }
            _3a = _39.parent.submenu;
        }
        var _3c = $("<div class=\"menu-item\"></div>").appendTo(_3a);
        $("<div class=\"menu-text\"></div>").html(_39.text).appendTo(_3c);
        if (_39.iconCls) {
            $("<div class=\"menu-icon\"></div>").addClass(_39.iconCls).appendTo(_3c);
        }
        if (_39.id) {
            _3c.attr("id", _39.id);
        }
        if (_39.href) {
            _3c.attr("href", _39.href);
        }
        if (_39.name) {
            _3c.attr("name", _39.name);
        }
        if (_39.onclick) {
            if (typeof _39.onclick == "string") {
                _3c.attr("onclick", _39.onclick);
            } else {
                _3c[0].onclick = eval(_39.onclick);
            }
        }
        if (_39.handler) {
            _3c[0].onclick = eval(_39.handler);
        }
        _f(_38, _3c);
        if (_39.disabled) {
            _e(_38, _3c[0], true);
        }
        _11(_38, _3a);
        _10(_38, _3a);
    };
    function _3d(_3e, _3f) {
        function _40(el) {
            if (el.submenu) {
                el.submenu.children("div.menu-item").each(function () {
                    _40(this);
                });
                var _41 = el.submenu[0].shadow;
                if (_41) {
                    _41.remove();
                }
                el.submenu.remove();
            }
            $(el).remove();
        };
        _40(_3f);
    };
    function _42(_43) {
        $(_43).children("div.menu-item").each(function () {
            _3d(_43, this);
        });
        if (_43.shadow) {
            _43.shadow.remove();
        }
        $(_43).remove();
    };
    $.fn.menu = function (_44, _45) {
        if (typeof _44 == "string") {
            return $.fn.menu.methods[_44](this, _45);
        }
        _44 = _44 || {};
        return this.each(function () {
            var _46 = $.data(this, "menu");
            if (_46) {
                $.extend(_46.options, _44);
            } else {
                _46 = $.data(this, "menu", { options: $.extend({}, $.fn.menu.defaults, $.fn.menu.parseOptions(this), _44) });
                _1(this);
            }
            $(this).css({ left: _46.options.left, top: _46.options.top });
        });
    };
    $.fn.menu.methods = {
        options: function (jq) {
            return $.data(jq[0], "menu").options;
        }, show: function (jq, pos) {
            return jq.each(function () {
                _24(this, pos);
            });
        }, hide: function (jq) {
            return jq.each(function () {
                _1a(this);
            });
        }, destroy: function (jq) {
            return jq.each(function () {
                _42(this);
            });
        }, setText: function (jq, _47) {
            return jq.each(function () {
                $(_47.target).children("div.menu-text").html(_47.text);
            });
        }, setIcon: function (jq, _48) {
            return jq.each(function () {
                var _49 = $(this).menu("getItem", _48.target);
                //console.log(_49);
                if (_49.iconCls) {
                    $(_49.target).children("div.menu-icon").removeClass(_49.iconCls).addClass(_48.iconCls);
                } else {
                    $("<div class=\"menu-icon\"></div>").addClass(_48.iconCls).appendTo(_48.target);
                }
            });
        }, getItem: function (jq, _4a) {
            var t = $(_4a);
            var _4b = {
                target: _4a, id: t.attr("id"), text: $.trim(t.children("div.menu-text").html()),
                // 增加下面一行代码，使得通过 getItem 方法返回的 menu-item 中包含其 hideOnClick 属性
                hideOnClick: t.attr("hideOnClick") == 'true' ? true : false,
                disabled: t.hasClass("menu-item-disabled"), href: t.attr("href"), name: t.attr("name"), onclick: _4a.onclick
            };

            //console.log(_4a.hideOnClick);
            //console.log(t.attr("hideOnClick"));

            var _4c = t.children("div.menu-icon");
            if (_4c.length) {
                var cc = [];
                var aa = _4c.attr("class").split(" ");
                for (var i = 0; i < aa.length; i++) {
                    if (aa[i] != "menu-icon") {
                        cc.push(aa[i]);
                    }
                }
                _4b.iconCls = cc.join(" ");
            }
            return _4b;
        }, findItem: function (jq, _4d) {
            return _2d(jq[0], _4d);
        }, appendItem: function (jq, _4e) {
            return jq.each(function () {
                _37(this, _4e);
            });
        }, removeItem: function (jq, _4f) {
            return jq.each(function () {
                _3d(this, _4f);
            });
        }, enableItem: function (jq, _50) {
            return jq.each(function () {
                _e(this, _50, false);
            });
        }, disableItem: function (jq, _51) {
            return jq.each(function () {
                _e(this, _51, true);
            });
        }
    };
    $.fn.menu.parseOptions = function (_52) {
        return $.extend({}, $.parser.parseOptions(_52, ["left", "top", { minWidth: "number" }]));
    };
    $.fn.menu.defaults = {
        zIndex: 110000, left: 0, top: 0, minWidth: 120, onShow: function () {
        }, onHide: function () {
        }, onClick: function (_53) {
        }
    };


    ///*
    //    真正的扩展从此行开始
    //*/

    //function hideAllMenu(target) {
    //    $.util.pageNestingExecute(function (win) {
    //        if (!win || !win.document || !win.jQuery) {
    //            return;
    //        }
    //        var jq = win.jQuery;
    //        if (target) {
    //            var m = jq(target).closest('div.menu,div.combo-p');
    //            if (m.length) {
    //                return
    //            }
    //        }

    //        jq('body>div.menu-top:visible').not('.menu-inline').menu('hide');
    //        hide(jq('body>div.menu:visible').not('.menu-inline'));

    //        function hide(menu) {
    //            if (menu && menu.length) {
    //                hideit(menu);
    //                menu.find('div.menu-item').each(function () {
    //                    if (this.submenu) {
    //                        hide(this.submenu);
    //                    }
    //                    jq(this).removeClass('menu-active');
    //                });
    //            }
    //            function hideit(m) {
    //                m.stop(true, true);
    //                if (m[0].shadow) {
    //                    m[0].shadow.hide();
    //                }
    //                m.hide();
    //            }
    //        }
    //    });
    //}

    ////    // 下面这段代码实现即使在跨 IFRAME 的情况下，一个 WEB-PAGE 中也只能同时显示一个 easyui-menu 控件。
    ////$.util.bindDocumentNestingEvent("mousedown.menu-nesting", function (doc, e) {
    ////    hideAllMenu(e.target);
    ////});




    ////$.util.namespace("$.easyui");


    //function createMenu(options) {
    //    var defaults = $.extend({}, $.fn.menu.defaults, {
    //        event: window.event || undefined,
    //        left: window.event ? window.event.clientX : 0,
    //        top: window.event ? window.event.clientY : 0,
    //        slideOut: false,
    //        autoDestroy: true,
    //        hideOnUnhover: false,
    //        hideDisabledItem: false,
    //        items: null,
    //        minWidth: 140
    //    });

    //    var root = $("").appendTo("body"),
    //        opts = $.util.likeArrayNotString(options)
    //            ? $.extend({}, defaults, { items: options })
    //            : $.extend({}, defaults, options || {});

    //    opts.items = $.util.likeArrayNotString(opts.items) ? opts.items : [];

    //    if (opts.id) {
    //        root.attr("id", opts.id);
    //    }
    //    if (opts.name) {
    //        root.attr("name", opts.name);
    //    }
    //    if (!opts.items.length) {
    //        opts.items.push({ text: "当前无菜单项", disabled: true });
    //    }
    //    $.each(opts.items, function (i, item) {
    //        appendItemToMenu(opts.event, root, item, root[0], opts.hideDisabledItem);
    //    });
    //    return { menu: root, options: opts };
    //}

    //function appendItemToMenu(e, menu, item, root, hideDisabledItem) {
    //    if ($.util.isString(item) && $.array.contains(["-", "—", "|"], $.trim(item))) {
    //        $("").appendTo(menu);
    //        return;
    //    }
    //    item = $.extend({
    //        id: null,
    //        text: null,
    //        iconCls: null,
    //        href: null,
    //        disabled: false,
    //        onclick: null,
    //        handler: null,
    //        bold: false,
    //        style: null,
    //        children: null,
    //        hideDisabledItem: hideDisabledItem,
    //        hideOnClick: true
    //    }, item || {});

    //    var onclick = item.onclick,
    //        handler = item.handler,
    //        itemEle = $("").appendTo(menu),
    //        args = [e, item, root];

    //    item.onclick = undefined;
    //    item.handler = undefined;
    //    item = $.util.parseMapFunction(item, args, itemEle[0]);
    //    item.onclick = onclick;
    //    item.handler = handler;

    //    if (item.disabled && item.hideDisabledItem) {
    //        return;
    //    }
    //    itemEle.attr({
    //        iconCls: item.iconCls,
    //        href: item.href,
    //        disabled: item.disabled,
    //        hideOnClick: item.hideOnClick
    //    });

    //    if (item.id) {
    //        itemEle.attr("id", item.id);
    //    }
    //    if (item.style) {
    //        itemEle.css(item.style);
    //    }

    //    if ($.isFunction(item.handler)) {
    //        var handler = item.handler;
    //        item.onclick = function (e, item, root) {
    //            handler.call(this, e, item, root);
    //        };
    //    }
    //    if ($.isFunction(item.onclick)) {
    //        itemEle.click(function (e) {
    //            if (item.disabled || itemEle.hasClass("menu-item-disabled")) {
    //                return;
    //            }
    //            item.onclick.call(this, e, item, root);
    //        });
    //    }
    //    var span = $("").appendTo(itemEle).text(item.text === null || item.text == undefined ? "" : item.text);
    //    if (item.bold) {
    //        span.css("font-weight", "bold");
    //    }

    //    if (item.children && item.children.length) {
    //        var itemNode = $("").appendTo(itemEle);
    //        $.each(item.children, function (i, n) {
    //            appendItemToMenu(e, itemNode, n, root, item.hideDisabledItem);
    //        });
    //    }
    //}

    //function createAndShowMenu(options) {
    //    var ret = createMenu(options),
    //        mm = ret.menu,
    //        opts = mm.menu(ret.options).menu("options");

    //    if (opts.autoDestroy) {
    //        var onHide = opts.onHide;
    //        opts.onHide = function () {
    //            if ($.isFunction(onHide)) {
    //                onHide.apply(this, arguments);
    //            }
    //            var m = $(this);
    //            $.util.delay(function () { m.menu("destroy"); });
    //        };
    //    }

    //    mm.menu("show", {
    //        left: opts.left, top: opts.top
    //    });

    //    if (opts.slideOut) {
    //        mm.hide().slideDown(200);
    //        if (mm[0] && mm[0].shadow) {
    //            mm[0].shadow.hide().slideDown(200);
    //        }
    //    }
    //    return mm;
    //}

    //function parseMenuItems(menuItems, args) {
    //    if (menuItems == null || menuItems == undefined || !$.util.likeArrayNotString(menuItems)) {
    //        return [];
    //    }
    //    args = (args == null || args == undefined) ? [] : ($.util.likeArrayNotString(args) ? args : [args])
    //    return $.array.map(menuItems, function (val, index) {
    //        if (!val || typeof val == "string") {
    //            return val;
    //        }
    //        var item = $.extend({}, val);
    //        $.each(item, function (key, value) {
    //            if ($.isFunction(value)) {
    //                item[key] = function () {
    //                    var newArgs = $.array.merge(arguments, args);
    //                    return value.apply(this, newArgs);
    //                };
    //            }
    //        });
    //        var children = item.children;
    //        if ($.util.likeArrayNotString(children)) {
    //            item.children = parseMenuItems(children, args);
    //        }
    //        return item;
    //    });
    //}


    //$.extend($.easyui, {

    //    //  根据指定的属性创建 easyui-menu 对象；该方法定义如下参数：
    //    //      options: JSON 对象类型，参数属性继承 easyui-menu 控件的所有属性和事件（参考官方 API 文档），并在此基础上增加了如下参数：
    //    //          id      : 一个 String 对象，表示创建的菜单对象的 ID 属性。
    //    //          name    : 一个 String 对象，表示创建的菜单对象的 name 属性。
    //    //          hideOnUnhover   : 这是官方 API 中原有属性，此处调整其默认值为 false；
    //    //          hideDisabledItem: 一个 Boolean 值，默认为 false；该属性表示当菜单项的 disabled: true，是否自动隐藏该菜单项；
    //    //          event   :  表示引发生成菜单列表的动作事件对象；该参数可选；
    //    //          items: 一个 Array 对象，该数组对象中的每一个元素都是一个 JSON 格式对象用于表示一个 menu-item （关于 menu-item 对象属性，参考官方 API）；
    //    //                  该数组中每个元素的属性，除 easyui-menu 中 menu-item 官方 API 定义的属性外，还增加了如下属性：
    //    //              hideDisabledItem: 该属性表示在当前子菜单级别下当菜单项的 disabled: true，是否自动隐藏该菜单项；一个 Boolean 值，默认取上一级的 hideDisabledItem 值；
    //    //              handler: 一个回调函数，表示点击菜单项时触发的事件；
    //    //                  回调函数 handler 和回调函数 onclick 的签名都为 function(e, itemOpts, target)，其中：
    //    //                      e       : 表示点击菜单列表中单个菜单项所触发的动作事件对象；
    //    //                      itemOpts: 表示当前点击的菜单项的 options 选项；
    //    //                      target  : 表示整个菜单控件的 HTML-DOM 对象。
    //    //                      函数中 this 指向触发事件的单个菜单项 HTML-DOM 对象本身
    //    //                  另，如果同时定义了 onclick 和 handler，则只处理 handler 而不处理 onclick，所以请不要两个回调函数属性同时使用。
    //    //              children: 同上一级对象的 items 属性，为一个 Array 对象；
    //    //          slideOut:   一个 Boolean 类型值，表示菜单是否以滑动方式显示出来；默认为 false；
    //    //          autoDestroy : Boolean 类型值，表示菜单是否在隐藏时自动销毁，默认为 true；
    //    //      options 参数也可直接定义为数组类型，即为 items 格式的 Array 对象；当 options 直接定义为 items 格式数组时，生成的 easyui-item 对象的其他属性则为系统默认值。
    //    //  返回值：返回一个 JSON 格式对象，该返回的对象中具有如下属性：
    //    //      menu: 依据于传入参数 options 构建出的菜单 DOM 元素对象，这是一个 jQuery 对象，该对象未初始化为 easyui-menu 控件，而只是具有该控件的 DOM 结构；
    //    //      options: 传入参数 options 解析后的结果，该结果尚未用于但可用于初始化 menu 元素。
    //    createMenu: function (options) { return createMenu(options); },

    //    //  根据指定的属性创建 easyui-menu 对象并立即显示出来；该方法定义的参数和本插件文件中的插件方法 createMenu 相同：
    //    //  注意：本方法与 createMenu 方法不同之处在于：
    //    //      createMenu: 仅根据传入的 options 参数创建出符合 easyui-menu DOM 结构要求的 jQuery DOM 对象，但是该对象并未初始化为 easyui-menu 控件；
    //    //      showMenu: 该方法在 createMenu 方法的基础上，对创建出来的 jQuery DOM 对象立即进行 easyui-menu 结构初始化，并显示出来。
    //    //  返回值：返回一个 jQuery 对象，该对象表示创建并显示出的 easyui-menu 元素，该返回的元素已经被初始化为 easyui-menu 控件。
    //    showMenu: function (options) { return createAndShowMenu(options); },

    //    // 将一个表示 easyui-menu 中菜单项集合的数组，解析成 $.easyui.showMenu 方法能够支持的菜单项数组格式。该方法定义如下参数：
    //    //      menuItems   : Array 数组格式，数组中的每个元素都是一个表示 menu-item 菜单项的 JSON-Object，该 JSON-Object 对象包含如下属性定义：
    //    //          id      : String 类型值，默认为 null ；表示菜单项的 ID ；
    //    //          text    : String 类型值，默认为 null ；表示菜单项的本文信息；
    //    //          iconCls : String 类型值，默认为 null ；表示菜单项的图标样式名称；
    //    //          href    : String 类型值，默认为 null ；表示菜单项指向的远程地址；
    //    //          disabled: Boolean 类型值，默认为 false；表示菜单项是否禁用；
    //    //          bold    : Boolean 类型值，默认为 false；表示该菜单项是否字体加粗；
    //    //          style   : JSON-Object 类型值，默认为 null；表示要附加到该菜单项的样式；
    //    //          hideOnClick     : Boolean 类型值，默认为 true；表示点击该菜单项后整个菜单是否会自动隐藏；
    //    //          hideDisabledItem: Boolean 类型值，默认为 false；该属性表示当菜单项的 disabled: true，是否自动隐藏该菜单项；
    //    //          handler : 回调函数，表示点击菜单项时触发的事件，签名为 function(e, itemOpts, target)；
    //    //          onclick : 回调函数，表示点击菜单项时触发的事件，签名为 function(e, itemOpts, target)；
    //    //      args        : 需要附加到解析回调函数的参数数组；该参数的作用在于：
    //    //          menu-item 中的任意属性可为具体的值，也可以为一个返回值的回调函数；但是在属性值为回调函数的情况下，函数签名为 function (e, itemOpts, target)
    //    //          假设在这种情况下 args 的值为 [a, b, c]，则在调用 $.easyui.parseMenuItems 方法解析后，menu-item 属性的回调函数签名将变更为 function (e, itemOpts, target, a, b, c)；
    //    //          也就是说，参数 args 也将一个数组值作为 menu-item 属性回调函数的调用参数附加（而不是替换，因为原来的参数会保留）至函数的调用中；
    //    // 返回值：返回一个 Array 数组；该数组可用于作为 $.easyui.showMenu 方法调用的参数；
    //    parseMenuItems: function (menuItems, args) { return parseMenuItems(menuItems, args); },

    //    // 隐藏/销毁 页面上所有的 easyui-menu 菜单项。
    //    hideAllMenu: function () { return hideAllMenu(); }


    //    // 另，本扩展增加 easyui-menu 控件中 menu-item 的如下自定义扩展属性:
    //    //      hideOnClick:    Boolean 类型值，默认为 true；表示点击该菜单项后整个菜单是否会自动隐藏；
    //    //      bold:           Boolean 类型值，默认为 false；表示该菜单项是否字体加粗；
    //    //      style:          JSON-Object 类型值，默认为 null；表示要附加到该菜单项的样式；
    //    // 注意：自定义扩展属性 bold、style 仅在通过 $.easyui.createMenu 或者 $.easyui.showMenu 生成菜单时，才有效。

    //});


})(jQuery);

