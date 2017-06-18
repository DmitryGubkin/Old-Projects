using System;
using SolidWorks.Interop.sldworks;

//Class for KeyProfile creating
namespace Shaft_Work
{
    class KeyProfile : IStageFeature
    {
        private object keyProfileType ;
        private double keyProfileWidth;

        public object KeyType
        {
            get { return keyProfileType; }
            set { keyProfileType = value; }
        }

        public double KeyWidth
        {
            get { return keyProfileWidth; }
            set { keyProfileWidth = value; }
        }



        public KeyProfile()
        {
            keyProfileType = (object) "None";
            keyProfileWidth = 10;
        }

        ~KeyProfile()
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

            double L = P2.x - P1.x; // stage length
            SketchManager sketchManager = SwModel.SketchManager;
            FeatureManager featureManager = SwModel.FeatureManager;
            Helper helper = new Helper(SwModel, SwApp);


            helper.select_feature("RefPlane", 2, false, 0);
            if (P1.x > 0)
                // if x cordinat of first point is 0 we use stantart plane, else - make new refplane by offset
            {
                SwModel.CreatePlaneAtOffset(P1.x, false);
            }
          

            sketchManager.InsertSketch(true);

            #region //Squre

            if (keyProfileType == (object) "Squre") // make squre stage for key (out)
            {
                sketchManager.CreateCenterRectangle(0, 0, 0, (keyProfileWidth/2),
                    (keyProfileWidth/2), 0);

                featureManager.FeatureCut3(true, true, true, 0, 0, L, 0,
                    false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true,
                    false, 0, 0, false); // add cut feature
            }

            #endregion

            #region //Special

            if (keyProfileType == (object) "Speacial") // make special stage for key (out)
            {
                double H = Math.Sqrt((Math.Pow((2*P1.y), 2) - Math.Pow(keyProfileWidth, 2)));

                sketchManager.Create3PointArc((keyProfileWidth/2), (H/2), 0,
                    (-1*keyProfileWidth/2), (H/2), 0, 0, P1.y, 0); // Top arc

                sketchManager.Create3PointArc((keyProfileWidth/2), (-1*H/2), 0, (-1*keyProfileWidth/2),
                    (-1*H/2), 0, 0, (-1*P1.y), 0); // Bottom arc

                sketchManager.CreateLine((keyProfileWidth/2), (H/2), 0, (keyProfileWidth/2),
                    (-1*H/2), 0); // Left line

                sketchManager.CreateLine((-1*keyProfileWidth/2), (H/2), 0, (-1*keyProfileWidth/2),
                    (-1*H/2), 0); // Rigth line

                featureManager.FeatureCut3(true, true, true, 0, 0, L, 0,
                    false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true,
                    false, 0, 0, false); // add cut feature
                sketchManager.InsertSketch(false);
            }

            #endregion

            #region // Hexagon

            if (keyProfileType == (object) "Hexagon") // make squre stage for key (out)
            {
                sketchManager.CreatePolygon(0, 0, 0, 0, P1.y, 0, 6, false);

                featureManager.FeatureCut3(true, true, true, 0, 0, L, 0,
                    false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true,
                    false, 0, 0, false); // add cut feature

            }

            #endregion

            helper.HidePlanes();
        }

        public bool CheckUsing()
        {
            return keyProfileType != (object)"None";
        }

        public void GetParams(MainWindow window)
        {
            window._KeyType = keyProfileType;
            window._KeyWidth = keyProfileWidth;
        }

        public void SetParams(object KeyType, double KeyWidth)
        {
            keyProfileType = KeyType;
            keyProfileWidth = KeyWidth;
        }
    }
}
