using System;
using System.Collections.Generic;

namespace EmployeeDLLLibrary
{
    public interface IGraph<T> where T : IComparable<T>
    {
        
        /// Returns true, if graph is directed; false otherwise.
        
        bool IsDirected { get; }

       
        /// Returns true, if graph is weighted; false otherwise.
        
        bool IsWeighted { get; }

       
        /// Gets the count of vetices.
       
        int VerticesCount { get; }

        
        /// Gets the count of edges.
       
        int EdgesCount { get; }

       
        /// Returns the list of Vertices.
       
        IEnumerable<T> Vertices { get; }

       
        /// An enumerable collection of edges.
       
        IEnumerable<IEdge<T>> Edges { get; }

        
        /// Get all incoming edges from vertex
        
        IEnumerable<IEdge<T>> IncomingEdges(T vertex);

        
        /// Get all outgoing edges from vertex
       
        IEnumerable<IEdge<T>> OutgoingEdges(T vertex);

       
        /// Connects two vertices together.
        
        bool AddEdge(T firstVertex, T secondVertex);

        
        /// Deletes an edge, if exists, between two vertices.
        
        bool RemoveEdge(T firstVertex, T secondVertex);

        
        /// Adds a list of vertices to the graph.
        
        void AddVertices(IList<T> collection);

        
        /// Adds a new vertex to graph.
       
        bool AddVertex(T vertex);

        
        /// Removes the specified vertex from graph.
        
        bool RemoveVertex(T vertex);

       
        /// Checks whether two vertices are connected (there is an edge between firstVertex & secondVertex)
        
        bool HasEdge(T firstVertex, T secondVertex);

        
        /// Determines whether this graph has the specified vertex.
        
        bool HasVertex(T vertex);

       
        /// Returns the neighbours doubly-linked list for the specified vertex.
        
        DLinkedList<T> Neighbours(T vertex);

       
        /// Returns the degree of the specified vertex.
        
        int Degree(T vertex);

      
        /// Returns a human-readable string of the graph.
      
        string ToReadable();

       
        /// A depth first search traversal of the graph. Prints nodes as they get visited.
        /// It considers the first inserted vertex as the start-vertex for the walk.
        
        IEnumerable<T> DepthFirstWalk();
        
        /// A depth first search traversal of the graph, starting from a specified vertex. Prints nodes as they get visited.
        
        IEnumerable<T> DepthFirstWalk(T startingVertex);
       
        /// Clear the graph.       
        void Clear();
    }
}