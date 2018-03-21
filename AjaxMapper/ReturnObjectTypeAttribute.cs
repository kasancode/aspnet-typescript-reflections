using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjaxMapper
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ReturnObjectTypeAttribute : Attribute
    {
        public Type ModelType { get; internal set; }

        public ReturnObjectTypeAttribute(Type modelType) : base()
        {
            this.ModelType = modelType;
        }
    }
}
