

//绑定文章父栏目  
//isSelect：是否查询子栏目（false：否、true：是）
function BindParentArticleColumn(value, childId, isSelect) {
    //加载文章父栏目下拉框
    $.ajax({
        type: "Get",
        url: "/Article/GetParentColumnList",
        dataType: "json",
        data: {},
        success: function (data) {
            var relData = data.data;
            if (relData != null) {
                var select = $("#selectArticleColumn");
                for (var i = 0; i < relData.length; i++) {
                    var val = relData[i].ArticleColumnId;
                    if (val == value)
                        select.append("<option value='" + val + "' selected>" + relData[i].ColumnName + "</option>");  //添加option
                    else
                        select.append("<option value='" + val + "'>" + relData[i].ColumnName + "</option>");  //添加option
                }
                layui.form.render('select');

                if (isSelect)
                    BindChildArticleColumn(value, childId);
            }
        }
    });
}

//绑定文章子栏目
function BindChildArticleColumn(parentId, value) {
    $("#selectChildArticleColumn").empty();  //初始清空select中的option
    //加载文章子栏目下拉框
    $.ajax({
        type: "Get",
        url: "/Article/GetChildArticleColumnList",
        dataType: "json",
        data: { parentId: parentId },
        success: function (data) {
            var relData = data.data;
            var select = $("#selectChildArticleColumn");
            select.append('<option value="0">请选择子栏目</option>');

            if (relData != null) {
                for (var i = 0; i < relData.length; i++) {
                    var val = relData[i].ArticleColumnId;
                    if (val == value)
                        select.append("<option value='" + val + "' selected>" + relData[i].ColumnName + "</option>");  //添加option
                    else
                        select.append("<option value='" + val + "'>" + relData[i].ColumnName + "</option>");  //添加option
                }
            }

            layui.form.render('select');
        }
    });
}
//绑定分类
function BindCategory(value) {
    //加载绑定分类下拉框
    $.ajax({
        type: "Get",
        url: "/Category/GetParentCategoryList",
        dataType: "json",
        data: {},
        success: function (data) {
            var relData = data.data;
            if (relData != null) {
                var select = $("#selectCategory");
                for (var i = 0; i < relData.length; i++) {
                    var val = relData[i].CategoryId;                    
                   if (val == value) {
                        select.append("<option value='" + val + "' selected>" + relData[i].CategoryName + "</option>");  //添加option
                    }                    
                    else {
                select.append("<option value='" + val + "'>" + relData[i].CategoryName + "</option>");  //添加option
                 }
                        
                }
            }
            layui.form.render('select');
        }
    });
}
function BindCanyinCategory(value) {
    //加载绑定分类下拉框
    $.ajax({
        type: "Get",
        url: "/Category/GetParentCategoryList",
        dataType: "json",
        data: {},
        success: function (data) {
            var relData = data.data;
            if (relData != null) {
                var select = $("#selectCategory");
                for (var i = 0; i < relData.length; i++) {
                    var val = relData[i].CategoryId;
                    if (val == value) {
                        select.append("<option value='" + val + "' selected >" + relData[i].CategoryName + "</option>");  //添加option
                    }                    
                }
            }
            layui.form.render('select');
        }
    });
}
//绑定二级分类
function BindChildCategory(bigCategoryId, smallCategoryId) {
    $("#selectChildCategory").empty();  //初始清空select中的option
    //绑定二级分类下拉框
    $.ajax({
        type: "Get",
        url: "/Category/GetChildCategoryList",
        dataType: "json",
        data: { bigCategoryId: bigCategoryId },
        success: function (data) {
            var relData = data.data;
            var select = $("#selectChildCategory");
            select.append('<option value="">请选择二级分类</option>');
            if (relData != null) {
                for (var i = 0; i < relData.length; i++) {
                    var val = relData[i].CategoryId;
                    if (smallCategoryId == val)
                        select.append("<option value='" + val + "' selected>" + relData[i].CategoryName + "</option>");  //添加option
                    else
                        select.append("<option value='" + val + "'>" + relData[i].CategoryName + "</option>");  //添加option
                }
            }
            layui.form.render('select');
        }
    });
}

//月度统计图表：绑定分类 select下拉框value=id
function BindMonthCategory(value) {
    //加载绑定分类下拉框
    $.ajax({
        type: "Get",
        url: "/Category/GetParentCategoryList",
        dataType: "json",
        data: {},
        success: function (data) {
            var relData = data.data;
            if (relData != null) {
                var select = $("#monthselectCategory");
                for (var i = 0; i < relData.length; i++) {
                    var val = relData[i].CategoryId;
                    if (val == value)
                        select.append("<option value='" + val + "' selected>" + relData[i].CategoryName + "</option>");  //添加option
                    else
                        select.append("<option value='" + val + "'>" + relData[i].CategoryName + "</option>");  //添加option
                }
            }
            layui.form.render('select');
        }
    });
}
//月度统计图表：绑定二级分类
function BindMonthChildCategory(bigCategoryId, smallCategoryId) {
    $("#monthselectChildCategory").empty();  //初始清空select中的option
    //绑定二级分类下拉框
    $.ajax({
        type: "Get",
        url: "/Category/GetChildCategoryList",
        dataType: "json",
        data: { bigCategoryId: bigCategoryId },
        success: function (data) {
            var relData = data.data;
            var select = $("#monthselectChildCategory");
            select.append('<option value="">请选择二级分类</option>');
            if (relData != null) {
                for (var i = 0; i < relData.length; i++) {
                    var val = relData[i].CategoryId;
                    if (smallCategoryId == val)
                        select.append("<option value='" + val + "' selected>" + relData[i].CategoryName + "</option>");  //添加option
                    else
                        select.append("<option value='" + val + "'>" + relData[i].CategoryName + "</option>");  //添加option
                }
            }
            layui.form.render('select');
        }
    });
}



