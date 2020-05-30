﻿using System;

namespace core
{
    public struct Block
    {
        private readonly Coordinate coordinate;
        private readonly BlockData blockData;
        
        public Block(int row, int column, BlockData blockData)
        {
            coordinate = new Coordinate(row, column);
            this.blockData = blockData;
        }
        
        public int Column
        {
            get => coordinate.Column;
        }

        public int Row
        {
            get => coordinate.Row;
        }

        public Coordinate Coord
        {
            get => coordinate;
        }

        public int Type
        {
            get => blockData.Type;
        }
    }
} // namespace core