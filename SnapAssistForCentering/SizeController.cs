using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapAssistForCentering
{
    class SizeController
    {
        public Form form;
        public System.Windows.Forms.Timer timer;

        private Rectangle area;
        private Size target;
        private Size delta;
        private int countdown;

        public SizeController(Form form)
        {
            this.form = form;
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new System.EventHandler(Animate);
            timer.Interval = 10;
            target = form.Size;
            delta = new Size();
            area = new Rectangle();
        }

        public void To(Rectangle area, Size size, int milliseconds)
        {
            this.area = area;
            delta = (size - form.Size) * timer.Interval / milliseconds;
            target = size;
            countdown = milliseconds;
            timer.Start();
        }

        private void Animate(object? sender, EventArgs e)
        {
            Console.WriteLine(delta);
            form.Size += delta;
            form.Location = LeftTop(Centering(area, form.Size));
            countdown -= 1;
            if (countdown <= 0)
            {
                form.Size = target;
                countdown = 0;
                timer.Stop();
            }
        }

        public static Rectangle Centering(Rectangle area, Size size)
        {
            return new Rectangle(
                area.X + area.Width / 2 - size.Width / 2,
                area.Y + area.Height / 2 - size.Height / 2,
                size.Width, size.Height);
        }

        public static Point LeftTop(Rectangle rectangle)
        {
            return new Point(rectangle.Left, rectangle.Top);
        }

        public static Size RectangleSize(Rectangle rectangle)
        {
            return new Size(rectangle.Width, rectangle.Height);
        }
    }
}
