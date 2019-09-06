using EmployeeDLLLibrary;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Tests
{
    public class Tests
    {
       
        [Test]
        public void TestEmployees()
        {
            var _lines = File.ReadAllLines("../../../testFileCsv.txt");
            Employees employees = new Employees(_lines);
            Assert.GreaterOrEqual(3300, employees.SalaryBudget("Employee1"));
            Assert.GreaterOrEqual(1000, employees.SalaryBudget("Employee2"));

        }
        [Test]
        public void TestDoubleLinkin()
        {
            var lines = File.ReadAllLines("../../../test1File.txt");
            Employees employees = new Employees(lines);
            Assert.GreaterOrEqual(3300, employees.SalaryBudget("Employee1"));
            Assert.GreaterOrEqual(1000, employees.SalaryBudget("Employee2"));

        }
        [Test]
        public void TestN2()
        {
            var lines = File.ReadAllLines("../../../test2File.txt");
            Employees employees = new Employees(lines);
            Assert.GreaterOrEqual(3800, employees.SalaryBudget("Employee1"));
            Assert.GreaterOrEqual(1800, employees.SalaryBudget("Employee2"));
            Assert.GreaterOrEqual(500, employees.SalaryBudget("Employee3"));

        }

        //test invalid salary value for employee 6
        [Test]
        public void TestN3()
        {
            var lines = File.ReadAllLines("../../../test3File.txt");
            Employees employees = new Employees(lines);
            Assert.GreaterOrEqual(3300, employees.SalaryBudget("Employee1"));
            Assert.GreaterOrEqual(1300, employees.SalaryBudget("Employee2"));
            Assert.GreaterOrEqual(500, employees.SalaryBudget("Employee3"));
            Assert.GreaterOrEqual(0, employees.SalaryBudget("Employee6"));

        }

        public static class DirectedGraphTest
        {
            [Test]
            public static void DoTest()
            {
                var graph = new DirectedGraph<Employee>();

                var employeeA = new Employee("a", "", 10);
                var employeeB = new Employee("b", "a", 10);
                var employeeC = new Employee("c", "a", 10);
                var employeeD = new Employee("d", "a", 10);
                var employeeE = new Employee("e", "b", 10);

                var verticesSet1 = new Employee[] { employeeA, employeeB, employeeC, employeeD, employeeE };

                graph.AddVertices(verticesSet1);

                graph.AddEdge(employeeA, employeeB);
                graph.AddEdge(employeeA, employeeC);
                graph.AddEdge(employeeA, employeeD);
                graph.AddEdge(employeeB, employeeE);

                var allEdges = graph.Edges.ToList();

                Assert.True(graph.VerticesCount == 5, "Wrong vertices count.");
                Assert.True(graph.EdgesCount == 4, "Wrong edges count.");
                Assert.True(graph.EdgesCount == allEdges.Count, "Wrong edges count.");

                Assert.True(graph.OutgoingEdges(employeeA).ToList().Count == 3, "Wrong outgoing edges from 'a'.");
                Assert.True(graph.OutgoingEdges(employeeB).ToList().Count == 1, "Wrong outgoing edges from 'b'.");
                Assert.True(graph.OutgoingEdges(employeeC).ToList().Count == 0, "Wrong outgoing edges from 'c'.");
                Assert.True(graph.OutgoingEdges(employeeD).ToList().Count == 0, "Wrong outgoing edges from 'd'.");
                Assert.True(graph.OutgoingEdges(employeeE).ToList().Count == 0, "Wrong outgoing edges from 'e'.");


                Assert.True(graph.IncomingEdges(employeeA).ToList().Count == 0, "Wrong incoming edges from 'a'.");
                Assert.True(graph.IncomingEdges(employeeB).ToList().Count == 1, "Wrong incoming edges from 'b'.");
                Assert.True(graph.IncomingEdges(employeeC).ToList().Count == 1, "Wrong incoming edges from 'c'.");
                Assert.True(graph.IncomingEdges(employeeD).ToList().Count == 1, "Wrong incoming edges from 'd'.");
                Assert.True(graph.IncomingEdges(employeeE).ToList().Count == 1, "Wrong incoming edges from 'e'.");


                // DFS from A
                // Walk the graph using DFS from A:
                var dfsWalk = graph.DepthFirstWalk(employeeA);
                // output: (s) (a) (x) (z) (d) (c) (f) (v)
               
                // DFS from F
                // Walk the graph using DFS from F:
                //dfsWalk = graph.DepthFirstWalk(employeeB);
                // output: (s) (a) (x) (z) (d) (c) (f) (v)
                foreach (var node in dfsWalk)
                {
                    Console.Write(string.Format("({0})", node.Id));
                }

                graph.Clear();

            }

        }
    }
}