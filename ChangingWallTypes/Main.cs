using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangingWallTypes
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        static public MainPanel window;
        public List<Element> selList { set; get; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Element> AllWallsTypes = new FilteredElementCollector(doc).OfClass(typeof(WallType)).ToList();
            
            window = new MainPanel(commandData);
            window.Show();

            return Result.Succeeded;
        }
    }
}
