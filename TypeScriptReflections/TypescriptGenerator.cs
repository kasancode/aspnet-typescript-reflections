using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptReflections
{
    public class TypescriptGenerator
    {
        public static string GenerateDefinition(IEnumerable<Type> types, bool listup = false, TsReflectionFilters filters = null)
        {
            if (!(types?.Count() > 0))
                throw new ArgumentNullException("types count must > 0.");

            if (filters == null)
                filters = TsReflectionFilters.DefaultFilters;

            if (listup)
            {
                var listupTypes = new List<Type>();
                foreach(var t in types)
                {
                    listupTypes.AddRange(t.ListUpDependeceTypes(filters));
                }
                return generateInner(listupTypes.Distinct(), filters);
            }
            else
            {
                return generateInner(types.Distinct(), filters);
            }
        }

        public static string GenerateDefinition(Type type, bool listup = false, TsReflectionFilters filters = null)
        {
            if (type == null)
                throw new ArgumentNullException("type must be not null.");

            if (filters == null)
                filters = TsReflectionFilters.DefaultFilters;

            var types = new List<Type> {type};

            if(listup)
              types = type.ListUpDependeceTypes(filters);

            return generateInner(types, filters);
        }

        private static string generateInner(IEnumerable<Type> types, TsReflectionFilters filters)
        {
            var cb = new TypescriptCodeBuilder();

            var lastNamespace = "";
            var scope = null as ScriptScope;

            types = types.OrderBy(t => t.ToTsName()).OrderBy(t => t.GetTsNamespace());

            foreach (var t in types)
            {
                // namespace
                var ns = t.GetTsNamespace();
                if (ns != lastNamespace)
                {
                    scope?.Dispose();
                    scope = cb.EnterNameSpace(ns);
                    lastNamespace = ns;
                }

                // class
                var baseTypes = t.BaseType?.ToTsName() != "any" && filters.TypeFilter(t.BaseType)
                    ? new[] { t.BaseType.ToTsFullName() }
                    : null;

                var interfaces = t.GetInterfaces()
                    .Where(i => filters.TypeFilter(i))
                    .Select(i => i.ToTsFullName())
                    .ToArray();

                if (t.IsEnum)
                {
                    GenerateEnum(t, cb);
                }
                else
                {
                    using (cb.EnterClass(t.ToTsName(), baseTypes, interfaces, Ambient.export))
                    {
                        // property
                        foreach (var prop in t.GetProperties().Where(p => filters.PropertyFilter(t, p)))
                        {
                            cb.AppendLine($"{prop.Name} : {prop.PropertyType.ToTsFullName()};");
                        }

                        // field
                        foreach (var field in t.GetFields().Where(f => filters.FieldFilter(t, f)))
                        {
                            cb.AppendLine($"{field.Name} : {field.FieldType.ToTsFullName()};");
                        }
                    }
                }
            }
            scope?.Dispose();
            return cb.ToString();
        }


        protected static void GenerateEnum(Type type, TypescriptCodeBuilder cb)
        {
            using (cb.EnterEnum(type.Name, Ambient.export))
            {
                var names = type.GetEnumNames();
                var values = type.GetEnumValues();
                var baseType = type.GetEnumUnderlyingType();

                for (var i = 0; i < names.Length; i++)
                {
                    cb.AppendLine($"{names[i]}={GetEnumValue(values.GetValue(i), baseType)},");
                }
            }
        }

        protected static string GetEnumValue(object value, Type type)
        {
            if (type == typeof(byte))
                return ((byte)value).ToString();

            if (type == typeof(sbyte))
                return ((sbyte)value).ToString();

            if (type == typeof(short))
                return ((short)value).ToString();

            if (type == typeof(ushort))
                return ((ushort)value).ToString();

            if (type == typeof(int))
                return ((int)value).ToString();

            if (type == typeof(uint))
                return ((uint)value).ToString();

            if (type == typeof(long))
                return ((long)value).ToString();

            if (type == typeof(ulong))
                return ((ulong)value).ToString();


            return $"\"{value.ToString()}\"";
        }
    }
}
