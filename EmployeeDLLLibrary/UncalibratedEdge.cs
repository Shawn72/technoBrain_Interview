using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDLLLibrary
{
    public class UncalibratedEdge<TVertex> : IEdge<TVertex> where TVertex : IComparable<TVertex>
    {
        private const int _edgeWeight = 0;

       
        /// Gets or sets the source vertex.
        
        public TVertex Source { get; set; }

        
        /// Gets or sets the destination vertex.
       
        public TVertex Destination { get; set; }

       
        /// Gets or sets the weight.
       
        public Int64 Weight
        {
            get { throw new NotImplementedException("Unweighted edges don't have weights."); }
            set { throw new NotImplementedException("Unweighted edges can't have weights."); }
        }

      
        /// Gets a value indicating whether this edge is weighted.
      
        public bool IsWeighted
        {
            get
            { return false; }
        }

       
        /// CONSTRUCTOR
     
        public UncalibratedEdge(TVertex src, TVertex dst)
        {
            Source = src;
            Destination = dst;
        }


        #region IComparable implementation
        public int CompareTo(IEdge<TVertex> other)
        {
            if (other == null)
                return -1;

            bool areNodesEqual = Source.IsEqualTo<TVertex>(other.Source) && Destination.IsEqualTo<TVertex>(other.Destination);

            if (!areNodesEqual)
                return -1;
            return 0;
        }
        #endregion
    }
}
