using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace rt
{
    class RayTracer
    {
        private Geometry[] geometries;
        private Light[] lights;

        public RayTracer(Geometry[] geometries, Light[] lights)
        {
            this.geometries = geometries;
            this.lights = lights;
        }

        private double ImageToViewPlane(int n, int imgSize, double viewPlaneSize)
        {
            var u = n * viewPlaneSize / imgSize;
            u -= viewPlaneSize / 2;
            return u;
        }

        private Intersection FindFirstIntersection(Line ray, double minDist, double maxDist)
        {
            var intersection = new Intersection();

            foreach (var geometry in geometries)
            {
                var intr = geometry.GetIntersection(ray, minDist, maxDist);

                if (!intr.Valid || !intr.Visible) continue;

                if (!intersection.Valid || !intersection.Visible)
                {
                    intersection = intr;
                }
                else if (intr.T < intersection.T)
                {
                    intersection = intr;
                }
            }

            return intersection;
        }

        private bool IsLit(Vector point, Light light)
        {
            // ADD CODE HERE: Detect whether the given point has a clear line of sight to the given light
            // to be honest I saw the implementation during the lab

            //we compute the distance from the light to the point
            var distance = (light.Position - point).Length();
            //we shoot a ray from the point to the light's position
            var ray = new Line(point, light.Position);
            //we go through all the spheres and if any of them intersect the ray we return false
            //else it doesn't intersect anything and it's `clear`
            foreach (var geometry in geometries)
            {
                var intersection = geometry.GetIntersection(ray, 0, distance);
                if (intersection.Visible && intersection.T > 0)
                    return false;
            }
            return true;
        }
        
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: rt.Vector")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: rt.Intersection")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: rt.Line")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: rt.Color")]
        public void Render(Camera camera, int width, int height, string filename)
        {
            var background = new Color(0, 0, 10, 4);
            var viewParallel = (camera.Up ^ camera.Direction).Normalize();
            

            var image = new Image(width, height);

            var vecW = camera.Direction * camera.ViewPlaneDistance;
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    //we get the two coords of the line
                    var x0 = camera.Position;
                    //use the nice formula from the course :^)
                    var x1 = camera.Position + vecW +
                             viewParallel * ImageToViewPlane(i, width, camera.ViewPlaneWidth)
                             + camera.Up * ImageToViewPlane(j, height, camera.ViewPlaneHeight);

                    //create the line
                    var line = new Line(x0, x1);

                    //initially I thought that the frontPlane was the max and the backPlane was min
                    //but had a look over the lecture and everything was well in the world again
                    Intersection intersection =
                        FindFirstIntersection(line, camera.FrontPlaneDistance, camera.BackPlaneDistance);

                    var initColor = new Color(0, 0, 0, 4);
                    if (intersection.Valid && intersection.Visible)
                    {
                        // ADD CODE HERE: Implement pixel color calculation
                        
                        foreach (var light in lights)
                        {
                            //HERE I WAS JUST DOING initColor = intersection.Geometry.Material.Ambient * light.Ambient;
                            //AND IT DIDN'T ADD THE LIGHTS AMBIENT...
                            initColor += intersection.Geometry.Material.Ambient * light.Ambient;
                            //normal to the surface at intersection
                            Vector Normal = intersection.Geometry.Normal(intersection.Position).Normalize();
                            //vector from the intersection to light
                            Vector T = (light.Position - intersection.Position).Normalize();
                            //vector from the intersection to the camera
                            Vector E = (camera.Position - intersection.Position).Normalize();
                            Vector R = Normal * (Normal * T) * 2 - T;

                            //add shadows
                            if (IsLit(intersection.Position, light))
                            {
                                if (Normal * T > 0)
                                {
                                    initColor += intersection.Geometry.Material.Diffuse * light.Diffuse * (Normal * T);
                                }

                                if (E * R > 0)
                                {
                                    initColor += intersection.Geometry.Material.Specular * light.Specular
                                    * Math.Pow(E * R, intersection.Geometry.Material.Shininess);
                                }
                            }

                            initColor *= light.Intensity;
                        }
                        image.SetPixel(i, j, initColor);
                    }
                    else
                    {
                        image.SetPixel(i, j, background);
                    }
                }
            }

            image.Store(filename);
        }
    }
}