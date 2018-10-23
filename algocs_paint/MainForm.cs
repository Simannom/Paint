using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace algocs_paint
{
    public partial class MainForm : Form
    {
        ViewCtrl viewCtrl;

        public MainForm()
        {
            InitializeComponent();

            viewCtrl = new ViewCtrl();
            viewCtrl.Dock = DockStyle.Fill;
            viewPanel.Controls.Add(viewCtrl);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //случайный прямоугольник
            Random r = new Random();
            float[] val = new float[4];
            for (int i = 0; i < val.Length; i++)
            {
                val[i] = (float)r.NextDouble();
            }
            viewCtrl.SetVal(val);
            
            
            PointF[] points = new PointF[20];
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X = (float)r.NextDouble();
                points[i].Y = (float)r.NextDouble();
            }
            viewCtrl.SetPoints(points);
            viewCtrl.SetTree(points);

            MessageBox.Show(this, viewCtrl.GetTree() , "Message");
            MessageBox.Show(this, viewCtrl.GetV(), "Message");

            numLines = 0;
            timer.Start();

        }

        private int numLines;
        private void timer_Tick(object sender, EventArgs e)
        {
            numLines++;
            viewCtrl.SetNumLines(numLines);
        }
    }
}
