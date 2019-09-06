using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDLLLibrary
{
    
    /// The Stack (LIFO) Data Structure.
    
    public class Stack<T> : IEnumerable<T> where T : IComparable<T>
    {
       
        /// Instance variables.
        /// collection: Array-Based List.
        /// Count: Public Getter for returning the number of elements.
       
        private ArrayList<T> collection { get; set; }
        public int Count { get { return collection.Count; } }

       
        /// CONSTRUCTORS
        
        public Stack()
        {
            // The internal collection is implemented as an array-based list.
            // See the ArrayList.cs for the list implementation.
            collection = new ArrayList<T>();
        }


        public Stack(int initialCapacity)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            // The internal collection is implemented as an array-based list.
            // See the ArrayList.cs for the list implementation.
            collection = new ArrayList<T>(initialCapacity);
        }


        /// Checks whether the stack is empty.       
        public bool IsEmpty
        {
            get
            {
                return collection.IsEmpty;
            }
        }
        
        /// Returns the top element in the stack.
        
        public T Top
        {
            get
            {
                try
                {
                    return collection[collection.Count - 1];
                }
                catch (Exception)
                {
                    throw new Exception("The stack is empty!");
                }
            }
        }


       
        /// Inserts an element at the top of the stack.
       
        public void Push(T dataItem)
        {
            collection.Add(dataItem);
        }


        /// Removes the top element from stack.
        
        public T Pop()
        {
            if (Count > 0)
            {
                var top = Top;
                collection.RemoveAt(collection.Count - 1);
                return top;
            }

            throw new Exception("Stack is empty.");
        }

       
        /// Returns an array version of this stack.
        public T[] ToArray()
        {
            return collection.ToArray();
        }

        /// Returns a human-readable, multi-line, print-out (string) of this stack.
       
        public string ToHumanReadable()
        {
            return collection.ToHumanReadable();
        }


        public IEnumerator<T> GetEnumerator()
        {
            for (int i = collection.Count - 1; i >= 0; --i)
                yield return collection[i];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}
