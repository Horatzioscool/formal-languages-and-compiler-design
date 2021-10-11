using System;
using System.Linq;
using System.Text;
using FLTC.Lab2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLTC.Tests
{
    [TestClass]
    public class SymbolTableUnitTests
    {
        private int GetPosition(string token, int tableSize)
        {
            return Math.Abs(Encoding.ASCII.GetBytes(token).Sum(b => b) % tableSize);
        }

        [TestMethod]
        public void Test_SingleInsert()
        {
            var exampleToken = "testToken";

            var tableSize = 32;

            ISymbolTable table = new SymbolTable(tableSize);

            var result = table.Insert(exampleToken);

            Assert.AreEqual(result, (GetPosition(exampleToken, tableSize), 0));
        }

        [TestMethod]
        public void Test_SingleSearch()
        {
            var exampleToken = "testToken";

            var tableSize = 32;

            ISymbolTable table = new SymbolTable(tableSize);

            table.Insert(exampleToken);

            var result = table.Search(exampleToken);

            Assert.IsNotNull(result);

            Assert.AreEqual(result, (GetPosition(exampleToken, tableSize), 0));
        }

        [TestMethod]
        public void Test_MultipleSearch()
        {
            var tableSize = 32;

            ISymbolTable table = new SymbolTable(tableSize);
        }
    }
}
