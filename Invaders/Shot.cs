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
    class Shot
    {
        private const int moveInterval = 20;
        private const int width = 5;
        private const int height = 15;

        /// <summary>
        /// Gets the current Point where the Shot is located.
        /// </summary>
        public Point Location { get; private set; }

        private Direction direction;
        private Rectangle boundaries;

        /// <summary>
        /// Instantiates a new instance of the Shot Class from a location, a direction, and a boundary rectangle.
        /// </summary>
        /// <param name="location">The shot's starting location</param>
        /// <param name="direction"> The shot's direction.</param>
        /// <param name="boundaries">The rectangle representing the gaming area.</param>
        public Shot(Point location, Direction direction,
            Rectangle boundaries){

            this.Location = location;
            this.direction = direction;
            this.boundaries = boundaries;
        } // end constructor method Shot

        /// <summary>
        /// This method draws a yellow rectangle representing a shot onto the screen.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Yellow, Location.X, Location.Y, width, height);
            //throw new NotImplementedException();
        } // end method Draw

        /// <summary>
        /// This method moves a shot object in the specified direction, returning 
        /// true if the shot object is still within the game area's boundaries.
        /// </summary>
        /// <param name="directionToMove">The direction the shot should move.</param>
        /// <returns>A boolean specifying whether it's beyond the game area's boundaries.</returns>
        public bool Move(Direction directionToMove)
        {
            switch (directionToMove)
            {
                case Direction.Up:
                    Location = new Point(Location.X, Location.Y - moveInterval);
                    break;
                case Direction.Down:
                    Location = new Point(Location.X, Location.Y + moveInterval);
                    break;
                default:
                    break;
            }
            if ((Location.Y <= boundaries.Top) || (Location.Y >= boundaries.Bottom))
                return false;
            return true;
        } // end method Move

       }
}
