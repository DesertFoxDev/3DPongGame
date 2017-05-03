namespace PongGame
{
    public class Round
    {
        readonly GameManager _gameManager;

        private bool _starting;
        private float _startTime;
        
        public bool Playing { get; private set; }

        public Round(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void StartNew()
        {
            Playing = false;

            // New round in 3..2...1..
            _startTime = _gameManager.TotalTimeElapsed;
            _gameManager.CurrentLevel.Ball.Reset();
            _gameManager.Message.AddMessage(new Message("Starting new Round!","3..", 1000),true);
            _gameManager.Message.AddMessage(new Message("Starting new Round!", ".2.", 1000), true);
            _gameManager.Message.AddMessage(new Message("Starting new Round!", "..1", 1000), true);
            _starting = true;
        }

        public void Update()
        {
            if (_starting)
            {
                // Push ball after 3 seconds
                if (_gameManager.TotalTimeElapsed > _startTime + 3000)
                {
                    _starting = false;
                    _gameManager.CurrentLevel.Ball.PushBall();
                    Playing = true;
                }
            }
        }
    }
}
