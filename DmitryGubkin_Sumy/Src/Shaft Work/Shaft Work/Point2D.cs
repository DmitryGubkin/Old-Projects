using System;
//Special custom class for storing information like an point(x,y)
namespace Shaft_Work
{
    class Point2D 
    {
        public Point2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Point2D()
        {
            x = 0;
            y = 0;
        }

        ~Point2D()
        {
            GC.Collect();
        }



        public double x { get; set; }
        public double y { get; set; }
    }
}
