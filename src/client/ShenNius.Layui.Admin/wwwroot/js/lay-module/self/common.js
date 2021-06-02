layui.define(['layer', 'table', 'toastr'], function (exports) {
    "use strict";

    var $ = layui.jquery,
        layer = layui.layer,
        toastr = layui.toastr,
        table = layui.table;
    toastr.options = {
        //toast-top-center  中间
        "positionClass": "toast-top-center",
        "timeOut": "1500"
    };
    var tmls, tool = {
        error: function (msg) {
            toastr.error(msg);
        },
        warning: function (msg) {
            toastr.warning(msg);
        },
        success: function (msg) {
            toastr.success(msg);
        },
        apiUrl() {
            return "https://localhost:44377/api/";
        },
        ajax: function (url, options, contentType = "application/json", method = 'post', callFun = null) {
            var token = this.getToken();
            options = method === 'get' ? options : JSON.stringify(options);
            var type = contentType != "application/json" ? "application/x-www-form-urlencoded" : contentType;
            //console.log(options);
            $.ajax(tool.apiUrl() + url, {
                data: options,
                contentType: type,
                dataType: 'json', //服务器返回json格式数据
                type: method, //HTTP请求类型
                //timeout: 10 * 2000, //超时时间设置为50秒；
                headers: {
                    'Authorization': 'Bearer ' + token
                },
                success: function (data) {
                    callFun(data);
                },
                error: function (e) {
                    //返回500错误 或者其他 http状态码错误时 需要在error 回调函数中处理了 并且返回的数据还不能直接alert，需要使用
                    //$.parseJSON 进行转译    res.msg 是自己组装的错误信息通用变量 
                    var res = JSON.parse(e.responseText);
                    console.log("erro object:" + e.responseText);
                    if (res.statusCode == 401) {
                        toastr.warning(res.msg);
                        setTimeout(function () {
                            window.location.href = "/sys/login";
                        }, 500)
                        return;
                    }
                    if (res.statusCode == 500) {
                        // tool.error(data.msg);
                        toastr.error(res.msg);
                        return;
                    }
                    if (res.statusCode == 400) {
                        toastr.error(res.msg);
                        return;
                    }
                    this.error('连接异常，请稍后重试！');
                    return;
                }
            });
        },
        render: function (obj) {
            var token = this.getToken();
            obj.headers = {
                'Authorization': 'Bearer ' + token,
                'cache-control': 'no-cache',
                'Pragma': 'no-cache'
            };
            obj.url = this.apiUrl() + obj.url;
            if (obj.limits == null || obj.limits == "") {
                obj.limits = [10, 15, 20, 25, 50, 100];
            }
            if (obj.limit == null || obj.limit == "") {
                obj.limit = 15;
            }
            if (obj.page == null || obj.page == "" || obj.page == undefined) {
                obj.page = true;
            }           
            obj.skin = 'line';
            table.render(obj);
        },
        parseDataFun: function (res) { //res 即为原始返回的数据         
            if (res.statusCode == 401) {
                layer.msg(res.msg);
                setTimeout(function () {
                    window.location.hash = "/";
                    // window.location.href = "/sys/login";
                }, 1000);
                return;
            }
            if (res.statusCode == 500) {
                layer.msg(res.msg);
                return;
            }
            return {
                "code": res.statusCode == 200 ? 0 : -1, //解析接口状态,必须是0 才可以）
                "msg": res.msg, //解析提示文本
                "count": res.data.count, //解析数据长度
                "data": res.data.items //解析数据列表
            };
        },
        //前台权限判断
        checkAuth: function (module, btnName, event) {
              //event 阻止事件冒泡；
            try {
                var obj = tool.GetSession('globalCurrentUserInfo');
                var menuAuths = obj.menuAuthOutputs;
                if (menuAuths.length <= 0 || menuAuths == null) {
                    toastr.error("不好意思，该用户目前没有分配权限，请联系系统管理员！");
                    return false;
                }
                var model = menuAuths.find(d => d.nameCode.trim() == module.trim());
                if (model == null || model == "" || model == undefined) {
                    toastr.error("不好意思，您没有该列表的操作权限");
                    event.stopPropagation();
                    return false;
                }
                if (model.btnCodeName != null && model.btnCodeName!="") {
                    var btnsArry = model.btnCodeName.split(',');
                    if (btnsArry != null && btnsArry.length > 0) {
                        var btnModel = btnsArry.find(d => d.trim() == btnName.trim());
                        if (btnModel == null || btnModel == undefined || btnModel == "") {
                            toastr.error("不好意思，您没有操作按钮权限");
                            event.stopPropagation();
                            return false;
                        }
                    }
                }                
            } catch (e) {
                toastr.error("不好意思，网络错误！" + e);
                event.stopPropagation();
                return false;
            }
        },
        getToken: function () {
            var obj = tool.GetSession('globalCurrentUserInfo');
            return obj.token;
        },
        getCurrentUser: function () {
            var currentUser = tool.GetSession('globalCurrentUserInfo');
            return currentUser;
        },
      
        tableLoading: function () {
            tmls = layer.msg('<i class="layui-icon layui-icon-loading layui-icon layui-anim layui-anim-rotate layui-anim-loop"></i> 正在加载...', { time: 20000 });
        },
        tableLoadingClose: function () {
            setTimeout(function () {
                layer.close(tmls);
            }, 500);
        },
        load: function () {
            $('body').append('<div class="loader-cur-wall"><div class="loader-cur"></div></div>');
        },
        loadClose: function () {
            setTimeout(function () {
                $('.loader-cur-wall').remove();
            }, 100);
        },
        getUrlParam: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        },
        formatdate: function (str) {
            if (str) {
                var d = eval('new ' + str.substr(1, str.length - 2));
                var ar_date = [
                    d.getFullYear(), d.getMonth() + 1, d.getDate()
                ];
                for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
                return ar_date.slice(0, 3).join('-') + ' ' + ar_date.slice(3).join(':');

                function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
            } else {
                return "无信息";
            }
        },
        SetSession: function (key, options) {
            localStorage.setItem(key, JSON.stringify(options));
        },
        GetSession: function (key) {
            try {
                var obj = localStorage.getItem(key);
                if (obj == "" || obj == null || obj == undefined) {
                    layer.msg("token信息丢失,即将跳入登陆页面...");
                    setTimeout(function () {
                        window.location.href = "/sys/login";
                    }, 500)
                    return;
                }
                // console.log("jsonobj:" + JSON.parse(obj));
                return JSON.parse(obj);
            } catch (e) {
                console.log("获取session错误原因:" + e);
            }
        },
        /**
         * 删除键值对json
         * @param {key} key : 键
         */
        SessionRemove: function (key) {
            localStorage.removeItem(key);
        },
        isExtImage: function (name) {
            var imgExt = new Array(".png", ".jpg", ".jpeg", ".bmp", ".gif");
            name = name.toLowerCase();
            var i = name.lastIndexOf(".");
            var ext;
            if (i > -1) {
                ext = name.substring(i);
            }
            for (var i = 0; i < imgExt.length; i++) {
                if (imgExt[i] === ext)
                    return true;
            }
            return false;
        }
    };
    exports('common', tool);
});

