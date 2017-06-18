using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shaft_Work
{
    public partial class LargePromp : Form
    {
        public LargePromp(Image img)
        {
            InitializeComponent();
            Large_img.Image = img;
            Large_img.Update();

        }

       
    }
}
