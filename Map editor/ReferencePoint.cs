using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Map_editor
{
    public partial class ReferencePoint : UserControl
    {
        public int X { get { return this.Location.X; }  }
        public int Y { get { return this.Location.Y; }  }
        public ReferencePoint(Point Location)
        {
            InitializeComponent();
            this.Left = Location.X;
            this.Top = Location.Y;
            this.GotFocus += ReferencePoint_GotFocus;
        }

        void ReferencePoint_GotFocus(object sender, EventArgs e)
        {
            this.BackColor = Color.Red;
        }

        private void ReferencePoint_Load(object sender, EventArgs e)
        {

        }

        public void Unfocus()
        {
            this.BackColor = Color.Yellow;
        }

        private void ReferencePoint_Leave(object sender, EventArgs e)
        {
            //this.BackColor = Color.Yellow;
        }

        private void ReferencePoint_Enter(object sender, EventArgs e)
        {
            this.BackColor = Color.Red;
        }
        private Point MouseDownLocation;

        private void ReferencePoint_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }

        private void ReferencePoint_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Left = e.X + this.Left - MouseDownLocation.X;
                this.Top = e.Y + this.Top - MouseDownLocation.Y;
            }
        }
    }
}
