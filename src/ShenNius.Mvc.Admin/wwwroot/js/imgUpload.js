layui.use('upload', function () {
    var $ = layui.jquery,
    upload = layui.upload;
    upload.render({
        elem: "#btnUploadShowImg"
        , url: '/api/goods/MultipleUploadImg'
        , multiple: true,
        accept: "images",
        exts: 'gif|jpg|jpeg|png|bmp|png',
        size: 1024 * 1.05 //限制文件大小，单位 KB
        ,method:"post"
        , before: function (obj) {
            obj.preview(function (index, file, result) {
            });
        },
        done: function (res) {
            if (res.success == true) {
                document.getElementById('demo2').innerHTML += "<div class='div_img' ><a href=" + res.data + " target='_blank'><img src=" + res.data + " class='layui-upload-img' /></a><span  class='delete-img'>删除</span><input type='hidden'  value=" + res.data + " name='imgUrl' id='imgUrl' />                                                  </div>";
                $('.delete-img').each(function (index) {
                    var _this = this;
                    $(this).click(function () {
                        $(_this).parent('div').remove();
                    });
                })
                return;
            } else {
                //layer.msg(res.msg);
                alert(res.msg);
                return;
            }
        }
    });
});