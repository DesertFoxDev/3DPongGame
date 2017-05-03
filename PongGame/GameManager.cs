using MTV3D65;

// Pong Game by Martin Schemmer
//
// More TV3D Samples available at http://tv3dsamples.blogspot.com/
//
// Controls: 
//   Player1: WASD 
//   Player2: Arrow Keys
//   Camera: IJKL NM

namespace PongGame
{
    public class GameManager
    {
        public TVEngine TvEngine { get; }

        public TVScene TvScene { get; }

        public TVCamera TvCamera { get; }

        public TVInputEngine TvInputEngine { get; }

        public TVPhysics TvPhysics { get; }

        public TVMathLibrary TvMaths { get; }

        public TVScreen2DText TvScreen2DText { get; }

        public TVLightEngine TvLights { get; }

        public TVMaterialFactory TvMaterials { get; }

        public TVGlobals TvGlobals { get; }

        public bool Running { get; set; }

        public float TotalTimeElapsed { get; private set; }

        public Game Game;
        public Level CurrentLevel;
        public Player Player1, Player2;
        public Input Input;
        public MessageManager Message;

        static bool windowed = true;
        public static int ScreenWidth = 800;
        public static int ScreenHeight = 600;

        public GameManager()
        {
            TotalTimeElapsed = 0;

            TvEngine = new TVEngine();
            TvEngine.DisplayFPS(true);

            if (windowed)
            {
                Form1 form = new Form1(this);
                form.Show();
                TvEngine.Init3DWindowed(form.Handle);
            }
            else
            {
                TvEngine.Init3DFullscreen(ScreenWidth, ScreenHeight);
            }

            TvInputEngine = new TVInputEngine();
            TvInputEngine.Initialize(true, true);

            TvScene = new TVScene();
            TvCamera = TvScene.GetCamera();

            TvPhysics = new TVPhysics();
            TvPhysics.Initialize();
            TvPhysics.SetSolverModel(CONST_TV_PHYSICS_SOLVER.TV_SOLVER_LINEAR_10_PASSES);
            TvPhysics.SetFrictionModel(CONST_TV_PHYSICS_FRICTION.TV_FRICTION_EXACT);
            TvPhysics.SetGlobalGravity(new TV_3DVECTOR(0F, -980F, 0F));

            TvPhysics.SetMaterialInteractionContinuousCollision(0, 0, true);
            TvPhysics.SetMaterialInteractionFriction(0, 0, 0.000001f, 0.000001f);
            TvPhysics.SetMaterialInteractionBounciness(0, 0, 0.5f);

            TvMaths = new TVMathLibrary();

            TvScreen2DText = new TVScreen2DText();

            TvLights = new TVLightEngine();
            TvMaterials = new TVMaterialFactory();
            TvGlobals = new TVGlobals();

            Message = new MessageManager(this);
        }

        public void LoadLevel()
        {
            CurrentLevel = new Level(this);
            CurrentLevel.Load();

            Input = new Input(this);
            Input.Load();
        }

        public void LoadPlayers()
        {
            Player1 = new Player(this);
            Player1.Load(1, CONST_TV_KEY.TV_KEY_W, CONST_TV_KEY.TV_KEY_S, CONST_TV_KEY.TV_KEY_A, CONST_TV_KEY.TV_KEY_D);

            Player2 = new Player(this);
            Player2.Load(2, CONST_TV_KEY.TV_KEY_UPARROW, CONST_TV_KEY.TV_KEY_DOWNARROW, CONST_TV_KEY.TV_KEY_LEFTARROW, CONST_TV_KEY.TV_KEY_RIGHTARROW);
        }

        public void StartGame()
        {
            LoadPlayers();
            LoadLevel();

            Game = new Game(this);
            Game.Start();

            MainLoop();
        }

        public void MainLoop()
        {
            Running = true;
            while (Running)
            {
                TvEngine.Clear(false);

                // Input
                Input.Update();

                // Gameplay
                Game.Update();

                // Level
                CurrentLevel.Render();
                TvScene.FinalizeShadows();

                // Players and Ball
                CurrentLevel.Ball.Render();
                Player1.Render();
                Player2.Render();

                // UI
                TvScreen2DText.Action_BeginText();
                Message.Render();
                TvScreen2DText.Action_BeginText();
                TvEngine.RenderToScreen();
                Message.Update();

                // Physics
                TvPhysics.Simulate(TvEngine.AccurateTimeElapsed() / 1000f);

                // Windows events
                System.Windows.Forms.Application.DoEvents();

                TotalTimeElapsed += TvEngine.AccurateTimeElapsed();
            }
        }
    }
}
