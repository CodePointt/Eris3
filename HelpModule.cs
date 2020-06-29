using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;


namespace Eris1.Modules
{
    public class HelpModule
    {
        private static List<HelpObject> _helpInfo = new List<HelpObject>();
        public static void Initialize(Assembly assembly)
        {

                var methods = assembly.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(Discord.Commands.CommandAttribute), false).Length > 0)
                .ToArray();
            foreach (var method in methods)
            {
                var obj = new HelpObject();
                var attributes = method.GetCustomAttributesData();
                foreach (var attrib in attributes)
                {
                    if (attrib.AttributeType == typeof(Discord.Commands.CommandAttribute))
                        obj.Command = attrib.ConstructorArguments[0].Value.ToString().ToLower();
                    if (attrib.AttributeType == typeof(Discord.Commands.SummaryAttribute))
                        obj.Summary = attrib.ConstructorArguments[0].Value.ToString();
                    if  (attrib.AttributeType == typeof(Attributes.CategoryAttribute))
                        obj.Category = attrib.ConstructorArguments[0].Value.ToString().ToLower();
                    if  (attrib.AttributeType == typeof(Attributes.UseageAttribute))
                        obj.Useage = attrib.ConstructorArguments[0].Value.ToString();
                }

                _helpInfo.Add(obj);
             }
        }

        public static HelpObject GetCommandByName(string command)
        {
            try
            {
                return _helpInfo.First(x => x.Command == command.ToLower());
            }
            catch
            {
                return null;
            }
        }
        public static HelpObject[] GetAllCommands()
        {
            return _helpInfo.ToArray();
        }
        public static HelpObject[] GetAllCommandsByCategory(string category)
        {
            return _helpInfo.Where(x => x.Category == category.ToLower()).ToArray();
        }

    }
    public class HelpObject
    {
        public string Useage;
        public string Category;
        public string Summary;
        public string Command;

        internal HelpObject(string command = "", string summary = "", string category = "", string useage = "")
        {
            Command = command;
            Summary = summary;
            Category = category;
            Useage = useage;
        }
    }
}
