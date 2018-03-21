using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptReflections
{
    public class ScriptCodeBuilder
    {
        public int IndentCount { get; set; } = 0;
        public int IndentSpaceSize { get; set; } = 4;

        protected bool newLine { get; set; } = false;

        internal StringBuilder stringBuilder = new StringBuilder();


        public void IncreaseIndent()
        {
            this.IndentCount++;
        }

        public void DecreaseIndent()
        {
            this.IndentCount--;
            if (this.IndentCount < 0)
                this.IndentCount = 0;
        }

        public ScriptCodeBuilder Append(char value, int repeatCount) => this.AppendHelper(() => this.stringBuilder.Append(value, repeatCount));
        public ScriptCodeBuilder Append(char[] value, int startIndex, int charCount) => this.AppendHelper(() => this.stringBuilder.Append(value, startIndex, charCount));
        public ScriptCodeBuilder Append(String value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(string value, int startIndex, int count) => this.AppendHelper(() => this.stringBuilder.Append(value, startIndex, count));
        public ScriptCodeBuilder Append(bool value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(char value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(sbyte value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(byte value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(short value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(int value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(long value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(float value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(double value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(decimal value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(ushort value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(uint value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(ulong value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(object value) => this.AppendHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder Append(char[] value) => this.AppendHelper(() => this.stringBuilder.Append(value));

        public ScriptCodeBuilder AppendLine(char value, int repeatCount) => this.AppendLineHelper(() => this.stringBuilder.Append(value, repeatCount));
        public ScriptCodeBuilder AppendLine(char[] value, int startIndex, int charCount) => this.AppendLineHelper(() => this.stringBuilder.Append(value, startIndex, charCount));
        public ScriptCodeBuilder AppendLine(String value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(string value, int startIndex, int count) => this.AppendLineHelper(() => this.stringBuilder.Append(value, startIndex, count));
        public ScriptCodeBuilder AppendLine(bool value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(char value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(sbyte value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(byte value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(short value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(int value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(long value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(float value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(double value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(decimal value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(ushort value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(uint value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(ulong value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(object value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));
        public ScriptCodeBuilder AppendLine(char[] value) => this.AppendLineHelper(() => this.stringBuilder.Append(value));

        public ScriptCodeBuilder AppendLine() => this.AppendLineHelper(() => { });

        protected ScriptCodeBuilder AppendHelper(Action action)
        {
            if (this.newLine)
                this.stringBuilder.Append(' ', this.IndentCount * this.IndentSpaceSize);

            action();

            this.newLine = false;
            return this;
        }

        protected ScriptCodeBuilder AppendLineHelper(Action action)
        {
            if (this.newLine)
                this.stringBuilder.Append(' ', this.IndentCount * this.IndentSpaceSize);

            action();
            this.stringBuilder.AppendLine();

            this.newLine = true;
            return this;
        }

        public ScriptScope EnterScope(string text = null)
        {
            if (!string.IsNullOrEmpty(text))
                this.Append(text);

            return new ScriptScope(
                 this,
                 "{",
                 "}",
                 true,
                 true);
        }

        public ScriptScope IncreaseIndent(string prefix = "", string suffix = "", bool preLine = true, bool sufLine = true)
        {
            return new ScriptScope(
                 this,
                 prefix,
                 suffix,
                 preLine,
                 sufLine);
        }

        public ScriptScope EnterBrackets(string text = null)
        {
            if (!string.IsNullOrEmpty(text))
                this.Append(text);

            return new ScriptScope(
                this,
                "(",
                ")",
                false,
                false);
        }

        public ScriptScope EnterGenericBrakets(string text = null)
        {
            if (!string.IsNullOrEmpty(text))
                this.Append(text);

            return new ScriptScope(
                this,
                "<",
                ">",
                false,
                false);
        }

        public ScriptScope EnterList()
        {
            return new ScriptScope(
                this,
                "[",
                "]",
                false,
                false);
        }



        public override string ToString()
        {
            return this.stringBuilder.ToString();
        }
    }
}
