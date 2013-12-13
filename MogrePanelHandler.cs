using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mogre;

namespace b1
{
    class MogrePanelHandler
    {
        private Root root;
        private SceneManager sceneMgr;
        private Camera camera;
        private Viewport viewport;
        private RenderWindow window;
        private Point position;
        private IntPtr hWnd;
        private Panel parentPanel;

        public MogrePanelHandler(Panel panel)
        {
            parentPanel = panel;
            root = new Root();
            // Set to D39 driver
            foreach (RenderSystem rs in root.GetAvailableRenderers())
            {
                root.RenderSystem = rs;
                String rname = root.RenderSystem.Name;
                if (rname == "Direct3D9 Rendering Subsystem")
                {
                    break;
                }
            }

            // Set up video mode settings
            root.RenderSystem.SetConfigOption("Full Screen", "No");
            root.RenderSystem.SetConfigOption("Video Mode", "640 x 480 @ 32-bit colour");

            // init root
            root.Initialise(false);

            // create panel rendering
            NameValuePairList misc = new NameValuePairList();
            misc["externalWindowHandle"] = panel.Handle.ToString();
            window = root.CreateRenderWindow("", 0, 0, false, misc);

            // add events ahndlers
            panel.Paint += panel_Paint;
            panel.Disposed += panel_Disposed;
        }

        void panel_Disposed(object sender, EventArgs e)
        {
            root.Dispose();
        }

        void panel_Paint(object sender, PaintEventArgs e)
        {
            root.RenderOneFrame();
        }
    }
}
