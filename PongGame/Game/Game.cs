using MTV3D65;

namespace PongGame
{
    public class Game
    {
        readonly GameManager _gameManager;
        private Round _round;

        private bool _resetPending;
        private float _resetTime;

        private bool _newGamePending;
        private float _newGameTime;

        private int _roundNumber;
        private readonly int _roundLimit;

        public Game(GameManager gameManager)
        {
            _gameManager = gameManager;
            _roundLimit = 5;
        }

        public void Start()
        {
            // Reset scores
            _gameManager.Player1.Score = 0;
            _gameManager.Player2.Score = 0;
            _roundNumber = 0;

            // Start new round
            _round = new Round(_gameManager);
            _round.StartNew();
        }

        private void CheckForGoal(Player goalOwner, Area goalArea, float goalHeight, Player scoringPlayer)
        {
            if (goalArea.IsPointInside(_gameManager.CurrentLevel.Ball.GetPosition().x, _gameManager.CurrentLevel.Ball.GetPosition().z))
            {
                if (_gameManager.CurrentLevel.Ball.GetPosition().y < goalHeight)
                {
                    // Player scores
                    scoringPlayer.Score++;
                    _gameManager.Message.AddMessage(new Message("Player " + scoringPlayer.Id + " Scores!", scoringPlayer.Score + ":" + goalOwner.Score, 3000), false);
                    _resetPending = true;
                    _resetTime = _gameManager.TotalTimeElapsed;
                }
            }
        }

        private void CheckBallPosition()
        {
            // Check ball position
            if (_round.Playing && !_resetPending)
            {
                TV_3DVECTOR linVel = _gameManager.CurrentLevel.Ball.GetLinVelocity();

                // Did player 2 score a goal?
                CheckForGoal(_gameManager.Player1, _gameManager.CurrentLevel.Player1GoalArea, _gameManager.CurrentLevel.Player1GoalHeight,
                    _gameManager.Player2);

                // Did player 1 score a goal?
                CheckForGoal(_gameManager.Player2, _gameManager.CurrentLevel.Player2GoalArea, _gameManager.CurrentLevel.Player2GoalHeight,
                    _gameManager.Player1);

                // Ball is not moving
                if (_gameManager.TvMaths.TVVec3Length(linVel) < 3f)
                {
                    _gameManager.Message.AddMessage(new Message("Ball too slow!", "", 3000), false);
                    _resetPending = true;
                    _resetTime = _gameManager.TotalTimeElapsed;
                }

                // Ball has fallen down
                else if (_gameManager.CurrentLevel.Ball.GetPosition().y < -1000)
                {
                    _gameManager.Message.AddMessage(new Message("Ball gone!", "", 3000), false);
                    _resetPending = true;
                    _resetTime = _gameManager.TotalTimeElapsed;
                }
            }
        }

        private void CheckForRoundLimit()
        {
            if (_roundNumber >= _roundLimit)
            {
                if (_gameManager.Player1.Score > _gameManager.Player2.Score)
                {
                    // Player 1 wins
                    _gameManager.Message.AddMessage(new Message("Player 1 Wins!", "", 4000), false);
                    _gameManager.Message.AddMessage(new Message("New Game in", "3..", 1000), true);
                    _gameManager.Message.AddMessage(new Message("New Game in", ".2.", 1000), true);
                    _gameManager.Message.AddMessage(new Message("New Game in", "..1", 1000), true);
                }
                else if (_gameManager.Player2.Score > _gameManager.Player1.Score)
                {
                    // Player 2 wins
                    _gameManager.Message.AddMessage(new Message("Player 2 Wins!", "", 4000), false);
                    _gameManager.Message.AddMessage(new Message("New Game in", "3..", 1000), true);
                    _gameManager.Message.AddMessage(new Message("New Game in", ".2.", 1000), true);
                    _gameManager.Message.AddMessage(new Message("New Game in", "..1", 1000), true);
                }
                else if (_gameManager.Player1.Score == _gameManager.Player2.Score)
                {
                    // Draw
                    _gameManager.Message.AddMessage(new Message("Draw!", "", 4000), false);
                    _gameManager.Message.AddMessage(new Message("New Game in", "3..", 1000), true);
                    _gameManager.Message.AddMessage(new Message("New Game in", ".2.", 1000), true);
                    _gameManager.Message.AddMessage(new Message("New Game in", "..1", 1000), true);
                }

                _resetPending = false;
                _newGamePending = true;
                _newGameTime = _gameManager.TotalTimeElapsed;
            }
        }

        public void Update()
        {
            if (_newGamePending)
            {
                // Start new game after 7 seconds
                if (_gameManager.TotalTimeElapsed > _newGameTime + 7000)
                {
                    _newGamePending = false;
                    Start();
                }
            }
            else
            {
                CheckBallPosition();

                CheckForRoundLimit();

                if (_resetPending)
                {
                    if (_gameManager.TotalTimeElapsed > _resetTime + 3500)
                    {
                        // Start new round
                        _resetPending = false;
                        _round.StartNew();
                        _roundNumber++;
                    }
                }

                _round.Update();
            }
            
        }
    }
}
