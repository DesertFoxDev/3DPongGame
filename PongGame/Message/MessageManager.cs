using System.Collections.Generic;

using MTV3D65;

namespace PongGame
{
    public class MessageManager
    {
        readonly GameManager _gameManager;

        private Message _currentMessage;
        private readonly Queue<Message> _queue;

        private float _beginShowTime;

        public MessageManager(GameManager gameManager)
        {
            _gameManager = gameManager;
            _queue = new Queue<Message>();
        }

        public void AddMessage(Message message, bool queue)
        {
            if (queue)
            {
                if (_currentMessage!=null) _queue.Enqueue(message);
                else
                {
                    _currentMessage = message;
                    _beginShowTime = _gameManager.TotalTimeElapsed;
                }
            }
            else
            {
                _queue.Clear();
                _currentMessage = message;
                _beginShowTime = _gameManager.TotalTimeElapsed;
            }

        }

        public void Update()
        {
            if (_currentMessage != null)
            {
                if (_queue.Count > 0)
                {
                    if (_gameManager.TotalTimeElapsed > _beginShowTime + _currentMessage.Duration)
                    {
                        _currentMessage = _queue.Dequeue();
                        _beginShowTime = _gameManager.TotalTimeElapsed;
                    }
                }
                else
                {
                    if (_gameManager.TotalTimeElapsed > _beginShowTime + _currentMessage.Duration)
                    {
                        _currentMessage = null;
                    }
                }
            }
        }

        public void Render()
        {
            if (_currentMessage != null)
            {
                int posX = GameManager.ScreenWidth / 2 - _currentMessage.Text.Length * 10;
                int posY = GameManager.ScreenHeight / 2;
                _gameManager.TvScreen2DText.TextureFont_DrawTextScaled(_currentMessage.Text, posX, posY, (int)CONST_TV_COLORKEY.TV_COLORKEY_WHITE, 3,3);

                if (_currentMessage.TextBottom != "")
                {
                    posX = GameManager.ScreenWidth / 2 - _currentMessage.TextBottom.Length * 10;
                    posY = GameManager.ScreenHeight / 2 + 30;
                    _gameManager.TvScreen2DText.TextureFont_DrawTextScaled(_currentMessage.TextBottom, posX, posY, (int)CONST_TV_COLORKEY.TV_COLORKEY_WHITE, 3, 3);
                }
            }
        }
    }
}
