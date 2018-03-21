using Microsoft.VisualStudio.TestTools.UnitTesting;
using AjaxMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjaxMapper.Tests
{
    [TestClass()]
    public class AjaxMethodGeneratorTests
    {
        [TestMethod()]
        public void GenerateTest()
        {
            var ag = new AjaxMethodGenerator(typeof(TestController));
            var code = ag.Generate();

            Console.WriteLine(code);
        }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestController {
        [ReturnObjectType(typeof(List<Item>))]
        public void Index() { }

        [ReturnObjectType(typeof(List<Item>))]
        public void Remove(int id) { }

        [ReturnObjectType(typeof(List<Item>))]
        public void Add(string name){}

        [ReturnObjectType(typeof(List<Item>))]
        public void Edit(int id, string name) { }

    }

}