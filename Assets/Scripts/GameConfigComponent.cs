using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalModel;
using core;

public class GameConfigComponent : MonoBehaviour, IGameConfig
{
    [SerializeField]
    private int rowsCount = 5;

    [SerializeField]
    private int columnsCount = 5;

    [SerializeField]
    private int seed = 0;

    [SerializeField]
    private Color[] colors;

    public int RowsCount => rowsCount;
    public int ColumnsCount => columnsCount;
    public int Seed => seed;
    public BlockData[] BlockDataPool
    {
        get
        {
            var typesPool = new BlockData[colors.Length];
            for(var i = 0; i < typesPool.Length; ++i)
            {
                typesPool[i] = new BlockData(i);
            }
            return typesPool;
        }
    }

    public Color GetColorForBlock(BlockData blockData)
    {
        return colors[blockData.Type];
    }
}
