/*
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
    class PlayerShip
    {
        private Bitmap image;

        private DateTime deadShipStartTime;

        private const int HorizontalInterval = 10;

        private int deadShipHeight;

        /// <summary>
        /// Gets the current Point where the PlayerShip is located.
        /// </summary>
        public Point Location { get; private set; }
        
        /// <summary>
        /// Gets a rectangle dependent on current player ship location.
        /// </summary>
        public Rectangle Area
        {
            get { return new Rectangle(Location, image.Size); }
        } // end property Area

        /// <summary>
        /// Returns a bitmap image of the playership.
        /// </summary>
        /// <returns>A bitmap image for displaying the lives left.</returns>
        public Bitmap GetLivesLeftImage() {
            return ResizeImage(Properties.Resources.player, 40, 40);
        } // end method GetLivesLeftImage

        /// <summary>
        /// Gets the Point at of the middle of the top of the area.
        /// </summary>
        public Point TopMiddle { 
            get { 
                return new Point((Area.Left + Area.Width / 2), Area.Top);} 
        } // end property TopMiddle
        
        /// <summary>
        /// Initializes a new instance of the PlayerShip class using a Point.
        /// </summary>
        /// <param name="location">The player ship's starting location.</param>
        public PlayerShip(Point location) {
            this.Location = location;
            image = ResizeImage(Properties.Resources.player, 40, 40);
            Alive = true;
            deadShipHeight = image.Height;
        } // end constructor method PlayerShip

        private bool alive;

        /// <summary>
        /// This method returns a bool stating whether the player ship is alive or not.
        /// </summary>
        public bool Alive {
            get { return alive;}
            set { 
                alive = value;
                deadShipStartTime = DateTime.Now;
            }
        } // end property Alive

        /// <summary>
        /// This method will draw the player ship onto the screen, and will 
        /// draw the 3-second death animation if the player is dead (was hit by
        /// an invader's shot). It will toggle the Alive property at the end
        /// of the animation.
        /// </summary>
        /// <param name="g">The Grahpics object to draw onto.</param>
        public void Draw(Graphics g)
        {
            if (!Alive)
            {
                Bitmap deadShipImage = new Bitmap(image);
                DateTime deadShipCurrentTime = DateTime.Now;
                TimeSpan duration = deadShipCurrentTime - deadShipStartTime;
                if(duration.Seconds < 3){
                    if (deadShipHeight > 0) deadShipHeight -= 2;
                    g.DrawImage(deadShipImage, Location.X, Location.Y, Area.Width, deadShipHeight);
                }
                else{
                    Alive = true;
                    deadShipHeight = image.Height;
                    g.DrawImageUnscaled(image, Location);
                }
            } 
            else {

                g.DrawImageUnscaled(image, Location);
            }
        } // end method Draw

        /// <summary>
        /// This method moves the playership either left or right.
        /// </summary>
        /// <param name="directionToMove">The direction the player ship moves.</param>
        public void Move(Direction directionToMove)
        {
            Point moveTo;
            switch (directionToMove)
            {
                case Direction.Left:
                    moveTo = new Point(Location.X - HorizontalInterval, Location.Y);
                    Location = moveTo;
                    break;

                case Direction.Right:
                    moveTo = new Point(Location.X + HorizontalInterval, Location.Y);
                    Location = moveTo;
                    break;

                default:
                    break;
            }
        } // end method Move
        
        /// <summary>
        /// This method changes the height and width of an image to a custom size.
        /// </summary>
        /// <param name="picture">The original picture to resize</param>
        /// <param name="width">The desired width of the new picture.</param>
        /// <param name="height">The desired height of the new picture.</param>
        /// <returns>A resized Bitmap Image</returns>
        private static Bitmap ResizeImage(Bitmap picture, int width, int height)
        {
            Bitmap resizedPicture = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedPicture))
            {
                graphics.DrawImage(picture, 0, 0, width, height);
            } // end using
            return resizedPicture;
        } // end method ResizeImage
    }
}
