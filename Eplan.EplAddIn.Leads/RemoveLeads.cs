using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eplan.EplAddIn.test
{
    public class Action_Test : IEplAction
    {
        public bool OnRegister(ref string Name, ref int Ordinal)
        {
            Name = "ActionTest";
            Ordinal = 20;
            return true;
        }

        public bool Execute(ActionCallingContext oActionCallingContext)
        {
            SelectionSet set = new SelectionSet();
            Page currentEdit = (Page)set.CurrentlyEdited;
            currentEdit.SmartLock();
            var selObj = ObjectsUtils.GetSelectedObjectsOfType<Group>();//var selObj = ObjectsUtils.GetSelectedStorableObjects();
            foreach (var group in selObj)
            {
                group.SmartLock();
                var placegroup = group.SubPlacements;

                //foreach (var pg in placegroup)
                // {
                if (placegroup[0].GetType().ToString() == "Eplan.EplApi.DataModel.ViewPart")
                {
                    var vp = (ViewPart)placegroup[0];
                    var exPropsvp = vp.PropertyPlacements;
                    foreach (var prp in exPropsvp)
                    {
                        if (prp.DisplayedProperty.Definition.Name == "Позиция легенды" && placegroup[1].GetType().ToString() == "Eplan.EplApi.DataModel.Block")
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
                // }
            }
            return true;
        }

        public void GetActionProperties(ref ActionProperties actionProperties)
        {
        }
    }
}
