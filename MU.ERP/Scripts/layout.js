/**
 * 设置语言类型： 默认为中文 
 */
var i18nLanguage = "zh_CN";

/* 
设置一下网站支持的语言种类 
zh-CN(中文简体)、en(英语)、zh_TW(台湾)
 */
var webLanguage = ['zh_CN', 'en', 'zh_TW'];

//获取网站语言 
function getWebLanguage() {
    //1.cookie是否存在 
    var lang = $.cookie("lang");
    if (lang)
        i18nLanguage = lang;
    else
        $.cookie("lang", i18nLanguage, { expires: 7 });
}
//国际化easyui中英文包 
function changeEasyuiLanguage(languageName) {
    // when login in China the language=zh-CN  
    var src = "/Content/easyui/locale/easyui-lang-" + languageName.replace('-', '_') + ".js";
    $.getScript(src);
};
/** 
 * 执行页面i18n方法 
 * @return 
 * @author dingxw 
 */
var execI18n = function () {
    //获取网站语言(i18nLanguage,默认为中文简体) 
    getWebLanguage();
    //国际化页面 
    jQuery.i18n.properties({
        name: i18nLanguage, //资源文件名称 
        path: "/Content/i18n/local/", //资源文件路径 
        mode: 'map', //用Map的方式使用资源文件中的值 
        language: i18nLanguage,
        cache: false, //指定浏览器是否对资源文件进行缓存,默认false 
        encoding: 'UTF-8', //加载资源文件时使用的编码。默认为 UTF-8。  
        callback: function () {//加载成功后设置显示内容 
            try {
                $('[data-i18n-placeholder]').each(function () {
                    $(this).attr('placeholder', $.i18n.prop($(this).data('i18n-placeholder')));
                });
                $('[data-i18n-text]').each(function () {
                    //如果text里面还有html需要过滤掉
                    var html = $(this).html();
                    var reg = /<(.*)>/;
                    if (reg.test(html)) {
                        var htmlValue = reg.exec(html)[0];
                        $(this).html(htmlValue + $.i18n.prop($(this).data('i18n-text')));
                    }
                    else {
                        $(this).text($.i18n.prop($(this).data('i18n-text')));
                    }
                });
                $('[data-i18n-value]').each(function () {
                    $(this).val($.i18n.prop($(this).data('i18n-value')));
                });
            } catch (ex) {}
        }
    });
}

/*页面执行加载执行*/
$(function () {
    execI18n();
    //国际化easyui 
    changeEasyuiLanguage(i18nLanguage);
});

var SelectLanguage = function (lang)
{
    $.cookie("lang", lang);
    location.reload();
}