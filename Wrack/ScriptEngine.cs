using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using Microsoft.Xna.Framework;

using Jint;

namespace WrackEngine
{
    public struct CommandValue
    {
        public MethodInfo MethodInfo;
        public object Invoker;

        public CommandValue(MethodInfo mi)
        {
            MethodInfo = mi;
            Invoker = Wrack.ScriptEngine;
        }

        public CommandValue(MethodInfo mi, object invoker)
        {
            MethodInfo = mi;
            Invoker = invoker;
        }
    }

    public class ScriptEngine
    {
        public static string ScriptsDirectory = "scripts/";
        public JintEngine Interpreter { get; set; }
        public Terminal Terminal { get; set; }
        public Dictionary<string, CommandValue> CommandList = new Dictionary<string, CommandValue>();
        public Dictionary<string, string> Aliases = new Dictionary<string, string>();
        public Dictionary<string, string> Shortcuts = new Dictionary<string, string>();
        public Dictionary<string, List<string>> Hooks = new Dictionary<string, List<string>>();

        public ScriptEngine()
        {
            if (!Directory.Exists(ScriptsDirectory)) Directory.CreateDirectory(ScriptsDirectory);
        }

        public virtual void Initialize()
        {
            InjectDefaultCode();

            CommandList.Clear();
            Aliases.Clear();

            MethodInfo[] mi = typeof(ScriptEngine).GetMethods();
            for (int i = 0; i < mi.Length; i++)
            {
                AddCommand(mi[i], this);
            }
        }
        
        public void AddCommand(MethodInfo mi, object invoker)
        {
            object[] oas = mi.GetCustomAttributes(typeof(CommandInfo), false);
            if (oas.Length > 0)
            {
                CommandInfo ci = (CommandInfo)oas[0];
                CommandList.Add(ci.Command.ToLower(), new CommandValue(mi, invoker));
                for (int j = 0; j < ci.Aliases.Length; j++)
                {
                    Aliases.Add(ci.Aliases[j], ci.Command.ToLower());
                }
            }
        }

        public delegate Vector2 NewVectorDelegate(double x, double y);
        public delegate Vector2 VectorOperationsDelegate(object x, object y);
        public delegate Color NewColorDelegate(double r, double g, double b, double a);
        public Color NewColor(double r, double g, double b, double a) { return new Color((byte)r, (byte)g, (byte)b, (byte)a); }
        public Vector2 NewVector(double x, double y) { return new Vector2((float)x, (float)y); }
        public Vector2 AddVectors(object x, object y) { return ((Vector2)x) + ((Vector2)y); }
        public Vector2 SubtractVectors(object x, object y) { return ((Vector2)x) - ((Vector2)y); }
        public Vector2 MultiplyVectors(object x, object y) { return ((Vector2)x) * ((Vector2)y); }
        public Vector2 DivideVectors(object x, object y) { return ((Vector2)x) / ((Vector2)y); }
        public virtual void InjectDefaultCode()
        {
            Interpreter = new JintEngine();
            Interpreter.DisableSecurity();

            Interpreter.SetFunction("AddHook", new AddHookDelegate(AddHook));
            Interpreter.SetFunction("AddShortcut", new AddHookDelegate(AddShortcut));

            Interpreter.SetFunction("Color", new NewColorDelegate(NewColor));
            Interpreter.SetFunction("Vector2", new NewVectorDelegate(NewVector));
            Interpreter.SetFunction("AddVectors", new VectorOperationsDelegate(AddVectors));
            Interpreter.SetFunction("SubtractVectors", new VectorOperationsDelegate(SubtractVectors));
            Interpreter.SetFunction("MultiplyVectors", new VectorOperationsDelegate(MultiplyVectors));
            Interpreter.SetFunction("DivideVectors", new VectorOperationsDelegate(DivideVectors));
        }

        public void CallHooks(string hook, params object[] args)
        {
            hook = hook.ToLower();
            if (!Hooks.ContainsKey(hook)) return;
            List<string> funcs = Hooks[hook];
            foreach (string func in funcs)
            {
                try
                {
                    bool exists = (bool)Interpreter.Run("typeof " + func + " == 'function'");
                    if (exists) Interpreter.CallFunction(func, args);
                    else Hooks[hook].Remove(func);
                    return;
                }
                catch (Jint.Native.JsException e)
                {
                    Terminal.WriteLine(TerminalMessageType.Error, "SCRIPTS: Hook \"{0}\" failed: {1}", hook, e.Value);
                }
                catch (Exception e)
                {
                    Terminal.WriteLine(TerminalMessageType.Error, "SCRIPTS: Hook \"{0}\" failed: {1}", hook, e.Message);
                    if (e.InnerException != null)
                    {
                        Terminal.WriteLine(TerminalMessageType.Error, "SCRIPTS: Internal Hook Error: {0}", hook, e.InnerException.Message);
                    }
                }
            }
        }

        public void RunScript(string script)
        {
            script = ScriptsDirectory + script.Replace("..", "");
            if (script.Length > 2) if (script.Substring(script.Length - 3).ToLower() != ".js") script = script + ".js";
            if (File.Exists(script))
            {
                try
                {
                    FileStream fs = new FileStream(script, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs);
                    string str = sr.ReadToEnd();
                    sr.Close();
                    fs.Close();
                    Terminal.WriteLine("SCRIPTS: Running script \"{0}\".", script);
                    Terminal.WriteLine("SCRIPTS: Returned: {0}", Interpreter.Run(str));
                }
                catch (Exception e)
                {
                    Terminal.WriteLine(TerminalMessageType.Error, "SCRIPTS: Script \"{0}\" failed: {1}", script, e.Message);
                    InjectDefaultCode();
                }
            }
            else Terminal.WriteLine(TerminalMessageType.Warning, "SCRIPTS: Could not find script \"{0}\".", script);        
        }

        public void CommandFailure(string reason, Func<string[], string, bool> f)
        {
            object[] oas = f.Method.GetCustomAttributes(typeof(CommandInfo), false);
            if (oas.Length > 0)
            {
                CommandInfo ci = (CommandInfo)oas[0];
                if (Terminal != null) Terminal.WriteLine(TerminalMessageType.Error, "SCRIPTS: " + ci.Command.ToUpper() + " Failed: " + reason);
                else Console.WriteLine("SCRIPTS: " + ci.Command.ToUpper() + " Failed: " + reason);
            }
        }

        public void CommandWarning(string warning, Func<string[], string, bool> f)
        {
            object[] oas = f.Method.GetCustomAttributes(typeof(CommandInfo), false);
            if (oas.Length > 0)
            {
                CommandInfo ci = (CommandInfo)oas[0];
                if (Terminal != null) Terminal.WriteLine(TerminalMessageType.Warning, "SCRIPTS: {0} Warning: {1}", ci.Command.ToString().ToUpper(), warning);
                else Console.WriteLine("SCRIPTS: {0} Warning: {1}", ci.Command.ToString().ToUpper(), warning);
            }
        }

        public void CallCommand(string command)
        {
            string[] args = Input.ParseArguments(command);
            if (Shortcuts.ContainsKey(command.Trim()))
            {
                args = new string[2];
                args[0] = "runsnippet";
                args[1] = Shortcuts[command.Trim()];
                command = args[0] + " " + args[1];
            }
            if (Aliases.ContainsKey(args[0].ToLower()))
            {
                int orl = args[0].Length;
                if (orl == command.Length) orl--;
                args[0] = Aliases[args[0]];
                command = args[0] + command.Trim().Substring(orl);
            }
            if (CommandList.ContainsKey(args[0].ToLower()))
            {
                CommandValue cv = CommandList[args[0]];
                cv.MethodInfo.Invoke(cv.Invoker, new object[] { args, command });
            }
            else
            {
                List<string> s = new List<string>(args);
                s.Insert(0, "runsnippet");
                command = "runsnippet " + command;
                //if (Terminal != null) Terminal.WriteLine(TerminalMessageType.Warning, "SCRIPTS: Could not find command " + args[0] + ".");
                //else Console.WriteLine("SCRIPTS: Could not find command " + args[0] + ".");
                RunSnippet(s.ToArray(), command);
            }
        }

        public void AutoComplete(string command)
        {

        }

        public bool AddShortcut(string cmd, string snippet)
        {
            if (cmd == null || snippet == null) return false;
            if (Shortcuts.ContainsKey(cmd))
            {
                Shortcuts[cmd] = snippet;
                return true;
            }
            else
            {
                Shortcuts.Add(cmd, snippet);
                return true;
            }
        }
        
        public delegate bool AddHookDelegate(string hook, string function);
        public bool AddHook(string hook, string function)
        {
            if (hook == null || function == null) return false;
            hook = hook.ToLower();
            if (Hooks.ContainsKey(hook))
            {
                if (Hooks[hook].Contains(function))
                {
                    return true;
                }
                else
                {
                    Hooks[hook].Add(function);
                    return true;
                }
            }
            else
            {
                Hooks.Add(hook, new List<string>(new string[] { function }));
                return true;
            }
        }

        [CommandInfo("runsnippet", "Runs the specified string as JavaScript.", "runjs", "js")]
        public bool RunSnippet(string[] args, string command)
        {
            if (args.Length < 2) { CommandFailure("Missing argument <code>.", RunSnippet); return false; }
            string runstr = command.Substring(args[0].Length).Trim();
            Terminal.WriteLine("SCRIPTS: Running \"{0}\".", runstr);
            try
            {
                Terminal.WriteLine("SCRIPTS: Returned: {0}", Interpreter.Run(runstr));
                return true;
            }
            catch (Exception e)
            {
                CommandFailure(e.Message, RunSnippet);
                return false;
            }
        }

        [CommandInfo("runscript", "Runs the specified JavaScript file in the scripts folder.", "script")]
        public bool RunScript(string[] args, string command)
        {
            if (args.Length < 2) { CommandFailure("Missing argument <scriptFile>.", RunScript); return false; }
            string runstr = command.Substring(args[0].Length).Trim();
            RunScript(runstr);
            return true;
        }

        [CommandInfo("listshortcuts", "Lists all the current shortcuts.", "shortcuts")]
        public bool ListShortcuts(string[] args, string command)
        {
            int counter = 1;
            Terminal.WriteLine("TERMINAL: Shortcut List");
            foreach (KeyValuePair<string, string> kvp in Shortcuts)
            {
                string str = "TERMINAL: " + counter + ". \"" + kvp.Key + "\" = \"" + kvp.Value + "\".";
                Terminal.WriteLine(str);
                counter++;
            }
            return true;
        }

        [CommandInfo("listaliases", "Lists all the current aliases.", "aliases")]
        public bool ListAliases(string[] args, string command)
        {
            int counter = 1;
            Terminal.WriteLine("TERMINAL: Alias List");
            foreach (KeyValuePair<string, string> kvp in Aliases)
            {
                string str = "TERMINAL: " + counter + ". \"" + kvp.Key + "\" = \"" + kvp.Value + "\".";
                Terminal.WriteLine(str);
                counter++;
            }
            return true;
        }

        [CommandInfo("help", "Lists all commands or the details of the specified command.", "halp")]
        public bool Help(string[] args, string command)
        {
            if (args.Length > 1)
            {
                if (Aliases.ContainsKey(args[1].ToLower()))
                {
                    int orl = args[1].Length;
                    if (orl == command.Length) orl--;
                    args[1] = Aliases[args[1]];
                    command = args[1] + command.Trim().Substring(orl);
                }

                if (!CommandList.ContainsKey(args[1].ToLower()))
                {
                    CommandFailure("Could not find command \"" + args[1] + "\".", Help);
                    return false;
                }
                MethodInfo mi = CommandList[args[1].ToLower()].MethodInfo;
                CommandInfo ci = (CommandInfo)(mi.GetCustomAttributes(typeof(CommandInfo), false)[0]);
                string aliases = ""; 
                for (int i = 0; i < ci.Aliases.Length; i++) { aliases += ci.Aliases[i]; if (i < ci.Aliases.Length - 1) aliases += ", "; }
                if (aliases == "") aliases = "None";
                Terminal.WriteLine(TerminalMessageType.Info, "TERMINAL: \"{0}\" Description: {1}", ci.Command, ci.Description);
                Terminal.WriteLine(TerminalMessageType.Info, "TERMINAL: Aliases: {0}.", aliases);
                return true;
            }
            else
            {
                int counter = 1;
                Terminal.WriteLine("TERMINAL: Help Index");
                foreach (KeyValuePair<string, CommandValue> kvp in CommandList)
                {
                    MethodInfo mi = kvp.Value.MethodInfo;
                    CommandInfo ci = (CommandInfo)(mi.GetCustomAttributes(typeof(CommandInfo), false)[0]);
                    string aliases = "";
                    for (int i = 0; i < ci.Aliases.Length; i++) { aliases += ci.Aliases[i]; if (i < ci.Aliases.Length - 1) aliases += ", "; }
                    string str = "TERMINAL: " + counter + ". \"" + ci.Command + "\" - " + ci.Description;
                    Terminal.WriteLine(TerminalMessageType.Info, str);
                    counter++;
                }
                return true;
            }
        }
    }

    public class CommandInfo : Attribute
    {
        public readonly string Command;
        public readonly string Description;
        public readonly string[] Aliases;
        public CommandInfo(string command) : this(command, "") { }
        public CommandInfo(string command, string description) : this(command, description, "") { }
        public CommandInfo(string command, string description, params string[] aliases)
        {
            Command = command;
            Description = description;
            if (aliases.Length < 1) return;
            if (aliases[0] == "") return;
            Aliases = aliases;
        }
    }
}
