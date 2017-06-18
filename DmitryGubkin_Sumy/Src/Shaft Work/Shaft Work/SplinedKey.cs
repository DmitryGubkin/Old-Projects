using System;
using System.Collections.Generic;
using System.Linq;
using SolidWorks.Interop.sldworks;

namespace Shaft_Work
{
    // Class for SplinedKey feature creating
    class SplinedKey :IStageFeature
    {
        private object splinedType;
        private object splinedMethod;
        private double splinedR;
        private double splinedLength;
        private double splinedArcL;
        private double splinedWidth;
        private int splinedArray;
        private double splinedChamfer;
        private double splinedFillet;


        public object SplinedType
        {
            get { return splinedType; }
            set { splinedType = value; }
        }

        public object SplinedMethod
        {
            get { return splinedMethod; }
            set { splinedMethod = value; }
        }

        public double SplinedR
        {
            get { return splinedR; }
            set { splinedR = value; }
        }

        public double SplinedLength
        {
            get { return splinedLength; }
            set { splinedLength = value; }
        }

        public double SplinedWidth
        {
            get { return splinedWidth; }
            set { splinedWidth = value; }
        }
        public double SplinedArcL
        {
            get { return splinedArcL; }
            set { splinedArcL = value; }
        }

        public int SplinedArray
        {
            get { return splinedArray; }
            set { splinedArray = value; }
        }

        public double SplinedChamfer
        {
            get { return splinedChamfer; }
            set { splinedChamfer = value; }
        }

        public double SplinedFillet
        {
            get { return splinedFillet; }
            set { splinedFillet = value; }
        }

        public SplinedKey()
        {
           splinedType = (object) "None";
           splinedMethod = (object) "From Start";
           splinedR = 8;
           splinedLength = 6;
           splinedArcL = 1.5;
           splinedWidth = 2;
           splinedArray = 1;
           splinedChamfer = 0.2;
           splinedFillet = 0.2;
    }

        ~SplinedKey()
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
            SketchManager sketchManager = SwModel.SketchManager;
            FeatureManager featureManager = SwModel.FeatureManager;
            SelectionMgr selectionMgr = SwModel.SelectionManager;
            Feature feature = default(Feature);
            Entity entity = default(Entity);
            Face2 face2 = default(Face2);
            Array faceArray = default(Array);
            Array edgeArray = default(Array);
            Edge swEdge = default(Edge);

            List<double> filletedges = new List<double>();
            double edgeL = default(double);

            double[] TNormal = {0, 1, 0}; // Y up normal
            double[] LNormal = {0, 0, 1}; // +Z normal
            double[] RNormal = {0, 0, -1}; // -Z normal

            CurveParamData curveParamData = default(CurveParamData);

            double LStage = P2.x - P1.x;

            #region // add features

            helper.select_feature("RefPlane", 0, false, 0);
            SwModel.InsertSketch();

            if (splinedMethod == (object) "From Start")
            {
                sketchManager.CreateLine(P1.x, P1.y, 0, (P1.x + splinedLength), P1.y, 0); // top line
                sketchManager.CreateLine(P1.x, splinedR, 0, ((P1.x + splinedLength) - splinedArcL), splinedR, 0);
                    // bottom line
                sketchManager.CreateLine(P1.x, P1.y, 0, P1.x, splinedR, 0); //left line
                sketchManager.CreateTangentArc((P1.x + (splinedLength - splinedArcL)), splinedR, 0,
                    (P1.x + splinedLength),
                    P1.y, 0, 1);
            }

            if (splinedMethod == (object) "From End")
            {
                sketchManager.CreateLine(P2.x, P1.y, 0, (P2.x - splinedLength), P2.y, 0); // top line
                sketchManager.CreateLine(P2.x, splinedR, 0, (P2.x - (splinedLength - splinedArcL)), splinedR, 0);
                    // bottom line
                sketchManager.CreateLine(P2.x, P1.y, 0, P2.x, splinedR, 0); //left line
                sketchManager.CreateTangentArc((P2.x - (splinedLength - splinedArcL)), splinedR, 0,
                    (P2.x - splinedLength),
                    P1.y, 0, 0);
            }

            if (splinedMethod == (object) "Through")
            {
                sketchManager.CreateCenterRectangle((P1.x + (LStage/2)), (splinedR + (P1.y - splinedR)/2), 0,
                    P1.x, P1.y, 0);
            }


            SwModel.FeatureManager.FeatureCut3(true, false, false, 6, 0, splinedWidth, 0,
                false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true,
                false, 0, 0, false); //cut from mide plane

            #endregion

            feature = (Feature) selectionMgr.GetSelectedObject6(1, -1);
            faceArray = feature.GetFaces() as Array;

            #region // edge filleting, 0 - pass

            if (splinedFillet != 0)

            {
                for (int j = 0; j < faceArray.Length; j++) // get face by normal 0,1,0
                {
                    face2 = faceArray.GetValue(j) as Face2;
                    var normal = face2.Normal;
                    if (TNormal.SequenceEqual(normal as double[]))
                    {
                        entity = faceArray.GetValue(j) as Entity;
                        entity.Select4(false, null);
                        //                                MessageBox.Show(j.ToString());
                        break;
                    }
                }

                face2 = selectionMgr.GetSelectedObject6(1, -1);
                edgeArray = face2.GetEdges() as Array;
                //                     MessageBox.Show(""+face2.GetEdgeCount().ToString());
                SwModel.ClearSelection();

                for (int j = 0; j < face2.GetEdgeCount(); j++) // finding of edge length
                {
                    swEdge = edgeArray.GetValue(j) as Edge;
                    curveParamData = swEdge.GetCurveParams3();
                    edgeL = Math.Abs(curveParamData.UMaxValue - curveParamData.UMinValue);
                    filletedges.Add(edgeL);
                }


                double max = filletedges.Max();
                for (int j = 0; j < filletedges.Count; j++) // select edges for filleting
                {
                    if (filletedges[j] == max)
                    {
                        entity = edgeArray.GetValue(j) as Entity;
                        entity.Select4(true, null);
                    }
                }

                featureManager.FeatureFillet(194, splinedFillet, 0, 0, null, null, null);
                    // edges fillet 194 - target propagate off
                SwModel.ClearSelection2(true);
            }

            #endregion

            #region // edge chamfering, 0 - pass

            if (splinedChamfer != 0)
            {
                
                List<Face2> facesList = new List<Face2>();
                facesList.Add(default(Face2)); // empty element
                facesList.Add(default(Face2)); // empty element

                for (int j = 0; j < faceArray.Length; j++) // select faces by normal
                {
                    face2 = faceArray.GetValue(j) as Face2;
                    var normal = face2.Normal;
                    if (LNormal.SequenceEqual(normal as double[]))
                    {
                        facesList[0] = face2;
                    }
                    if (RNormal.SequenceEqual(normal as double[]))
                    {
                        facesList[1] = face2;
                    }
                }

                faceArray = facesList.ToArray() as Array; // refill array of neded face's
                int index_f0 = 0; // index of top edge L
                int index_f1 = 0; // index of top edge R

                for (int j = 0; j < faceArray.Length; j++)
                {
                    face2 = faceArray.GetValue(j) as Face2;
                    edgeArray = face2.GetEdges() as Array;
                    swEdge = edgeArray.GetValue(0) as Edge;

                    CurveParamData cData = swEdge.GetCurveParams3() as CurveParamData;

                    var Spoint = (double[]) cData.StartPoint;
                    var Epoint = (double[]) cData.EndPoint;


                    for (int k = 0; k < edgeArray.Length; k++)
                    {
                        swEdge = edgeArray.GetValue(k) as Edge;
                        cData = swEdge.GetCurveParams3() as CurveParamData;
                        var SP = (double[]) cData.StartPoint;
                        var EP = (double[]) cData.EndPoint;

                        if ((SP[1] >= Spoint[1]) && (EP[1] >= Epoint[1])) // check max y cordinats of the edge
                        {
                            if (Spoint[1] == Spoint[1])// horisontal line always have same y cordinats
                            {
                                Epoint = EP;
                                Spoint = SP;
                                if (j == 0)
                                {
                                    index_f0 = k;
                                }
                                else
                                {
                                    index_f1 = k;
                                }
                            }
                        }
                    }

                }
                

                // finally selecy top up edges
                face2 = faceArray.GetValue(0) as Face2;
                edgeArray = face2.GetEdges() as Array;
                entity = edgeArray.GetValue(index_f0) as Entity;
                entity.Select4(false, null);


                face2 = faceArray.GetValue(1) as Face2;
                edgeArray = face2.GetEdges() as Array;
                entity = edgeArray.GetValue(index_f1) as Entity;
                entity.Select4(true, null);

                int chamferOpt = 4; // target propagate by default

                if (splinedMethod == (object) "From Start" ||
                    splinedMethod == (object) "From End")
                {
                    chamferOpt = 1; // flip direction
                }
                featureManager.InsertFeatureChamfer(chamferOpt, 1, splinedChamfer, helper.ToRad(45), 0, 0, 0, 0);
                SwModel.ClearSelection2(true);
            }

            #endregion

            #region //Pattern if value less then 1 pass

            if (splinedArray > 1)
            {
                helper.select_feature("Cut", (helper.get_features_count("Cut") - 1), false, 4);
                helper.select_feature("RefAxis", 0, true, 1);

                if (splinedChamfer > 0)
                {
                    helper.select_feature("Chamfer", (helper.get_features_count("Chamfer") - 1), true, 4);
                }

                if (splinedFillet > 0)
                {
                    helper.select_feature("Fillet", (helper.get_features_count("Fillet") - 1), true, 4);
                }

                featureManager.FeatureCircularPattern4(splinedArray, helper.ToRad(360), false, "null", false, true,
                    false);
            }

            #endregion
            helper.HidePlanes();
        }

        public bool CheckUsing()
        {
            return splinedType != (object)"None";
        }

        public void GetParams(MainWindow window)
        {
            window._SplinedType = splinedType;
            window._SplinedMethod = splinedMethod;
            window._SplinedLength = splinedLength;
            window._SplinedR = splinedR;
            window._SplinedWidth = splinedWidth;
            window._SplindeArcL = splinedArcL;
            window._SplinedChamfer = splinedChamfer;
            window._SplinedFillet = splinedFillet;
            window._SplinedArray = splinedArray;
        }

        public void SetParams(object SplinedType, object SplinedMethod, double SplinedR, double SplinedLength,
            double SplinedWidth, double SplinedArcLength, double SplinedChamfer, double SplinedFillet, int SplinedArray)
        {
            splinedType = SplinedType;
            splinedMethod = SplinedMethod;
            splinedR = SplinedR;
            splinedLength = SplinedLength;
            splinedWidth = SplinedWidth;
            splinedArcL = SplinedArcLength;
            splinedChamfer = SplinedChamfer;
            splinedFillet = SplinedFillet;
            splinedArray = SplinedArray;

        }
    }
}
