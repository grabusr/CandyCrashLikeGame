using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public struct BlockData
    {
        public static readonly int invalidColorId = -1;
        private int type;

        public BlockData(int type)
        {
            this.type = type;
        }

        public int Type
        {
            get => type;
        }
    }
}
