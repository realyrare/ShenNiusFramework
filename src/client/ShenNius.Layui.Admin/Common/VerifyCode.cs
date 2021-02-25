using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
namespace ShenNius.Client.Admin.Common
{
    public class VerifyCode
    {
        /// <summary>  
        /// 生成随机码  
        /// </summary>  
        /// <param  name="length">随机码个数</param>  
        /// <returns></returns>  
        public static string CreateRandomCode(int length)
        {
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }

        /// <summary>  
        /// 创建随机码图片  
        /// </summary>  
        /// <param  name="vcode">验证码</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="background">背景颜色</param>
        /// <param name="border">边框颜色</param>
        /// <returns>Gif图片二进制流</returns>
        public static byte[] DrawImage(string vcode, float fontSize = 14, Color background = default(Color), Color border = default(Color))
        {
            // 随机转动角度 
            const int RandAngle = 45;

            // var height = (int) (fontSize + 4);
            var width = vcode.Length * (int)fontSize;

            // 创建图片背景 
            using (var map = new Bitmap(width + 3, (int)fontSize + 10))
            {
                using (var graphics = Graphics.FromImage(map))
                {
                    graphics.Clear(background); // 清除画面，填充背景  
                    graphics.DrawRectangle(new Pen(border, 0), 0, 0, map.Width - 1, map.Height - 1); // 画一个边框  
                    // graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//模式 
                    var random = new Random();

                    // 背景噪点生成
                    var blackPen = new Pen(Color.DarkGray, 0);
                    for (var i = 0; i < 50; i++)
                    {
                        int x = random.Next(0, map.Width);
                        int y = random.Next(0, map.Height);
                        graphics.DrawRectangle(blackPen, x, y, 1, 1);
                    }

                    // 验证码旋转，防止机器识别    
                    var chars = vcode.ToCharArray(); // 拆散字符串成单字符数组  
                    // 文字距中  
                    var format = new StringFormat(StringFormatFlags.NoClip)
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    // 定义颜色  
                    Color[] colors = { Color.Black, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple, Color.DarkGoldenrod };

                    FontStyle[] styles = { FontStyle.Bold, FontStyle.Italic, FontStyle.Regular, /*FontStyle.Strikeout,*/ FontStyle.Underline };

                    // 定义字体  
                    string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
                    foreach (char item in chars)
                    {
                        int cindex = random.Next(8);
                        int findex = random.Next(5);
                        int sindex = random.Next(4);
                        var font = new Font(fonts[findex], fontSize, styles[sindex]); // 字体样式(参数2为字体大小)  
                        Brush b = new SolidBrush(colors[cindex]);
                        var dot = new Point(16, 16);

                        // graph.DrawString(dot.X.ToString(),fontstyle,new SolidBrush(Color.Black),10,150);//测试X坐标显示间距的  
                        float angle = random.Next(-RandAngle, RandAngle); // 转动的度数  
                        graphics.TranslateTransform(dot.X, dot.Y); // 移动光标到指定位置  
                        graphics.RotateTransform(angle);
                        graphics.DrawString(item.ToString(CultureInfo.InvariantCulture), font, b, 1, 1, format);

                        // graph.DrawString(chars.ToString(),fontstyle,new SolidBrush(Color.Blue),1,1,format);  
                        graphics.RotateTransform(-angle); // 转回去  
                        graphics.TranslateTransform(2, -dot.Y); // 移动光标到指定位置  
                    }
                }
                // graph.DrawString(randomcode,fontstyle,new SolidBrush(Color.Blue),2,2); //标准随机码  
                // 生成图片  
                var stream = new MemoryStream();
                map.Save(stream, ImageFormat.Gif);

                // 输出图片流
                return stream.ToArray();
            }
        }
    }
}
