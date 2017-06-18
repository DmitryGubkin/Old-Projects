using System;
using SolidWorks.Interop.sldworks;

namespace Shaft_Work
{
    // Class for theard feature creating
    class Theard :IStageFeature
    {


        private object theardUse;
        private object theardType;
        private bool theardCW;
        private bool theardFit;
        private double theardR;
        private double theardPinch;

        public object TheardUse
        {
            get { return theardUse; }
            set { theardType = value; }
        }

        public object TheardType
        {
            get { return theardType; }
            set { theardType = value; }
        }

        public bool TheardFit
        {
            get { return theardFit; }
            set { theardFit = value; }
        }

        public bool TheardCW
        {
            get { return theardCW; }
            set { theardCW = value; }
        }

        public double TheardR
        {
            get { return theardR; }
            set { theardR = value; }
        }

        public double TheardPinch
        {
            get { return theardPinch; }
            set { theardPinch = value; }
        }

  

        public Theard()
        {
            theardUse = (object) "None";
            theardType = (object)"Triangle";
            theardCW = false;
            theardFit = false;
            theardPinch = 0.75;
            theardR = 8.5;
        }

        ~Theard()
        {
            GC.Collect();
        }

        public void AddFeature(SldWorks SwApp, ModelDoc2 SwModel, Point2D P1, Point2D P2)
        {
            if (CheckUsing() == false)
            {
                return;
            }
            SwModel.ClearSelection2(true);
            Helper helper = new Helper(SwModel, SwApp);
            SelectionMgr selectionMgr = SwModel.SelectionManager;
            SketchManager sketchManager = SwModel.SketchManager;
            FeatureManager featureManager = SwModel.FeatureManager;
            const double Aprox = 0.0005; // needs to make correct theard
            Sketch swSketch = default(Sketch);

            double L = P2.x - P1.x;

            helper.select_feature("RefPlane", 2, false, 0);

            if (P1.x > 0)
            {
                SwModel.CreatePlaneAtOffset(P1.x, false);
            }


            sketchManager.InsertSketch(true);

            // create helix
            sketchManager.CreateCircleByRadius(0, 0, 0, P1.y);
            SwModel.InsertHelix(false, theardCW, false, true, 2, L, theardPinch, 0, 0, 0);
            SwModel.ClearSelection2(true);


            #region //Triangle profile

            if (theardType == (object) "Triangle")
            {
                double XL = (P1.y - theardR)/2; // haf of triangle base
                double YL = XL*(double) Math.Sqrt(3); // median of triangle

                helper.select_feature("RefPlane", 1, false, 0);
                sketchManager.InsertSketch(true);
                sketchManager.CreateLine((P1.x - XL + Aprox), P1.y, 0, (P1.x + XL - Aprox), P1.y, 0);
                sketchManager.CreateLine((P1.x - XL + Aprox), P1.y, 0, P1.x, (P1.y - YL), 0);
                sketchManager.CreateLine((P1.x + XL - Aprox), P1.y, 0, P1.x, (P1.y - YL), 0);
            }

            #endregion

            #region //Trapeze profile

            if (theardType == (object) "Trapeze")
            {
                helper.select_feature("RefPlane", 1, false, 0);
                sketchManager.InsertSketch(true);
                double H = (P1.y - theardR)/2; // haf of trapaze top base and heigth
                double ctgA = 1/Math.Tan(helper.ToRad(105));// ctg value of bottom base angle                                  
                double A = 2*(H - Aprox) + 4*H*ctgA;

                sketchManager.CreateLine((P1.x - H/2 + Aprox), P1.y, 0, (P1.x + H/2 - Aprox), P1.y, 0);
                sketchManager.CreateLine((P1.x - H/2 + Aprox), P1.y, 0, (P1.x - (A/4)), P1.y - H, 0);
                sketchManager.CreateLine((P1.x + H/2 - Aprox), P1.y, 0, (P1.x + (A/4)), P1.y - H, 0);
                sketchManager.CreateLine((P1.x - (A/4)), P1.y - H, 0, (P1.x + (A/4)), P1.y - H, 0);
            }

            #endregion

            swSketch = SwModel.GetActiveSketch2() as Sketch;
            SwModel.EditRebuild3(); // model rebuild needs to make cutsweep feature avaible
            
            Entity ent = swSketch as Entity;
            ent.Select2(false, 1); // select sketch using mark 1 for sweep cut
            helper.select_feature("Helix", helper.get_features_count("Helix") - 1, true, 4);

            featureManager.InsertCutSwept4(false, true, 0, false, false, 0, 0, false, 0, 0, 0, 0, true, true, 0,
                true, true, true, false);

            helper.HidePlanes();
        }

        public bool CheckUsing()
        {
            return theardUse != (object)"None";
        }

        public void GetParams(MainWindow window)
        {
            window._TheardUse = theardUse;
            window._TheardType = theardType;
            window._TheardCW = theardCW;
            window._TheaedFit = theardFit;
            window._TheardPinch = theardPinch;
            window._TheardR = theardR;
        }

        public void SetParams(object TheardUse, object TheardType, double TheardR, double TheardPinch, bool TheardCW, bool TheardFit)
        {
            theardUse = TheardUse;
            theardType = TheardType;
            theardCW = TheardCW;
            theardFit = TheardFit;
            theardPinch = TheardPinch;
            theardR = TheardR;
        }
    }
}
