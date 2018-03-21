using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptReflections
{
    public enum Ambient
    {
        none,
        export,
        declare
    }

    public class TypescriptCodeBuilder : ScriptCodeBuilder
    {
        public ScriptScope EnterNameSpace(string name, Ambient ambient = Ambient.none)
        {
            var value = $"namespace {name}";
            if (ambient != Ambient.none)
                value = $"{ambient} {value}";

            return this.EnterScope(value);
        }

        public ScriptScope EnterClass(string name, string[] bases = null, string[] interfaces = null, Ambient ambient = Ambient.none)
        {
            var value = $"class {name}";
            if (ambient != Ambient.none)
                value = $"{ambient} {value}";

            if (bases?.Length > 0)
                value = $"{value} extends {string.Join(",", bases)}";

            if (interfaces?.Length > 0)
                value = $"{value} impliment {string.Join(",", bases)}";

            return this.EnterScope(value);
        }

        public ScriptScope EnterEnum(string name, Ambient ambient = Ambient.none)
        {
            var value = $"enum {name}";
            if (ambient != Ambient.none)
                value = $"{ambient} {value}";

            return this.EnterScope(value);
        }

        public ScriptScope EnterInerface(string name, string[] bases = null, Ambient ambient = Ambient.none)
        {
            var value = $"interface {name}";
            if (ambient != Ambient.none)
                value = $"{ambient} {value}";

            if (bases?.Length > 0)
                value = $"{value} extends {string.Join(",", bases)}";


            return this.EnterScope(value);
        }
    }
}
