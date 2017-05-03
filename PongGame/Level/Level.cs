using System;

using MTV3D65;

namespace PongGame
{
    public class Level
    {
        readonly GameManager _gameManager;
        private TVMesh _field;
        private int _fieldPhysicsBody;
        private int _light;

        public Random Random { get; set; }

        public Ball Ball { get; set; }

        public int StandardMaterial { get; set; }

        public Area Player1GoalArea { get; set; }

        public float Player1GoalHeight { get; set; }

        public Area Player2GoalArea { get; set; }

        public float Player2GoalHeight { get; set; }

        public Level(GameManager gameManager)
        {
            _gameManager = gameManager;
            Random = new Random();
        }

        public void Load()
        {
            // Camera
            _gameManager.TvCamera.SetPosition(100, 150, 50);
            _gameManager.TvCamera.SetLookAt(100, 0, 55);

            // Scene and lighting
            _gameManager.TvScene.SetShadowParameters(_gameManager.TvGlobals.RGBA(0.1f,0.1f,0.1f,0.5f), false);
            _gameManager.TvScene.SetBackgroundColor(0, 0, 0);
            _light = _gameManager.TvLights.CreatePointLight(new TV_3DVECTOR(100, 200, -100), 0.2f, 0.2f, 0.2f,500);
            _gameManager.TvLights.SetLightProperties(_light, false, true, true);
            _gameManager.TvLights.EnableLight(_light, true);
            StandardMaterial = _gameManager.TvMaterials.CreateLightMaterial(1, 1, 1, 1);           

            // Walls and floor. 100 units = 1m
            _field = _gameManager.TvScene.CreateMeshBuilder();
            _field.AddWall3D(0, 0, 50f, 200, 50f, 10f, 100);  // floor
            _field.AddWall3D(0, 0, 105f, 200, 105f, 20f, 10f, false, false, 0, 0.1f, 1f);  // top side
            _field.AddWall3D(0, 0, -5f, 200, -5f, 20f, 10f, false, false, 0, 0.1f, 1f);  // bottom side

            _field.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_NORMAL);
            _field.SetMaterial(StandardMaterial);
            _field.SetShadowCast(true, true);
            _field.ComputeNormalsEx();
           
            _fieldPhysicsBody = _gameManager.TvPhysics.CreateStaticMeshBody(_field);
            _gameManager.TvPhysics.SetBodyMaterialGroup(_fieldPhysicsBody, 0);

            // Ball
            Ball = new Ball(_gameManager);
            Ball.Load(new TV_3DVECTOR(100f, 20f, 55f));

            // Player 1
            Area player1MoveArea = new Area(20, 25, 50, 75);
            Player1GoalArea = new Area(-50, 0, 50, 100);
            Player1GoalHeight = -20;
            _gameManager.Player1.Bat.Load(player1MoveArea);

            // Player 2
            Area player2MoveArea = new Area(150, 25, 180, 75);
            Player2GoalArea = new Area(150, 0, 250, 100);
            Player2GoalHeight = -20;
            _gameManager.Player2.Bat.Load(player2MoveArea);
        }

        public void Unload()
        {
            _gameManager.TvPhysics.DestroyBody(_fieldPhysicsBody);
            _field.Destroy();
            _field = null;
            _gameManager.TvMaterials.DeleteMaterial(StandardMaterial);

            _gameManager.TvLights.DeleteLight(_light);

            _gameManager.Player1.Bat.Unload();
            _gameManager.Player2.Bat.Unload();

            Ball.Unload();
        }

        public void ReLoad()
        {
            TV_3DVECTOR cameraPos = _gameManager.TvCamera.GetPosition();
            TV_3DVECTOR lookAtPos = _gameManager.TvCamera.GetLookAt();
            Unload();
            Load();
            _gameManager.TvCamera.SetPosition(cameraPos.x, cameraPos.y, cameraPos.z);
            _gameManager.TvCamera.SetLookAt(lookAtPos.x, lookAtPos.y, lookAtPos.z);

        }


        public void Render()
        {
            _field.Render();
            RenderAreas();
        }

        public void RenderAreas()
        {
            //Debug boxes for help with level design

            //_gameManager.TVScreen2D.Action_Begin2D();

            //_gameManager.TVScreen2D.Draw_Box3D(new TV_3DVECTOR(Player1GoalArea.X1, Player1GoalHeight, Player1GoalArea.Y1),
            //                                   new TV_3DVECTOR(Player1GoalArea.X2, Player1GoalHeight - 10, Player1GoalArea.Y2),
            //                                   (int)CONST_TV_COLORKEY.TV_COLORKEY_GREEN);

            //_gameManager.TVScreen2D.Draw_Box3D(new TV_3DVECTOR(Player2GoalArea.X1, Player2GoalHeight, Player2GoalArea.Y1),
            //                                   new TV_3DVECTOR(Player2GoalArea.X2, Player2GoalHeight - 10, Player2GoalArea.Y2),
            //                                   (int)CONST_TV_COLORKEY.TV_COLORKEY_GREEN);

            //_gameManager.TVScreen2D.Draw_Box3D(new TV_3DVECTOR(Player1MoveArea.X1, 0, Player1MoveArea.Y1),
            //                                   new TV_3DVECTOR(Player1MoveArea.X2, 10, Player1MoveArea.Y2),
            //                                   (int)CONST_TV_COLORKEY.TV_COLORKEY_YELLOW);

            //_gameManager.TVScreen2D.Draw_Box3D(new TV_3DVECTOR(Player2MoveArea.X1, 0, Player2MoveArea.Y1),
            //                                   new TV_3DVECTOR(Player2MoveArea.X2, 10, Player2MoveArea.Y2),
            //                                   (int)CONST_TV_COLORKEY.TV_COLORKEY_YELLOW);

            //_gameManager.TVScreen2D.Action_End2D();

        }

    }
}
