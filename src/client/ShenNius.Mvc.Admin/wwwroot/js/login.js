function changeSrcCode() {
    $("#captchaPic").attr("src", $("#captchaPic").attr("src") + 1);// 取得img属性 得到src地址给它+1 是为了每次变换验证码
};

layui.use(['jquery', 'form', 'common'], function () {
    var form = layui.form,
        $ = layui.jquery,
        os = layui.common;
    layer = layui.layer;

    $(document).ready(function () {
        $('.layui-container').particleground({
            dotColor: '#7ec7fd',
            lineColor: '#7ec7fd'
        });
    });

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

        $("#btnlogin").text("正在登陆中...");
        $("#btnlogin").attr('disabled', 'disabled');
        $.ajax({
            url: "/api/user/mvcLogin",
            type: "post",
            contentType: "application/json",
            data: data.field,
            success: function (res) {
                console.log("resmsg:" + res.msg);
                if (res.statusCode == 200 && res.success == true) {
                    if (res.data.menuAuthOutputs == null || res.data.menuAuthOutputs.length <= 0) {
                        os.error("不好意思，该用户当前没有权限。请联系系统管理员分配权限！");
                        return;
                    }
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
                } else {
                    $("#btnlogin").text("立即登录");
                    $("#btnlogin").attr('disabled', false);
                    os.error(res.msg);
                }
            },
            error: function (e) {
                var res = $.parseJSON(e.responseText);
                console.log("erro object:" + e.responseText);
                os.error(res.msg);
                return;
            },
        });
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