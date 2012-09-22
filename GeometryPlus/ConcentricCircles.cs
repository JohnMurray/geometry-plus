using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace GeometryPlus
{    
    /// <summary>
    /// Class responsible for managing Concentric Circles. For our
    /// purposes, managing simply means creating. This class will
    /// also contain any needed defaults for creating the circles.
    /// </summary>
    public class ConcentricCircles
    {
        /// <summary>
        /// The initial radius of the outer-most circle. All other radii
        /// will be a fraction of this initial radius.
        /// </summary>
        private const int INITIAL_RADIUS = 50;

        /// <summary>
        /// This is the ratio by which the initial radius will be reduced
        /// for each inner-circle.
        /// </summary>
        private const double REDUCTION_RATIO = 0.2;

        /// <summary>
        /// Draws the concentric circles given the center-point and the colors
        /// for each circle. It is assumed that the colors are ordered with
        /// the outer-most circle's color first, working it way toward the
        /// inner-most circle.
        /// </summary>
        /// <param name="center">Center-Point for all circles</param>
        /// <param name="colors">
        ///   Colors for each circle ordered by outer-most circle's color first
        /// </param>
        public static void DrawConcentricCircles(Point3d center, Color[] colors)
        {
            // get current document and DB
            var acDocument = Application.DocumentManager.MdiActiveDocument;
            var acCurrentDB = acDocument.Database;

            // start a transaction in which the concentric circles will
            // be created
            using (var acTransaction = acCurrentDB.TransactionManager.StartTransaction())
            {
                var acBlockTable = acTransaction.GetObject(acCurrentDB.BlockTableId,
                                                           OpenMode.ForRead) as BlockTable;
                    
                var acBlockTableRecord = acTransaction.GetObject(acBlockTable[BlockTableRecord.ModelSpace],
                                                                 OpenMode.ForWrite) as BlockTableRecord;
                // create a circle for each color and commit the transaction
                int i = 0;
                foreach (var color in colors)
                {
                    var circle = CreateCircle(i, center, color);
                    acBlockTableRecord.AppendEntity(circle);
                    acTransaction.AddNewlyCreatedDBObject(circle, true);
                    i++;
                }

                acTransaction.Commit();
            }
        }

        /// <summary>
        /// Create a circle based on the center-point and color provided. We're
        /// also going to require a count. This count represents the number of
        /// circles that have already been created. With this, we can scale the
        /// circle accordingly such that each circle is smaller than the previous.
        /// </summary>
        /// <param name="count">Number of circles already created</param>
        /// <param name="center">Center-Point for circle</param>
        /// <param name="color">Color of the circle</param>
        /// <returns>Created circle to store in DB</returns>
        private static Circle CreateCircle(int count, Point3d center, Color color)
        {
            var circle = new Circle();
            circle.SetDatabaseDefaults();
            circle.Center = center;
            circle.Radius = (1 - (count * REDUCTION_RATIO)) * INITIAL_RADIUS;
            circle.Color = color;
            return circle;
        }
    }
}
