using MTV3D65;

namespace PongGame
{
    public class Player
    {
        private readonly GameManager _gameManager;

        public int Id { get; private set; }

        public int Score { get; set; }

        public Bat Bat { get; private set; }

        public CONST_TV_KEY UpKey { get; private set; }
        public CONST_TV_KEY DownKey { get; private set; }
        public CONST_TV_KEY LeftKey { get; private set; }
        public CONST_TV_KEY RightKey { get; private set; }

        public Player(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Load(int id, CONST_TV_KEY upKey, CONST_TV_KEY downKey, CONST_TV_KEY leftKey, CONST_TV_KEY rightKey)
        {
            Id = id;
            UpKey = upKey;
            DownKey = downKey;
            LeftKey = leftKey;
            RightKey = rightKey;

            Bat = new Bat(_gameManager);
        }

        public void Update(float inputX, float inputY)
        {
            Bat.Update(inputX, inputY);      
        }

        public void Render()
        {
            Bat.Render();
        }
    }
}
