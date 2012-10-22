using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Browser;
using IronPython.Hosting;
using System.Reflection;

namespace Keggy.Scripting
{
    [Scriptable]
    public class ScriptEngine
    {
        private static ScriptEngine instance;

        protected ScriptEngine()
        {
            WebApplication.Current.RegisterScriptableObject(GetType().Name, this);
        }

        public static void Initialize()
        {
            instance = new ScriptEngine();
        }

        public static ScriptEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScriptEngine();
                }

                return instance;
            }
        }
        
        [Scriptable]
        public void Execute(string script)
        {
            try
            {                
                PythonEngine engine = PythonEngine.CurrentEngine;                
                engine.Execute(script);                
                object result = engine.Evaluate("func()");
                
                if (result != null)
                {
                    Debug.WriteLine(result);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}