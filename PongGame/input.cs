using MTV3D65;

namespace PongGame
{
    public class Input
    {
        private readonly GameManager _gameManager;
        private TV_3DVECTOR _camerapos;
        private float _cameraRotation;

        public Input(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Load()
        {
            _camerapos = _gameManager.TvCamera.GetPosition();
        }

        private void MoveBat(Player player)
        {
            TV_3DVECTOR batPos = player.Bat.GetPosition();
            Area moveArea = player.Bat.MoveArea;

            if (_gameManager.TvInputEngine.IsKeyPressed(player.UpKey)) batPos.z += 0.001f;
            if (_gameManager.TvInputEngine.IsKeyPressed(player.DownKey)) batPos.z -= 0.001f;

            if (_gameManager.TvInputEngine.IsKeyPressed(player.LeftKey)) batPos.x -= 0.001f;
            if (_gameManager.TvInputEngine.IsKeyPressed(player.RightKey)) batPos.x += 0.001f;

            //make sure bat is inside area
            if (batPos.x < moveArea.X1) batPos.x = moveArea.X1;
            if (batPos.x > moveArea.X2) batPos.x = moveArea.X2;

            //make sure bat is inside area
            if (batPos.z < moveArea.Y1) batPos.z = moveArea.Y1;
            if (batPos.z > moveArea.Y2) batPos.z = moveArea.Y2;

            player.Update(batPos.x, batPos.z);
        }

        public void Update()
        {
            _gameManager.TvInputEngine.ForceUpdate();

            //Player 1
            MoveBat(_gameManager.Player1);

            //Player 2
            MoveBat(_gameManager.Player2);

            //Camera
            if (_gameManager.TvInputEngine.IsKeyPressed(CONST_TV_KEY.TV_KEY_I)) _camerapos.z += 0.1f;
            if (_gameManager.TvInputEngine.IsKeyPressed(CONST_TV_KEY.TV_KEY_K)) _camerapos.z -= 0.1f;

            if (_gameManager.TvInputEngine.IsKeyPressed(CONST_TV_KEY.TV_KEY_J)) _camerapos.x -= 0.1f;
            if (_gameManager.TvInputEngine.IsKeyPressed(CONST_TV_KEY.TV_KEY_L)) _camerapos.x += 0.1f;

            if (_gameManager.TvInputEngine.IsKeyPressed(CONST_TV_KEY.TV_KEY_N)) _camerapos.y += 0.1f;
            if (_gameManager.TvInputEngine.IsKeyPressed(CONST_TV_KEY.TV_KEY_M)) _camerapos.y -= 0.1f;

            _gameManager.TvCamera.SetPosition(_camerapos.x, _camerapos.y, _camerapos.z);

            _cameraRotation = 0;
            if (_gameManager.TvInputEngine.IsKeyPressed(CONST_TV_KEY.TV_KEY_U)) _cameraRotation = 0.0005f;
            if (_gameManager.TvInputEngine.IsKeyPressed(CONST_TV_KEY.TV_KEY_O)) _cameraRotation = -0.0005f;

            _gameManager.TvCamera.RotateX(_cameraRotation,false);

            //Reload Level
            if (_gameManager.TvInputEngine.IsKeyPressed(CONST_TV_KEY.TV_KEY_R)) _gameManager.CurrentLevel.ReLoad();

            //End Game
            if (_gameManager.TvInputEngine.IsKeyPressed(CONST_TV_KEY.TV_KEY_ESCAPE)) _gameManager.Running = false;
        }
    }
}
