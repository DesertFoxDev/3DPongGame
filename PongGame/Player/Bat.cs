using MTV3D65;

namespace PongGame
{
    public class Bat
    {
        readonly GameManager _gameManager;
        private TVMesh _mesh;
        private int _physicsBody;
        public Area MoveArea { get; private set; }
    
        public Bat(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Load(Area moveArea)
        {
            MoveArea = moveArea;

            _mesh = new TVMesh();
            _mesh = _gameManager.TvScene.CreateMeshBuilder();
            _mesh.CreateCylinder(15, 10, 100);

            _mesh.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_NORMAL);
            _mesh.SetMaterial(_gameManager.CurrentLevel.StandardMaterial);
            _mesh.ComputeNormalsEx();
            _mesh.SetShadowCast(true, true);

            _physicsBody = _gameManager.TvPhysics.CreateMeshBody(1000000, _mesh, CONST_TV_PHYSICSBODY_BOUNDING.TV_BODY_CYLINDER);

            _gameManager.TvPhysics.SetBodyMaterialGroup(_physicsBody, 0);

            _gameManager.TvPhysics.SetBodyPosition(_physicsBody, MoveArea.MidPoint.x, 14f, MoveArea.MidPoint.y);
        }

        public void Unload()
        {
            _gameManager.TvPhysics.DestroyBody(_physicsBody);
            _mesh.Destroy();
            _mesh = null;
        }


        public void Update(float posX, float posY)
        {
            TV_3DVECTOR oldPos = _gameManager.TvPhysics.GetBodyPosition(_physicsBody);

            if (MoveArea.IsPointInside(posX, posY))
            {
                // move the bat
                TV_3DVECTOR linVel = new TV_3DVECTOR();

                if (posY > oldPos.z) linVel.z = 100;
                else if (posY < oldPos.z) linVel.z = -100;

                if (posX > oldPos.x) linVel.x = 100;
                else if (posX < oldPos.x) linVel.x = -100;

                _gameManager.TvPhysics.SetBodyLinearVelocity(_physicsBody, linVel);
            }
        }

        public void Render()
        {
            _mesh.Render();
        }

        public TV_3DVECTOR GetPosition()
        {
            return _gameManager.TvPhysics.GetBodyPosition(_physicsBody);
        }
    }
}
