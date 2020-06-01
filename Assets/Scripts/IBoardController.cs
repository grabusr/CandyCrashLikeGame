using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace view
{
    public interface IBoardController
    {
        void OnBlockClicked(Element element);
    }
}
