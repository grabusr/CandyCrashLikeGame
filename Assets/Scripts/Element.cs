using UnityEngine;
using System.Collections;

using core;

namespace view
{
    public class Element : MonoBehaviour
    {
        [SerializeField]
        private float swapTime = 0.5F;
        private GameConfigComponent gameConfig;

        private BlockData blockData;
        private Coordinate coordinate;
        private IBoardView boardView;

        bool clickStarted = false;

        public void SetBoardView(IBoardView view)
        {
            boardView = view;
        }

        public void SetGameConfig(GameConfigComponent gameConfig)
        {
            this.gameConfig = gameConfig;
        }

        public void SetBlockData(BlockData blockData)
        {
            this.blockData = blockData;
            var color = gameConfig.GetColorForBlock(blockData);
            SetColor(color);
        }
        
        public Coordinate Coordinate
        {
            get => coordinate;
            set => coordinate = value;
        }

        public void Select()
        {
            transform.localScale = new Vector3(1.15F, 1.15F);
        }

        public void Deselect()
        {
            transform.localScale = new Vector3(1.0F, 1.0F);
        }

        public void MoveToPosition(Coordinate coordinate)
        {
            StartCoroutine(AnimateMove(coordinate, swapTime));
        }

        IEnumerator AnimateMove(Coordinate toCoord, float duration)
        {
            float time = 0f;
            var from = transform.position;
            var destination = boardView.GetPositionOfCoordinate(toCoord);
            while (time <= duration)
            {
                time = time + Time.deltaTime;
                float percent = Mathf.Clamp01(time / duration);
                float factor = Mathf.Sin(percent * Mathf.PI * 0.5F);
                transform.position = Vector3.Lerp(from, destination, factor);

                yield return null;
            }
            this.coordinate = toCoord;
            boardView.AnimationEnded(this);
        }

        private void OnMouseEnter()
        {
            var color = gameConfig.GetColorForBlock(blockData);
            color.a = 0.6F;
            SetColor(color);
        }

        private void OnMouseExit()
        {
            var color = gameConfig.GetColorForBlock(blockData);
            SetColor(color);
            if (clickStarted)
            {
                clickStarted = false;
            }
        }

        private void OnMouseDown()
        {
            clickStarted = true;
        }

        private void OnMouseUp()
        {
            if (!clickStarted)
            {
                return;
            }
            clickStarted = false;
            if (null == boardView)
            {
                return;
            }
            boardView.OnBlockSelect(this);
        }

        private void SetColor(Color color)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (null == spriteRenderer)
            {
                return;
            }
            spriteRenderer.color = color;
        }
    }
}