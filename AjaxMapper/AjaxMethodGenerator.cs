using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Web.Mvc;
using TypeScriptReflections;

namespace AjaxMapper
{
    public class AjaxActionInfo
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ModuleName { get; set; }
        public ParameterInfo[] ArgInfos { get; set; }
        public string ModelType { get; set; }
        public string ClassName { get; internal set; }
    }


    public class AjaxMethodGenerator
    {
        Type type = null;

        AjaxMethodGenerator() { }

        public AjaxMethodGenerator(Type type)
        {
            this.type = type ?? throw new ArgumentNullException("type must be not null.");

        }

        public Type GetType(string fullName, Assembly assembly)
        {
            var type = assembly.GetType(fullName);

            if (type != null)
                return type;

            var refAssemblies = assembly.GetReferencedAssemblies();

            foreach (var a in refAssemblies)
            {
                type = Assembly.Load(a).GetType(fullName);
                if (type != null)
                    return type;
            }

            return null;
        }

        string getName(Type type)
        {
            switch (type.Name)
            {
                case "Boolean":
                    return "boolean";
                case "String":
                case "Char":
                    return "string";
                case "Byte":
                case "SByte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "UInt16":
                case "UInt32":
                case "UInt64":
                case "Single":
                case "Double":
                case "Decimal":
                case "IntPtr":
                case "UIntPtr":
                    return "number";
                case "DateTime":
                case "DateTimeOffset":
                    return "Date";
            }
            return type.ToTsFullName();
        }


        public string Generate(string[,] convertStrings = null)
        {
            if (convertStrings == null)
                convertStrings = new string[0, 2];

            if (convertStrings.GetLength(1) != 2)
                throw new Exception("convertStrings 's size must be [*, 2].");

            var methods = type.GetMethods().Where(m =>
                m.GetCustomAttributes(typeof(ReturnObjectTypeAttribute)).Count() > 0).ToList();

            if (methods == null || methods.Count() < 1)
                return "";

            var actions = new List<AjaxActionInfo>();

            var types = new List<Type>();

            foreach (var m in methods)
            {
                var retType = m.GetCustomAttribute<ReturnObjectTypeAttribute>().ModelType;
                var typeName = "";

                if (retType == null)
                    throw new Exception($"Can not get Type of {m.Name}");


                foreach (var t in retType.GenericTypeArguments)
                {
                    types.Add(t);
                }

                if (typeof(IEnumerable).IsAssignableFrom(retType))
                {
                    if (retType.GenericTypeArguments.Length == 1)
                        typeName = this.getName(retType.GenericTypeArguments[0]) + "[]";
                    else
                        typeName = "any[]";
                }
                else
                {
                    types.Add(retType);
                    typeName = this.getName(retType);
                }

                actions.Add(new AjaxActionInfo
                {
                    ControllerName = type.Name,
                    ActionName = m.Name,
                    ArgInfos = m.GetParameters(),
                    ModelType = typeName,
                    ClassName = type.Name,
                    ModuleName = type.Namespace,
                });
            }

            var modelString = TypescriptGenerator.GenerateDefinition(types, true);

            var sb = new TypescriptCodeBuilder();

            sb.AppendLine();

            var lastModule = string.Empty;
            var lastClass = string.Empty;

            ScriptScope moduleScope = null;
            ScriptScope classScope = null;

            foreach (var act in actions.OrderBy(a => a.ClassName).OrderBy(a => a.ModuleName))
            {
                var argObjName = "null";

                if (act.ModuleName != lastModule)
                {
                    moduleScope?.Dispose();
                    sb.AppendLine();

                    moduleScope = sb.EnterNameSpace(act.ModuleName);

                    lastModule = act.ModuleName;
                }

                if (act.ClassName != lastClass)
                {
                    classScope?.Dispose();
                    sb.AppendLine();

                    classScope = sb.EnterClass(act.ClassName, ambient: Ambient.export);

                    lastClass = act.ClassName;
                }

                // Method Header

                sb.Append($"static {act.ActionName}(");
                if (act.ArgInfos?.Length > 0)
                {
                    foreach (var a in act.ArgInfos)
                    {
                        sb.Append($"{a.Name}: {getName(a.ParameterType)}, ");
                    }
                }
                sb.Append($"then: (model: {act.ModelType})=>void, ");
                sb.Append($"fail: (message: string)=>void,");
                sb.Append($"token:string = null): void");

                // Method body
                using (sb.EnterScope())
                {
                    // Create argument object
                    if (act.ArgInfos?.Length > 0)
                    {
                        argObjName = "__arg";

                        using (sb.IncreaseIndent("let __arg = {", "};"))
                        {
                            foreach (var arg in act.ArgInfos)
                            {
                                sb.AppendLine($"{arg.Name} : {arg.Name},");
                            }
                        }
                    }

                    var url = "./" + act.ControllerName.Replace("Controller", "") + "/" + act.ActionName;
                    sb.AppendLine($"this.__loadModel('{url}', {argObjName}, then, fail, token);");
                }

                sb.AppendLine();

            }


            // Aditinal methods
            using (sb.EnterScope("static __convert(text: string): string "))
            {
                using (sb.IncreaseIndent("let dict = [", "];"))
                {

                    for (var i = 0; i < convertStrings.GetLength(0); i++)
                    {
                        var s0 = convertStrings[i, 0].Replace("'", "\\'");
                        var s1 = convertStrings[i, 1].Replace("'", "\\'");
                        sb.AppendLine($"['{s0}', '{s1}'],");
                    }
                }

                sb.AppendLine("for (let d of dict) {");
                sb.AppendLine("    text = text.replace(d[0], d[1]);");
                sb.AppendLine("}");
                sb.AppendLine("return text;");
            }
            sb.AppendLine();
            sb.AppendLine("static __loadModel<TArg, TModel>(");
            sb.AppendLine("url: string,");
            sb.AppendLine("arg: TArg,");
            sb.AppendLine("then: (model: TModel) => void,");
            sb.AppendLine("fail: (message: string) => void,");
            sb.Append("token:string = null): void");

            using (sb.EnterScope())
            {
                sb.AppendLine("let xhr = new XMLHttpRequest();");
                sb.AppendLine("xhr.onreadystatechange = () => {");
                sb.AppendLine("    if (xhr.readyState == 4) {");
                sb.AppendLine("        let message = 'unkwon error.';");
                sb.AppendLine("        if (xhr.status == 200) {");
                sb.AppendLine("            try {");
                sb.AppendLine("                let jsonText = this.__convert(xhr.responseText);");
                sb.AppendLine("                then(JSON.parse(jsonText));");
                sb.AppendLine("                return;");
                sb.AppendLine("            } catch (ex) {");
                sb.AppendLine("                if (ex instanceof Error) {");
                sb.AppendLine("                    message = ex.message;");
                sb.AppendLine("                }");
                sb.AppendLine("            }");
                sb.AppendLine("        } else {");
                sb.AppendLine("            fail(message);");
                sb.AppendLine("        }");
                sb.AppendLine("    }");
                sb.AppendLine("};");
                sb.AppendLine("xhr.open('POST', url, true);");
                sb.AppendLine("xhr.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');");
                sb.AppendLine("xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');");
                sb.AppendLine("if (token)");
                sb.AppendLine("    xhr.setRequestHeader('__RequestVerificationToken', token);");
                sb.AppendLine("xhr.send(JSON.stringify(arg));");
            }

            classScope?.Dispose();
            moduleScope?.Dispose();
            sb.AppendLine();

            return modelString + sb.ToString();
        }
    }
}