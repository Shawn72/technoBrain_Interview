using System;

namespace EmployeeDLLLibrary
{
    
       ///An interface to be implemented by all edges classes.
    
    public interface IEdge<TVertex> : IComparable<IEdge<TVertex>> where TVertex : IComparable<TVertex>
    {      
        /// Gets a value indicating whether this edge is weighted.
        bool IsWeighted { get; }

       
        /// Gets or sets the source.
        TVertex Source { get; set; }
        
        /// Gets or sets the destination.       
        TVertex Destination { get; set; }

       
        /// Gets or sets the weight of edge.    
       
        Int64 Weight { get; set; }
    }
}