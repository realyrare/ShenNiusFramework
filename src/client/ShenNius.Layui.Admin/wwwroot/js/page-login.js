function changeSrcCode() {

    $("#captchaPic").attr("src", $("#captchaPic").attr("src") + 1);// 取得img属性 得到src地址给它+1 是为了每次变换验证码
};
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
            url: "/sys/login?handler=submit",
            type: "post",
            contentType: "application/x-www-form-urlencoded",
            data: data.field,
            success: function (res) {
                console.log("resmsg:" + res.msg);
                if (res.statusCode == 200 && res.success == true) {
                    //console.log("token:" + res.data.token);
                    //console.log("data:" + res.data);
                    os.SetSession('globalCurrentUserInfo', res.data);
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
                    $("#btnlogin").text("登陆");
                    $("#btnlogin").attr('disabled', false);
                    layer.msg(res.msg);                  
                }
            }
        });
        return false;
    });
});