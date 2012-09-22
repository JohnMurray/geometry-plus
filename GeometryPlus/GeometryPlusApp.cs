using System.Windows.Forms;
using Autodesk.AutoCAD.Runtime;

namespace GeometryPlus
{
    /// <summary>
    /// Main initialization of our extension application.
    /// </summary>
    public class GeometryPlusApp : IExtensionApplication
    {
        /// <summary>
        /// Initialize our application. This basically means building the
        /// toolbar with a button to create our concentric circles. While
        /// I realize that this might not be necessary for a single feature,
        /// GUIs are always nice for clients and a toolbar adds an easy way
        /// to add in more commands/features. 
        /// 
        /// For now this will exist in a single method. If there ever is more
        /// happening here, then it might make sense to refactor, but no need
        /// to refactor prematurely. 
        /// </summary>
        public void Initialize()
        {
            // Get the module path for loading images
            var geometryPlusModule = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0];
            var modulePath = geometryPlusModule.FullyQualifiedName;
            try
            {
                modulePath = modulePath.Substring(0, modulePath.LastIndexOf(@"\"));
            }
            catch
            {
                // I imagine some type of common logging or error-reporting code
                // would also be located here.
                MessageBox.Show("Error with Module Path. Toolbar could not be loaded");
                return;
            }

            // Create toolbar (empty at this point)
            var acadApp = Autodesk.AutoCAD.ApplicationServices.Application.AcadApplication as Autodesk.AutoCAD.Interop.AcadApplication;
            var toolBar = acadApp.MenuGroups.Item(0).Toolbars.Add("Geometry Plus");

            // Create toolbar buttons and set their bitmaps-images
            var ccButton = toolBar.AddToolbarButton(0,
                                                    "Concentric Circles",
                                                    "Create Concentric Circles",
                                                    "_ConcentricCircle ");
            ccButton.SetBitmaps(modulePath + @"\Images\tbConCir.bmp",
                                modulePath + @"\Images\tbConCir.bmp");
        }

        /// <summary>
        /// We don't have any resources (or what not) to clean up. So, this
        /// for now will remain empty.
        /// </summary>
        public void Terminate() { }


        /// <summary>
        /// This command can be called wither by clicking on the toolbar (as
        /// created in the Initialize method) or by directly typing the command
        /// 'ConcentricCircle'
        /// 
        /// This command will then launch a Windows Form that will collect the
        /// needed configuration information before creating the circles.
        /// <seealso cref="GeometryPlusApp.Initialize"/>
        /// </summary>
        [CommandMethod("ConcentricCircle")]
        public void ConcentricCircleCommand()
        {
            var ccForm = new ConcentricCirclesForm();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(ccForm);
        }
    }
}
