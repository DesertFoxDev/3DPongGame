using MTV3D65;

namespace PongGame
{
    public class Ball
    {
        readonly GameManager _gameManager;
        private TVMesh _mesh;
        private int _physicsBody;
        private TV_3DVECTOR _startPos;
        private bool _visible;

        public Ball(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Load(TV_3DVECTOR startPosition)
        {
            _startPos = startPosition;

            // Mesh
            _mesh = _gameManager.TvScene.CreateMeshBuilder();
            _mesh.CreateSphere(4.5f);

            _mesh.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_NORMAL);
            _mesh.SetMaterial(_gameManager.CurrentLevel.StandardMaterial);
            _mesh.ComputeNormalsEx();

            // Physics
            _physicsBody = _gameManager.TvPhysics.CreateMeshBody(0.01f, _mesh, CONST_TV_PHYSICSBODY_BOUNDING.TV_BODY_BOX);

            _gameManager.TvPhysics.SetBodyMaterialGroup(_physicsBody, 0);
            _gameManager.TvPhysics.SetDamping(_physicsBody, 0.00001f, new TV_3DVECTOR());

            _gameManager.TvPhysics.SetBodyPosition(_physicsBody, _startPos.x, _startPos.y, _startPos.z);
        }

        public void Unload()
        {
            _gameManager.TvPhysics.DestroyBody(_physicsBody);
            _mesh.Destroy();
            _mesh = null;
        }

        public void SetVisible(bool visible)
        {
            _visible = visible;
            if (_visible) _mesh.SetShadowCast(true, true);
            else _mesh.SetShadowCast(false, false);
        }

        public void Reset()
        {
            SetVisible(true);
            _gameManager.TvPhysics.SetBodyLinearVelocity(_physicsBody, new TV_3DVECTOR());
            _gameManager.TvPhysics.SetBodyAngularVelocity(_physicsBody, new TV_3DVECTOR());
            _gameManager.TvPhysics.SetBodyPosition(_physicsBody, _startPos.x, _startPos.y, _startPos.z);
        }

        public void PushBall()
        {
            int rotation = _gameManager.CurrentLevel.Random.Next(0, 6);
            TV_2DVECTOR impulse = new TV_2DVECTOR(50f, 50f);
            impulse = RotateVector(impulse, rotation);

            _gameManager.TvPhysics.AddImpulse(_physicsBody, new TV_3DVECTOR(impulse.x,0,impulse.y));
        }

        public TV_2DVECTOR RotateVector(TV_2DVECTOR vector, float radians)
        {
            TV_2DVECTOR outVector = new TV_2DVECTOR
            {
                x = vector.x * (float) System.Math.Cos(radians) - vector.y * (float) System.Math.Sin(radians),
                y = vector.y * (float) System.Math.Cos(radians) + vector.x * (float) System.Math.Sin(radians)
            };

            return outVector;
        }

        public TV_3DVECTOR GetPosition()
        {
            return _gameManager.TvPhysics.GetBodyPosition(_physicsBody);
        }

        public TV_3DVECTOR GetLinVelocity()
        {
            return _gameManager.TvPhysics.GetBodyLinearVelocity(_physicsBody);
        }


        public void Render()
        {
            if(_visible) _mesh.Render();
        }

      
    }
}
