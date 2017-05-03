namespace PongGame
{
    public class Message
    {
        public string Text { get; }

        public string TextBottom { get; }

        public float Duration { get; }

        public Message(string text, string textBottom, float duration)
        {
            Text = text;
            TextBottom = textBottom;
            Duration = duration;
        }
    }
}
