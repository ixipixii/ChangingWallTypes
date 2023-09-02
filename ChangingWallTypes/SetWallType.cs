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
    [Transaction(TransactionMode.Manual)]
    public class SetWallType
    {
        private ExternalCommandData _commandData;
        public DelegateCommand SetType { get; }
        public object PickedObjects { get; }

        public SetWallType(ExternalCommandData commandData)
        {
            _commandData = commandData;
            SetType = new DelegateCommand(OnSetType);
            PickedObjects = PickObjects(commandData);
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

            using (var tr = new Transaction(doc, "Set Wall Type"))
            {
                tr.Start();

                TaskDialog.Show("Transaction", "Ok");

                tr.Commit();
            }
        }

        public class EventTypeWall : IExternalEventHandler
        {
            public void Execute(UIApplication app)
            {
                throw new NotImplementedException();
            }

            public string GetName()
            {
                throw new NotImplementedException();
            }
        }
    }
}
