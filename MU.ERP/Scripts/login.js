window.onload = function () {
    var app = new Vue({
        el: '#app',
        data: {
            user: {
                name: 'admin',
                password: '123456',
                times: 1,
            },
            msg: '',
        },
        methods: {
            Submit: function () {
                event.preventDefault();
                msg = "登录中，请稍后...";
                if (this.user.times < 10) {
                    //$.post();
                }
                if (this.user.name == "admin" && this.user.password == "123456") {
                    this.msg = '登录成功';
                }
                else {
                    localStorage.setItem("times", this.user.times++);
                    this.msg = "登录失败";
                }
            }
        }
    });
};