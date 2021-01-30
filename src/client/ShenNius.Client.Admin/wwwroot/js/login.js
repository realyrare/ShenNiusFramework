
layui.config({
    base: '/js/modules/'
});
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
    os.ajax('user/LoadLoginInfo', "", "application/json", "get", function (res) {
        if (res.success == true && res.statusCode === 200) {
            if (res.data.rsaKey[0] != null && res.data.rsaKey[0] != "") {
                console.log("rsaKey:" + res.data.rsaKey[0]);
                $("#privateKey").val(res.data.rsaKey[0]);
            }
            if (res.data.number != null && res.data.number != "") {
                console.log("numberguid:" + res.data.number);
                $("#numberguid").val(res.data.number);
            }
            return;
        } else {
            alert(res.message);
            return;
        }
    });
    // 登录过期的时候，跳出ifram框架
    if (top.location != self.location) top.location = self.location;

    // 进行登录操作
    form.on('submit(login)', function (data) {
        //if (data.captcha == '') {
        //    layer.msg('验证码不能为空');
        //    return false;
        //}
        var crypt = new JSEncrypt();
        crypt.setPrivateKey(data.field.privateKey);
        var enc = crypt.encrypt(data.field.password);
        $("#password").val(enc);
        data.field.password = enc;
        //console.log(data.field);
        os.ajax('user/signin', data.field, "application/json", "post", function (res) {
            if (res.statusCode == 200 && res.success == true) {
                console.log("token:" + res.data.token);
                os.SetSession('admin_ACCESS_TOKEN', res.data.token);
                setTimeout(function () {
                    var rurl = os.getUrlParam('ReturnUrl');
                    if (!rurl) {
                        layer.msg('登录成功', function () {
                            window.location.href = '/index';
                        });
                    }
                    else {
                        window.location.href = rurl;
                    }
                }, 1000);
            } else {
                console.log(res.msg);
                layer.msg(res.msg);
            }
        });
        return false;
    });
});