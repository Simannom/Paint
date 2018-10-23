using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace algocs_paint
{
    public partial class ViewCtrl : UserControl
    {
        static Brush brushBack = new SolidBrush(Color.White);
        static Brush brushPoint = new SolidBrush(Color.Black);
        static Pen penVertical = new Pen(Color.Red);
        static Pen penHorizontal = new Pen(Color.Green);

        //для заполнения прямоугольника 
        static Brush brushFill = new SolidBrush(Color.Aquamarine);
        //для выделения точек внутри прямоугольника
        static Brush InBrushPoint = new SolidBrush(Color.Blue);
        
        public ViewCtrl()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private float minX = 0, maxX = 1;
        private float minY = 0, maxY = 1;
        private int MARGIN_X = 8, MARGIN_Y = 8;

        private float[] val;
        public void SetVal(float[] val)
        {
            this.val = val;
            Invalidate();
        }

        private PointF[] points;
        public void SetPoints(PointF[] points)
        {
            this.points = points;
            Invalidate();
        }

        // Tree2D на основе массива точек
        private Tree2D tree = new Tree2D();
        private string path = @"..\tmp\Sequence.txt";
        public void SetTree(PointF[] points) {
            tree = new Tree2D();
            //пишем последовательность проходимых вершин в файл
            File.Create(path);
            for (int i = 0; i < points.Length; ++i) {
                tree.insert(points[i], i);
            }
            Invalidate();
        }
        
        public String GetTree() {
            String tr = tree.toString();
            if (val!= null)
                for (int i = 0; i < val.Length; ++i) {
                    tr += "   \n" + val[i];
                }
            Invalidate();
            return tr;
        }

        int[] in_Rect; // = tree.range(new PointF(val[0], val[1]), new PointF(val[0]+val[2], val[1]+val[2]));
        public String GetV()
        {
            String RPoints = "";
            in_Rect = tree.range(new PointF(val[0], val[1]), new PointF(val[0] + val[2], val[1] + val[3]));
            if (in_Rect != null)
            {
                Array.Sort(in_Rect);
                for (int i = 0; i < in_Rect.Length; ++i)
                {
                    RPoints += in_Rect[i] + "  ";
                }
            }
            return RPoints;
        }

        private int numLines;
        public void SetNumLines(int n)
        {
            numLines = n;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            var rect = ClientRectangle;
            g.FillRectangle(brushBack, rect);

            //отрисовка прямоугольника
            //выбирался рандомно
            var sRect = new RectangleF(0, 0, 0, 0); ;
            if (val != null)
            {
                int x = (int)Math.Round((rect.Width - 2 * MARGIN_X) * (val[0] - minX) / (maxX - minX));
                int y = (int)Math.Round((rect.Height - 2 * MARGIN_Y) * (val[1] - minY) / (maxY - minY));
                int width = (int)Math.Round((rect.Width - 2 * MARGIN_X) * (val[2] - minX) / (maxX - minX));
                int height = (int)Math.Round((rect.Height - 2 * MARGIN_Y) * (val[3] - minY) / (maxY - minY));

                sRect = new RectangleF(x, y, width, height);
                g.FillRectangle(brushFill, sRect);
            }

            
            int i = numLines;
            if (points != null)
            {
                for (i = 0; i < points.Length; i++)
                {
                    int x = (int)Math.Round((rect.Width - 2 * MARGIN_X) * (points[i].X - minX) / (maxX - minX));
                    int y = (int)Math.Round((rect.Height - 2 * MARGIN_Y) * (points[i].Y - minY) / (maxY - minY));
                    //float x = (rect.Width - 2 * MARGIN_X) * (points[i].X - minX) / (maxX - minX);
                    //float y = (rect.Height - 2 * MARGIN_Y) * (points[i].Y - minY) / (maxY - minY);

                    //сканируем последовательность из файла и рисуем линеечки
                    //нихуя. без файла кажется лучше
                    if (i < numLines)
                    {
                        if (i % 2 == 0)
                            g.DrawLine(penVertical, x, 0, x, rect.Height);
                        else
                            g.DrawLine(penHorizontal, 0, y, rect.Width, y);
                    }
                    
                    var rectP = new RectangleF(x - 3, y - 3, 6, 6);
                    g.FillEllipse(brushPoint, rectP);
                }
            }
            //делаем по нему поиск
            //in_Rect = tree.range(new PointF(sRect.Left, sRect.Top), new PointF(sRect.Right, sRect.Bottom));
            //Array.Sort(in_Rect);

            //рисуем точечки по-другому
            if (in_Rect != null )
            {
                for (int l = 0; l < in_Rect.Length; l++)
                {
                    int j = in_Rect[l];
                    int x = (int)Math.Round((rect.Width - 2 * MARGIN_X) * (points[j].X - minX) / (maxX - minX));
                    int y = (int)Math.Round((rect.Height - 2 * MARGIN_Y) * (points[j].Y - minY) / (maxY - minY));
                    var rectP = new RectangleF(x - 5, y - 5, 10, 10);
                    //if (i < numLines)
                        g.FillEllipse(InBrushPoint, rectP);
                }
            }

            

        }
    }
}
