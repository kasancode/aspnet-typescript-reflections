using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeScriptReflections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptReflections.Tests
{
    public class tsTest<T> {
        public Dictionary<int, string> dict;
        public IEnumerable<T> list;
        public List<int> intList;
    }

    [TestClass()]
    public class TypeScriptGeneratorTests
    {

        [TestMethod()]
        public void GenerateDefinitionTest1()
        {
            var expect =
@"namespace TypeScriptReflections.Tests.TypeScriptGeneratorTests{
    export class test1{
        id : number;
        name : string;
    }
}
";
            var actual = TypescriptGenerator.GenerateDefinition(typeof(test1));
            Console.WriteLine(actual);

            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void GenerateDefinitionTest2()
        {
            var expect =
@"namespace TypeScriptReflections.Tests.TypeScriptGeneratorTests{
    export class test2{
        id : number;
        ids : number[];
        ids2 : number[];
        dict : {[index:number]:string;};
    }
}
";
            var actual = TypescriptGenerator.GenerateDefinition(typeof(test2));
            Console.WriteLine(actual);

            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void GenerateDefinitionTest3()
        {
            var expect =
@"namespace TypeScriptReflections.Tests.TypeScriptGeneratorTests{
    export class test1{
        id : number;
        name : string;
    }
    export class test3{
        id : number;
        ids : TypeScriptReflections.Tests.TypeScriptGeneratorTests.test1[];
    }
}
";
            var actual = TypescriptGenerator.GenerateDefinition(typeof(test3), listup:true);
            Console.WriteLine(actual);

            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void GenerateDefinitionTest4()
        {
            var expect =
@"namespace TypeScriptReflections.Tests.TypeScriptGeneratorTests{
    export class test4{
        id : number;
    }
}
";
            var actual = TypescriptGenerator.GenerateDefinition(typeof(test4), listup: true);
            Console.WriteLine(actual);

            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void GenerateDefinitionTest5()
        {
            var expect =
@"namespace TypeScriptReflections.Tests.TypeScriptGeneratorTests{
    export class test1{
        id : number;
        name : string;
    }
    export class test5 extends TypeScriptReflections.Tests.TypeScriptGeneratorTests.test1{
        note : string;
    }
}
";
            var actual = TypescriptGenerator.GenerateDefinition(typeof(test5), listup: true);
            Console.WriteLine(actual);

            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void GenerateDefinitionTest6()
        {
            var expect =
@"namespace TypeScriptReflections.Tests.TypeScriptGeneratorTests{
    export class test6<T>{
        test1 : T;
        test2 : T[];
        test3 : T[];
        test4 : {[index:string]:T;};
        test5 : {[index:T]:number;};
    }
}
";
            var actual = TypescriptGenerator.GenerateDefinition(typeof(test6<>), listup: true);
            Console.WriteLine(actual);

            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void GenerateDefinitionTest7()
        {
            var expect =
@"namespace TypeScriptReflections.Tests.TypeScriptGeneratorTests{
    export enum test7{
        test1=0,
        test2=2,
        test3=3,
    }
}
";
            var actual = TypescriptGenerator.GenerateDefinition(typeof(test7), listup: true);
            Console.WriteLine(actual);

            Assert.AreEqual(expect, actual);
        }

        public class test1
        {
            public int id;
            public string name;
        }

        public class test2
        {
            public int id;
            public int[] ids;
            public List<int> ids2;
            public Dictionary<int,string> dict;
        }

        public class test3
        {
            public int id;
            public test1[] ids;
        }

        public class test4
        {
            public int id;
            test2 test;
        }

        public class test5 : test1
        {
            public string note;
        }

        public class test6<T>
        {
            public T test1;
            public T[] test2;
            public List<T> test3;
            public Dictionary<string, T> test4;
            public Dictionary<T,int> test5;
        }

        public enum test7
        {
            test1,
            test2=2,
            test3
        }


    }
}