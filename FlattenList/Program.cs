using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlattenList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Flattening a list of type T using generic method that accepts an object of type T
            PrimeSourceHierarchyList parent = new PrimeSourceHierarchyList();
            parent.ProductId = 1;

            PrimeSourceHierarchyList child1 = new PrimeSourceHierarchyList();
            child1.ProductId = 2;

            PrimeSourceHierarchyList child2 = new PrimeSourceHierarchyList();
            child2.ProductId = 3;

            parent.ChildSourcesList.Add(child1);
            parent.ChildSourcesList.Add(child2);

            //Calling generic method
            var finalList = FlattenList(parent);
            foreach (var item in finalList)
            {
                Console.WriteLine("ProductId: " + item.ProductId);
            }
            Console.ReadKey();
        }

        public static IEnumerable<T> FlattenList<T>(T Node)
        {
            yield return Node;
            foreach (PropertyInfo propertyInfo in Node.GetType().GetProperties())
            {
                var childListType = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Node.GetType()));
                var childPropertyType = childListType.GetType();

                if (propertyInfo.PropertyType == childPropertyType)
                {
                    var childObj = propertyInfo.GetValue(Node, null);
                    if (childObj != null)
                    {
                        foreach (var childNode in childObj as IList)
                        {
                            foreach (var descendant in FlattenList(childNode))
                            {
                                yield return (T)descendant;
                            }
                        }
                    }
                }
            }
        }
    }
}

public class PrimeSourceHierarchyList
{
    public PrimeSourceHierarchyList()
    {
        ChildSourcesList = new List<PrimeSourceHierarchyList>();
    }
    public int ProductId { get; set; }
    public List<PrimeSourceHierarchyList> ChildSourcesList { get; set; }
}