/*
 * The Game class...
 * 
 * Created by Daniel Hopkins, based on the Invaders Lab
 * in Head First C# (2nd Edition).
 * E-Mail: dahopkin2@gmail.com
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Invaders
{
    
    class Game
    {
        private int score = 0;
        private int livesLeft = 3; 
        private int currentInvaderWave = 0;
        private int framesSkipped = 0;
        private int framesToSkip = 7; // number of frames to wait before rendering.
        private int maxNumberOfWaves = 3;

        private Rectangle boundaries; // the game's entire area for rendering.
        private Random random; // All functions using a random use this one instance.

        private Point playerShipStartingLocation;
        private Direction invaderDirection;
        private List<Invader> invaders;

        private PlayerShip playerShip;
        private List<Shot> playerShots;
        private List<Bitmap> livesLeftDisplay;
        private List<Shot> invaderShots;

        private Stars stars;

        public event EventHandler GameOver;

        /// <summary>
        /// Initializes a new instance of the Game class.
        /// </summary>
        /// <param name="boundaries">The rectangle representing the gaming area.</param>
        /// <param name="random">A Random for use with all inner functions requiring a random.</param>
        public Game(Rectangle boundaries, Random random) {
            /* initialize the two accepted parameters before 
             ANYTHING else.*/
            this.boundaries = boundaries;
            this.random = random;
            this.playerShipStartingLocation = new Point(boundaries.Right - 100, boundaries.Bottom - 50);
            this.playerShip = new PlayerShip(playerShipStartingLocation);
            this.playerShots = new List<Shot>();
            this.invaderShots = new List<Shot>();
            this.invaders = new List<Invader>();
            this.livesLeftDisplay = new List<Bitmap>();
            this.stars = new Stars(boundaries, random);
            for (int i = 0; i < livesLeft; i++)
                livesLeftDisplay.Add(playerShip.GetLivesLeftImage());
            NextWave();
        } // end constructor method Game

        /// <summary>
        /// This method triggers the game over event.
        /// </summary>
        /// <param name="e"></param>
        public void OnGameOver(EventArgs e) {
            EventHandler gameOver = GameOver;
            if (gameOver != null)
                gameOver(this, e);
        } // end method OnGameOver

        /// <summary>
        /// This method adds a shot to the player's shot list.
        /// </summary>
        public void FirePlayerShot() {

            if (playerShots.Count < 2) {
                Shot newPlayerShot = new Shot(playerShip.TopMiddle,Direction.Up, boundaries);
                playerShots.Add(newPlayerShot);
            } 
        } // end method FireShot

        /// <summary>
        /// This method makes the stars twinkle in the background.
        /// </summary>
        public void Twinkle()
        {
            stars.Twinkle();
        } // end method Twinkle

        /// <summary>
        /// This method moves onscreen Player/Invader shots, makes the invaders shoot, 
        /// checks shot collisions, checks to see if invaders are at the bottom of the screen, and
        /// goes to the next Wave of invaders if the player has shot them all.
        /// If the player is not alive at the moment, it won't do anything.
        /// </summary>
        public void Go()
        {            
            if (!playerShip.Alive) return;
            
            MovePlayerShots();
            MoveInvaderShots();

            MoveInvaders();
            ReturnFire(); 
           
            CheckForInvaderCollisions();
            CheckForPlayerCollisions();
            CheckForInvadersAtBottomOfScreen();

            if (invaders.Count == 0) NextWave();
        } // end method Go

        /// <summary>
        /// This method moves all invader shots down, removing
        /// them from the player shots list if they're past the boundaries.
        /// </summary>
        private void MoveInvaderShots()
        {
            for (int i = invaderShots.Count - 1; i >= 0; i--)
                if (!invaderShots[i].Move(Direction.Down))
                    invaderShots.Remove(invaderShots[i]);
        } // end method MoveInvaderShots

        /// <summary>
        /// This method moves all player shots up, removing
        /// them from the invader shots list if they're past the boundaries.
        /// </summary>
        private void MovePlayerShots()
        {
            for (int i = playerShots.Count - 1; i >= 0; i--)
                if (!playerShots[i].Move(Direction.Up))
                    playerShots.Remove(playerShots[i]);
        } // end method MovePlayerShots

        /// <summary>
        /// This method moves the player in a certain direction.
        /// </summary>
        /// <param name="directionToMove">The direction you want the player to move.</param>
        public void MovePlayer(Direction directionToMove)
        {
            if (playerShip.Alive){
                if(!WillTouchBorder(playerShip.Area, directionToMove, 10))
                playerShip.Move(directionToMove);
            }
        } // end method MovePlayer

        /// <summary>
        /// This method draws everything the game needs to display
        /// (black background, player, invaders, shots, stars, score, lives left) onto the screen.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        /// <param name="animationCell">The number representing which picture of animation every invader should display.</param>
        public void Draw(Graphics g, int animationCell) {
            g.FillRectangle(Brushes.Black, 0, 0, boundaries.Width, boundaries.Height);

            stars.Draw(g);
            
            foreach (Invader invader in invaders)
                invader.Draw(g, animationCell);
            
            playerShip.Draw(g);
            
            foreach (Shot shot in playerShots)
                shot.Draw(g);
            
            foreach (Shot shot in invaderShots)
                shot.Draw(g);
            
            DrawScoreAndWaveProgress(g);

            DrawLivesLeft(g);

        } // end method Draw

        /// <summary>
        /// This method draws the score and wave progress onto the upper left of the screen.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        private void DrawScoreAndWaveProgress(Graphics g)
        {
                string scoreString = "WAVE " + 
                    (currentInvaderWave > maxNumberOfWaves ? 
                    maxNumberOfWaves.ToString() : currentInvaderWave.ToString()) + 
                    "/" + maxNumberOfWaves.ToString() + Environment.NewLine
                + "SCORE:" + Environment.NewLine + score.ToString();
                Font scoreFont = new Font("Arial", 12, FontStyle.Bold);
                Point scorePoint = new Point(25, 25);
                g.DrawString(scoreString, scoreFont, Brushes.Red, scorePoint);
        }

        /// <summary>
        /// This method draws the amount of ships the player has left onto the upper right of the screen.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        private void DrawLivesLeft(Graphics g)
        {
            Point drawPoint = new Point(boundaries.Right - (livesLeft * playerShip.Area.Width), boundaries.Top + 10);
            int xSkip = playerShip.Area.Width;
            foreach (Bitmap shipToDraw in livesLeftDisplay)
            {
                g.DrawImage(shipToDraw, drawPoint);
                Point newLifePoint = new Point(drawPoint.X + xSkip, drawPoint.Y);
                drawPoint = newLifePoint;
            }
        } // end method DrawLivesLeft

        /// <summary>
        /// This method progresses to the next wave of invaders while updating the Current Invader Wave and Frames To Skip
        /// and resetting the invader and shot lists.
        /// </summary>
        private void NextWave() {
            if (++currentInvaderWave > maxNumberOfWaves) {
                OnGameOver(new EventArgs());
                return;
            }
            framesToSkip--;
            //playerShip.Area.Location = playerShipStartingLocation;
            invaders = new List<Invader>();
            invaderDirection = Direction.Right;
            invaderShots = new List<Shot>();
            playerShots = new List<Shot>();
            int xMove = 85;
            int yMove = 50;
            int yPosition = 50;
            int xPosition = 50;
            yPosition = AddInvaderRow(ShipType.Satellite, 50, xMove, yMove, xPosition, yPosition);
            yPosition = AddInvaderRow(ShipType.Bug, 40, xMove, yMove, xPosition, yPosition);
            yPosition = AddInvaderRow(ShipType.Saucer, 30, xMove, yMove, xPosition, yPosition);
            yPosition = AddInvaderRow(ShipType.Spaceship, 20, xMove, yMove, xPosition, yPosition);
            yPosition = AddInvaderRow(ShipType.Star, 10, xMove, yMove, xPosition, yPosition);
        } // end method

        /// <summary>
        /// This method adds 6 invaders of a certain type to the invaders list collection. It returns the position 
        /// of the next row on the screen.
        /// </summary>
        /// <param name="invaderType">The type of invader to add.</param>
        /// <param name="score">The amount of points for killing the invader</param>
        /// <param name="xMove">Amount of pixels to move across after creating an invader.</param>
        /// <param name="yMove">Amount of pixels to move down after creating a row of invaders.</param>
        /// <param name="xPosition">X-Coordinate of the starting position for the current row.</param>
        /// <param name="yPosition">Y-Coordinate of the starting position for the current row.</param>
        /// <returns>The position where the next row should start.</returns>
        private int AddInvaderRow(ShipType invaderType, int score, int xMove, int yMove, int xPosition, int yPosition) {
                        
            for (int j = 1; j <= 6; j++)
            {
                Invader newInvader = new Invader(invaderType, new Point(xPosition, yPosition), score);
                invaders.Add(newInvader);
                xPosition += xMove;
            }
            yPosition += yMove;
            return yPosition;
        } // end method AddInvaderRow

        /// <summary>
        /// This method moves invaders to within 100px of the game form's edge, then
        /// moves them down and the opposite way.
        /// </summary>
        private void MoveInvaders()
        {

            /*If this frame should be skipped, return.*/
            if (++framesSkipped < framesToSkip)
                return;
            else
            {
                
                var invadersOnBorder =
                      from invader in invaders
                      where WillTouchBorder(invader.Area, invaderDirection, 100)
                      select invader;
                if (invadersOnBorder.Count() > 0)
                {
                    foreach (Invader invader in invaders)
                        invader.Move(Direction.Down);
                    invaderDirection = (invaderDirection == Direction.Left ? Direction.Right : Direction.Left);
                } // end if
                else 
                foreach (Invader invader in invaders)
                    invader.Move(invaderDirection);
                framesSkipped = 0;
            } // end else

        } // end method MoveInvaders

        /// <summary>
        /// This method checks to see if a rectangle traveling left or right is a certain number of pixels away from 
        /// either the left or right edge of the game area (form's boundaries in this case). It returns true if so.
        /// </summary>
        /// <param name="area">The rectangle we're checking for border collision.</param>
        /// <param name="direction">The current direction the rectangle is traveling.</param>
        /// <param name="pixelBoundary">The maximum amount of pixels away from the edge the rectangle can be.</param>
        /// <returns>A boolean representing whether or not the rectangle is on the established border.</returns>
        private bool WillTouchBorder(Rectangle area, Direction direction, int pixelBoundary)
        {
            if (((direction == Direction.Right) &&
                (area.Right + pixelBoundary > boundaries.Right)) ||
                ((direction == Direction.Left) &&
                (area.Left - pixelBoundary < boundaries.Left))) return true;
            else return false;
        } // end method WillTouchBorder

        /// <summary>
        /// This method makes the invaders at the bottom fire shots if there aren't enough
        /// shots (one more than the current wave number) on screen already.
        /// </summary> 
        /* Big Note: I got help online for the LINQ in the method below. Before, the book
         * made it seem like I had to order by Location.X descending after
         * grouping by Location.X. However, this led to any Invader in the 
         * column being able to shoot. After trying some failed things regarding
         * Location.Y, I finally looked online at the Head First site forums and saw
         * one of the authors say you had to sort by Y position first.
         * 
         * This is the only method I got ANY online help for.
         */
        private void ReturnFire()
        {
            if(invaderShots.Count >= currentInvaderWave + 1) return;
            else if (random.Next(10) < (10 - currentInvaderWave)) return; 
            else {
                var invaderLocationGroups =
                    from bottomInvader in invaders
                    orderby bottomInvader.Location.Y descending
                    group bottomInvader by bottomInvader.Location.X
                    into invaderLocationGroup
                    select invaderLocationGroup;
                int randomInvaderNumber = random.Next(invaderLocationGroups.Count());
                Invader shooter = invaderLocationGroups.ElementAt(randomInvaderNumber).First();
                Shot newShot = new Shot(shooter.BottomMiddle, Direction.Up, boundaries);
                invaderShots.Add(newShot);
            } // end else
        } // end method ReturnFire

        /// <summary>
        /// This method checks to see if any of the invaders' shots have collided with the player.
        /// </summary>
        private void CheckForPlayerCollisions() {
            for (int i = invaderShots.Count - 1; i >= 0; i--) {
                if (playerShip.Area.Contains(invaderShots[i].Location)) {
                    playerShip.Alive = false;
                    invaderShots.Remove(invaderShots[i]);
                    if (--livesLeft < 0)
                        OnGameOver(new EventArgs());
                    else { 
                        int shipPictureToRemove = livesLeftDisplay.Count-1;
                        livesLeftDisplay.Remove(livesLeftDisplay[shipPictureToRemove]);
                    } // end else
                } // end if
            } // end for
        } // end method CheckForPlayerCollisions

        /// <summary>
        /// This method checks to see if any of the player's shots have collided with an invader.
        /// </summary>
        private void CheckForInvaderCollisions() { 
            for (int i = playerShots.Count - 1 ; i >= 0; i--)
			{
                /*
                 * Note: For some reason, putting playerShots[i].Location directly into the 
                 * LINQ query below gives me an ArgumentOutOfRangeException. 
                 * Placing the location into its own variable fixed that.
                 */
                Point shotLocation = playerShots[i].Location;
                var invaderCollisions = 
                    from hitInvader in invaders
                    where hitInvader.Area.Contains(shotLocation)
                    select hitInvader;

                if (invaderCollisions.Count() > 0) { 
                    // remove the shot from the player
                    // shot list.
                    playerShots.Remove(playerShots[i]);
                    // remove the invader(s) hit from the
                    // invaders list.
                    List<Invader> deadInvaders = new List<Invader>();
                    foreach (var deadInvader in invaderCollisions)
                        deadInvaders.Add(deadInvader);
                    foreach (Invader deadInvader in deadInvaders) {
                        score += deadInvader.Score;
                        invaders.Remove(deadInvader);
                    } // end foreach
                } // end if
			} // end for
        }  // end method CheckForInvaderCollisions

        /// <summary>
        /// This method checks to see if any invaders are where the player is
        /// (at the bottom of the screen). The game is over if there are.
        /// </summary>
        private void CheckForInvadersAtBottomOfScreen() {
            var invadersAtBottom =
                    from bottomInvader in invaders
                    where bottomInvader.Area.Bottom >= playerShip.Area.Bottom
                    select bottomInvader;

            if (invadersAtBottom.Count() > 0) { OnGameOver(new EventArgs());} // trigger game over.
        } // end method CheckForInvadersAtBottomOfScreen
        
        private void CheckCollisions()
        {
            CheckForInvaderCollisions();
            CheckForPlayerCollisions();
            CheckForInvadersAtBottomOfScreen();
        }


    }
}
