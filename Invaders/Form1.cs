/*
 * Created by Daniel Hopkins, based on the Invaders Lab
 * in Head First C# (2nd Edition).
 * E-Mail: dahopkin2@gmail.com
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Invaders
{
    public partial class Form1 : Form
    {
   
        int animationCounter;
        bool reverseInvaderAnimation;
        bool gameOver;
        Game game;
        Random random = new Random();
        List<Keys> keysPressed = new List<Keys>();
        public Form1()
        {
            InitializeComponent();
            StartNewGame();
        } // end constructor method Form1

        void StartNewGame() {
            game = new Game(this.ClientRectangle, random);
            game.GameOver += new EventHandler(game_GameOver);
            gameOver = false;
            reverseInvaderAnimation = false;
            animationCounter = 0;
            animationTimer.Enabled = true;
            gameTimer.Enabled = true;
        } // end method StartNewGame


        void game_GameOver(object sender, EventArgs e)
        {
            gameTimer.Stop();
            gameOver = true;
            Invalidate();
        } // end method game_GameOver

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            if (animationCounter == 3) reverseInvaderAnimation = true;
            else if (animationCounter == 0) reverseInvaderAnimation = false;

            if (reverseInvaderAnimation) animationCounter--;
            else animationCounter++;

            game.Twinkle();

            Refresh();

        } // end method animationTimer_Tick

        /*If the user types Q at any point, the game quits.
         If game is over and the user types S, the game restarts.
         If the user types space, a shot fires.
         */
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Q)
                Application.Exit();

            if (gameOver)
            {
                if (e.KeyCode == Keys.S)
                {
                    StartNewGame();
                    return;
                } 
            }

            if (e.KeyCode == Keys.Space)
               game.FirePlayerShot();

            if (keysPressed.Contains(e.KeyCode))
                keysPressed.Remove(e.KeyCode);

            keysPressed.Add(e.KeyCode);
        } // end method Form1_KeyDown

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (keysPressed.Contains(e.KeyCode))
                keysPressed.Remove(e.KeyCode);
        } // end method Form1_KeyUp

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            game.Go();

            foreach (Keys key in keysPressed) { 
                if(key == Keys.Left){
                    game.MovePlayer(Direction.Left);
                    return;
                } // end if

                else if (key == Keys.Right)
                {
                    game.MovePlayer(Direction.Right);
                    return;
                } // end else if

            } // end foreach
            
        } // end method gameTimer_Tick

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // check gameover at some point.
            Graphics g = e.Graphics;

            game.Draw(g, animationCounter);

            if (gameOver) {
                string playAgain = "Press S to start a new game or Q to quit";

                Font playAgainFont = new Font("Arial", 12);
                Font gameOverTopFont = new Font("Arial", 30, FontStyle.Bold);

                Point gameOverPoint = new Point((ClientRectangle.Width/3),(ClientRectangle.Height/2));
                Point playAgainPoint = new Point(ClientRectangle.Width/2, (int)((double)(ClientRectangle.Height*.9)));
                g.DrawString("GAME OVER!", gameOverTopFont, Brushes.Yellow, gameOverPoint);
                g.DrawString(playAgain, playAgainFont, Brushes.Yellow, playAgainPoint);
            }
        } // end method Form1_Paint

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
