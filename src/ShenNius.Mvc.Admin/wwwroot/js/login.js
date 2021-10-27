function changeSrcCode() {
    $("#captchaPic").attr("src", $("#captchaPic").attr("src") + 1);// 取得img属性 得到src地址给它+1 是为了每次变换验证码
};
layui.use(['jquery', 'form', 'common'], function () {
    var form = layui.form,
        $ = layui.jquery,
        os = layui.common,
    layer = layui.layer;

    function login(data) {
        os.ajax('user/mvcLogin', data, "application/json", "post", function (res) {
            if (res.statusCode == 200) {
                if (res.success == false) {
                    if (res.msg.indexOf("已经登录") != -1) {
                        layer.confirm(res.msg, /*{ icon: 3, title: '提示' },*/ {
                            btn: ['继续登录', '取消']
                        }, function (index) {
                            data.confirmLogin = true;
                            layer.close(index);
                            //此处请求后台程序，下方是成功后的前台处理……
                            var index = layer.load(0, { shade: [0.7, '#393D49'] }, { shadeClose: true }); //0代表加载的风格，支持0-2
                            login(data);
                        }, function (index) {
                            //取消事件
                            $("#btnlogin").text("立即登录");
                            $("#btnlogin").attr('disabled', false);
                            layer.close(index);
                        });
                    }
                } else {
                    if (res.data.menuAuthOutputs == null || res.data.menuAuthOutputs.length <= 0) {
                        os.error("不好意思，该用户当前没有权限。请联系系统管理员分配权限！");
                        return;
                    }
                     //如果当前用户已经登录过，在新的token生成之前则把它当前的token缓存移除掉                   
                   os.SetSession('globalCurrentUserInfo', res.data);                                                                        
                    setTimeout(function () {
                        os.success("恭喜您，登录成功");
                        var rurl = os.getUrlParam('returnUrl');
                        if (!rurl) {
                            window.location.href = '/home/index';
                        }
                        else {
                            window.location.href = "/home/index#" + rurl;
                        }
                    }, 500);
                }
            } else {
                $("#btnlogin").text("立即登录");
                $("#btnlogin").attr('disabled', false);
                os.error(res.msg);
            }
        });
    }
    // 登录过期的时候，跳出ifram框架
    if (top.location != self.location) top.location = self.location;

    // 进行登录操作
    form.on('submit(login)', function (data) {
        if (data.field.captcha == '') {
            layer.msg('验证码不能为空');
            return false;
        }
        var crypt = new JSEncrypt();
        crypt.setPrivateKey(data.field.privateKey);
        var enc = crypt.encrypt(data.field.password);
        // $("#password").val(enc);
        data.field.password = enc;
        //console.log("password:" + data.field.password)
        data.field.confirmLogin = false;
        $("#btnlogin").text("正在登录中...");
        $("#btnlogin").attr('disabled', 'disabled');
        login(data.field);
        return false;
    });
    $(window).resize(
        bodysize
    );
    bodysize();
    function bodysize() {
        $("body").height($(window).height());
    }
});