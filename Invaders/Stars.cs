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
    class Stars
    {
        private List<Star> starField;
        Random random;
        Rectangle boundaries;

        /// <summary>
        /// Instantiates an instance of the Stars class from a boundary rectangle and a Random object.
        /// </summary>
        /// <param name="boundaries">A rectangle representing the gaming area</param>
        /// <param name="random">An instance of the Random class for use in functions needing a random.</param>
        public Stars(Rectangle boundaries, Random random) {
            this.boundaries = boundaries;
            this.random = random;
            starField = new List<Star>();
            AddStars(300);
        } // end constructor method Stars

        /// <summary>
        /// This method draws a tiny 1-by-1 pixel dot onto the screen.
        /// </summary>
        /// <param name="g">The graphics objec to draw onto.</param>
        public void Draw(Graphics g)
        {
            foreach (Star star in starField)
            {
                g.DrawEllipse(star.pen, star.point.X, star.point.Y, 1, 1);
            }
        } // end method Draw

        /// <summary>
        /// This method takes 5 stars from the starfield list and adds 5 after.
        /// Combined with the draw method, this creates a twinkle effect in the
        /// gaming area.
        /// </summary>
        public void Twinkle()
        {
            RemoveStars(5);
            AddStars(5);
        } // end method Twinkle

        /// <summary>
        /// This method adds a certain number of stars to the star list. 
        /// Every star has a random location within the game area's boundaries
        /// and a random color.
        /// </summary>
        /// <param name="numberOfStarsToAdd">How many stars to add.</param>
        private void AddStars(int numberOfStarsToAdd) {
            for (int i = 0; i < numberOfStarsToAdd; i++)
            {
                Point randomPointWithinBoundaries = new Point(random.Next(boundaries.Right), 
                    random.Next(boundaries.Bottom));
                Star newStar = new Star(randomPointWithinBoundaries, RandomColorPen(random));
                starField.Add(newStar);
            }
        } // end method AddStars

        /// <summary>
        /// This method removes a certain number of stars from the star list.
        /// </summary>
        /// <param name="numberOfStarsToRemove">How many stars to take away.</param>
        private void RemoveStars(int numberOfStarsToRemove)
        {
            for (int i = 0; i < numberOfStarsToRemove; i++) {
                int randomStarToRemoveIndex = random.Next(starField.Count);
                starField.Remove(starField[randomStarToRemoveIndex]);
            } // end for
        } // end method RemoveStars

        /// <summary>
        /// Returns a pen of a random shade of blue.
        /// </summary>
        /// <param name="random">The Random object needed</param>
        /// <returns>A pen object with random color</returns>
        private Pen RandomColorPen(Random random) {
            Pen penToReturn = new Pen(Brushes.White);

            int colorSelectNumber = random.Next(5);

            switch(colorSelectNumber){
                case 0:
                    penToReturn.Brush = Brushes.Blue;
                    break;
                case 1:
                    penToReturn.Brush = Brushes.PowderBlue;
                    break;
                case 2:
                    penToReturn.Brush = Brushes.SkyBlue;
                    break;
                case 3:
                    penToReturn.Brush = Brushes.RoyalBlue;
                    break;
                case 4:
                    penToReturn.Brush = Brushes.CornflowerBlue;
                    break;
                default: break;
            }
            return penToReturn;
        }// end method RandomPen
    }
}
