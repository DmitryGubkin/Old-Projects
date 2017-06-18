using System;
using System.Collections.Generic;
using System.Windows;
using  SolidWorks.Interop.sldworks;
//specail class whith very useful features

namespace Shaft_Work
{
    class Helper
    {
       public ModelDoc2 SwModel { get; set; }
       public SldWorks SwApp { get; set; }
       private List<int> FeaturesList;

        public Helper(ModelDoc2 swModel, SldWorks swApp)
        {
            SwModel = swModel;
            SwApp = swApp;
            FeaturesList = new List<int>();
        }

        public Helper()
        {
            FeaturesList = new List<int>();
        }

        ~Helper()
        {
            GC.Collect();
        }

        public double ToRad(double angle) //convert degrees to radians
        {
            return Math.Round((Math.PI * angle / 180.0), 14);
        }

        #region // select feature (helper)
        public void select_feature(string type, int index, bool Append, int mark) // Select plane by id,default: 0 - front, 1- top, 2- right; append - add to selection, index - inex in the list 
        {
            //FeatureManager fmManager = SwModel.FeatureManager;
            //FeatureStatistics statistics = fmManager.FeatureStatistics;
            //"RefPlane"
            // "RefAxis" 

            CheckList();

            Feature swFeature = (Feature)SwModel.FirstFeature();//get first feture

            if (FeaturesList.Count != 0)// clear list if it not empty
            {
                FeaturesList.Clear();
            }
            while (swFeature != null) // filling list of planes index
            {

                if (type == swFeature.GetTypeName())
                {
                    FeaturesList.Add(swFeature.GetID());
                }

                swFeature = (Feature)swFeature.GetNextFeature();

            }

            try
            {
                swFeature = (Feature)SwModel.FirstFeature();

                while (swFeature != null)//Finding and selecting feature by new index
                {

                    if (type == swFeature.GetTypeName() && swFeature.GetID() == FeaturesList[index])
                    {
                        swFeature.Select2(Append, mark);
                        break;

                    }
                    swFeature = (Feature)swFeature.GetNextFeature();
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Selection error!");
                throw;
            }
        }
        public int get_features_count(string type) // get feature count type of 
        {

            Feature swFeature = (Feature)SwModel.FirstFeature();//get first feture
            int count = 0;
            while (swFeature != null)
            {

                if (type == swFeature.GetTypeName())// check feature by type
                {
                    count++;
                }

                swFeature = (Feature)swFeature.GetNextFeature();

            }

            return count;

        }

        public void HidePlanes() // hide planes
        {
            SwModel.ClearSelection2(true);

            for (int i = 0; i < (get_features_count("RefPlane")); i++)//hide helper planes
            {
                SwModel.ClearSelection2(true);
                select_feature("RefPlane", i, false, 0);
                SwModel.BlankRefGeom();

            }
            SwModel.ClearSelection2(true);
        }
        #endregion

        private void CheckList()
        {
            if (FeaturesList.Count == 0)
            {
                FeaturesList.Clear();
            }

        }
    }
}
