using System.Windows.Forms;

namespace PongGame
{
    public partial class Form1 : Form
    {
        private readonly GameManager _gameManager;

        public Form1(GameManager gameManager)
        {
            InitializeComponent();
            _gameManager = gameManager;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _gameManager.Running = false;
        }
    }
}