using System;

namespace Shaft_Work
{
    // Class for Conical stage
    class ConicStage : IStage
    {
        public double R1
        {
            get { return r1; }
            set { r1 = value; }
        }

        public double R2
        {
            get { return r2; }
            set { r2 = value; }
        }

        public double Length
        {
            get { return length; }
            set { length = value; }
        }

        public object SOperation
        {
            get { return sOperation; }
            set { sOperation = value; }
        }

        public double SValue
        {
            get { return sValue; }
            set { sValue = value; }
        }

        public object EOperation
        {
            get { return eOperation; }
            set { eOperation = value; }
        }

        public double EValue
        {
            get { return eValue; }
            set { eValue = value; }
        }

        public Point2D P1
        {
            get { return p1; }
            set { p1 = value; }
        }

        public Point2D P2
        {
            get { return p2; }
            set { p2 = value; }
        }



        public void GetParams(MainWindow window)
        {
            window._R1 = r1;
            window._R2 = r2;
            window._Length = length;
            window._Soperation = sOperation;
            window._Svalue = sValue;
            window._Eoperation = eOperation;
            window._Evalue = eValue;
            window._Profile = (object) "Conic";
        }

        public void SetParams(double R1, double R2, double Length, object SOperation, double SValue, object EOperation,
            double EValue)
        {
            r1 = R1;
            r2 = R2;
            length = Length;
            sOperation = SOperation;
            eOperation = EOperation;
            sValue = SValue;
            sValue = EValue;
            p1 = new Point2D(0, R1);
            p1 = new Point2D(length, R2);
        }


        private double r1;
        private double r2;
        private double length;
        private object sOperation;
        private double sValue;
        private object eOperation;
        private double eValue;
        private Point2D p1;
        private Point2D p2;


        public ConicStage()
        {
            r1 = 10;
            r2 = 10;
            length = 10;
            sOperation = (object) "None";
            sValue = 1;
            eOperation = (object) "None";
            eValue = 1;
            p1 = new Point2D(0, r1);
            p2 = new Point2D(length, r2);
        }


        ~ConicStage()
        {
            GC.Collect();
        }

        
    }
}
