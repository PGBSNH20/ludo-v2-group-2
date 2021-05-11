using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine.Engine
{
    public class Piece : IEqualityComparer<Piece>
    {
        public int PlayerId { get; set; }
        public int ColorId { get; set; }
        public int PieceNumber { get; set; }

        public bool Equals(Piece x, Piece y)
        {
            return x.PlayerId == y.PlayerId && x.PieceNumber == y.PieceNumber;
        }

        public int GetHashCode([DisallowNull] Piece obj)
        {
            return obj.GetHashCode();
        }
    }
}
