using System;
using System.Windows.Forms;
using Autodesk.AutoCAD.Geometry;
using AcColor = Autodesk.AutoCAD.Colors.Color;

namespace GeometryPlus
{
    /// <summary>
    /// Form to collect configuration options from user before rendering
    /// concentric circles.
    /// </summary>
    public partial class ConcentricCirclesForm : Form
    {
        public ConcentricCirclesForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check whether or not a given string is numeric or not. If
        /// it is not, then set the validation label to indicate as such. 
        /// </summary>
        /// <param name="Item">Name of field for error-message</param>
        /// <param name="value">Value of field being checked</param>
        private bool ValidateTextAsNumber(string Item, string value)
        {
            double num;
            var isNum = Double.TryParse(value, out num);
            if (!isNum)
            {
                validationLabel.Text = String.Format("{0}: Must be numeric",
                                                     Item);
            }
            return isNum;
        }

        /// <summary>
        /// The form has been submitted. We now need to check and see if 
        /// everything validates and then we can send off our data to the
        /// ConcentricCircles class to actually do the creating of the
        /// circles.
        /// 
        /// If we find some invalid data, then we need to let the user know
        /// (via the validationLabel in the form) and keep the form window
        /// open.
        ///
        /// <seealso cref="ConcentricCirclesForm.validationLabel"/>
        /// <seealso cref="ConcentricCircles"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createButton_Click(object sender, EventArgs e)
        {
            // check validity (halt processing if necessary)
            var valid = ValidateTextAsNumber("Position X", positionX.Text);
            valid = valid && ValidateTextAsNumber("Position Y", positionY.Text);
            valid = valid && ValidateTextAsNumber("Position Z", positionZ.Text);

            if (!valid) return;

            // collect input
            var centerPoint = new Point3d(Double.Parse(positionX.Text),
                                          Double.Parse(positionY.Text),
                                          Double.Parse(positionZ.Text));
            var outerCircleColor = AcColor.FromRgb((byte)outerCircleRed.Value,
                                                   (byte)outerCircleGreen.Value,
                                                   (byte)outerCircleGreen.Value);
            var middleCircleColor = AcColor.FromRgb((byte)middleCircleRed.Value,
                                                    (byte)middleCircleGreen.Value,
                                                    (byte)middleCircleBlue.Value);
            var innerCircleColor = AcColor.FromRgb((byte)innerCircleRed.Value,
                                                   (byte)innerCircleGreen.Value,
                                                   (byte)innerCircleBlue.Value);


            // draw the circles
            ConcentricCircles.DrawConcentricCircles(centerPoint,
                                                    new AcColor[] {
                                                        outerCircleColor,
                                                        middleCircleColor,
                                                        innerCircleColor
                                                    });

            // close the form
            this.Close();

        }

    }
}
