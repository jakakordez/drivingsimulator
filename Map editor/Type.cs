using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Map_editor
{
    public class Type
    {
        string name;
        public TreeNode Node;
        public string Name { get { return name; } set { name = value; Node.Text = value; } }
        public Type(TreeNode node)
        {
            Node = node;
        }
    }
}
