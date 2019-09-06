using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeDLLLibrary
{
    public class Employees
    {
        private DirectedGraph<Employee> myGraph;
        private Dictionary<string, Employee> myEmployees;

        /// <summary>
        /// Employee class CONSTRUCTOR
        /// </summary>
        public Employees(string[] lines)
        {
            myGraph = new DirectedGraph<Employee>();
            myEmployees = new Dictionary<string, Employee>();
            var alns = lines.Select(a => a.Split('\t'));
            var csv = from aline in alns
                      select (from piece in aline
                              select piece);
            int ceos = 0;
            foreach (var n in csv)
            {

                var p = n.GetEnumerator();
                while (p.MoveNext())
                {
                    try
                    {
                        var data = p.Current.Split(',');
                        if (string.IsNullOrEmpty(data[0]))
                        {
                            Console.WriteLine("Employee cannot have empty Id, skipping ...");
                            continue;
                        }

                        if (string.IsNullOrEmpty(data[1]) && ceos < 1)
                        {
                            ceos++;
                        }
                        else if (string.IsNullOrEmpty(data[1]) && ceos == 1)
                        {
                            Console.WriteLine("There can only be 1 ceo in the organization, skipping ...");
                            continue;
                        }


                        int salary = 0;
                        // ensure that employee salary is a valid integer
                        if (int.TryParse(data[2], out salary))
                        {
                            var empl = new Employee(data[0], data[1], salary);
                            try
                            {
                                myEmployees.Add(empl.Id, empl);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error adding employee to dictionary", e);
                            }

                            if (!myGraph.HasVertex(empl))
                            {
                                myGraph.AddVertex(empl);
                            }

                        }
                        else
                        {
                            Console.WriteLine("Salary not a valid integer, skipping ...");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                p.Dispose();

            }

            foreach (KeyValuePair<string, Employee> kvp in myEmployees)
            {
                if (!string.IsNullOrEmpty(kvp.Value.Manager))
                {
                    // check for double linking
                    bool doubleLinked = false;
                    foreach (Employee employee in myGraph.DepthFirstWalk(kvp.Value).ToArray())
                    {
                        if (employee.Equals(kvp.Value.Manager))
                        {
                            doubleLinked = true;
                            break;
                        }
                    }
                    // ensure that each employee has only one manager
                    if (myGraph.IncomingEdges(kvp.Value).ToArray().Length < 1 && !doubleLinked)
                    {
                        myGraph.AddEdge(myEmployees[kvp.Value.Manager], kvp.Value);
                    }
                    else
                    {
                        Console.WriteLine(myGraph.IncomingEdges(kvp.Value).ToArray().Length >= 1 ?
                            String.Format("Employee {0} have more than one manager", kvp.Value.Id) :
                            "Double linking not allowed");
                    }
                }

            }

        }

        public long SalaryBudget(string manager)
        {
            var salaryBudget = 0;
            try
            {
                var employeesInPath = myGraph.DepthFirstWalk(myEmployees[manager]).GetEnumerator();
                while (employeesInPath.MoveNext())
                {
                    salaryBudget += employeesInPath.Current.Salary;

                }
            }
            catch (Exception var0)
            {
                Console.WriteLine("Error occured when getting salary budget ", var0);
            }

            return salaryBudget;
        }
    }
    public class Employee : IComparable<Employee>
    {
        public string Id { get; set; }
        public int Salary { get; set; }
        public string Manager { get; set; }

        public Employee(string id, string manager, int salary)
        {
            Id = id;
            Salary = salary;
            Manager = manager;
        }

        public int CompareTo(Employee other)
        {
            if (other == null)
            {
                return -1;
            }

            return string.Compare(Id, other.Id,StringComparison.OrdinalIgnoreCase);
        }
    }
}
