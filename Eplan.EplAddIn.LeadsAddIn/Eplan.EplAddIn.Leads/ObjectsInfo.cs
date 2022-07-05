using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.E3D;
using Eplan.EplApi.DataModel.Graphics;
using Eplan.EplApi.HEServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eplan.EplAddIn.test
{
    class ObjectsInfo : IEplAction
    {
        public bool OnRegister(ref string Name, ref int Ordinal)
        {
            Name = "PageAction";
            Ordinal = 20;

            return true;
        }

        public bool Execute(ActionCallingContext oActionCallingContext)
        {
            SelectionSet set = new SelectionSet();

            Page[] pages = set.OpenedPages;
            Page currentEdit = (Page)set.CurrentlyEdited;
            var selObj = ObjectsUtils.GetSelectedObjectsOfType<ViewPart>();//var selObj = ObjectsUtils.GetSelectedStorableObjects();
            foreach (var vp in selObj)
            {
                var props = ObjectsUtils.getPlacementPropsOfViewPart(vp);
                foreach (var prp in props)
                {
                    var pA = prp.ExistingValues;
                    var result = Array.Find(pA, element => element.Id.Definition.Name == "Позиция легенды");
                    Create(currentEdit, vp, result);
                }
            }

            return true;
        }
        public static void Create(Page page, ViewPart obj, PropertyValue result)
        {
            var eA = obj.PropertyPlacements;
            var res = Array.Find(eA, element => element.DisplayedProperty == result.Id);
            if (res != null) return;
            page.SmartLock();
            var size = obj.GetBoundingBox();
            var location = obj.Location;
            ContextPropertyPlacement3D oPropertyPlacement = new ContextPropertyPlacement3D();
            oPropertyPlacement.Create(obj, result.Id);
            oPropertyPlacement.IsVisible = true;
            oPropertyPlacement.Height = 40;
            oPropertyPlacement.Location = new PointD(location.X, location.Y);
            oPropertyPlacement.DisplayedProperty = result.Id;
            oPropertyPlacement.IsDocked = false;
            oPropertyPlacement.Scale(1,1, new PointD(location.X, location.Y));
            oPropertyPlacement.SmartLock();
            var line1 = new Line();
            line1.Create(page, new PointD(location.X, location.Y), new PointD(location.X + 10, location.Y + 10));
            line1.SmartLock();
            line1.Pen = new Pen((short)GetRandomNumber(1, 255), -16002, -16002, -16002, 0);
            var line2 = new Line();
            line2.Create(page, new PointD(location.X + 10, location.Y + 10), new PointD(location.X + 10 + 5, location.Y + 10));
            line2.SmartLock();
            line2.Pen = new Pen((short)GetRandomNumber(1, 255), -16002, -16002, -16002, 0);
            line2.LockObject();
            Placement[] placeArr = new Placement[2];
            placeArr[0] = line1;
            placeArr[1] = line2;
            var block = new Block();
            block.Create(page, placeArr);
            block.SmartLock();
            Placement[] placeGroup = new Placement[2];
            placeGroup[0] = obj;
            placeGroup[1] = block;
            var group = new Group();
            group.Create(placeGroup);
            group.SmartLock();
        }

        public static double GetRandomNumber(int minimum, int maximum)
        {
            Random random = new Random();
            return random.Next(minimum, maximum);
        }

        public void GetActionProperties(ref ActionProperties actionProperties)
        {
        }
    }
}
