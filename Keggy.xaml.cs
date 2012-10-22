using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Keggy.Scripting;
using System.Diagnostics;
using System.Windows.Browser;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Scripting;

namespace Keggy
{
    public partial class Page : Canvas
    {
        public void Page_Loaded(object o, EventArgs e)
        {
            //InitializeComponent();

            Keggy.Scripting.ScriptEngine.Initialize();

            ScriptEnvironmentSetup setup = new ScriptEnvironmentSetup(true);
            setup.PALType = typeof(SilverlightPAL);
            ScriptEnvironment.Create(setup);
        }
    }
    
    // Definition of Silverlight Platform Adoption Layer:
    public class SilverlightPAL : PlatformAdaptationLayer
    {
        public override Assembly LoadAssembly(string name)
        {
            return Assembly.Load(LookupFullName(name));
        }

        private Dictionary<string, string> _assemblyFullNames = new Dictionary<string, string>();

        public SilverlightPAL()
        {
            LoadSilverlightAssemblyNameMapping();
        }

        // TODO: This will not be necesary as it will eventually move down into the host
        private void LoadSilverlightAssemblyNameMapping()
        {
            AssemblyName clrAssembly = new AssemblyName(typeof(object).Assembly.FullName);
            foreach (string asm in new string[] { "mscorlib", "System", "System.Core", "System.Xml.Core" })
            {
                clrAssembly.Name = asm;
                _assemblyFullNames.Add(asm.ToLower(), clrAssembly.FullName);
            }

            _assemblyFullNames.Add("system.silverlight",
                 "System.SilverLight, Version=1.0.0.0, PublicKeyToken=b03f5f7f11d50a3a");
            _assemblyFullNames.Add("agclr",
                 "agclr, Version=0.0.0.0, PublicKeyToken=b03f5f7f11d50a3a");
            _assemblyFullNames.Add("microsoft.visualbasic",
                 "Microsoft.VisualBasic, Version=8.1.0.0, PublicKeyToken=b03f5f7f11d50a3a");

            AssemblyName dlrAssembly = new AssemblyName(typeof(PlatformAdaptationLayer).Assembly.FullName);
            foreach (string asm in new string[] {
                "Microsoft.Scripting",
                "Microsoft.Scripting.Silverlight",
                "IronPython",
                "IronPython.Modules",
                "Microsoft.JScript.Compiler",
                "Microsoft.JScript.Runtime",
                "Microsoft.VisualBasic.Compiler",
                "Microsoft.VisualBasic.Scripting",
                "Ruby"})
            {
                dlrAssembly.Name = asm;
                _assemblyFullNames.Add(asm.ToLower(), dlrAssembly.FullName);
            }
        }

        protected string LookupFullName(string name)
        {
            AssemblyName asm = new AssemblyName(name);
            if (asm.Version != null || asm.GetPublicKeyToken() != null || asm.GetPublicKey() != null)
            {
                return name;
            }
            return _assemblyFullNames.ContainsKey(name.ToLower()) ? _assemblyFullNames[name.ToLower()] : name;
        }
    }
}
