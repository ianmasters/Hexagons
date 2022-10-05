# Hexagons

Simple example of how to quickly deal with point inside hexagon tests as a proof of concept after discussions on a CodeMonkey Youtube channel video dealing with the same problem.

Works in 2D or 3D.
Hexagon(s) can be on any plane, offset / rotated / scaled, as the IsInside call takes a local transform point. This also makes it easier to set up a single set of plane equations that can be used for a regular sized grid.
Given the hexagons can be rotated it is easy to change from pointy top to flat top hexagons with a 30 degree rotation.

The core math only requires the hexagon radius that the Awake() method sets up up two planes for the IsInside check. IsInside then uses the absolute value of the point in local space to check to allow for only 2 planes to check against, since the hexagon is symmetrical on both axis on the positive quadrant.

Further changes could clearly be made depending on the requirements of an engine.
Global checks would require each hexagon to have its own coordinate system that can be set up in their Awake/Start methods just the same.

Unity project version is currently 2022.1.19f1 but it should work in just about any version.
Usage: Open in Unity, press Play and move the cursor around the hexagon using the mouse.

References:
CodeMonkey video for reference to the discussion: https://youtu.be/dqframYHfws

License:
Use the code as you see fit. Reference the source if you found it useful.
