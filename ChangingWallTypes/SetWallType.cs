using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChangingWallTypes
{
    public class SetWallType
    {
        private ExternalCommandData _commandData;
        public DelegateCommand SetType { get; }
        public WallType SelectedType { get; set; }
        public List<Element> PickedObjects { get; }
        public List<WallType> WallTypes { get; }

        public SetWallType(ExternalCommandData commandData)
        {
            _commandData = commandData;
            SetType = new DelegateCommand(OnSetType);
            PickedObjects = PickObjects(commandData);
            WallTypes = ListWallTypes(commandData);
        }

        public static List<Element> PickObjects(ExternalCommandData commandData)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var selectedObjects = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, "Выберите элементы");
            List<Element> elementList = selectedObjects.Select(selectedObject => doc.GetElement(selectedObject)).ToList();
            return elementList;
        }

        public static List<WallType> ListWallTypes(ExternalCommandData commandData)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<WallType> AllWallsTypes = new FilteredElementCollector(doc)
                                            .OfClass(typeof(WallType))
                                            .Cast<WallType>()
                                            .ToList();
            return AllWallsTypes;
        }

        public event EventHandler CloseRequest;
        private void RaiseCoseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
        private void OnSetType()
        {
            RaiseCoseRequest();
            var uiapp = _commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (SelectedType == null || PickedObjects.Count == 0)
                return;

            using (var tr = new Transaction(doc, "Set Wall Type"))
            {
                tr.Start();
                foreach (var elementWall in PickedObjects)
                {
                    if (elementWall is Wall)
                    {
                        var wall = elementWall as Wall;
                        wall.WallType = SelectedType;
                    }                    
                }
                tr.Commit();
            }
        }
    }
}
