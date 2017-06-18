using SolidWorks.Interop.sldworks;
// Intarface of stage(Avaible only for Circular stages)
namespace Shaft_Work
{
    internal interface IStageFeature
    {
        void AddFeature(SldWorks SwApp, ModelDoc2 SwModel, Point2D P1, Point2D P2); // add stage feature
        bool CheckUsing();
        // check using of feature("None" - pass) recomended used inside of features classes(such as theard, seatkey ect..)
        void GetParams(MainWindow window); // get feature paramiters and apply them
    }
}



