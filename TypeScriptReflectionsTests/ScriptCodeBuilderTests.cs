using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeScriptReflections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptReflections.Tests
{
    [TestClass()]
    public class ScriptCodeBuilderTests
    {
        [TestMethod()]
        public void EnterBracketsTest()
        {
            var cb = new ScriptCodeBuilder();
            var n = Environment.NewLine;

            using (cb.EnterBrackets("1"))
            {
                cb.AppendLine(2);
                using (cb.EnterBrackets("3"))
                {
                    cb.AppendLine(4.0);
                }
            }
            var result = cb.ToString();

            Assert.AreEqual($"1(2{n}    3(4{n}    ))", result);
        }

        [TestMethod()]
        public void EnterScopeTest()
        {
            var cb = new ScriptCodeBuilder();
            var n = Environment.NewLine;

            using (cb.EnterScope("1"))
            {
                cb.AppendLine(2);
                using (cb.EnterScope("3"))
                {
                    cb.AppendLine(4.0);
                }
            }
            var result = cb.ToString();

            Assert.AreEqual($"1{{{n}    2{n}    3{{{n}        4{n}    }}{n}}}{n}", result);
        }

        [TestMethod()]
        public void EnterGenericBraketsTest()
        {
            var cb = new ScriptCodeBuilder();
            var n = Environment.NewLine;

            using (cb.EnterGenericBrakets("1"))
            {
                cb.AppendLine(2);
                using (cb.EnterGenericBrakets("3"))
                {
                    cb.AppendLine(4.0);
                }
            }
            var result = cb.ToString();

            Assert.AreEqual($"1<2{n}    3<4{n}    >>", result);
        }


        [TestMethod()]
        public void TotalTest()
        {
            var cb = new ScriptCodeBuilder();

            using (cb.EnterScope("namespace hoge"))
            {
                using (cb.EnterScope("enum test"))
                {
                    cb.AppendLine("item1,");
                    cb.AppendLine("item2,");
                    cb.Append("item");
                    cb.Append(3);
                    cb.AppendLine();
                }

                cb.AppendLine();

                using (cb.EnterScope("class foo"))
                {
                    using (cb.EnterBrackets("test"))
                    {
                        cb.Append("a:number, ");
                        cb.Append("b:number, ");
                        cb.Append("c:number");
                    }
                    using (cb.EnterScope())
                    {
                        cb.AppendLine("return null;");
                    }

                    cb.AppendLine("prop1:string;");
                    cb.AppendLine("prop2:string;");

                    cb.AppendLine();

                    using (cb.EnterBrackets("test"))
                    {
                        cb.Append("a:number, ");
                        cb.Append("b:number, ");
                        cb.Append("c:number");
                    }
                    using (cb.EnterScope())
                    {
                        cb.AppendLine("return null;");
                    }

                }
            }

            Console.WriteLine(cb.ToString());
        }
    }
}