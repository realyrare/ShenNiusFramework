



layui.config({
    base: '/js/modules/'
});
layui.use(['jquery', 'form', 'common'], function () {
    var form = layui.form,
        $ = layui.jquery,
        os = layui.common;
        layer = layui.layer;
    // 登录过期的时候，跳出ifram框架
    if (top.location != self.location) top.location = self.location;

    // 进行登录操作
    form.on('submit(login)', function (data) {
        data = data.field;
        if (data.username == '') {
            layer.msg('用户名不能为空');
            return false;
        }
        if (data.password == '') {
            layer.msg('密码不能为空');
            return false;
        }
        //if (data.captcha == '') {
        //    layer.msg('验证码不能为空');
        //    return false;
        //}
        $.ajax({
            url: 'https://localhost:5001/api/user/signin',
            type: 'POST',
            contentType: 'application/json',
            data: data.field,
            success: function (res) {
                if (res.statusCode === 200 && res.success == true) {
                    console.log(res.data);
                    os.SetSession('admin_ACCESS_TOKEN', res.data);
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
                    layer.msg(res.msg);
                    return false;
                }
            }
        });

    });
});