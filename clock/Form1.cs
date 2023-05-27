using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace mod_lab09_clock {

    public partial class Form1 : Form {
        DoubleBufferedPanel panel1 = new DoubleBufferedPanel();

        public class DoubleBufferedPanel : Panel {
            public DoubleBufferedPanel() {
                DoubleBuffered = true;
            }
        }

        public Form1() {
            InitializeComponent();

            Font boldFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            panel1.Font = boldFont;

            timer1.Interval = 20;
            timer1.Start();
            
            panel1.Width = 400;
            panel1.Height = 400;
            panel1.Location = new Point(12, 12);
            panel1.Paint += new PaintEventHandler(panel2_Paint);

            Controls.Add(panel1);
        }

        private void panel2_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            

            int[] times = new int[3];
            times[0] = DateTime.Now.Hour % 12;
            times[1] = DateTime.Now.Minute;
            times[2] = DateTime.Now.Second;

            Color[] colors = new Color[3];
            colors[0] = Color.RoyalBlue;
            colors[1] = Color.DarkGreen;
            colors[2] = Color.IndianRed;

            int center = panel1.Width / 2;
            int clockRadius = center - 10;

            Pen pen = new Pen(Color.Black, 3);
            g.DrawEllipse(pen, center - clockRadius, center - clockRadius, panel1.Width - 20, panel1.Height - 20);

            for (int i = 1; i <= 12; i++) {
                double alfa = i * 30 * Math.PI / 180;
                int x = center - 5 + (int)(0.90 * clockRadius * Math.Sin(alfa));
                int y = center - 8 - (int)(0.90 * clockRadius * Math.Cos(alfa));
                g.DrawString(i.ToString(), panel1.Font, Brushes.Black, x, y);
            }

            for (int i = 0; i < 3; i++) {
                double angle = times[i] * Math.PI / 180;
                angle *= i == 0 ? 30 : 6;
                int x = center + (int)((0.4 + i * 0.15) * clockRadius * Math.Sin(angle));
                int y = center - (int)((0.4 + i * 0.15) * clockRadius * Math.Cos(angle));

                int x1 = center + (int)(0.03 * clockRadius * Math.Sin(angle + Math.PI/2));
                int y1 = center - (int)(0.03 * clockRadius * Math.Cos(angle + Math.PI/2));
                int x2 = center - (int)(0.03 * clockRadius * Math.Sin(angle + Math.PI/2));
                int y2 = center + (int)(0.03 * clockRadius * Math.Cos(angle + Math.PI/2));

                GraphicsPath path = new GraphicsPath();
                Point[] triangle = new Point[] { new Point(x1, y1), new Point(x2, y2), new Point(x, y) };
                path.AddPolygon(triangle);
                
                Pen pen2 = new Pen(colors[i], 3);

                g.DrawPath(pen2, path);
                g.FillPolygon(new SolidBrush(colors[i]), triangle);
            }

            g.FillEllipse(new SolidBrush(Color.Red), center - 20 / 2, center - 20 / 2, 20, 20);

            for (int i = 0; i < 60; i++) {
                double angle = i * Math.PI / 30;

                int x = center + (int)(clockRadius * Math.Sin(angle));
                int y = center - (int)(clockRadius * Math.Cos(angle));
                double startPoint = i % 5 == 0 ? 0.95 : 0.97;
                int x1 = center + (int)(startPoint * clockRadius * Math.Sin(angle));
                int y1 = center - (int)(startPoint * clockRadius * Math.Cos(angle));

                g.DrawLine(pen, x1, y1, x, y);
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            panel1.Invalidate();
        }
    }
}
