using UnityEngine;
using core;

namespace view
{
    public interface IBoardView
    {        
        void AnimationEnded(Element element);
        Vector3 GetPositionOfCoordinate(Coordinate coordinate);
        bool IsAnimating();
    }
}
