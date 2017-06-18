using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using  SolidWorks.Interop.sldworks;
// UI
namespace Shaft_Work
{
    public partial class MainWindow : Form
    {
        #region //constructor init

        public MainWindow() // components initialization
        {
            InitializeComponent();
            Profile_type.SelectedIndex = 0;
            Profile_R2.Enabled = false;
            S_operation_type.SelectedIndex = 0;
            E_operation_type.SelectedIndex = 0;
            Key_seating_type.SelectedIndex = 0;
            Seating_operation.SelectedIndex = 0;
            Key_profile_type.SelectedIndex = 0;
            Splined_Type.SelectedIndex = 0;
            Splined_Method.SelectedIndex = 0;
            S_value.Enabled = false;
            E_value.Enabled = false;
            btnRemove_Stage.Enabled = false;
            Key_profile_width.Enabled = false;

            Seating_width.Maximum = Profile_R1.Value;
            Seating_length.Maximum = Profile_legth.Value;
            set_offset_range();
            set_key_width();
            draw_preview();
            Node_down.Enabled = false;
            Node_up.Enabled = false;

            Splined_Radius.Maximum = Profile_R1.Value*(decimal) 0.98;
            Splined_Arc_Length.Maximum = (decimal) 0.9*Splined_Length.Value;
            Splined_Width.Maximum = Profile_R1.Value*(decimal) 0.8;

            set_splined_array();

            Theard_Use.SelectedIndex = 0;
            Theard_Type.SelectedIndex = 0;
            Theard_CW.Checked = false;
            Theard_Fit.Checked = false;
            Theard_Fit.Enabled = false;
            Theard_CW.Enabled = false;
            set_theard_range();
            set_theard_pinch_range();

            SLDConnect = new SolidWorksConnector();
        }

        #endregion

        // sw variables
        public SldWorks SwApp = null;
        public ModelDoc2 SwModel = null;


        public int node_counter = 1; // Node counter, using to generete node name at nodetree
        public string current_node_name = "Shaft Stage "; // name of the selected node of node view component
        public int current_node_inx = 0; // Current index of selected node

        private SolidWorksConnector SLDConnect;// Solidworks connector
        private Shaft MyShaft = new Shaft();// Shaft variable
//      private double Dimention = 1; // Dimention divader


        //Add new stage(node)
        private void btnAdd_Stage_Click(object sender, EventArgs e)
        {
            if (Auto_Name.Checked == false)
            {
                Node_View.Nodes.Add("Shaft Stage " + node_counter);
            }
            else
            {
                Node_View.Nodes.Add(Node_name.Text);
            }

            MyShaft.AddStage();
            node_counter++;
        }
        //Remove selected node from nodetree
        private void btnRemove_Stage_Click(object sender, EventArgs e)
        {
            if (Node_View.Nodes.Count == 0)
            {
                return;
            }

            DialogResult dialogResult = MessageBox.Show(@"Are you sure you want to delete " + Node_name.Text + @" ?",
                @"Node delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Node_View.Nodes.RemoveAt(current_node_inx);
                MyShaft.RemoveStage(current_node_inx);
                if (Node_View.Nodes.Count != 0)
                {
                    current_node_inx = Node_View.Nodes.Count - 1;
                    Node_View.SelectedNode = Node_View.Nodes[current_node_inx];
                    MyShaft.OnSelect(current_node_inx);
                }
                btnRemove_Stage.Enabled = false;
                Node_up.Enabled = false;
                Node_down.Enabled = false;


                //  MessageBox.Show("count"+ INodeShaft.Count);
            }
        }

        // Apply new values to the List(now this button is hided, because program use it for auto property applying)
        private void btnApply_Property_Click(object sender, EventArgs e)
        {
            try
            {
                if (Node_View.Nodes.Count == 0 || (current_node_inx > Node_View.Nodes.Count - 1))
                {
                    return;
                }
                else
                {
                    auto_check_key();
                    set_theard_range();
                    set_theard_fit();
                    MyShaft.OnChange(current_node_inx);
                    name_builder();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select any node to apply property");
                return;
            }
        }

        // Select node
        private void Node_View_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Node_name.Text = e.Node.Text;
            current_node_name = e.Node.Text;
            current_node_inx = e.Node.Index;
            // MessageBox.Show("" + e.Node.Index);
            btnRemove_Stage.Enabled = true;
            //Get_node_property(current_node_inx);
            MyShaft.OnSelect(current_node_inx);
            draw_preview();
            Node_down.Enabled = true;
            Node_up.Enabled = true;
        }

        //Create Shaft
        private void btn_Create_Click(object sender, EventArgs e)
        {
            if (Node_View.Nodes.Count < 1)
            {
                return;
            }

            try
            {
                SLDConnect.SldConnect();
                SwApp = SLDConnect.MyApp;
                SwModel = SLDConnect.MySwModel;
                Helper helper = new Helper(SwModel, SwApp);
                MyShaft.SWApp = SwApp;
                MyShaft.SWModel = SwModel;
                MyShaft.SetHelper = helper;
                MyShaft.CreteShaft();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Error on Create!\n Please click this Button again,\nor check out your nodes paramiters\nThanks)");
                return;
            }
        }

        //move up node
        private void Node_up_Click(object sender, EventArgs e) 
        {
            if (current_node_inx == 0 || Node_View.Nodes.Count < 1 || Node_View.SelectedNode.Index == 0)
            {
                return;
            }

            try
            {
                string prevtName = default(string);
                prevtName = Node_View.Nodes[current_node_inx - 1].Text.ToString();
                MyShaft.OnMoveUp(current_node_inx);
                Node_View.Nodes[current_node_inx - 1].Text = Node_View.Nodes[current_node_inx].Text;
                Node_View.Nodes[current_node_inx].Text = prevtName;
                current_node_inx--;
                Node_View.SelectedNode = Node_View.SelectedNode.PrevNode;
            }
            catch (Exception)
            {
                MessageBox.Show("Move UP Error");
                throw;
            }
        }

        // move down node
        private void Node_down_Click(object sender, EventArgs e) 
        {
            if ((current_node_inx == (Node_View.Nodes.Count - 1)) || Node_View.Nodes.Count < 1 ||
                (Node_View.SelectedNode.Index == (Node_View.Nodes.Count - 1)))
            {
                return;
            }
            else
            {
                try
                {
                    string nextName = default(string);
                    nextName = Node_View.Nodes[current_node_inx + 1].Text.ToString();
                    MyShaft.OnMoveDown(current_node_inx);
                    Node_View.Nodes[current_node_inx + 1].Text = Node_View.Nodes[current_node_inx].Text;
                    Node_View.Nodes[current_node_inx].Text = nextName;
                    current_node_inx++;
                    Node_View.SelectedNode = Node_View.SelectedNode.NextNode;
                }
                catch (Exception)
                {
                    MessageBox.Show("Move DOWN Error");
                    throw;
                }
            }
        }

        #region //Get property of window  controls(Window Accessors)

        public object _Profile
        {
            get { return Profile_type.SelectedItem; }
            set { Profile_type.SelectedItem = value; }
        }

        public object _Soperation
        {
            get { return S_operation_type.SelectedItem; }
            set { S_operation_type.SelectedItem = value; }
        }

        public object _Eoperation
        {
            get { return E_operation_type.SelectedItem; }
            set { E_operation_type.SelectedItem = value; }
        }

        public double _R1
        {
            get { return Convert.ToDouble(Profile_R1.Value) / 2; }
            set { Profile_R1.Value = 2 * Convert.ToDecimal(value); }
        }

        public double _R2
        {
            get { return Convert.ToDouble(Profile_R2.Value) / 2; }
            set { Profile_R2.Value = 2 * Convert.ToDecimal(value); }
        }

        public double _Length
        {
            get { return Convert.ToDouble(Profile_legth.Value); }
            set { Profile_legth.Value = Convert.ToDecimal(value); }
        }


        public double _Svalue
        {
            get { return Convert.ToDouble(S_value.Value); }
            set { S_value.Value = Convert.ToDecimal(value); }
        }

        public double _Evalue
        {
            get { return Convert.ToDouble(E_value.Value); }
            set { E_value.Value = Convert.ToDecimal(value); }
        }

        public object _Seating_type
        {
            get { return Key_seating_type.SelectedItem; }
            set { Key_seating_type.SelectedItem = value; }
        }

        public double _SLength
        {
            get { return Convert.ToDouble(Seating_length.Value); }
            set { Seating_length.Value = Convert.ToDecimal(value); }
        }

        public double _SDepth
        {
            get { return Convert.ToDouble(Seating_depth.Value); }
            set { Seating_depth.Value = Convert.ToDecimal(value); }
        }

        public double _SWidth
        {
            get { return Convert.ToDouble(Seating_width.Value); }
            set { Seating_width.Value = Convert.ToDecimal(value); }
        }

        public double _SOffset
        {
            get { return Convert.ToDouble(Offset_value.Value); }
            set { Offset_value.Value = Convert.ToDecimal(value); }
        }

        public double _Seating_CF_value
        {
            get { return Convert.ToDouble(Seating_chamfer_fillet.Value); }
            set { Seating_chamfer_fillet.Value = (decimal)value; }
        }

        public object _Seating_operation
        {
            get { return Seating_operation.SelectedItem; }
            set { Seating_operation.SelectedItem = value; }
        }

        public bool _SetKey00
        {
            get { return Key00.Checked; }
            set { Key00.Checked = value; }
        }

        public bool _SetKey01
        {
            get { return Key01.Checked; }
            set { Key01.Checked = value; }
        }

        public bool _SetKey02
        {
            get { return Key02.Checked; }
            set { Key02.Checked = value; }
        }

        public bool _SetKey03
        {
            get { return Key03.Checked; }
            set { Key03.Checked = value; }
        }

        public bool _NameBuilder
        {
            get { return Auto_Name.Checked; }
            set { Auto_Name.Checked = value; }
        }

        public object _KeyType
        {
            get { return Key_profile_type.SelectedItem; }
            set { Key_profile_type.SelectedItem = value; }
        }

        public double _KeyWidth
        {
            get { return Convert.ToDouble(Key_profile_width.Value); }
            set { Key_profile_width.Value = (decimal)value; }
        }

        public object _SplinedType
        {
            get { return Splined_Type.SelectedItem; }
            set { Splined_Type.SelectedItem = value; }
        }

        public object _SplinedMethod
        {
            get { return Splined_Method.SelectedItem; }
            set { Splined_Method.SelectedItem = value; }
        }


        public double _SplinedR
        {
            get { return Convert.ToDouble(Splined_Radius.Value) / 2; }
            set { Splined_Radius.Value = 2 * (decimal)value; }
        }

        public double _SplinedLength
        {
            get { return Convert.ToDouble(Splined_Length.Value); }
            set { Splined_Length.Value = (decimal)value; }
        }

        public double _SplinedWidth
        {
            get { return Convert.ToDouble(Splined_Width.Value); }
            set { Splined_Width.Value = (decimal)value; }
        }

        public double _SplindeArcL
        {
            get { return Convert.ToDouble(Splined_Arc_Length.Value); }
            set { Splined_Arc_Length.Value = (decimal)value; }
        }

        public double _SplinedChamfer
        {
            get { return Convert.ToDouble(Splined_Chamfer.Value); }
            set { Splined_Chamfer.Value = (decimal)value; }
        }

        public double _SplinedFillet
        {
            get { return Convert.ToDouble(Splined_Fillet.Value); }
            set { Splined_Fillet.Value = (decimal)value; }
        }

        public int _SplinedArray
        {
            get { return Convert.ToInt32(Splined_Array.Value); }
            set { Splined_Array.Value = (decimal)value; }
        }

        public object _TheardUse
        {
            get { return Theard_Use.SelectedItem; }
            set { Theard_Use.SelectedItem = value; }
        }

        public object _TheardType
        {
            get { return Theard_Type.SelectedItem; }
            set { Theard_Type.SelectedItem = value; }
        }

        public bool _TheardCW
        {
            get { return Theard_CW.Checked; }
            set { Theard_CW.Checked = value; }
        }

        public bool _TheaedFit
        {
            get { return Theard_Fit.Checked; }
            set { Theard_Fit.Checked = value; }
        }

        public double _TheardPinch
        {
            get { return Convert.ToDouble(Theard_Pitch.Value); }
            set { Theard_Pitch.Value = (decimal)value; }
        }

        public double _TheardR
        {
            get { return Convert.ToDouble(Theard_Radius.Value) / 2; }
            set { Theard_Radius.Value = 2 * (decimal)value; }
        }

        public TreeView _NodeTree
        {
            get { return Node_View; }
            set { Node_View = value; }
        }

        #endregion

        #region // Other click and change control events and they helpers
        private void set_splined_array() // maximum number of copies
        {
            Splined_Array.Minimum = 1;
            Splined_Array.Maximum =
                (decimal) Math.Ceiling((Math.PI*(double) Splined_Radius.Value)/(double) Splined_Width.Value) - 1;

            if (Splined_Array.Maximum < 1)
            {
                Splined_Array.Maximum = 1;
            }
        }

        ///***
        private void set_key_width() // set width
        {
            if (Key_profile_type.SelectedIndex == 1) // set width by diametr value
            {
                if (Profile_R1.Value <= 1) // check min value
                {
                    Key_profile_width.Value = (decimal) Math.Sqrt(2)*(decimal) 0.5;
                    Profile_R1.Value = 1;
                    return;
                }

                Key_profile_width.Value = (decimal) Math.Sqrt(2)*(Profile_R1.Value/2);
            }

            if (Key_profile_type.SelectedIndex == 2)
            {
                if (Profile_R1.Value == 1)
                {
                    Key_profile_width.Value = (decimal) ((decimal) 0.5*(decimal) Math.Sqrt(3));
                    return;
                }
                Key_profile_width.Value = (decimal) (Profile_R1.Value/2*(decimal) Math.Sqrt(3));
            }

            if (Key_profile_type.SelectedIndex == 3)
            {
                if (Key_profile_width.Value > (Profile_R1.Value - (decimal) 0.01*Profile_R1.Value))
                    // check max value for width 
                {
                    Key_profile_width.Value = (Profile_R1.Value - (decimal) 0.01*Profile_R1.Value);
                }
            }
        }


        private void name_builder() // name builder
        {
            if (Auto_Name.Checked == true)
            {
                Node_name.Enabled = false;
                StringBuilder tample = new StringBuilder();
                tample.Append(Profile_type.Text);
                tample.Append(" : " + Math.Round(Profile_R1.Value, 2) + "x" + Math.Round(Profile_legth.Value, 2) + "x" +
                              Math.Round(Profile_R2.Value, 2) + "; ");

                if (Profile_type.SelectedIndex == 0)
                {
                    if (Key_seating_type.SelectedIndex != 0)
                    {
                        tample.Append(Key_seating_type.Text);
                        tample.Append(": H= " + Math.Round(Seating_depth.Value, 2) + ",L =" +
                                      Math.Round(Seating_length.Value, 2) + ",W= " + Math.Round(Seating_width.Value, 2));
                    }

                    if (Key_profile_type.SelectedIndex != 0)
                    {
                        tample.Append(Key_profile_type.Text);
                        tample.Append(": W= " + Math.Round(Key_profile_width.Value, 2));
                    }

                    if (Splined_Type.SelectedIndex != 0)
                    {
                        tample.Append(Splined_Type.Text);
                        var H = (Profile_R1.Value - Splined_Radius.Value)/2;
                        tample.Append(": H= " + Math.Round(H, 2) + ",L= " + Math.Round(Splined_Length.Value, 2) + ",W= " +
                                      Math.Round(Splined_Width.Value, 2));
                        if (Splined_Array.Value > 1)
                        {
                            tample.Append("x" + Splined_Array.Value);
                        }
                    }

                    if (Theard_Use.SelectedIndex != 0)
                    {
                        tample.Append(Theard_Type.Text);
                        tample.Append(": D = " + Math.Round(Profile_R1.Value, 2) + ",P= " +
                                      Math.Round(Theard_Pitch.Value, 2));

                        if (Theard_CW.Checked == true)
                        {
                            tample.Append(", RH");
                        }
                        else
                        {
                            tample.Append(", LH");
                        }
                    }
                }
                Node_name.Text = tample.ToString();
                if (Node_View.Nodes.Count > 0 && Node_View.SelectedNode != null)
                {
                    Node_View.SelectedNode.Text = Node_name.Text;
                }
            }
            else
            {
                Node_name.Enabled = true;
            }
        }

        private void set_key_diametr() // set diametr
        {
            if (Key_profile_type.SelectedIndex == 1) // set diametr by width value
            {
                if (Key_profile_width.Value <= (decimal) 0.71) // check min value
                {
                    Key_profile_width.Value = (decimal) Math.Sqrt(2)*(decimal) 0.5;
                    Profile_R1.Value = 1;
                    return;
                }

                Profile_R1.Value = (2*Key_profile_width.Value)/(decimal) Math.Sqrt(2);
            }

            if (Key_profile_type.SelectedIndex == 3)
            {
                if (Key_profile_width.Value > (Profile_R1.Value - (decimal) 0.01*Profile_R1.Value))
                    // check max value for width 
                {
                    Key_profile_width.Value = (Profile_R1.Value - (decimal) 0.01*Profile_R1.Value);
                }
            }


            if (Key_profile_type.SelectedIndex == 2)
            {
                if (Key_profile_width.Value <= (decimal) 0.87)
                {
                    Key_profile_width.Value = (decimal) ((decimal) 0.5*(decimal) Math.Sqrt(3));
                    Profile_R1.Value = 1;
                    return;
                }

                Profile_R1.Value = (decimal) (2*Key_profile_width.Value)/(decimal) (Math.Sqrt(3));
            }
        }

        ///***
        private static double toRad(double angle) //convert degrees to radians
        {
            return Math.Round((Math.PI*angle/180.0), 14);
        }


        private void Node_name_Leave(object sender, EventArgs e)
        {
            if (Node_View.Nodes.Count < 1)
            {
                return;
            }

            if (Node_name.Text != "")
            {
                Node_View.SelectedNode.Text = Node_name.Text;
            }
            else
            {
                Node_name.Text = current_node_name;
            }
        }

        private void Node_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter && Node_View.Nodes.Count > 0)
            {
                if (Node_name.Text != "")
                {
                    Node_View.SelectedNode.Text = Node_name.Text;
                }
                else
                {
                    Node_name.Text = current_node_name;
                }
            }
        }

        private void Profile_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Profile_type.SelectedIndex == 0)
            {
                Profile_R2.Enabled = false;
                Profile_R2.Value = Profile_R1.Value;
                Key_seating_type.Enabled = true;
                Key_profile_type.Enabled = true;
                Splined_Type.Enabled = true;
                Theard_Use.Enabled = true;
            }

            else
            {
                Profile_R2.Enabled = true;
                Key_seating_type.SelectedItem = (object) "None";
                Key_seating_type.Enabled = false;
                Key_profile_type.Enabled = false;
                Splined_Type.Enabled = false;
                Key_profile_type.SelectedItem = (object) "None";
                Splined_Type.SelectedItem = (object) "None";
                Theard_Use.SelectedItem = (object) "None";
                Theard_Use.Enabled = false;
            }
        }

        private void S_operation_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (S_operation_type.SelectedIndex == 0)
            {
                S_value.Enabled = false;
            }
            else
            {
                S_value.Enabled = true;
            }
        }

        private void E_operation_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (E_operation_type.SelectedIndex == 0)
            {
                E_value.Enabled = false;
            }
            else
            {
                E_value.Enabled = true;
            }
        }

        private void Profile_R1_ValueChanged(object sender, EventArgs e)
        {
            draw_preview();
            set_key_width();
            if (Profile_type.SelectedIndex == 0)
            {
                Profile_R2.Value = Profile_R1.Value;
                Seating_width.Maximum = Profile_R1.Value;
                Splined_Radius.Maximum = Profile_R1.Value*(decimal) 0.98;
                Splined_Width.Maximum = Profile_R1.Value*(decimal) 0.8;

                set_offset_range();

                if (Profile_R1.Value < (Theard_Pitch.Value*5))
                {
                    Theard_Use.SelectedIndex = 0;
                    Theard_Use.Enabled = false;
                }
                else
                {
                    Theard_Use.Enabled = true;
                    set_theard_fit();
                }
            }
        }

        private void set_offset_range() // calcucate offset range
        {
            object[] seating_types = new[] {"None", "Prismatic A", "Prismatic B", "Segmented"};


            if (Key_seating_type.SelectedItem != seating_types[0])
            {
                if (Key_seating_type.SelectedItem == seating_types[1] ||
                    Key_seating_type.SelectedItem == seating_types[3])
                {
                    Seating_length.Maximum = Profile_legth.Value;
                    Seating_width.Maximum = Profile_R2.Value;

                    Offset_value.Maximum = (decimal) ((Profile_legth.Value - Seating_length.Value)/2);
                    Offset_value.Minimum = -1*Offset_value.Maximum;
                }

                if (Key_seating_type.SelectedItem == seating_types[2])
                {
                    Seating_length.Maximum = Profile_legth.Value;
                    Seating_width.Maximum = Profile_legth.Value - Seating_length.Value;

                    Offset_value.Maximum =(decimal) ((Profile_legth.Value - (Seating_length.Value + Seating_width.Value))/2);
                    Offset_value.Minimum = -1*Offset_value.Maximum;
                }

                Offset_control.SetRange((int) (Offset_value.Minimum*1000), (int) (Offset_value.Maximum*1000));
            }

            else
            {
                Offset_value.Value = 0;
                Offset_control.Value = 0;
                Offset_control.SetRange(-100000, 100000);
            }
        }

        private void set_theard_range() // set range of thead inner diametr
        {
            if (Profile_type.SelectedIndex == 0)
            {
                Theard_Radius.Maximum = Profile_R1.Value - Theard_Pitch.Value;
                Theard_Radius.Minimum = Profile_R1.Value - 4*Theard_Pitch.Value;
            }
            else
            {
                Theard_Radius.Maximum = 9999;
                Theard_Radius.Minimum = (decimal) 0.01;
                Theard_Radius.Value = Profile_R1.Value - Theard_Pitch.Value*2;
            }
        }


        #region reset offset range and outher ranges

        private void Offset_control_ValueChanged(object sender, EventArgs e)
        {
            Offset_value.Value = (decimal) Offset_control.Value/1000;
        }

        private void Offset_value_ValueChanged(object sender, EventArgs e)
        {
            Offset_control.Value = (int) (Offset_value.Value*1000);
        }

        private void Profile_legth_ValueChanged(object sender, EventArgs e)
        {
            draw_preview();
            Seating_length.Maximum = Profile_legth.Value;
            set_offset_range();

            if (Splined_Type.SelectedIndex != 0 && Splined_Method.SelectedIndex == 2)
            {
                Splined_Length.Value = Profile_legth.Value;
            }
        }

        private void Seating_length_ValueChanged(object sender, EventArgs e)
        {
            set_offset_range();
        }

        private void Seating_width_ValueChanged(object sender, EventArgs e)
        {
            //Seating_width.Maximum = Profile_R2.Value;
            set_offset_range();
        }


        private void Key_seating_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Key_seating_type.SelectedIndex == 0) //disable seating control elements
            {
                Seating_length.Enabled = false;
                Seating_depth.Enabled = false;
                Seating_width.Enabled = false;
                Offset_value.Enabled = false;
                Offset_control.Enabled = false;
                Seating_chamfer_fillet.Enabled = false;
                Seating_operation.Enabled = false;
                Seating_operation.SelectedIndex = 0;
                Offset_control.Value = 0;

                Key01.Enabled = false;
                Key02.Enabled = false;
                Key03.Enabled = false;

                if (Key_profile_type.SelectedIndex == 0) // check for none value of key_profile type
                {
                    draw_preview();
                }
            }
            else //enable seating control elements
            {
                Seating_length.Enabled = true;
                Seating_depth.Enabled = true;
                Seating_width.Enabled = true;
                Offset_value.Enabled = true;
                Offset_control.Enabled = true;
                Seating_chamfer_fillet.Enabled = true;
                Seating_operation.Enabled = true;
                Offset_control.Value = 0;

                Key01.Enabled = true;
                Key02.Enabled = true;
                Key03.Enabled = true;
                Key_profile_type.SelectedIndex = 0;
                Splined_Type.SelectedIndex = 0;
                Theard_Use.SelectedIndex = 0;

                if (Key_seating_type.SelectedIndex == 1)
                {
                    Stage_Preview.Image = Shaft_Work.Properties.Resources.Prismatic_A as Image;
                        // changing of prompt image
                }
            }

            if (Key_seating_type.SelectedIndex == 3) //change lable text if selected segmented key seating
            {
                lb_seating_depth.Text = (string) "Heigth:";
                // lb_seating_chamfer_fillet.Text = (string)"Chamfer:";
                Stage_Preview.Image = Shaft_Work.Properties.Resources.Segmented_ as Image; // changing prompt image
            }
            else
            {
                lb_seating_depth.Text = (string) "Depth:";
                // lb_seating_chamfer_fillet.Text = (string)"Fillet:";
            }

            if (Key_seating_type.SelectedIndex == 2)
            {
                Seating_width.Maximum = Profile_legth.Value - Seating_length.Value;
                Stage_Preview.Image = Shaft_Work.Properties.Resources.Prismatic_B as Image; // changing prompt image
            }
            set_offset_range();
        }


        private void Offset_value_MouseDoubleClick(object sender, MouseEventArgs e)
            //fast set center point by double click
        {
            Offset_value.Value = 0;
        }

        #endregion

        private void draw_preview() // draw preview of shape
        {
            if (Stage_Preview.Image != null)
            {
                Stage_Preview.Image.Dispose();
            }
            //if (Node_View.Nodes.Count == 0)
            //{
            //    return;
            //}
            float XScale = 1;
            float YScale = 1;


            XScale = (float) Stage_Preview.Width/(float) Profile_legth.Value;
            ;

            float D = 0;
            if (Profile_R1.Value > Profile_R2.Value)
            {
                D = (float) Profile_R1.Value;
            }
            else
            {
                D = (float) Profile_R2.Value;
            }


            YScale = (float) Stage_Preview.Height/D;

            if (YScale > XScale) // Set auto scaling
            {
                YScale = XScale;
            }

            else
            {
                XScale = YScale;
            }


            Bitmap Preview = new Bitmap(Stage_Preview.Width, Stage_Preview.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int xcenter = Preview.Width/2;
            int ycenter = Preview.Height/2;
            float[] dashValues = {5, 2, 10, 4};
            Pen DP = new Pen(Color.Blue, 1);
            DP.DashPattern = dashValues;
            Graphics g = Graphics.FromImage(Preview);

            float X1 = xcenter - 0.9F*XScale*(float) (Profile_legth.Value/2);
            float X2 = xcenter + 0.9F*XScale*(float) (Profile_legth.Value/2);
            //float Y1 = default(float);
            //float Y2 = default(float);

            g.DrawLine(DP, X1, ycenter, X2, ycenter); // draww center line
            Pen SP = new Pen(Color.Black, 1.5F);

            //X1 = xcenter - 0.7F*XScale*(float)(Profile_legth.Value / 2);
            //X2 = xcenter + 0.7F*XScale*(float) (Profile_legth.Value/2);
            //Y1 = ycenter - 0.7F*YScale*(float) (Profile_R1.Value/2);
            //Y2 = ycenter - 0.7F*YScale*(float) (Profile_R2.Value/2);

            Point Center = new Point(xcenter, ycenter);
            Point LTPoint = new Point();
            LTPoint.X = (int) (xcenter - 0.7F*XScale*(float) (Profile_legth.Value/2));
            LTPoint.Y = (int) (ycenter - 0.7F*YScale*(float) (Profile_R1.Value/2));

            Shape myShape = new Shape(0.7F*XScale, 0.7F*YScale, (float) Profile_R1.Value, (float) Profile_R2.Value,
                (float) Profile_legth.Value, Center, LTPoint, SP, DP);
            Bitmap newBitmap = myShape.DrawShape(Preview.Width, Preview.Height, S_operation_type.SelectedItem,
                E_operation_type.SelectedItem, (int) S_value.Value, (int) E_value.Value);
            g.DrawImage(newBitmap, new Point(0, 0));
            Stage_Preview.Image = Preview;

            // clean up memory
            g.Dispose();
            GC.Collect();
        }

        private void Profile_R2_ValueChanged(object sender, EventArgs e)
        {
            draw_preview();
        }

        #region // seatkey array options

        private void auto_check_key()
        {
            if (Key00.Checked == false && Key01.Checked == false && Key02.Checked == false && Key03.Checked == false)
            {
                Key00.Checked = true;
            }
        }


        private void Key01_MouseClick(object sender, MouseEventArgs e)
        {
            auto_check_key();
        }

        private void Key02_MouseClick(object sender, MouseEventArgs e)
        {
            auto_check_key();
        }

        private void Key03_MouseClick(object sender, MouseEventArgs e)
        {
            auto_check_key();
        }

        private void Key00_MouseClick(object sender, MouseEventArgs e)
        {
            auto_check_key();
        }

        #endregion


        private void Key_profile_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Key_profile_type.SelectedIndex != 0)
            {
                if (Key_profile_type.SelectedIndex == 1)
                {
                    Stage_Preview.Image = Shaft_Work.Properties.Resources.KeyProfile_Squre as Image;
                        // change prompt image
                }

                if (Key_profile_type.SelectedIndex == 2)
                {
                    Stage_Preview.Image = Shaft_Work.Properties.Resources.KeyProfile_Hexagon as Image;
                        // change prompt image
                }

                if (Key_profile_type.SelectedIndex == 3)
                {
                    Stage_Preview.Image = Shaft_Work.Properties.Resources.KeyProfile_Special as Image;
                        // change prompt image
                }


                Key_seating_type.SelectedIndex = 0;
                Splined_Type.SelectedIndex = 0;
                Theard_Use.SelectedIndex = 0;
                Key_profile_width.Enabled = true;
                set_key_width();
            }
            else
            {
                Key_profile_width.Enabled = false;
                draw_preview();
            }
        }

        private void Key_profile_width_ValueChanged(object sender, EventArgs e)
        {
            set_key_diametr();
        }


        private void set_splinet_image() // set the splinet prompt image in promt view
        {
            if (Splined_Type.SelectedIndex != 0)
            {
                if (Splined_Method.SelectedIndex == 0)
                {
                    Stage_Preview.Image = Shaft_Work.Properties.Resources.Splined_From_Start as Image;
                }
                if (Splined_Method.SelectedIndex == 1)
                {
                    Stage_Preview.Image = Shaft_Work.Properties.Resources.Splined_From_End as Image;
                }
                if (Splined_Method.SelectedIndex == 2)
                {
                    Stage_Preview.Image = Shaft_Work.Properties.Resources.Splined_Through as Image;
                }
            }
            else
            {
                if (Key_profile_type.SelectedIndex == 0 && Key_seating_type.SelectedIndex == 0)
                {
                    draw_preview();
                }
            }
        }

        #region // Splined control elements

        private void Splined_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Splined_Type.SelectedIndex != 0)
            {
                Splined_Width.Enabled = true;
                Splined_Radius.Enabled = true;
                Splined_Chamfer.Enabled = true;
                Splined_Fillet.Enabled = true;
                Splined_Method.Enabled = true;

                if (Splined_Method.SelectedIndex != 2)
                {
                    Splined_Length.Enabled = true;
                    Splined_Arc_Length.Enabled = true;
                }

                Key_seating_type.SelectedIndex = 0;
                Key_profile_type.SelectedIndex = 0;
                Theard_Use.SelectedIndex = 0;
            }
            else
            {
                Splined_Length.Enabled = false;
                Splined_Width.Enabled = false;
                Splined_Radius.Enabled = false;
                Splined_Arc_Length.Enabled = false;
                Splined_Chamfer.Enabled = false;
                Splined_Fillet.Enabled = false;
                Splined_Method.Enabled = false;
            }

            set_splinet_image();
        }

        private void Splined_Method_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Splined_Method.SelectedIndex == 2)
            {
                Splined_Arc_Length.Enabled = false;
                Splined_Length.Enabled = false;
                Splined_Length.Value = Profile_legth.Value;
            }
            else
            {
                if (Splined_Type.SelectedIndex != 0)
                {
                    Splined_Arc_Length.Enabled = true;
                    Splined_Length.Enabled = true;
                }
            }

            set_splinet_image();
        }

        #endregion

        private void Splined_Length_ValueChanged(object sender, EventArgs e)
        {
            if (Splined_Type.SelectedIndex != 0)
            {
                Splined_Arc_Length.Maximum = (decimal) 0.9*Splined_Length.Value;
            }
        }

        private void Splined_Radius_ValueChanged(object sender, EventArgs e)
        {
            set_splined_array();
        }

        private void Stage_Preview_MouseDoubleClick(object sender, MouseEventArgs e) // maximaze promp image preview
        {
            GC.Collect();
            LargePromp img_form = new LargePromp(Stage_Preview.Image as Image);
            img_form.Show();
            img_form.Focus();
        }

        private void Theard_Use_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Theard_Use.SelectedIndex == 0)
            {
                Theard_Type.Enabled = false;
                Theard_Radius.Enabled = false;
                Theard_Pitch.Enabled = false;
                Theard_Fit.Enabled = false;
                Theard_CW.Enabled = false;

                draw_preview();
            }
            else
            {
                Theard_Type.Enabled = true;

                if (Theard_Fit.Checked == false)
                {
                    Theard_Radius.Enabled = true;
                }

                Theard_Pitch.Enabled = true;

                Theard_Fit.Enabled = true;
                Theard_CW.Enabled = true;

                Splined_Type.SelectedIndex = 0;
                Key_seating_type.SelectedIndex = 0;
                Key_profile_type.SelectedIndex = 0;

                set_theard_promp_image();
            }
        }


        private void set_theard_fit()
        {
            set_theard_range();
            if (Theard_Fit.Checked == true && Profile_type.SelectedIndex == 0)
            {
                Theard_Radius.Value = Profile_R1.Value - Theard_Pitch.Value*2;
                Theard_Radius.Enabled = false;
            }
            else
            {
                Theard_Radius.Enabled = true;
            }
        }

        private void set_theard_pinch_range()
        {
            Theard_Pitch.Maximum = Profile_R1.Value/(decimal) 2.5;
        }

        private void Theard_Fit_CheckedChanged(object sender, EventArgs e)
        {
            set_theard_pinch_range();
            set_theard_fit();
        }

        private void Theard_Pitch_ValueChanged(object sender, EventArgs e)
        {
            set_theard_fit();
        }

        private void Auto_Name_CheckedChanged(object sender, EventArgs e)
        {
            name_builder();
        }

        private void set_theard_promp_image()
        {
            if (Theard_Use.SelectedIndex != 0)
            {
                if (Theard_Type.SelectedIndex == 0)
                {
                    Stage_Preview.Image = Shaft_Work.Properties.Resources.Theard_Triangle as Image;
                }
                if (Theard_Type.SelectedIndex == 1)
                {
                    Stage_Preview.Image = Shaft_Work.Properties.Resources.Theard_Trapaze as Image;
                }
            }
        }

        private void Theard_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_theard_promp_image();
        }


        #endregion

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SLDConnect = null;
            SwApp = null;
            SwModel = null;
            MyShaft = null;
            GC.Collect();
        }
    }
}
