var viewModel = function () {
    var self = this;
    this.form = {
        usercode: ko.observable(),
        password: ko.observable(),
        remember: ko.observable(false)
    };
    this.message = ko.observable();
    this.loginClick = function (form) {
        //if (!self.form.password()) self.form.password($('[type=password]').val());
        self.message("正在登录，请稍后...");
        $.post("Check", self.form, function (result) {
            alert(result);
            self.message("登录成功");
            //location.href = '/';
        });
    };
};
    
ko.applyBindings(viewModel);