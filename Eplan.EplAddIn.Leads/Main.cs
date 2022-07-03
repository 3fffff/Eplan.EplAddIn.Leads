using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.Gui;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;

namespace Eplan.EplAddIn.test
{
    public class Main : IEplAddIn
    {
        public bool OnRegister(ref bool bLoadOnStart)
        {
            bLoadOnStart = true;
            return true;
        }

        public bool OnUnregister()
        {
            return true;
        }

        public bool OnInit()
        {
            return true;
        }

        public bool OnInitGui()
        {
            string[] references = new string[1];
            string binPath = PathMap.SubstitutePath("$(BIN)");
            references[0] = @"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.AFu.dll";
            

            //new Decider().Decide(EnumDecisionType.eOkDecision, references.ToString(), "RegisterScriptMenu", EnumDecisionReturn.eOK, EnumDecisionReturn.eOK);

            /* var results = CompileAssembly(references);


             if (!results.Errors.HasErrors)
             {
                 InvokeMethod(results.CompiledAssembly);
             }
             else
             {
                 foreach (CompilerError compilerError in results.Errors)
                 {
                     var baseException = new BaseException(compilerError.ErrorText, MessageLevel.Error);
                     baseException.FixMessage();
                 }
             }*/
            Menu oMenu = new Menu();
            uint num = oMenu.AddMainMenu("Выноски", Menu.MainMenuName.eMainMenuUtilities, "Добавить выноски", "PageAction",
                "Имя проекта, фирмы и дата создания", 1);
            uint num1 = oMenu.AddMenuItem("Удалить выноски", "ActionTest", "Информация", num, 1, true, false);
            return true;
        }

        private CompilerResults CompileAssembly(string[] source)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            Assembly.LoadFrom(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.AFu.dll");
            AssemblyReference asref = new AssemblyReference(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.AFu.dll");
            //parameters.
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("mscorlib.dll");
            //parameters.ReferencedAssemblies.Add("Eplan.EplApi.Starteru.dll");
            //parameters.ReferencedAssemblies.Add("Eplan.EplApi.Baseu.dll");
            parameters.ReferencedAssemblies.Add(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.AFu.dll");
            //parameters.ReferencedAssemblies.Add("Eplan.EplApi.DataModelu.dll");
            //parameters.ReferencedAssemblies.Add("Eplan.EplApi.HEServicesu.dll");
            //parameters.ReferencedAssemblies.Add("Eplan.EplAddIn.Leads.dll");
            CompilerResults compilerResults = codeProvider.CompileAssemblyFromFile(parameters, source);
            return compilerResults;
        }

        private void InvokeMethod(Assembly assembly)
        {
            new Decider().Decide(EnumDecisionType.eOkDecision, "load", "RegisterScriptMenu", EnumDecisionReturn.eOK, EnumDecisionReturn.eOK);
            /*Object obj = assembly.CreateInstance("Foo.Bar");
            MethodInfo execute = assembly.GetType("Foo.Bar").GetMethod("Execute");
            try
            {
              execute.Invoke(obj, null);
            }
            catch (Exception exception)
            {
              if (exception.InnerException != null)
              {
                var baseException = new BaseException(exception.InnerException.Message, MessageLevel.Error);
                baseException.FixMessage();
              }
              else
              {
                var baseException = new BaseException(exception.Message, MessageLevel.Error);
                baseException.FixMessage();
              }
            }*/
        }

        public bool OnExit()
        {
            return true;
        }
    }
}
