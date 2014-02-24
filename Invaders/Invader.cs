/*
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
    class Invader
    {
        private const int HorizontalInterval = 10;
        private const int VerticalInterval = 40;

        private Bitmap image;

        private Size invaderSize = new Size(40, 40);

        /// <summary>
        /// Gets the current Point where the Invader is located.
        /// </summary>
        public Point Location { get; private set; }

        /// <summary>
        /// Gets the current ShipType of the invader.
        /// </summary>
        public ShipType InvaderType { get; private set; }

        /// <summary>
        /// Gets the point at the middle of the bottom of the invader's area. The invader
        /// will shoot from this area.
        /// </summary>
        public Point BottomMiddle { get { return new Point((Area.Left + Area.Width / 2), Area.Bottom); } }

        /// <summary>
        /// Gets a rectangle dependent on current player ship location.
        /// </summary>
        public Rectangle Area
        {
            get { return new Rectangle(Location, image.Size); }
        } // end property Area

        /// <summary>
        /// Gets the invader's score (points a player gets for killing it).
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Instantiates an Invader object from a ShipType, a location, and a score.
        /// </summary>
        /// <param name="invaderType">The type of invader ship.</param>
        /// <param name="location">The invader's starting location</param>
        /// <param name="score">The invader's score (points a player gets for killing it).</param>
        public Invader(ShipType invaderType, Point location, int score) {
            this.InvaderType = invaderType;
            this.Location = location;
            this.Score = score;
            image = InvaderImage(0);
            
        } // end constructor

        /// <summary>
        /// Moves the invader in a specified direction.
        /// </summary>
        /// <param name="directionToMove">The direction the invader moves.</param>
        public void Move(Direction directionToMove)
        {
            switch (directionToMove) {
                case Direction.Left:
                    Location = new Point(Location.X - HorizontalInterval, Location.Y);
                    break;
                case Direction.Right:
                    Location = new Point(Location.X + HorizontalInterval, Location.Y);
                    break;
                case Direction.Down:
                    Location = new Point(Location.X, Location.Y + VerticalInterval);
                    break;
                default:
                    break;
            }
        } // end method Move

        /// <summary>
        /// Draws an invader onto the screen from a Graphics object and a number for current animation cell.
        /// </summary>
        /// <param name="g">The Grahpics object to draw onto.</param>
        /// <param name="animationCell">The animation cell number representing which invader image to return.</param>
        public void Draw(Graphics g, int animationCell)
        {
            image = InvaderImage(animationCell);
            g.DrawImageUnscaled(image, Location);
        } // end method Draw

        /// <summary>
        /// Returns a bitmap representing the proper invader image for the current animation number.
        /// </summary>
        /// <param name="animationCell">The animation cell number representing which image to return.</param>
        /// <returns>The proper image for the current animation cell number.</returns>
        private Bitmap InvaderImage(int animationCell)
        {
            Bitmap imageToReturn = new Bitmap(invaderSize.Width, invaderSize.Height);
            switch (animationCell)
            {
                case 0:
                    if (InvaderType == ShipType.Bug) imageToReturn = ResizeImage(Properties.Resources.bug1, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Saucer) imageToReturn = ResizeImage(Properties.Resources.flyingsaucer1, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Satellite) imageToReturn = ResizeImage(Properties.Resources.satellite1, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Spaceship) imageToReturn = ResizeImage(Properties.Resources.spaceship1, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Star) imageToReturn = ResizeImage(Properties.Resources.star1, invaderSize.Width, invaderSize.Height);
                    break;
                case 1:
                    if (InvaderType == ShipType.Bug) imageToReturn = ResizeImage(Properties.Resources.bug2, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Saucer) imageToReturn = ResizeImage(Properties.Resources.flyingsaucer2, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Satellite) imageToReturn = ResizeImage(Properties.Resources.satellite2, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Spaceship) imageToReturn = ResizeImage(Properties.Resources.spaceship2, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Star) imageToReturn = ResizeImage(Properties.Resources.star2, invaderSize.Width, invaderSize.Height);
                    break;
                case 2:
                    if (InvaderType == ShipType.Bug) imageToReturn = ResizeImage(Properties.Resources.bug3, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Saucer) imageToReturn = ResizeImage(Properties.Resources.flyingsaucer3, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Satellite) imageToReturn = ResizeImage(Properties.Resources.satellite3, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Spaceship) imageToReturn = ResizeImage(Properties.Resources.spaceship3, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Star) imageToReturn = ResizeImage(Properties.Resources.star3, invaderSize.Width, invaderSize.Height);
                    break;
                case 3:
                    if (InvaderType == ShipType.Bug) imageToReturn = ResizeImage(Properties.Resources.bug4, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Saucer) imageToReturn = ResizeImage(Properties.Resources.flyingsaucer4, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Satellite) imageToReturn = ResizeImage(Properties.Resources.satellite4, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Spaceship) imageToReturn = ResizeImage(Properties.Resources.spaceship4, invaderSize.Width, invaderSize.Height);
                    else if (InvaderType == ShipType.Star) imageToReturn = ResizeImage(Properties.Resources.star4, invaderSize.Width, invaderSize.Height);
                    break;
                default:
                    break;
            }
            return imageToReturn;
        } // end method InvaderImage

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
