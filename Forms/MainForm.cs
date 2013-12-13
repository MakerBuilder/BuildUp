using System.Windows.Forms;

namespace b1.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            MogrePanelHandler panelHandler = new MogrePanelHandler(panelMogre);
        }
    }
}
