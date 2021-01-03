using AntDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Client.Admin.Extensions
{
    public static class ConfirmServiceExtension
    {
        public async static Task<ConfirmResult> YesNo(this ConfirmService confirmService, string title, string content, ConfirmIcon confirmIcon = ConfirmIcon.Info, string btn1Text = "确定", string btn2Text = "取消", string btn3Text = "")
        {
            return await confirmService.Show(content, title, ConfirmButtons.YesNo, confirmIcon, new ConfirmButtonOptions()
            {
                Button1Props = new ButtonProps
                {
                    ChildContent = btn1Text
                },
                Button2Props = new ButtonProps
                {
                    ChildContent = btn2Text
                }
            });
        }

        public async static Task<ConfirmResult> YesNoDelete(this ConfirmService confirmService, string title, string content)
        {
            return await confirmService.YesNo(title, content, ConfirmIcon.Question, "确定", "取消");
        }

        public async static Task<ConfirmResult> YesNoDelete(this ConfirmService confirmService)
        {
            return await confirmService.YesNoDelete("删除", "确定要执行删除吗？");
        }
    }
}
