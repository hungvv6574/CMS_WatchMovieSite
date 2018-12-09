using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace CMSSolutions.Websites.Extensions
{
    public class CaptchaImage
    {
        public string Text
        {
            get { return text; }
        }

        public Bitmap Image
        {
            get { return image; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public static int MaxLength { get; set; }

        private string text;
        private int width;
        private int height;
        private Bitmap image;
        private Random random = new Random();

        public CaptchaImage(string capchaText, int width = 110, int height = 40)
        {
            text = capchaText;
            SetDimensions(width, height);
            GenerateImage();
            MaxLength = 4;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                image.Dispose();
        }

        private void SetDimensions(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException("width", width,
                    "Chiều rộng phải lớn hơn 0.");
            }
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException("height", height,
                    "Chiều cao phải lớn hơn 0.");
            }

            this.width = width;
            this.height = height;
        }

        private void GenerateImage()
        {
            var bitmap = new Bitmap
              (width, this.height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, this.width, this.height);
            var hatchBrush = new HatchBrush(HatchStyle.SmallConfetti,
                Color.LightGray, Color.White);
            g.FillRectangle(hatchBrush, rect);
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;

            do
            {
                fontSize--;
                font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Italic);
                size = g.MeasureString(text, font);
            } while (size.Width > rect.Width);

            var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            var path = new GraphicsPath();

            //path.AddString(this.text, font.FontFamily, (int) font.Style, 
            //    font.Size, rect, format);

            path.AddString(this.text, font.FontFamily, (int)font.Style, 35, rect, format);
            float v = 3F;
            PointF[] points =
            {
                new PointF(random.Next(rect.Width)/v, random.Next(
                    rect.Height)/v),
                new PointF(rect.Width - random.Next(rect.Width)/v,
                           random.Next(rect.Height)/v),
                new PointF(random.Next(rect.Width)/v,
                           rect.Height - random.Next(rect.Height)/v),
                new PointF(rect.Width - random.Next(rect.Width)/v,
                           rect.Height - random.Next(rect.Height)/v)
            };

            var matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
            hatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.SkyBlue);
            g.FillPath(hatchBrush, path);

            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = random.Next(m / 50);
                int h = random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();
            image = bitmap;
        }

        public string GetCapchaImage()
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return "data:image/png;base64," + base64String;
            }
        }

        public static string GenerateRandomCode()
        {
            var r = new Random();
            string s = "";
            for (int j = 0; j < MaxLength; j++)
            {
                int i = r.Next(3);
                int ch;
                switch (i)
                {
                    case 1:
                        ch = r.Next(0, 9);
                        s = s + ch.ToString();
                        break;
                    case 2:
                        ch = r.Next(65, 90);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    case 3:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    default:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                }

                r.NextDouble();
                r.Next(100, 1999);
            }

            return s;
        }
    }
}
