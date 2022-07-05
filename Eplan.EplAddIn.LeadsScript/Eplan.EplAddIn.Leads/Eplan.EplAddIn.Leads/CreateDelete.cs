using System;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;
using Eplan.EplApi.DataModel.E3D;
using Eplan.EplApi.DataModel.Graphics;
using Eplan.EplApi.Base;

namespace Eplan.EplAddIn.Leads
{
    public class CreateDelete
    {
        public static void Create()
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
                    CreateLead(currentEdit, vp, result);
                }
            }
        }
        public static void Delete()
        {
            SelectionSet set = new SelectionSet();
            Page currentEdit = (Page)set.CurrentlyEdited;
            currentEdit.SmartLock();
            var selObj = ObjectsUtils.GetSelectedObjectsOfType<Group>();//var selObj = ObjectsUtils.GetSelectedStorableObjects();
            foreach (var group in selObj)
            {
                group.SmartLock();
                var placegroup = group.SubPlacements;

                if (placegroup[0].GetType() == typeof(ViewPart))
                {
                    var vp = (ViewPart)placegroup[0];
                    var exPropsvp = vp.PropertyPlacements;
                    foreach (var prp in exPropsvp)
                    {
                        if (prp.DisplayedProperty.Definition.Name == "Позиция легенды" && placegroup[1].GetType() == typeof(Block))
                        {
                            group.UnGroup(placegroup, true);
                            prp.Remove();
                            var block = (Block)placegroup[1];
                            block.SmartLock();
                            var sp = block.SubPlacements;

                            foreach (var s in sp)
                            {
                                s.SmartLock();
                                s.Remove();
                            }
                        }
                    }
                }
            }
        }
        public static void CreateLead(Page page, ViewPart obj, PropertyValue result)
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
            oPropertyPlacement.Height = 10;
            oPropertyPlacement.Location = new PointD(location.X, location.Y);
            oPropertyPlacement.DisplayedProperty = result.Id;
            oPropertyPlacement.IsDocked = true;
            oPropertyPlacement.Scale(1, 1, new PointD(location.X, location.Y));
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
    }
}


