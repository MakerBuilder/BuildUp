using System;
using Mogre;
using MOIS;

namespace b1
{
    class GameApp
    {
        private Root root;
        private RenderWindow window;
        private SceneManager sceneManager;
        private Overlay debugOverlay;

        private MOIS.InputManager inputManager;
        private MOIS.Keyboard inputKeyboard;

        private bool isShutdown = false;

        public GameApp()
        {
            root = new Root();
            
            // Показываем окно настройки
            if (root.ShowConfigDialog())
            {
                window = root.Initialise(true);
            }

            sceneManager = root.CreateSceneManager(SceneType.ST_GENERIC, "sceneManager");

            debugOverlay = OverlayManager.Singleton.GetByName("Core/DebugOverlay");
            debugOverlay.Show();

            root.FrameStarted += new FrameListener.FrameStartedHandler(root_FrameStarted);

            LogManager.Singleton.LogMessage("init ois");
            MOIS.ParamList paramList = new ParamList();
            IntPtr windowHwnd;
            window.GetCustomAttribute("WINDOW", out windowHwnd);
            paramList.Insert("WINDOW", windowHwnd.ToString());
            inputManager = MOIS.InputManager.CreateInputSystem(paramList);
            inputKeyboard = (MOIS.Keyboard) inputManager.CreateInputObject(MOIS.Type.OISKeyboard, false);

            // start app
            root.StartRendering();

            // end 
            root.Dispose();
            root = null;
        }

        bool root_FrameStarted(FrameEvent evt)
        {
            if (window.IsClosed)
                return false;

            HandleEvent(evt);

            return !isShutdown;
        }

        private void HandleEvent(FrameEvent evt)
        {
            inputKeyboard.Capture();

            if (inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
            {
                isShutdown = true;
            }
        }
    }
}
