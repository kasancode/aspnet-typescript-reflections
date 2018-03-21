using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptReflections
{
    public class ScriptScope : IDisposable
    {
        protected ScriptCodeBuilder codeBuilder = null;

        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public bool PrefixWithNewLine { get; set; }
        public bool SuffixWithNewLine { get; set; }

        public ScriptScope
            (ScriptCodeBuilder builder,
            string prefix = "{",
            string suffix = "}",
            bool preLine = true,
            bool sufLine = true)
        {
            this.Prefix = prefix;
            this.Suffix = suffix;
            this.PrefixWithNewLine = preLine;
            this.SuffixWithNewLine = sufLine;

            this.codeBuilder = builder;

            if (this.PrefixWithNewLine)
                this.codeBuilder.AppendLine(this.Prefix);
            else
                this.codeBuilder.Append(this.Prefix);

            this.codeBuilder.IncreaseIndent();
        }

        public void Dispose()
        {
            this.codeBuilder.DecreaseIndent();

            if (this.SuffixWithNewLine)
                this.codeBuilder.AppendLine(this.Suffix);
            else
                this.codeBuilder.Append(this.Suffix);

        }
    }
}
