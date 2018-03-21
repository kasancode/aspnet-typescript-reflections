using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeScriptReflections.Attributes;

namespace TypeScriptReflections
{
    public class TsReflectionFilters
    {
        public readonly static TsReflectionFilters DefaultFilters = new TsReflectionFilters();

        public virtual bool ClassAsInterface { get; set; } = true;
        public virtual bool PropertyAsFieald { get; set; } = true;



        public virtual bool TypeFilter(Type type)
        {
            return type.GetCustomAttribute(typeof(TypeScriptIgnoreAttribute)) == null;
        }

        public virtual bool PropertyFilter(Type type, PropertyInfo info)
        {
            return
                info.GetCustomAttribute(typeof(TypeScriptIgnoreAttribute)) == null &&
                info.GetMethod.IsPublic &&
                info.DeclaringType == type;
        }

        public virtual bool FieldFilter(Type type, FieldInfo info)
        {
            return
                info.GetCustomAttribute(typeof(TypeScriptIgnoreAttribute)) == null &&
                info.IsPublic &&
                info.DeclaringType == type;
        }

        public virtual bool MethodFilter(Type type, MethodInfo info)
        {
            return
                info.GetCustomAttribute(typeof(TypeScriptIgnoreAttribute)) == null &&
                info.IsPublic &&
                info.DeclaringType == type &&
                !info.IsPropertyAccessor();
        }

        internal bool EnumFilter(Type type)
        {
            return type.GetCustomAttribute(typeof(TypeScriptIgnoreAttribute)) == null;
        }
    }
}
