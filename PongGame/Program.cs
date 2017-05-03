using System;
using System.Windows.Forms;

namespace PongGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GameManager gameManager = new GameManager();
            gameManager.StartGame();
        }
    }
}