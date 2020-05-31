using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using core;
using LocalModel;
using view;

public class UnityBoardViewManager : MonoBehaviour, core.ICandyCrashLikeView
{
    [SerializeField]
    private Sprite blockSprite;

    [SerializeField]
    private GameConfigComponent gameConfig;

    [SerializeField]
    private Transform elementPrefab;

    private float spriteSize = 3.0F;

    private LocalGameModel gameModel;
    private Element[,] blockElements;    

    public void Start()
    {
        var randomDevice = new SeededRandomTypeProvider(gameConfig.Seed);
        var boardProvider = new RandomBoardGenerator(randomDevice, gameConfig);
        gameModel = new LocalGameModel(boardProvider);

        var gameController = new GameController(gameModel);
        gameController.AddGameView(this);
    }

    public void Init(IGameModelInfo gameModelInfo)
    {
        var boardData = gameModelInfo.Board;

        var middleX = spriteSize * (gameModelInfo.RowsCount - 1) * 0.5F;
        var middleY = spriteSize * (gameModelInfo.ColumnsCount - 1) * 0.5F;

        blockElements = new Element[gameModelInfo.RowsCount, gameModelInfo.ColumnsCount];

        for (var row = 0; row < gameModelInfo.RowsCount; ++row)
        {
            for (var column = 0; column < gameModelInfo.ColumnsCount; ++column)
            {                
                var position = new Vector3(column * spriteSize - middleX, row * spriteSize - middleY, 0);
                blockElements[row, column] = SpawnElement(position, boardData[row, column]);
            }
        }
    }

    public void OnMoveEvent(MoveResult[] moveResults)
    {
        
    }

    private int CalculateBlockSize(int rowsCount, int columnsCount)
    {
        var widthFactor = Screen.width / rowsCount;
        var heightFactor = Screen.height / columnsCount;

        return Mathf.Min(widthFactor, heightFactor);
    }
    
    private Element SpawnElement(Vector3 position, BlockData blockData)
    {        
        var obj = Instantiate(elementPrefab, position, Quaternion.identity);
        obj.transform.SetParent(this.transform);
        var element = obj.GetComponent<Element>();
        element.SetColor(gameConfig.GetColorForBlock(blockData));
        return element;
    }    
}
