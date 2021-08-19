
/*绑定分类*/
function BindParentCategory(value) {
    apiUtil.ajax('category/getAllParentcategory', {}, "application/json", "get", function (res) {
        if (res.statusCode == 200 && res.success == true) {
            if (res.data !== null) {
                var select = $("#categoryId");
                for (var i = 0; i < res.data.length; i++) {
                    var val = res.data[i].id;
                    if (val === value)
                        select.append("<option value='" + val + "' selected>" + res.data[i].name + "</option>");
                    else
                        select.append("<option value='" + val + "'>" + res.data[i].name + "</option>");
                }
                layui.form.render('select');
            }
        }
    });
}

/*绑定栏目*/
function BindParentColumn(value) {
    apiUtil.ajax('column/getAllParentColumn', {}, "application/json", "get", function (res) {
        if (res.statusCode == 200 && res.success == true) {
            if (res.data !== null) {
                var select = $("#parentId");
                for (var i = 0; i < res.data.length; i++) {
                    var val = res.data[i].id;
                    if (val === value)
                        select.append("<option value='" + val + "' selected>" + res.data[i].title + "</option>"); 
                    else
                        select.append("<option value='" + val + "'>" + res.data[i].title + "</option>"); 
                }
                layui.form.render('select');
            }
        }
    });
}
/*绑定菜单*/
function BindParentMenu(value) {
    apiUtil.ajax('menu/getAllParentMenu', {}, "application/json", "get", function (res) {
        if (res.statusCode == 200 && res.success == true) {
            if (res.data !== null) {
                var select = $("#parentId");
                for (var i = 0; i < res.data.length; i++) {
                    var val = res.data[i].id;
                    if (val === value)
                        select.append("<option value='" + val + "' selected>" + res.data[i].name + "</option>"); 
                    else
                        select.append("<option value='" + val + "'>" + res.data[i].name + "</option>"); 
                }
                layui.form.render('select');
            }
        }
    });
}




