namespace Shaft_Work
{
    //Intarface of stage
    internal interface IStage
    {
        double R1 { get; set; } // firs radius
        double R2 { get; set; } // second radius
        double Length { get; set; } // length of stage
        object SOperation { get; set; } // operation on start edge of the stage
        double SValue { get; set; } // value of start operation
        object EOperation { get; set; } // operation on end edge of the stage
        double EValue { get; set; } // value of end operation
        Point2D P1 { get; set; } // firs point of stage sketch
        Point2D P2 { get; set; } // firs point of stage sketch

        void GetParams(MainWindow window); // Get paramiters of this stage and set them to the main window controls

        void SetParams(double R1, double R2, double Length, object SOperation, double SValue, object EOperation,
            double EValue); // set paramiters of the stage
    }
}


