using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Point = System.Drawing.Point;


// Special class for drawing Stage preview image

namespace Shaft_Work
{
    class Shape
    {
        public float D1;
        public float D2;
        public float L;
        public Point CenterPoint;
        public Point LTPoint;
        public Pen MLine;
        public Pen SLine;
        public float Yscale;
        public float Xscale;
        public  List<Point> ShapePoints = new List<Point>(); 
     
        public Pen MLine1
        {
            get { return MLine; }
            set { MLine = value; }
        }

        public Pen SLine1
        {
            get { return SLine; }
            set { SLine = value; }
        }

        public MainWindow MainWindow
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public Shape(float Xscale, float Yscale,float D1, float D2, float L, Point CenterPoint, Point LTPoint, Pen MLine, Pen Sline)
        {
            this.Xscale = Yscale;
            this.Yscale = Yscale;
            this.D1 = D1*Yscale;
            this.D2 = D2*Yscale;
            this.CenterPoint = CenterPoint;
            this.L = L*Xscale;
            this.LTPoint = LTPoint;
            this.MLine = MLine;
            this.SLine = Sline;
         

        }
        public Shape()
        {
            Xscale = default(float);
            Yscale = default(float);
            D1 = default(float);
            D2 = default(float);
            L = default(float);
            CenterPoint = default(Point);
            LTPoint = default(Point);
            MLine = default(Pen);
            SLine = default(Pen);
          

        }

      ~Shape()
        {
            GC.Collect();
            GC.SuppressFinalize(typeof(Shape));
        }

        public Bitmap DrawShape (int Width, int Heigth, object SO, object EO, int SValue, int EValue)
        {
            if (ShapePoints.Count > 0)
            {
                ShapePoints.Clear();
            }
    
            Bitmap ShapeBMP_half1 = new Bitmap(Width, Heigth, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(ShapeBMP_half1);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Point p1 = LTPoint;
            p1.Y = CenterPoint.Y;
            Point p2 = p1;
            p2.Y = LTPoint.Y;

            g.DrawLine(MLine, p1, p2);
            for (int i = 1; i < 3; i++)
            {
                if (i % 2 == 0)
                {
                    p1 = p2;
                    p2.Y = p1.Y + (int)Math.Ceiling(D2 / 2);

                }
                else
                {
                    p1 = p2;
                    p2.Y = CenterPoint.Y - (int)Math.Ceiling(D2 / 2);
                    p2.X = p1.X + (int)L;
                }
                g.DrawLine(MLine, p1, p2);
            }

            // image mirroring
            Bitmap ShapeBMP_half2 = ShapeBMP_half1.Clone(new Rectangle(0, 0, ShapeBMP_half1.Width, ShapeBMP_half1.Height),
                PixelFormat.Format32bppArgb);
            ShapeBMP_half2.RotateFlip(RotateFlipType.Rotate180FlipX);
            g.DrawImage(ShapeBMP_half2, new Point(0,0));
           
            ShapeBMP_half2.Dispose();


            return  ShapeBMP_half1;
        }



    }
}
