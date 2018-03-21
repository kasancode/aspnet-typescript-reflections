using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeScriptReflections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace TypeScriptReflections.Tests
{
    [TestClass()]
    public class TsTypeExtensionsTests
    {
        [TestMethod()]
        public void ToTsNameTest()
        {
            Assert.AreEqual("boolean", typeof(bool).ToTsName());
            Assert.AreEqual("string", typeof(string).ToTsName());
            Assert.AreEqual("string", typeof(char).ToTsName());
            Assert.AreEqual("number", typeof(byte).ToTsName());
            Assert.AreEqual("number", typeof(sbyte).ToTsName());
            Assert.AreEqual("number", typeof(short).ToTsName());
            Assert.AreEqual("number", typeof(int).ToTsName());
            Assert.AreEqual("number", typeof(long).ToTsName());
            Assert.AreEqual("number", typeof(ushort).ToTsName());
            Assert.AreEqual("number", typeof(uint).ToTsName());
            Assert.AreEqual("number", typeof(ulong).ToTsName());
            Assert.AreEqual("number", typeof(float).ToTsName());
            Assert.AreEqual("number", typeof(double).ToTsName());
            Assert.AreEqual("number", typeof(decimal).ToTsName());

            Assert.AreEqual("boolean", typeof(Boolean).ToTsName());
            Assert.AreEqual("string", typeof(String).ToTsName());
            Assert.AreEqual("string", typeof(Char).ToTsName());
            Assert.AreEqual("number", typeof(Byte).ToTsName());
            Assert.AreEqual("number", typeof(SByte).ToTsName());
            Assert.AreEqual("number", typeof(Int16).ToTsName());
            Assert.AreEqual("number", typeof(Int32).ToTsName());
            Assert.AreEqual("number", typeof(Int64).ToTsName());
            Assert.AreEqual("number", typeof(UInt16).ToTsName());
            Assert.AreEqual("number", typeof(UInt32).ToTsName());
            Assert.AreEqual("number", typeof(UInt64).ToTsName());
            Assert.AreEqual("number", typeof(Single).ToTsName());
            Assert.AreEqual("number", typeof(Double).ToTsName());
            Assert.AreEqual("number", typeof(Decimal).ToTsName());
            Assert.AreEqual("number", typeof(IntPtr).ToTsName());
            Assert.AreEqual("number", typeof(UIntPtr).ToTsName());
            Assert.AreEqual("Date", typeof(DateTime).ToTsName());
            Assert.AreEqual("Date", typeof(DateTimeOffset).ToTsName());

            Assert.AreEqual("Type", typeof(Type).ToTsName());
            Assert.AreEqual("StringBuilder", typeof(StringBuilder).ToTsName());

        }

        [TestMethod()]
        public void ToTsNameTest2()
        {
            Assert.AreEqual("number[]", typeof(int[]).ToTsName());
            Assert.AreEqual("number[]", typeof(List<int>).ToTsName());
            Assert.AreEqual("number[]", typeof(IEnumerable<int>).ToTsName());
            Assert.AreEqual("any[]", typeof(IEnumerable).ToTsName());

            Assert.AreEqual("number[[]]", typeof(int[,]).ToTsName());
            Assert.AreEqual("number[[[]]]", typeof(int[,,]).ToTsName());

            Assert.AreEqual("number[][]", typeof(IEnumerable<IEnumerable<int>>).ToTsName());

        }

        [TestMethod()]
        public void ToTsNameTest3()
        {
            Assert.AreEqual("{[index:number]:string;}", typeof(Dictionary<int, string>).ToTsName());
            Assert.AreEqual("{[index:string]:string;}", typeof(Dictionary<string, string>).ToTsName());
            Assert.AreEqual("{[index:number]:Date;}", typeof(Dictionary<int, DateTime>).ToTsName());
        }

        [TestMethod()]
        public void ToTsNameTest4()
        {
            Assert.AreEqual(
                "test1",
                typeof(test1).ToTsName()
                );
        }

        [TestMethod()]
        public void ToTsFullNameTest()
        {
            Assert.AreEqual(
                "TypeScriptReflections.Tests.TsTypeExtensionsTests.test1",
                typeof(test1).ToTsFullName()
                );
        }

        [TestMethod()]
        public void IsTsBuiltinTypeTest()
        {
            Assert.IsTrue(typeof(bool).IsTsBuiltinType());
            Assert.IsTrue(typeof(string).IsTsBuiltinType());
            Assert.IsTrue(typeof(char).IsTsBuiltinType());
            Assert.IsTrue(typeof(byte).IsTsBuiltinType());
            Assert.IsTrue(typeof(sbyte).IsTsBuiltinType());
            Assert.IsTrue(typeof(short).IsTsBuiltinType());
            Assert.IsTrue(typeof(int).IsTsBuiltinType());
            Assert.IsTrue(typeof(long).IsTsBuiltinType());
            Assert.IsTrue(typeof(ushort).IsTsBuiltinType());
            Assert.IsTrue(typeof(uint).IsTsBuiltinType());
            Assert.IsTrue(typeof(ulong).IsTsBuiltinType());
            Assert.IsTrue(typeof(float).IsTsBuiltinType());
            Assert.IsTrue(typeof(double).IsTsBuiltinType());
            Assert.IsTrue(typeof(decimal).IsTsBuiltinType());

            Assert.IsTrue(typeof(Boolean).IsTsBuiltinType());
            Assert.IsTrue(typeof(String).IsTsBuiltinType());
            Assert.IsTrue(typeof(Char).IsTsBuiltinType());
            Assert.IsTrue(typeof(Byte).IsTsBuiltinType());
            Assert.IsTrue(typeof(SByte).IsTsBuiltinType());
            Assert.IsTrue(typeof(Int16).IsTsBuiltinType());
            Assert.IsTrue(typeof(Int32).IsTsBuiltinType());
            Assert.IsTrue(typeof(Int64).IsTsBuiltinType());
            Assert.IsTrue(typeof(UInt16).IsTsBuiltinType());
            Assert.IsTrue(typeof(UInt32).IsTsBuiltinType());
            Assert.IsTrue(typeof(UInt64).IsTsBuiltinType());
            Assert.IsTrue(typeof(Single).IsTsBuiltinType());
            Assert.IsTrue(typeof(Double).IsTsBuiltinType());
            Assert.IsTrue(typeof(Decimal).IsTsBuiltinType());
            Assert.IsTrue(typeof(IntPtr).IsTsBuiltinType());
            Assert.IsTrue(typeof(UIntPtr).IsTsBuiltinType());
            Assert.IsTrue(typeof(DateTime).IsTsBuiltinType());
            Assert.IsTrue(typeof(DateTimeOffset).IsTsBuiltinType());

            Assert.IsFalse(typeof(Dictionary<int, string>).IsTsBuiltinType());
            Assert.IsFalse(typeof(Type).IsTsBuiltinType());
            Assert.IsFalse(typeof(StringBuilder).IsTsBuiltinType());

            Assert.IsFalse(typeof(test.Int32).IsTsBuiltinType());

        }

        [TestMethod()]
        public void ListUpDependeceTypesTest()
        {
            var ts = typeof(test1).ListUpDependeceTypes().ToArray();

            ts.ToList().ForEach(t => Console.WriteLine(t.ToTsFullName()));

            var res = new[] { typeof(test1) };

            AssertArray(res, ts, (t) => t.ToTsFullName());
        }

        [TestMethod()]
        public void ListUpDependeceTypesTest2()
        {
            var ts = typeof(test2).ListUpDependeceTypes().ToArray();

            ts.ToList().ForEach(t => Console.WriteLine(t.ToTsFullName()));

            var res = new[] { typeof(test1), typeof(test2) };

            AssertArray(res, ts, (t) => t.ToTsFullName());
        }

        [TestMethod()]
        public void ListUpDependeceTypesTest3()
        {
            var ts = typeof(test3).ListUpDependeceTypes().ToArray();

            ts.ToList().ForEach(t => Console.WriteLine(t.ToTsFullName()));

            var res = new[] { typeof(test1), typeof(test3) };

            AssertArray(res, ts, (t) => t.ToTsFullName());
        }

        [TestMethod()]
        public void ListUpDependeceTypesTest4()
        {
            var ts = typeof(Itest4).ListUpDependeceTypes().ToArray();

            ts.ToList().ForEach(t => Console.WriteLine(t.ToTsFullName()));

            var res = new[] { typeof(Itest4), typeof(test1) };

            AssertArray(res, ts, (t) => t.ToTsFullName());

        }

        [TestMethod()]
        public void ListUpDependeceTypesTest5()
        {
            var ts = typeof(test5).ListUpDependeceTypes().ToArray();

            ts.ToList().ForEach(t => Console.WriteLine(t.ToTsFullName()));

            var res = new[] { typeof(Itest4), typeof(test1), typeof(test2), typeof(test5) };

            AssertArray(res, ts, (t) => t.ToTsFullName());
        }

        [TestMethod()]
        public void ListUpDependeceTypesTest6()
        {
            var ts = typeof(test6<>).ListUpDependeceTypes().ToArray();

            ts.ToList().ForEach(t => Console.WriteLine(t.ToTsFullName()));

            var res = new[] { typeof(test1), typeof(test6<>) };

            AssertArray(res, ts, (t) => t.ToTsFullName());
        }

        [TestMethod()]
        public void ListUpDependeceTypesTest7()
        {
            var ts = typeof(test7).ListUpDependeceTypes().ToArray();

            ts.ToList().ForEach(t => Console.WriteLine(t.ToTsFullName()));

            var res = new[] { typeof(test1), typeof(test2), typeof(test3), typeof(test7) };

            AssertArray(res, ts, (t) => t.ToTsFullName());
        }

        public void AssertArray<T, TKey>(T[] a1, T[] a2, Func<T, TKey> func)
        {
            a1 = a1.OrderBy(func).ToArray();
            a2 = a2.OrderBy(func).ToArray();

            Assert.AreEqual(a1.Length, a2.Length);
            for (var i = 0; i < a1.Length; i++)
                Assert.AreEqual(a1[i], a2[i]);
        }

        #region test classes
        public class test1
        {
            public int id;
            public string name;
        }

        public class test2
        {
            public int id;
            public string name;
            public test1 test1;
        }

        public class test3 : test1 { }

        public interface Itest4
        {
            test1 test1 { get; set; }
        }

        public class test5 : Itest4
        {
            public test1 test1 { get; set; }
            public test2 gettest(test1 test) => null;
        }

        public class test6<T>
        {
            public test1 test1 { get; set; }
            public T test2 { get; set; }
        }

        public class test7
        {
            public List<test1> test1;
            public IEnumerable<test2> test2;
            public test3[] test3;
        }



        #endregion

        [TestMethod()]
        public void IsTsCollectionTest()
        {
            Assert.IsTrue(typeof(IEnumerable).IsTsCollection());
            Assert.IsTrue(typeof(IEnumerable<>).IsTsCollection());
            Assert.IsTrue(typeof(IEnumerable<int>).IsTsCollection());
            Assert.IsTrue(typeof(IEnumerable<IEnumerable<int>>).IsTsCollection());
            Assert.IsTrue(typeof(IList).IsTsCollection());
            Assert.IsTrue(typeof(IList<>).IsTsCollection());
            Assert.IsTrue(typeof(IList<int>).IsTsCollection());
            Assert.IsTrue(typeof(ICollection).IsTsCollection());
            Assert.IsTrue(typeof(ICollection<>).IsTsCollection());
            Assert.IsTrue(typeof(ICollection<int>).IsTsCollection());
            Assert.IsTrue(typeof(int[]).IsTsCollection());
            Assert.IsTrue(typeof(int[,]).IsTsCollection());
            Assert.IsTrue(typeof(ArrayList).IsTsCollection());
            Assert.IsTrue(typeof(List<>).IsTsCollection());
            Assert.IsTrue(typeof(List<int>).IsTsCollection());
            Assert.IsTrue(typeof(IDictionary<int,int>).IsTsCollection());
            Assert.IsTrue(typeof(Dictionary<int, int>).IsTsCollection());
        }
    }
}

namespace test
{
    public class Int32 { }

}