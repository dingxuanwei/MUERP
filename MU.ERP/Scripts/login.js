window.onload = function () {
    var app = new Vue({
        el: '#app',
        data: {
            msg: "正在测试链接..."
        },
        methods: {
            setMsg: function (m) {
                this.msg = m;
            }
        }
    });

    var url = "/Login/ConnectionTest";
    var xhr = new XMLHttpRequest();
    xhr.open('GET', url, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200 || xhr.status == 304) {
            app.setMsg(xhr.responseText);
        }
    };
    xhr.send();
};