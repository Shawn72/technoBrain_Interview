using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmployeeDLLLibrary
{
    public class ArrayList<T> : IEnumerable<T>
    {
      
        /// variables.       

        
        bool DefaultMaxCapacityIsX64 = true;
        bool IsMaximumCapacityReached = false;

        /// The C# Maximum Array Length (before encountering overflow)
        public const int MAXIMUM_ARRAY_LENGTH_x64 = 0X7FEFFFFF; //x64
        public const int MAXIMUM_ARRAY_LENGTH_x86 = 0x8000000; //x86

        /// This is used as a default empty list initialization.
        private readonly T[] _emptyArray = new T[0];

        /// The default capacity to resize to, when a minimum is lower than 5.
        private const int _defaultCapacity = 8;

        /// The internal array of elements.
       
        private T[] collection;

        /// This keeps track of the number of elements added to the array.
        /// Serves as an index of last item + 1.
        private int size { get; set; }


       
        /// CONSTRUCTORS
       
        public ArrayList() : this(capacity: 0) { }

        public ArrayList(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (capacity == 0)
            {
                collection = _emptyArray;
            }
            else
            {
                collection = new T[capacity];
            }

            /// set size to 0;
            size = 0;
        }
        
        
        private void _ensureCapacity(int minCapacity)
        {
            /// If the length of the inner collection is less than the minCapacity
            ///... and if the maximum capacity wasn't reached yet, 
            /// ... then maximize the inner collection.
            if (collection.Length < minCapacity && IsMaximumCapacityReached == false)
            {
                int newCapacity = (collection.Length == 0 ? _defaultCapacity : collection.Length * 2);

                /// Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
                /// Note that this check works even when _items.Length overflowed thanks to the (uint) cast
                int maxCapacity = (DefaultMaxCapacityIsX64 == true ? MAXIMUM_ARRAY_LENGTH_x64 : MAXIMUM_ARRAY_LENGTH_x86);

                if (newCapacity < minCapacity)
                    newCapacity = minCapacity;

                if (newCapacity >= maxCapacity)
                {
                    newCapacity = maxCapacity - 1;
                    IsMaximumCapacityReached = true;
                }

                _resizeCapacity(newCapacity);
            }
        }

        /// Resizes the collection to a new maximum number of capacity.
       
        private void _resizeCapacity(int newCapacity)
        {
            if (newCapacity != collection.Length && newCapacity > size)
            {
                try
                {
                    Array.Resize<T>(ref collection, newCapacity);
                }
                catch (OutOfMemoryException)
                {
                    if (DefaultMaxCapacityIsX64 == true)
                    {
                        DefaultMaxCapacityIsX64 = false;
                        _ensureCapacity(newCapacity);
                    }

                    throw;
                }
            }
        }

               
        /// Gets the the number of elements in list.
       
        public int Count
        {
            get
            {
                return size;
            }
        }


        
        /// Returns the capacity of list, which is the total number of slots.
       
        public int Capacity
        {
            get { return collection.Length; }
        }


        
        /// Determines whether this list is empty.
       
        public bool IsEmpty
        {
            get
            {
                return (Count == 0);
            }
        }


        
        /// Gets the first element in the list.
      
        public T First
        {
            get
            {
                if (Count == 0)
                {
                    throw new IndexOutOfRangeException("The List is empty!");
                }

                return collection[0];
            }
        }

        /// Gets the last element in the list.
        public T Last
        {
            get
            {
                if (IsEmpty)
                {
                    throw new IndexOutOfRangeException("List is empty.");
                }

                return collection[Count - 1];
            }
        }

        /// Gets or sets the item at the specified index.
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= size)
                {
                    throw new IndexOutOfRangeException();
                }

                return collection[index];
            }

            set
            {
                if (index < 0 || index >= size)
                {
                    throw new IndexOutOfRangeException();
                }

                collection[index] = value;
            }
        }

        /// Add the specified dataItem to list.
        public void Add(T dataItem)
        {
            if (size == collection.Length)
            {
                _ensureCapacity(size + 1);
            }

            collection[size++] = dataItem;
        }

        /// Adds an enumerable collection of items to list.
        public void AddRange(IEnumerable<T> elements)
        {
            if (elements == null)
                throw new ArgumentNullException();

            /// make sure the size won't overflow by adding the range
            if (((uint)size + elements.Count()) > MAXIMUM_ARRAY_LENGTH_x64)
                throw new OverflowException();

            /// grow the internal collection once to avoid doing multiple redundant grows
            if (elements.Any())
            {
                _ensureCapacity(size + elements.Count());

                foreach (var element in elements)
                    this.Add(element);
            }
        }

        /// Adds an element to list repeatedly for a specified count.
       
        public void AddRepeatedly(T value, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();

            if (((uint)size + count) > MAXIMUM_ARRAY_LENGTH_x64)
                throw new OverflowException();

            // grow the internal collection once to avoid doing multiple redundant grows
            if (count > 0)
            {
                _ensureCapacity(size + count);

                for (int i = 0; i < count; i++)
                    this.Add(value);
            }
        }


        /// Inserts a new element at an index. Doesn't override the cell at index.
       
        public void InsertAt(T dataItem, int index)
        {
            if (index < 0 || index > size)
            {
                throw new IndexOutOfRangeException("Please provide a valid index.");
            }

            // If the inner array is full and there are no extra spaces, 
            // ... then maximize it's capacity to a minimum of size + 1.
            if (size == collection.Length)
            {
                _ensureCapacity(size + 1);
            }

            // If the index is not "at the end", then copy the elements of the array
            // ... between the specified index and the last index to the new range (index + 1, size);
            // The cell at "index" will become available.
            if (index < size)
            {
                Array.Copy(collection, index, collection, index + 1, (size - index));
            }

            // Write the dataItem to the available cell.
            collection[index] = dataItem;

            // Increase the size.
            size++;
        }

        /// Removes the specified dataItem from list.
        public bool Remove(T dataItem)
        {
            int index = IndexOf(dataItem);

            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }


        /// Removes the list element at the specified index.
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= size)
            {
                throw new IndexOutOfRangeException("Please pass a valid index.");
            }

            // Decrease the size by 1, to avoid doing Array.Copy if the element is to be removed from the tail of list. 
            size--;

            // If the index is still less than size, perform an Array.Copy to override the cell at index.
            // This operation is O(N), where N = size - index.
            if (index < size)
            {
                Array.Copy(collection, index + 1, collection, index, (size - index));
            }

            // Reset the writable cell to the default value of type T.
            collection[size] = default(T);
        }

         /// Clear this instance.
        public void Clear()
        {
            if (size > 0)
            {
                size = 0;
                Array.Clear(collection, 0, size);
                collection = _emptyArray;
            }
        }


        /// Resize the List to a new size
        public void Resize(int newSize)
        {
            Resize(newSize, default(T));
        }


        /// Resize the list to a new size.
        
        public void Resize(int newSize, T defaultValue)
        {
            int currentSize = this.Count;

            if (newSize < currentSize)
            {
                this._ensureCapacity(newSize);
            }
            else if (newSize > currentSize)
            {
                // Optimisation step.
                // This is just to avoid multiple automatic capacity changes.
                if (newSize > this.collection.Length)
                    this._ensureCapacity(newSize + 1);

                this.AddRange(Enumerable.Repeat<T>(defaultValue, newSize - currentSize));
            }
        }


        /// Reverses the list.
       
        public void Reverse()
        {
            Reverse(0, size);
        }


        
        /// Reverses the order of a number of elements. Starting a specific index.
        
        public void Reverse(int startIndex, int count)
        {
            // Handle the bounds of startIndex
            if (startIndex < 0 || startIndex >= size)
            {
                throw new IndexOutOfRangeException("Please pass a valid starting index.");
            }

            // Handle the bounds of count and startIndex with respect to size.
            if (count < 0 || startIndex > (size - count))
            {
                throw new ArgumentOutOfRangeException();
            }

            // Use Array.Reverse
            // Running complexity is better than O(N). But unknown.
            // Array.Reverse uses the closed-source function TrySZReverse.
            Array.Reverse(collection, startIndex, count);
        }

        /// For each element in list, apply the specified action to it.
        public void ForEach(Action<T> action)
        {
            // Null actions are not allowed.
            if (action == null)
            {
                throw new ArgumentNullException();
            }

            for (int i = 0; i < size; ++i)
            {
                action(collection[i]);
            }
        }


       
        /// Checks whether the list contains the specified dataItem.
        
        public bool Contains(T dataItem)
        {
            // Null-value check
            if ((Object)dataItem == null)
            {
                for (int i = 0; i < size; ++i)
                {
                    if ((Object)collection[i] == null) return true;
                }
            }
            else
            {
                // Construct a default equality comparer for this Type T
                // Use it to get the equal match for the dataItem
                EqualityComparer<T> comparer = EqualityComparer<T>.Default;

                for (int i = 0; i < size; ++i)
                {
                    if (comparer.Equals(collection[i], dataItem)) return true;
                }
            }

            return false;
        }


        
        /// Checks whether the list contains the specified dataItem.
        
        public bool Contains(T dataItem, IEqualityComparer<T> comparer)
        {
            // Null comparers are not allowed.
            if (comparer == null)
            {
                throw new ArgumentNullException();
            }

            // Null-value check
            if ((Object)dataItem == null)
            {
                for (int i = 0; i < size; ++i)
                {
                    if ((Object)collection[i] == null) return true;
                }
            }
            else
            {
                for (int i = 0; i < size; ++i)
                {
                    if (comparer.Equals(collection[i], dataItem)) return true;
                }
            }

            return false;
        }

        /// Checks whether an item specified via a Predicate<T> function exists exists in list.
      
        public bool Exists(Predicate<T> searchMatch)
        {
            // Use the FindIndex to look through the collection
            // If the returned index != -1 then it does exist, otherwise it doesn't.
            return (FindIndex(searchMatch) != -1);
        }


        
        /// Finds the index of the element that matches the predicate.
       
        public int FindIndex(Predicate<T> searchMatch)
        {
            return FindIndex(0, size, searchMatch);
        }


       
        /// Finds the index of the element that matches the predicate.
       
        public int FindIndex(int startIndex, Predicate<T> searchMatch)
        {
            return FindIndex(startIndex, (size - startIndex), searchMatch);
        }


        
        /// Finds the index of the first element that matches the given predicate function.
        
        public int FindIndex(int startIndex, int count, Predicate<T> searchMatch)
        {
            // Check bound of startIndex
            if (startIndex < 0 || startIndex > size)
            {
                throw new IndexOutOfRangeException("Please pass a valid starting index.");
            }

            // CHeck the bounds of count and startIndex with respect to size
            if (count < 0 || startIndex > (size - count))
            {
                throw new ArgumentOutOfRangeException();
            }

            // Null match-predicates are not allowed
            if (searchMatch == null)
            {
                throw new ArgumentNullException();
            }

            // Start the search
            int endIndex = startIndex + count;
            for (int index = startIndex; index < endIndex; ++index)
            {
                if (searchMatch(collection[index]) == true) return index;
            }

            // Not found, return -1
            return -1;
        }


        /// Returns the index of a given dataItem.
       
        public int IndexOf(T dataItem)
        {
            return IndexOf(dataItem, 0, size);
        }


      
        /// Returns the index of a given dataItem.
        
        public int IndexOf(T dataItem, int startIndex)
        {
            return IndexOf(dataItem, startIndex, size);
        }


       
        /// Returns the index of a given dataItem.
        
        public int IndexOf(T dataItem, int startIndex, int count)
        {
            // Check the bound of the starting index.
            if (startIndex < 0 || (uint)startIndex > (uint)size)
            {
                throw new IndexOutOfRangeException("Please pass a valid starting index.");
            }

            // Check the bounds of count and starting index with respect to size.
            if (count < 0 || startIndex > (size - count))
            {
                throw new ArgumentOutOfRangeException();
            }

            return Array.IndexOf(collection, dataItem, startIndex, count);
        }


        
        /// Find the specified element that matches the Search Predication.
        
        public T Find(Predicate<T> searchMatch)
        {
            // Null Predicate functions are not allowed. 
            if (searchMatch == null)
            {
                throw new ArgumentNullException();
            }

            // Begin searching, and return the matched element
            for (int i = 0; i < size; ++i)
            {
                if (searchMatch(collection[i]))
                {
                    return collection[i];
                }
            }

            // Not found, return the default value of the type T.
            return default(T);
        }


       
        /// Finds all the elements that match the typed Search Predicate.
       
        public ArrayList<T> FindAll(Predicate<T> searchMatch)
        {
            // Null Predicate functions are not allowed. 
            if (searchMatch == null)
            {
                throw new ArgumentNullException();
            }

            ArrayList<T> matchedElements = new ArrayList<T>();

            // Begin searching, and add the matched elements to the new list.
            for (int i = 0; i < size; ++i)
            {
                if (searchMatch(collection[i]))
                {
                    matchedElements.Add(collection[i]);
                }
            }

            // Return the new list of elements.
            return matchedElements;
        }


        
        /// Get a range of elements, starting from an index..
       
        public ArrayList<T> GetRange(int startIndex, int count)
        {
            // Handle the bound errors of startIndex
            if (startIndex < 0 || (uint)startIndex > (uint)size)
            {
                throw new IndexOutOfRangeException("Please provide a valid starting index.");
            }

            // Handle the bound errors of count and startIndex with respect to size
            if (count < 0 || startIndex > (size - count))
            {
                throw new ArgumentOutOfRangeException();
            }

            var newArrayList = new ArrayList<T>(count);

            // Use Array.Copy to quickly copy the contents from this array to the new list's inner array.
            Array.Copy(collection, startIndex, newArrayList.collection, 0, count);

            // Assign count to the new list's inner size counter.
            newArrayList.size = count;

            return newArrayList;
        }


       
        /// Return an array version of the list.
       
        public T[] ToArray()
        {
            T[] newArray = new T[Count];

            if (Count > 0)
            {
                Array.Copy(collection, 0, newArray, 0, Count);
            }

            return newArray;
        }


        
        /// Return an array version of this list.
        
        public List<T> ToList()
        {
            var newList = new List<T>(this.Count);

            if (this.Count > 0)
            {
                for (int i = 0; i < this.Count; ++i)
                {
                    newList.Add(collection[i]);
                }
            }

            return newList;
        }

        /// Return a human readable, multi-line, print-out (string) of this list.
         public string ToHumanReadable(bool addHeader = false)
        {
            int i = 0;
            string listAsString = string.Empty;

            string preLineIndent = (addHeader == false ? "" : "\t");

            for (i = 0; i < Count; ++i)
            {
                listAsString = String.Format("{0}{1}[{2}] => {3}\r\n", listAsString, preLineIndent, i, collection[i]);
            }

            if (addHeader == true)
            {
                listAsString = String.Format("ArrayList of count: {0}.\r\n(\r\n{1})", Count, listAsString);
            }

            return listAsString;
        }

               
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return collection[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}
