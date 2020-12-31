using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extend
{
    public static class ColorExtned
    {
        /// <summary>
        /// 获取一个随机颜色
        /// </summary>
        /// <returns></returns>
        public static Color GetRandomColor()
        {
            // 一次设置透明度，红绿蓝
            return Color.FromArgb(GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber());
        }

        /// <summary>
        /// 获取颜色的半反色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color Inversion(this Color color)
        {
            return Color.FromArgb(color.A
                , (BitConverter.ToInt32(new byte[] { color.R }, 0) + 127) % 255
                , (BitConverter.ToInt32(new byte[] { color.R }, 0) + 127) % 255
                , (BitConverter.ToInt32(new byte[] { color.R }, 0) + 127) % 255
           );
        }


        private static  int GetRandomNumber()
        {
            Random random = new Random(0);

            return random.Next(255);
        }
    }
}
