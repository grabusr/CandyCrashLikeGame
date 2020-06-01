using UnityEngine;
using System.Collections;

using QuickTurnStudio.CandyCrashLike.Core;

namespace QuickTurnStudio.CandyCrashLike.UnityView
{
    public class Element : MonoBehaviour
    {
        [SerializeField] private float swapTime = 0.5F;
        [SerializeField] private float spawnTime = 0.8F;
        [SerializeField] private float destroyTime = 0.43F;

        private Config.GameConfig gameConfig;

        private BlockData blockData;
        private IBoardView view;
        private IBoardController boardController;        
        private bool clickStarted = false;

        public Coordinate Coordinate { get; set; }

        public void SetBoardView(IBoardView view)
        {
            this.view = view;
        }

        public void SetPlayerController(IBoardController controller)
        {
            boardController = controller;
        }

        public void SetGameConfig(Config.GameConfig gameConfig)
        {
            this.gameConfig = gameConfig;
        }

        public void SetBlockData(BlockData blockData)
        {
            this.blockData = blockData;
            var color = gameConfig.GetColorForBlock(blockData);
            SetColor(color);
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

        public void AnimateSpawn()
        {            
            StartCoroutine(AnimateScaing(new Vector3(0.0F, 0.0F, 0.0F),
                                         new Vector3(1.0F, 1.0F, 1.0F),
                                         spawnTime));
        }

        public void AnimateDestroy()
        {
            StartCoroutine(AnimateScaing(new Vector3(1.0F, 1.0F, 1.0F),
                                         new Vector3(0.0F, 0.0F, 0.0F),
                                         destroyTime));
        }

        private IEnumerator AnimateScaing(Vector3 from, Vector3 to, float duration)
        {
            float time = 0f;
            transform.localScale = from;
            while (time <= duration)
            {
                time = time + Time.deltaTime;
                float percent = Mathf.Clamp01(time / duration);
                float factor = Mathf.Sin(percent * Mathf.PI * 0.5F);
                transform.localScale = Vector3.Lerp(from, to, factor);

                yield return null;
            }
            view.AnimationEnded(this);
        }

        private IEnumerator AnimateMove(Coordinate toCoord, float duration)
        {
            float time = 0f;
            var from = transform.position;
            var destination = view.GetPositionOfCoordinate(toCoord);
            while (time <= duration)
            {
                time = time + Time.deltaTime;
                float percent = Mathf.Clamp01(time / duration);
                float factor = Mathf.Sin(percent * Mathf.PI * 0.5F);
                transform.position = Vector3.Lerp(from, destination, factor);

                yield return null;
            }
            this.Coordinate = toCoord;
            view.AnimationEnded(this);
        }

        private void OnMouseEnter()
        {
            if (view.IsAnimating())
            {
                return;
            }
            var color = gameConfig.GetColorForBlock(blockData);
            color.a = 0.6F;
            SetColor(color);
        }

        private void OnMouseExit()
        {
            if (view.IsAnimating())
            {
                return;
            }
            var color = gameConfig.GetColorForBlock(blockData);
            SetColor(color);
            if (clickStarted)
            {
                clickStarted = false;
            }
        }

        private void OnMouseDown()
        {
            if (view.IsAnimating())
            {
                return;
            }
            clickStarted = true;
        }

        private void OnMouseUp()
        {
            if (view.IsAnimating())
            {
                return;
            }
            if (!clickStarted)
            {
                return;
            }
            clickStarted = false;
            if (null == view)
            {
                return;
            }
            boardController.OnBlockClicked(this);
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
