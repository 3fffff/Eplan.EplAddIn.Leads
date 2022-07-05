using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using Microsoft.CSharp;

public class RegisterScriptMenu
{
    //ВАЖНО переменные в классе должны быть статическими 
    static CompilerResults results = null;

    [DeclareAction("MyScriptActionWithMenu")]
    public void MyFunctionAsAction()
    {
      if(results == null)
        results = CompileAssembly();

       //new Decider().Decide(EnumDecisionType.eOkDecision, "MyFunctionAsAction was called!", "RegisterScriptMenu", EnumDecisionReturn.eOK, EnumDecisionReturn.eOK);
	     Startup("PageName");
       return;
    }

    [DeclareAction("ActionCreate")]
    public void ActionCreate()
    {
      if(results == null)
        results = CompileAssembly();

	     Startup("Create");
       return;
    }

    [DeclareAction("ActionRemove")]
    public void ActionRemove()
    {
      if(results == null)
        results = CompileAssembly();

	     Startup("Delete");
       return;
    }

    [DeclareMenu]
    public void MenuFunction()
    {
        results = CompileAssembly();
        Eplan.EplApi.Gui.Menu oMenu1 = new Eplan.EplApi.Gui.Menu();
        oMenu1.AddMenuItem("Имя страницы", "MyScriptActionWithMenu");
        Eplan.EplApi.Gui.Menu oMenu = new Eplan.EplApi.Gui.Menu();
        uint num = oMenu.AddMainMenu("Выноски", Eplan.EplApi.Gui.Menu.MainMenuName.eMainMenuUtilities, "Добавить выноски", "ActionCreate",
                "Имя проекта", 1);
        uint num1 = oMenu.AddMenuItem("Удалить выноски", "ActionRemove", "Информация", num, 1, true, false);
        //oMenu.AddMenuItem("Удалить выноски", "ActionRemove");
    }

    public void Startup(string method)
    {
      if(results == null){
        new Decider().Decide(EnumDecisionType.eOkDecision, "Ошибка компиляции...", "RegisterScriptMenu", EnumDecisionReturn.eOK, EnumDecisionReturn.eOK);
        return;      
      }

      if (!results.Errors.HasErrors)
      {
        //new Decider().Decide(EnumDecisionType.eOkDecision, "InvokeMethod", "RegisterScriptMenu", EnumDecisionReturn.eOK, EnumDecisionReturn.eOK);
        InvokeMethod(results.CompiledAssembly,method);
      }
      else
      {
        new Decider().Decide(EnumDecisionType.eOkDecision, "exceptionInvoke", "RegisterScriptMenu", EnumDecisionReturn.eOK, EnumDecisionReturn.eOK);
        foreach (CompilerError compilerError in results.Errors)
        {
          var baseException = new BaseException(compilerError.ErrorText, MessageLevel.Error);
          baseException.FixMessage();
        }
      }
    }
    private CompilerResults CompileAssembly()
    {
      CSharpCodeProvider codeProvider = new CSharpCodeProvider();
      Assembly DataModelu = Assembly.LoadFrom(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.DataModelu.dll");
      Assembly AFu = Assembly.LoadFrom(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.AFu.dll");
      Assembly Baseu = Assembly.LoadFrom(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.Baseu.dll");
      Assembly HEServicesu = Assembly.LoadFrom(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.HEServicesu.dll");
      Assembly Starteru = Assembly.LoadFrom(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.Starteru.dll");
      Assembly Guiu = Assembly.LoadFrom(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.Guiu.dll");
      Assembly MasterDatau = Assembly.LoadFrom(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.MasterDatau.dll");
      Assembly EServicesu = Assembly.LoadFrom(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplApi.EServicesu.dll");
      //здесь изменить путь
      Assembly EplAddin = Assembly.LoadFrom(@"C:\Program Files\EPLAN\Platform\2.9.4\Bin\Eplan.EplAddIn.Leads.dll");
      CompilerParameters parameters = new CompilerParameters();
      parameters.GenerateExecutable = false;
      parameters.ReferencedAssemblies.Add("System.dll");
      parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
      parameters.ReferencedAssemblies.Add("mscorlib.dll");
     //var references = Assembly.GetAssembly(typeof(AFu)).GetReferencedAssemblies();
     // parameters.ReferencedAssemblies.AddRange(references.Select(x => Assembly.Load(x.FullName).Location).ToArray());
      parameters.ReferencedAssemblies.Add(Assembly.Load(DataModelu.FullName).Location);
      parameters.ReferencedAssemblies.Add(Assembly.Load(AFu.FullName).Location);
      parameters.ReferencedAssemblies.Add(Assembly.Load(Baseu.FullName).Location);
      parameters.ReferencedAssemblies.Add(Assembly.Load(HEServicesu.FullName).Location);
      parameters.ReferencedAssemblies.Add(Assembly.Load(Starteru.FullName).Location);
      parameters.ReferencedAssemblies.Add(Assembly.Load(Guiu.FullName).Location);
      parameters.ReferencedAssemblies.Add(Assembly.Load(MasterDatau.FullName).Location);
      parameters.ReferencedAssemblies.Add(Assembly.Load(EServicesu.FullName).Location);
      parameters.ReferencedAssemblies.Add(Assembly.Load(EplAddin.FullName).Location);
      //string binPath = PathMap.SubstitutePath("$(BIN)");
      string source = AddinCode();
      CompilerResults compilerResults = codeProvider.CompileAssemblyFromSource(parameters, source);
      return compilerResults;
    }

    private void InvokeMethod(Assembly assembly,string method)
    {
      Object obj = assembly.CreateInstance("Epl.Exec");
      MethodInfo execute = assembly.GetType("Epl.Exec").GetMethod(method);
      try
      {
        execute.Invoke(obj, null);
      }
      catch (Exception exception)
      {
        new Decider().Decide(EnumDecisionType.eOkDecision, "exception", "RegisterScriptMenu", EnumDecisionReturn.eOK, EnumDecisionReturn.eOK);
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
      }
    }
    private string AddinCode(){
            return "using System;\n" +
             "using System.Windows.Forms;\n" +
             "using Eplan.EplApi.DataModel;\n" +
             "using Eplan.EplApi.HEServices;\n" +
             "using Eplan.EplAddIn.Leads;\n" +
             "\n" +
             "namespace Epl\n" +
             "{\n" +
             "  public class Exec\n" +
             "  {\n" +
             "    public void Create()\n" +
             "    {\n" +
             "      using (LockingStep oLS = new LockingStep())\n" +
             "      {\n" +
             "       CreateDelete.Create();\n" +
             "      }\n" +
             "    }\n" + 
             "    public void Delete()\n" +
             "    {\n" +
             "      using (LockingStep oLS = new LockingStep())\n" +
             "      {\n" +
             "       CreateDelete.Delete();\n" +
             "      }\n" + 
             "    }\n" + 
             "    public void PageName()\n" +
             "    {\n" +
             "      using (LockingStep oLS = new LockingStep())\n" +
             "      {\n" +
             "        var prj = new SelectionSet().GetCurrentProject(true);\n" +
             "        var pageName = prj.Pages[0].Name;\n" +
             "        MessageBox.Show(pageName);\n" +
             "      }\n" +
             "    }\n" + 
             "  }\n" +
             "}";
    }
}