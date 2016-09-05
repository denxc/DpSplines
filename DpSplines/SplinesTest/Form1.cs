using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DpSplines;

namespace SplinesTest {
    public partial class Form1 : Form {

        private List<Vector2> points = new List<Vector2>();

        public Form1() {
            InitializeComponent();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                points.Add(new Vector2(e.X, e.Y));
            } else if (e.Button == MouseButtons.Right) {
                points.Clear();
            }

            pictureBox1.Invalidate();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e) {
            nCountLabel.Text = trackBar1.Value + "";
            pictureBox1.Invalidate();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e) {
            rLabel.Text = trackBar2.Value + "";
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var point in points) {
                graphics.DrawEllipse(Pens.Red, (float)point.X - 3, (float)point.Y - 3, 6, 6);
            }

            if (points.Count > 2) {
                var vectors = new Vector2[points.Count];
                for (var i = 0; i < points.Count; ++i) {
                    vectors[i] = new Vector2(points[i].X, points[i].Y);
                }

                var n = trackBar1.Value;
                var r = trackBar2.Value;
                var includeSourcePoints = checkBox1.Checked;

                var spline = DpSpline.Calculate(vectors, r, n, includeSourcePoints);

                var splinePoints = new PointF[spline.Length + 1];
                for (var i = 0; i < spline.Length; ++i) {
                    splinePoints[i] = new PointF((float)spline[i].X, (float)spline[i].Y);
                }
                splinePoints[splinePoints.Length - 1] = splinePoints[0];

                for (var i = 0; i < splinePoints.Length; ++i) {
                    graphics.DrawEllipse(Pens.Blue, new RectangleF(splinePoints[i].X - 1, splinePoints[i].Y - 1, 2, 2));
                }

                graphics.DrawLines(Pens.Blue, splinePoints);
            }            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            pictureBox1.Invalidate();
        }
    }
}
