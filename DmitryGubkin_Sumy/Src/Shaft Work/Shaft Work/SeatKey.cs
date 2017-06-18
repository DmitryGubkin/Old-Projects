using System;
using System.Collections.Generic;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

//Class for SeatKey creating
namespace Shaft_Work
{
    class SeatKey :IStageFeature
    {
        public SeatKey()
        {
            seatingType = (object) "None";
            seatingLength = 1;
            seatingDepth = 1;
            seatingWidth = 1;
            seatingOffset = 0;
            seatingChamferFilletValue = 0.01;
            seatingOpearationType = (object) "None";
            sKey00 = true;
            sKey01 = false;
            sKey02 = false;
            sKey03 = false;
        }

        ~SeatKey()
        {
            GC.Collect();
        }

        public object SeatingType
        {
            get { return seatingType; }
            set { seatingType = value; }
        }

        public double SeatingLength
        {
            get { return seatingLength; }
            set { seatingLength = value; }
        }

        public double SeatingDepth
        {
            get { return seatingDepth; }
            set { seatingDepth = value; }
        }

        public double SeatingWidth
        {
            get { return seatingWidth; }
            set { seatingWidth = value; }
        }

        public double SeatingOffset
        {
            get { return seatingOffset; }
            set { seatingOffset = value; }
        }

        public double SeatingChamferFilletValue
        {
            get { return seatingChamferFilletValue; }
            set { seatingChamferFilletValue = value; }
        }

        public object SeatingOpearationType
        {
            get { return seatingOpearationType; }
            set { seatingOpearationType = value; }
        }


        public bool SKey01
        {
            get { return sKey01; }
            set { sKey01 = value; }
        }

        public bool SKey02
        {
            get { return sKey02; }
            set { sKey02 = value; }
        }

        public bool SKey03
        {
            get { return sKey03; }
            set { sKey03 = value; }
        }


        private object seatingType;
        private double seatingLength;
        private double seatingDepth;
        private double seatingWidth;
        private double seatingOffset;
        private double seatingChamferFilletValue;
        private object seatingOpearationType;
        private static bool sKey00; // allways true
        private bool sKey01;
        private bool sKey02;
        private bool sKey03;


        public void AddFeature(SldWorks SwApp, ModelDoc2 SwModel, Point2D P1, Point2D P2)
        {
            if (CheckUsing() == false)
            {
                return;
            }

            SwModel.ClearSelection2(true);

            object[] seating_types = new[] {"None", "Prismatic A", "Prismatic B", "Segmented"};
            object[] operations_types = new[] {"None", "Chamfer", "Fillet"}; // array of operation names
            Helper helper = new Helper(SwModel, SwApp);

            FeatureManager featureManager = SwModel.FeatureManager;
            SketchManager sketchManager = SwModel.SketchManager;
            SelectionMgr selectionMgr = (SelectionMgr) SwModel.SelectionManager;
            Entity ent = default(Entity);
            Array facesArray = default(Array);
            Array edgesArray = default(Array);
            Feature swFeature = default(Feature);
            Face2 face2 = default(Face2); // neds to get edges of selected face
            double LStage = 0;
            double x_center = 0;
            double y_center = 0;
            double L = 0;
            double W = 0;
            bool append = false;

            LStage = P2.x - P1.x; // length of current stage
            x_center = P1.x + (LStage/2) + seatingOffset; // center of current stage
            y_center = 0;
            L = x_center + seatingLength/2;
            W = seatingWidth/2;


            SwModel.ClearSelection2(true);

            #region// make prismatic A keyseat

            if (seatingType == seating_types[1]) //Prismatic A
            {
                helper.select_feature("RefPlane", 1, false, 0);
                SwModel.CreatePlaneAtOffset(P1.y, false);
                sketchManager.InsertSketch(true);

                sketchManager.CreateCenterRectangle(x_center, y_center, 0, L, W, 0);
                SwModel.FeatureManager.FeatureCut3(true, false, false, 0, 0, seatingDepth, 0,
                    false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true,
                    false, 0, 0, false); // add cut feature

                if (seatingOpearationType != operations_types[0]) // add chamfer or fillet, if none or value is 0 - pass
                {
                    swFeature = (Feature) selectionMgr.GetSelectedObject6(1, -1);
                    facesArray = (Array) swFeature.GetFaces();
                    ent = facesArray.GetValue(0) as Entity;
                    ent.Select4(false, null);

                    if (seatingOpearationType == operations_types[1])
                    {
                        featureManager.InsertFeatureChamfer(4, 1, seatingChamferFilletValue, helper.ToRad(45), 0, 0, 0,
                            0);
                    }
                    if (seatingOpearationType == operations_types[2])
                    {
                        SwModel.FeatureManager.FeatureFillet(195, seatingChamferFilletValue, 0,
                            0, null, null, null);
                    }

                    swFeature = selectionMgr.GetSelectedObject6(1, -1);
                    swFeature.Select2(false, 4); //create selection for pattern
                    append = true;
                }


                CreatePattern(helper, append, SwModel, swFeature, selectionMgr);
            }

            #endregion

            #region // make prismatic B keyseat

            if (seatingType == seating_types[2]) //Prismatic B
            {
                W = seatingWidth; //Width

                helper.select_feature("RefPlane", 1, false, 0);
                SwModel.CreatePlaneAtOffset(P1.y, false);
                sketchManager.InsertSketch(true);
                sketchManager.CreateSketchSlot((int) swSketchSlotCreationType_e.swSketchSlotCreationType_line,
                    (int) swSketchSlotLengthType_e.swSketchSlotLengthType_CenterCenter, W, L - seatingLength, 0, 0, L, 0,
                    0,
                    0, 0, 0, 1, false);
                SwModel.FeatureManager.FeatureCut3(true, false, false, 0, 0, seatingDepth, 0,
                    false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true,
                    false, 0, 0, false);

                if (seatingOpearationType != operations_types[0]) // add chamfer or fillet, if none or value is 0 - pass
                {
                    swFeature = (Feature) selectionMgr.GetSelectedObject6(1, -1);
                    facesArray = (Array) swFeature.GetFaces();
                    ent = facesArray.GetValue(0) as Entity;
                    ent.Select4(false, null);

                    if (seatingOpearationType == operations_types[1])
                    {
                        featureManager.InsertFeatureChamfer(4, 1,
                            seatingChamferFilletValue, helper.ToRad(45), 0, 0, 0, 0);
                    }
                    if (seatingOpearationType == operations_types[2])
                    {
                        SwModel.FeatureManager.FeatureFillet(195, seatingChamferFilletValue, 0,
                            0, null, null, null);
                    }


                    swFeature = selectionMgr.GetSelectedObject6(1, -1);
                    swFeature.Select2(false, 4); //create selection for pattern
                    append = true;
                }


                CreatePattern(helper, append, SwModel, swFeature, selectionMgr);
            }

            #endregion

            #region// make segmented seatkey

            if (seatingType == seating_types[3]) //Segmented
            {
                helper.select_feature("RefPlane", 0, false, 0);
                sketchManager.InsertSketch(true);
                sketchManager.Create3PointArc(L - seatingLength, P1.y, 0, L, P1.y, 0, x_center, (P1.y - seatingDepth), 0);
                sketchManager.CreateLine(L - seatingLength, P1.y, 0, L, P1.y, 0); //close sketsh
                SwModel.FeatureManager.FeatureCut3(true, false, false, 6, 0, (seatingWidth), 0,
                    false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true,
                    false, 0, 0, false); //cut from mide plane

                if (SeatingOpearationType != operations_types[0]) // add chamfer or fillet, if nono or value is 0 - pass
                {
                    // select edges for chamfering or filleting
                    swFeature = (Feature) selectionMgr.GetSelectedObject6(1, -1);
                    facesArray = (Array) swFeature.GetFaces();
                    SelectData selectData = selectionMgr.CreateSelectData();
                    ent = facesArray.GetValue(2) as Entity;
                    ent.Select4(false, selectData);
                    face2 = selectionMgr.GetSelectedObject6(1, -1);
                    edgesArray = (Array) face2.GetEdges();
                    ent = edgesArray.GetValue(0) as Entity;
                    ent.Select4(false, null);
                    ent = edgesArray.GetValue(2) as Entity;
                    ent.Select4(true, null);
                    // select edges for chamfering or filleting
                    //  MessageBox.Show("" + face2.GetEdgeCount());

                    if (seatingOpearationType == operations_types[1])
                    {
                        featureManager.InsertFeatureChamfer(4, 1, seatingChamferFilletValue, helper.ToRad(45), 0, 0, 0,
                            0);
                    }

                    if (seatingOpearationType == operations_types[2])
                    {
                        SwModel.FeatureManager.FeatureFillet(195, seatingChamferFilletValue, 0, 0, null, null, null);
                    }


                    swFeature = selectionMgr.GetSelectedObject6(1, -1);
                    swFeature.Select2(false, 4); //create selection for pattern
                    append = true;
                }

                CreatePattern(helper, append, SwModel, swFeature, selectionMgr);
            }

            #endregion

            helper.HidePlanes();
        }

        private void CreatePattern(Helper helper, bool append, ModelDoc2 SwModel, Feature swFeature,
            SelectionMgr selectionMgr)
        {
            int[] skip_array = set_key_array();

            if (skip_array[0] != -1) // check for circular array option
            {
                CircularPatternFeatureData swPattern;

                helper.select_feature("Cut", helper.get_features_count("Cut") - 1, append, 4);
                helper.select_feature("RefAxis", 0, true, 1);
                SwModel.FeatureManager.FeatureCircularPattern4(4, helper.ToRad(360), false, "null", false, true,
                    false);
                if (skip_array[0] != 4) // check for non skip option
                {
                    swFeature = (Feature) selectionMgr.GetSelectedObject6(1, -1);
                    swPattern = swFeature.GetDefinition() as CircularPatternFeatureData;
                    swPattern.AccessSelections(SwModel, null);
                    swPattern.SkippedItemArray = (Array) skip_array;
                    swFeature.ModifyDefinition(swPattern, SwModel, null);
                }
            }
        }

        public bool CheckUsing()
        {
            return seatingType != (object) "None";
        }

        public void GetParams(MainWindow window)
        {
            window._Seating_type = seatingType;
            window._SLength = seatingLength;
            window._SDepth = seatingDepth;
            window._SWidth = seatingWidth;
            window._SOffset = seatingOffset;
            window._Seating_operation = seatingOpearationType;
            window._Seating_CF_value = seatingChamferFilletValue;
            window._SetKey00 = true;
            window._SetKey01 = sKey01;
            window._SetKey02 = sKey02;
            window._SetKey03 = sKey03;
        }

        public void SetParams(object SeatingType, double SeatingLength, double SeatingWidth, double SeatingDepth,
            double SeatingOffset, object SeatingOperatinType, double SeatingOperationValue, bool Copy01, bool Copy02,
            bool Copy03)
        {
            seatingType = SeatingType;
            seatingLength = SeatingLength;
            seatingDepth = SeatingDepth;
            seatingWidth = SeatingWidth;
            seatingOffset = SeatingOffset;
            seatingOpearationType = SeatingOperatinType;
            seatingChamferFilletValue = SeatingOperationValue;
            sKey00 = true;
            sKey01 = Copy01;
            sKey02 = Copy02;
            sKey03 = Copy03;
        }

        private int[] set_key_array() // set array of skipped objects
        {
            // -1 dont do circular pattern
            // 4 make circular pattern dont skip any element
            // else skip elements by index

            if (sKey00 == true && sKey01 == false && sKey02 == false && sKey03 == false)
            {
                int[] false_arr = {-1};
                return false_arr;
            }

            else
            {
                List<int> skip_list = new List<int>();


                if (sKey01 == false)
                {
                    skip_list.Add(1);
                }
                if (sKey02 == false)
                {
                    skip_list.Add(2);
                }
                if (sKey03 == false)
                {
                    skip_list.Add(3);
                }

                if (skip_list.Count != 0)
                {
                    int[] true_arr = skip_list.ToArray();
                    return true_arr;
                }

                else
                {
                    int[] not_skip = new[] {4};
                    return not_skip;
                }
            }
        }
    }
}
