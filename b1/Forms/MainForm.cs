using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Mogre;

namespace b1.Forms
{
    public partial class MainForm : Form
    {
        protected OgreWindow mogreWin;

        public MainForm()
        {
            InitializeComponent();
            this.Disposed += new EventHandler(MogreForm_Disposed);

            mogreWin = new OgreWindow(new Point(100, 30), mogrePanel.Handle);
            mogreWin.InitMogre();
        }

        void MogreForm_Disposed(object sender, EventArgs e)
        {
            mogreWin.Dispose();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            mogreWin.Paint();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            mogreWin.ClearScreen();
        }
    }

    public class OgreWindow
    {
        public Root root;
        public SceneManager sceneMgr;
        
        protected Camera camera;
        protected Viewport viewport;
        protected RenderWindow window;
        protected Point position;
        protected IntPtr hWnd;

        public OgreWindow(Point origin, IntPtr hWnd)
        {
            position = origin;
            this.hWnd = hWnd;
        }

        public void InitMogre()
        {
            // init
            root = new Root();

            bool foundit = false;
            foreach (RenderSystem rs in root.GetAvailableRenderers())
            {
                root.RenderSystem = rs;
                String rname = root.RenderSystem.Name;
                if (rname == "Direct3D9 Rendering Subsystem")
                {
                    foundit = true;
                    break;
                }
            }

            // Config resources
            ConfigFile cf = new ConfigFile();
            cf.Load("resources.cfg", "\t:=", true);

            ConfigFile.SectionIterator seci = cf.GetSectionIterator();

            String secName, typeName, archName;

            // Normally we would use the foreach syntax, which enumerates the values, but in this case we need CurrentKey too;
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName = pair.Key;
                    archName = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
                }
            }

            // init resume 

            root.RenderSystem.SetConfigOption("Full Screen", "No");
            root.RenderSystem.SetConfigOption("Video Mode", "640 x 480 @ 32-bit colour");

            root.Initialise(false);
            NameValuePairList misc = new NameValuePairList();
            misc["externalWindowHandle"] = hWnd.ToString();
            window = root.CreateRenderWindow("Simple Mogre Form Window", 0, 0, false, misc);

            sceneMgr = root.CreateSceneManager(SceneType.ST_GENERIC, "SceneMgr");
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            camera = sceneMgr.CreateCamera("SimpleCamera");
            camera.Position = new Vector3(0f, 0f, 100f);
            // Look back along -Z
            camera.LookAt(new Vector3(0f, 0f, -300f));
            camera.NearClipDistance = 5;

            viewport = window.AddViewport(camera);
            viewport.BackgroundColour = new ColourValue(0.0f, 0.0f, 0.0f, 1.0f);

            Entity ent = sceneMgr.CreateEntity("ogre", "ogrehead.mesh");
            SceneNode node = sceneMgr.RootSceneNode.CreateChildSceneNode("ogreNode");
            node.AttachObject(ent);
        }

        public void Paint()
        {
            root.RenderOneFrame();
        }

        public void Dispose()
        {
            if (root != null)
            {
                root.Dispose();
                root = null;
            }
        }

        public void ClearScreen()
        {
            sceneMgr.RootSceneNode.DetachAllObjects();
        }
    }
}
