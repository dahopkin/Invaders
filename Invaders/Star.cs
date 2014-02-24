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
    struct Star
    {
        public Point point;
        public Pen pen;

        /// <summary>
        /// Instantiates an instance of the Star struct from a Point and a Pen.
        /// </summary>
        /// <param name="point">Starting point of the star.</param>
        /// <param name="pen">The color of the star's pen (for drawing).</param>
        public Star(Point point, Pen pen) {
            this.point = point;
            this.pen = pen;
        } // end constructor
    }
}
