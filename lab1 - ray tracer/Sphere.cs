using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace rt
{
    public class Sphere : Geometry
    {
        private Vector Center { get; set; }
        private double Radius { get; set; }

        public Sphere(Vector center, double radius, Material material, Color color) : base(material, color)
        {
            Center = center;
            Radius = radius;
        }

        public override Intersection GetIntersection(Line line, double minDist, double maxDist)
        {
            //i had a look over the course notes and i use this
            //(x-xc)^2 + (y - yc)^2 + (z - zc)^2 = r^2
            double a, b, c, d, e, f, xc, yc, zc;

            a = line.Dx.X;
            b = line.X0.X;

            c = line.Dx.Y;
            d = line.X0.Y;

            e = line.Dx.Z;
            f = line.X0.Z;
            
            xc = Center.X;
            yc = Center.Y;
            zc = Center.Z;
            
            // did some reformatting of the vars in order to follow the formulas easier when exploding the ()
            // turns out I forgot a parenthesis in C and it messed things up
            double A, B, C;
            
            A = a * a + c * c + e * e;
            B = 2 * (a * b + c * d + e * f - a * xc - c * yc - e * zc);
            C = b * b + d * d + f * f + xc * xc + yc * yc + zc * zc
                - 2 * (b * xc + d * yc + f * zc)
                - Radius * Radius;

            double delta = B * B - 4 * A * C;

            //no intersection
            if (delta < 0)
            {
                return new Intersection();
            }
            //one point of intersection
            else if (delta == 0)
            {
                //removed Math.Sqrt(delta) cuz it was 0 anyway
                double t = -B / (2 * A);
                return new Intersection(true, true, this, line, t);
            }
            //two points
            else
            {
                //it works by returning the smaller one
                //do I have to check if it is between the [minDist,maxDist]?
                double t1 = (-B + Math.Sqrt(delta)) / (2 * A);
                double t2 = (-B - Math.Sqrt(delta)) / (2 * A);
                if (t1 < t2)
                {
                    return new Intersection(true, true, this, line, t1);
                }
                else
                    return new Intersection(true, true, this, line, t2);
            }
        }

        public override Vector Normal(Vector v)
        {
            var n = v - Center;
            n.Normalize();
            return n;
        }
    }
}