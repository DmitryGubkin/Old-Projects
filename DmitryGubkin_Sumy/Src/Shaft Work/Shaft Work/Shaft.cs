using System;
using System.Collections.Generic;
using SolidWorks.Interop.sldworks;
using MessageBox = System.Windows.MessageBox;

//Class for shaft creating whith main logic

namespace Shaft_Work
{
    internal class Shaft
    {
        private SldWorks SwApp;
        private ModelDoc2 SwModel;
        public List<IStage> Stages = new List<IStage>();
        private List<Point2D> points_of_base = new List<Point2D>();
        private Helper helper;

        //
        public Shaft(SldWorks SwApp, ModelDoc2 SwModel)
        {
            this.SwApp = SwApp;
            this.SwModel = SwModel;
            helper = new Helper(SwModel, SwApp);
        }

        public Shaft()
        {
            helper = new Helper(SwModel, SwApp);
        }

        ~Shaft()
        {
            GC.Collect();
        }

        public Helper SetHelper
        {
            set { helper = value; }
        }

        //

        public SldWorks SWApp
        {
            get { return SwApp; }
            set { SwApp = value; }
        }

        public ModelDoc2 SWModel
        {
            get { return SwModel; }
            set { SwModel = value; }
        }


        //

        public void AddStage() // add stage
        {
            try
            {
                IStage stage = GetStage();
                Stages.Add(stage);
            }
            catch (Exception)
            {
                MessageBox.Show("Error on stage adding");
                throw;
            }
        }


        public void RemoveStage(int index)
        {
            Stages.RemoveAt(index);
        }

        public void OnSelect(int index)
        {
            MainWindow window = MainWindow.ActiveForm as MainWindow;
            Stages[index].GetParams(window);
        }

        public void OnChange(int index)
        {
            Stages[index] = GetStage();
        }

        public void OnMoveUp(int index)
        {
            IStage stage = Stages[index - 1];
            Stages[index - 1] = Stages[index];
            Stages[index] = stage;
        }

        public void OnMoveDown(int index)
        {
            IStage stage = Stages[index + 1];
            Stages[index + 1] = Stages[index];
            Stages[index] = stage;
        }

        private IStageFeature OnFeatureAdd()
        {
            MainWindow window = MainWindow.ActiveForm as MainWindow;


            if (window._Seating_type != (object) "None") // check seatkey
            {
                SeatKey seatKey = new SeatKey();
                seatKey.SetParams(window._Seating_type, window._SLength, window._SWidth, window._SDepth,
                    window._SOffset, window._Seating_operation, window._Seating_CF_value, window._SetKey01,
                    window._SetKey02, window._SetKey03);

                return seatKey;
            }


            if (window._KeyType != (object) "None") // check keyprofile
            {
                KeyProfile keyProfile = new KeyProfile();
                keyProfile.SetParams(window._KeyType, window._KeyWidth);
                return keyProfile;
            }

            if (window._TheardUse != (object) "None") // check theard
            {
                Theard theard = new Theard();
                theard.SetParams(window._TheardUse, window._TheardType, window._TheardR, window._TheardPinch,
                    window._TheardCW, window._TheaedFit);
                return theard;
            }


            if (window._SplinedType != (object) "None")
            {
                SplinedKey splinedKey = new SplinedKey();
                splinedKey.SetParams(window._SplinedType, window._SplinedMethod, window._SplinedR,
                    window._SplinedLength, window._SplinedWidth, window._SplindeArcL, window._SplinedChamfer,
                    window._SplinedFillet, window._SplinedArray);
                return splinedKey;
            }

            return null;
        }


        public void CreteShaft()
        {
            if(CheckSWConnect()==false)
            { return;}

            CreateBase();
            foreach (var stage in Stages)
            {
                if (stage.GetType() == typeof (CircularStage))
                {
                    CircularStage CurrentStage = stage as CircularStage;
                    if (CurrentStage.StageFeature != null)
                    {
                        CurrentStage.StageFeature.AddFeature(SwApp, SwModel, CurrentStage.P1, CurrentStage.P2);
                    }
                }
            }

            SwModel.Rebuild(3);
            SwModel.ClearSelection2(true);
        }

        private IStage GetStage()
        {
            IStage stage;

            MainWindow window = MainWindow.ActiveForm as MainWindow;

            if (window._Profile == (object) "Circular")
            {
                stage = new CircularStage();
                stage.SetParams(window._R1, window._R2, window._Length, window._Soperation, window._Svalue,
                    window._Eoperation, window._Evalue);
                CircularStage cirstage = (CircularStage) stage;
                cirstage.StageFeature = OnFeatureAdd();
                return cirstage;
            }


            stage = new ConicStage();
            stage.SetParams(window._R1, window._R2, window._Length, window._Soperation, window._Svalue,
                window._Eoperation, window._Evalue);
            return stage;
        }

        private void GetPoints2D()
        {
           
            if(CheckStages()==false)
            { return;}
            ClearPoints2D();
            bool flag = true; // is true if  current x cordinat of first node point
            points_of_base.Add(new Point2D(0, 0)); // Add firs point sketch
            double L = 0;
            Point2D s_point = new Point2D(0,0);
            Point2D e_point = new Point2D(0,0);

            for (int i = 0; i < Stages.Count; i++)
            {
                L += Stages[i].Length;

                if (flag)
                {
                    s_point.x = 0;
                    flag = false;
                }

                s_point.y = Stages[i].R1;

                Stages[i].P1 = new Point2D(s_point.x, s_point.y); // set start point of this node

                points_of_base.Add(new Point2D(s_point.x, s_point.y)); //add first point of current node

                e_point.x += Stages[i].Length;
                s_point.x = e_point.x;
                e_point.y = Stages[i].R2;

                points_of_base.Add(new Point2D(e_point.x, e_point.y)); //add last point of current node

                Stages[i].P2 = new Point2D(e_point.x, e_point.y); //add second point of current node
            }

            points_of_base.Add(new Point2D(L, 0)); // add last point sketch
        }


        public void DrawShaftBase() //draw base sketch of shaft
        {
            SketchManager sketchManager = SwModel.SketchManager;
            sketchManager.InsertSketch(true);

            GetPoints2D();

            for (int i = 0; i < points_of_base.Count; i++)
            {
                if (i != (points_of_base.Count - 1))
                {
                    sketchManager.CreateLine(points_of_base[i].x, points_of_base[i].y, 0, points_of_base[i + 1].x,
                        points_of_base[i + 1].y, 0); //draw sketch lines
                }
                else
                {
                    sketchManager.CreateLine(points_of_base[0].x, points_of_base[0].y, 0,
                        points_of_base[points_of_base.Count - 1].x,
                        points_of_base[points_of_base.Count - 1].y, 0); //close sketch
                }
            }
        }


        public void CreateBase()
        {
            helper.select_feature("RefPlane", 0, true, 0); // select Front plane
            DrawShaftBase();

            SwModel.ClearSelection2(true); //clear selection

            helper.select_feature("RefPlane", 0, true, 0); // select Front plane
            helper.select_feature("RefPlane", 1, true, 0); // select Top plane
            SwModel.InsertAxis2(true); //insert main axis

            SwModel.ClearSelection2(true);

            helper.select_feature("RefAxis", 0, false, 4);
            SwModel.FeatureManager.FeatureRevolve2(true, true, false, false, false, false, 0, 0, helper.ToRad(360), 0,
                false, false, 0.01, 0.01, 0, 0, 0, true, true, true); //Create base body by revolving

            SwModel.ClearSelection2(true);

            AddChamferFillet();
        }


        private void AddChamferFillet() // add chamfer or fillet on the edges
        {
            PartDoc SwPart = (PartDoc) SwApp.IActiveDoc2; // get intarfaces of PartDoc
            Array AllBodies = (Array) SwPart.GetBodies2(0, true); //get array of bodys
            Body2 SwBody = AllBodies.GetValue(0) as Body2;
                //get first body of array, because we have single solid body at the active document
            Array AllEges = (Array) SwBody.GetEdges(); //get array of body edges
            int current_edge = 0; // current edge index of body
            Entity ent = AllEges.GetValue(current_edge) as Entity; // needs to use some intarfaces such as Select4()
            bool flag = true; // flag 

            for (int i = 0; i < Stages.Count; i++)
            {
                ent = AllEges.GetValue(current_edge) as Entity;
                ent.Select4(false, null); //select firs edge

                if (flag == true)
                    //do chamfer or fillet on the current edge of body
                {
                    AddEdgeFeature(i, true); // add feature on start of stage
                    current_edge++; // index of the next edge
                }

                ent = AllEges.GetValue(current_edge) as Entity;
                ent.Select4(false, null); //select next edge 

                if (i == (Stages.Count - 1)) // end ege of whole shaft
                {
                    AddEdgeFeature(i, false); //add feature on end of stage
                    break; //break cycle
                }


                if (Stages[i].R2 != Stages[i + 1].R1)
                    //Check end radius of current stage node and start radius of the next stage node, if they are not the same we can do chamfer or fillet
                {
                    AddEdgeFeature(i, false); //add feature on end of stage
                    flag = true;
                    current_edge++;

                    if (Stages[i].GetType() == typeof (ConicStage) && Stages[i].R2 == Stages[i + 1].R1)
                    {
                        flag = false;
                        current_edge++;
                    }
                }
                else // if they are the same, disable any operation(Chamfer or Fillet)
                {
                    if (Stages[i + 1].GetType() == typeof (ConicStage) || Stages[i].GetType() == typeof (ConicStage))
                    {
                        if (Stages[i + 1].R1 != Stages[i + 1].R2 || Stages[i].R1 != Stages[i].R2)
                        {
                            current_edge++;
                        }
                    }
                    flag = false;
                }
            }
        }

        private void AddEdgeFeature(int index, bool start) // true if start of the edge, false - end of edge
        {
            if (index < 0 || (index > Stages.Count - 1))
            {
                MessageBox.Show(@"Error on Chamfering/Filleting\nthis index not exsist");
                return;
            }
            //default initisialize
            double value = 1;
            object operation = "None";

            if (start == true)
            {
                operation = Stages[index].SOperation;
                value = Stages[index].SValue;
            }
            else
            {
                operation = Stages[index].EOperation;
                value = Stages[index].EValue;
            }

            if (operation == (object) "Chamfer")
            {
                SwModel.FeatureManager.InsertFeatureChamfer(4, 1, value, helper.ToRad(45), 0, 0, 0, 0);
            }
            if (operation == (object) "Fillet")
            {
                SwModel.FeatureManager.FeatureFillet(195, value, 0, 0, null, null, null);
            }
        }


        private void ClearPoints2D()
        {
            if (points_of_base.Count > 0)
            {
                points_of_base.Clear(); // clear list of point if it is not empty
            }
        }


        private bool CheckStages()
        {
            if (Stages.Count > 0)
            {
                return true;
            }
            return false;
        }

        private bool CheckSWConnect()
        {
            if (SwApp == null || SwModel == null)
            {
                MessageBox.Show("Not exsist SolidWorks connection");
                return false;
            }
            return true;
        }

       

    }
}
