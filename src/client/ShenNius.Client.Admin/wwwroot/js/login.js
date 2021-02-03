
layui.config({
    base: '/js/lay-module/self/'
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
    $.ajax({
        url: os.apiUrl()+"user/load-login-info",
        cache: false,
        type: "get",
        contentType: "application/json",
        data: { },
        dataType: "json",
        success: function (res) {
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
        $.ajax({
            url: os.apiUrl() + "user/sign-in",
            type: "post",
            contentType: "application/json",
            data: data.field,
            dataType: "json",
            success: function (res) {
                alert(res.msg);
                if (res.statusCode == 200 && res.success == true) {
                    console.log("token:" + res.data.token);
                    console.log("data:" + res.data);
                    os.SetSession('globalCurrentUserInfo', res.data);
                    // os.SetSession('globalCurrentUserId', res.data.id);
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
            }
        });
        //os.ajax('user/sign-in', data.field, "application/json", "post", function (res) {
        //    if (res.statusCode == 200 && res.success == true) {
        //        console.log("token:" + res.data.token);
        //        console.log("data:" + res.data);
        //        os.SetSession('globalCurrentUserInfo', res.data);
        //       // os.SetSession('globalCurrentUserId', res.data.id);
        //        setTimeout(function () {
        //            var rurl = os.getUrlParam('ReturnUrl');
        //            if (!rurl) {
        //                layer.msg('登录成功', function () {
        //                    window.location.href = '/index';
        //                });
        //            }
        //            else {
        //                window.location.href = rurl;
        //            }
        //        }, 1000);
        //    } else {
        //        console.log(res.msg);
        //        layer.msg(res.msg);
        //    }
        //});
        return false;
    });
});