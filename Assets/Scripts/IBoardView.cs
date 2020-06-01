using UnityEngine;
using core;

namespace view
{
    public interface IBoardView
    {
        void OnBlockSelect(Element element);
        void AnimationEnded(Element element);
        Vector3 GetPositionOfCoordinate(Coordinate coordinate);
    }
}
