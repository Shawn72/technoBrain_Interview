using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDLLLibrary
{  

    public class DirectedGraph<T> : IGraph<T> where T : IComparable<T>
    {
        
        protected virtual int _edgesCount { get; set; }
        protected virtual T _firstInsertedNode { get; set; }
        protected virtual Dictionary<T, DLinkedList<T>> _adjacencyList { get; set; }
        
        /// CONSTRUCTOR
       
        public DirectedGraph() : this(10) { }

        public DirectedGraph(uint initialCapacity)
        {
            _edgesCount = 0;
            _adjacencyList = new Dictionary<T, DLinkedList<T>>((int)initialCapacity);
        }

        /// Helper function. Checks if edge exist in graph.
        
        protected virtual bool _doesEdgeExist(T vertex1, T vertex2)
        {
            return (_adjacencyList[vertex1].Contains(vertex2));
        }
       
        /// Returns true, if graph is directed; false otherwise.
       
        public virtual bool IsDirected
        {
            get { return true; }
        }

       
        /// Returns true, if graph is weighted; false otherwise.
        
        public virtual bool IsWeighted
        {
            get { return false; }
        }

       
        /// Gets the count of vetices.
       
        public virtual int VerticesCount
        {
            get { return _adjacencyList.Count; }
        }

        /// Gets the count of edges.
       
        public virtual int EdgesCount
        {
            get { return _edgesCount; }
        }

        /// Returns the list of Vertices.
    
        public virtual IEnumerable<T> Vertices
        {
            get
            {
                foreach (var vertex in _adjacencyList)
                    yield return vertex.Key;
            }
        }


        IEnumerable<IEdge<T>> IGraph<T>.Edges
        {
            get { return Edges; }
        }

        IEnumerable<IEdge<T>> IGraph<T>.IncomingEdges(T vertex)
        {
            return this.IncomingEdges(vertex);
        }

        IEnumerable<IEdge<T>> IGraph<T>.OutgoingEdges(T vertex)
        {
            return this.OutgoingEdges(vertex);
        }


       
        /// An enumerable collection of all directed unweighted edges in graph.
       
        public virtual IEnumerable<UncalibratedEdge<T>> Edges
        {
            get
            {
                foreach (var vertex in _adjacencyList)
                    foreach (var adjacent in vertex.Value)
                        yield return (new UncalibratedEdge<T>(
                            vertex.Key,   // from
                            adjacent      // to
                        ));
            }
        }

       
        /// Get all incoming directed unweighted edges to a vertex.
       
        public virtual IEnumerable<UncalibratedEdge<T>> IncomingEdges(T vertex)
        {
            if (!HasVertex(vertex))
                throw new KeyNotFoundException("Vertex doesn't belong to graph.");

            foreach (var adjacent in _adjacencyList.Keys)
            {
                if (_adjacencyList[adjacent].Contains(vertex))
                    yield return (new UncalibratedEdge<T>(
                        adjacent,   // from
                        vertex      // to
                    ));
            }//end-foreach
        }

       
        /// Get all outgoing directed unweighted edges from a vertex.
        
        public virtual IEnumerable<UncalibratedEdge<T>> OutgoingEdges(T vertex)
        {
            if (!HasVertex(vertex))
                throw new KeyNotFoundException("Vertex doesn't belong to graph.");

            foreach (var adjacent in _adjacencyList[vertex])
                yield return (new UncalibratedEdge<T>(
                    vertex,     // from
                    adjacent    // to
                ));
        }


       
        /// Connects two vertices together in the direction: first->second.
      
        public virtual bool AddEdge(T source, T destination)
        {
            // Check existence of nodes and non-existence of edge
            if (!HasVertex(source) || !HasVertex(destination))
                return false;
            if (_doesEdgeExist(source, destination))
                return false;

            // Add edge from source to destination
            _adjacencyList[source].Append(destination);

            // Increment edges count
            ++_edgesCount;

            return true;
        }

       
        /// Removes edge, if exists, from source to destination.
       
        public virtual bool RemoveEdge(T source, T destination)
        {
            // Check existence of nodes and non-existence of edge
            if (!HasVertex(source) || !HasVertex(destination))
                return false;
            if (!_doesEdgeExist(source, destination))
                return false;

            // Remove edge from source to destination
            _adjacencyList[source].Remove(destination);

            // Decrement the edges count
            --_edgesCount;

            return true;
        }

        
        /// Add a collection of vertices to the graph.
        
        public virtual void AddVertices(IList<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            foreach (var vertex in collection)
                AddVertex(vertex);
        }

        
        /// Add vertex to the graph
        
        public virtual bool AddVertex(T vertex)
        {
            if (HasVertex(vertex))
                return false;

            if (_adjacencyList.Count == 0)
                _firstInsertedNode = vertex;

            _adjacencyList.Add(vertex, new DLinkedList<T>());

            return true;
        }

      
        /// Removes the specified vertex from graph.
        
        public virtual bool RemoveVertex(T vertex)
        {
            // Check existence of vertex
            if (!HasVertex(vertex))
                return false;

            // Subtract the number of edges for this vertex from the total edges count
            _edgesCount = _edgesCount - _adjacencyList[vertex].Count;

            // Remove vertex from graph
            _adjacencyList.Remove(vertex);

            // Remove destination edges to this vertex
            foreach (var adjacent in _adjacencyList)
            {
                if (adjacent.Value.Contains(vertex))
                {
                    adjacent.Value.Remove(vertex);

                    // Decrement the edges count.
                    --_edgesCount;
                }
            }

            return true;
        }

        
        /// Checks whether there is an edge from source to destination.
       
        public virtual bool HasEdge(T source, T destination)
        {
            return (_adjacencyList.ContainsKey(source) && _adjacencyList.ContainsKey(destination) && _doesEdgeExist(source, destination));
        }

       
        /// Checks whether a vertex exists in the graph
       
        public virtual bool HasVertex(T vertex)
        {
            return _adjacencyList.ContainsKey(vertex);
        }

        
        /// Returns the neighbours doubly-linked list for the specified vertex.
       
        public virtual DLinkedList<T> Neighbours(T vertex)
        {
            if (!HasVertex(vertex))
                return null;

            return _adjacencyList[vertex];
        }

      
        /// Returns the degree of the specified vertex.
        
        public virtual int Degree(T vertex)
        {
            if (!HasVertex(vertex))
                throw new KeyNotFoundException();

            return _adjacencyList[vertex].Count;
        }

       
        /// Returns a human-readable string of the graph.
       
        public virtual string ToReadable()
        {
            string output = string.Empty;

            foreach (var node in _adjacencyList)
            {
                var adjacents = string.Empty;

                output = String.Format("{0}\r\n{1}: [", output, node.Key);

                foreach (var adjacentNode in node.Value)
                    adjacents = String.Format("{0}{1},", adjacents, adjacentNode);

                if (adjacents.Length > 0)
                    adjacents = adjacents.TrimEnd(new char[] { ',', ' ' });

                output = String.Format("{0}{1}]", output, adjacents);
            }

            return output;
        }

       
        /// A depth first search traversal of the graph starting from the first inserted node.
        /// Returns the visited vertices of the graph.
       
        public virtual IEnumerable<T> DepthFirstWalk()
        {
            return DepthFirstWalk(_firstInsertedNode);
        }

        
        /// A depth first search traversal of the graph, starting from a specified vertex.
        /// Returns the visited vertices of the graph.
        
        public virtual IEnumerable<T> DepthFirstWalk(T source)
        {
            // Check for existence of source
            if (VerticesCount == 0)
                return new ArrayList<T>(0);
            if (!HasVertex(source))
                throw new KeyNotFoundException("The source vertex doesn't exist.");

            var visited = new HashSet<T>();
            var stack = new Stack<T>();
            var listOfNodes = new ArrayList<T>(VerticesCount);

            stack.Push(source);

            while (!stack.IsEmpty)
            {
                var current = stack.Pop();

                if (!visited.Contains(current))
                {
                    listOfNodes.Add(current);
                    visited.Add(current);

                    foreach (var adjacent in Neighbours(current))
                        if (!visited.Contains(adjacent))
                            stack.Push(adjacent);
                }
            }

            return listOfNodes;
        }

        /// Clear this graph.
        
        public virtual void Clear()
        {
            _edgesCount = 0;
            _adjacencyList.Clear();
        }

    }
}
