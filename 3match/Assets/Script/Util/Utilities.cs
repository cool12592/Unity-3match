﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { RIGHT, LEFT, UP, DOWN };
public enum kindType {None, block1, block2, block3, block4, Snow, Rainbow };

public static class Utilities
{

    public static BasicBlock clickBlock1;
    public static BasicBlock clickBlock2;

    [SerializeField]
    public static Transform startPoint1, startPoint2;

    public static void init()
    {
        startPoint1 = GameObject.Find("StartPoint1").transform;
        startPoint2 = GameObject.Find("StartPoint2").transform;
    }


    public static bool checkBoardRange(int x, int y)
    {
        if (x <= 0 || MainLogic.colSize <= x)
            return false;
        if (y <= 0 || MainLogic.rowSize <= y)
            return false;

        return true;
    }

    public static void ChangeBlock(BasicBlock[,] grid, BasicBlock block, BasicBlock newBlock)
    {
        newBlock.init(grid, block.row, block.col);
        newBlock.y = block.y;
        newBlock.x = block.x;

        grid[block.row, block.col] = newBlock;

        block.deactivate();
    }

    public static void swapMove(BasicBlock[,] grid, Direction dir)
    {
        if (MainLogic.isSwap || MainLogic.isLocked)
            return;

        if (clickBlock1 is SnowBlock)
        {
            (clickBlock1 as SnowBlock).readyItem(dir);
            return;
        }

        MainLogic.isSwap = true;

        switch (dir)
        {
            case Direction.RIGHT:
                clickBlock2 = clickBlock1.getRight();
                swap(grid, clickBlock1, clickBlock2, true);
                break;
            case Direction.LEFT:
                clickBlock2 = clickBlock1.getLeft();
                swap(grid, clickBlock1, clickBlock2, true);
                break;
            case Direction.UP:
                clickBlock2 = clickBlock1.getUp();
                swap(grid, clickBlock1, clickBlock2, true);
                break;
            case Direction.DOWN:
                clickBlock2 = clickBlock1.getDown();
                swap(grid, clickBlock1, clickBlock2, true);
                break;
            default:
                break;
        }
    }

    public static void swap(BasicBlock[,] grid, BasicBlock p1, BasicBlock p2, bool isControl = false)
    {
        if (isControl)
        {
            if (p1 is RainbowBlock || p2 is RainbowBlock)
            {
                RainbowBlock rain;
                BasicBlock other;
                if (p1 is RainbowBlock)
                {
                    rain = p1 as RainbowBlock;
                    other = p2;

                    rain.readyItem(other);
                }
                else
                {
                    rain = p2 as RainbowBlock;
                    other = p1;

                    other.arriveDestAction += rain.arrive;
                    rain.readyItem(other);
                }
                
                p1.row = p2.row;
                p1.col = p2.col;
                return;
            }

        }

        int p1__row = p1.row;
        int p1__col = p1.col;

        p1.row = p2.row;
        p1.col = p2.col;

        p2.row = p1__row;
        p2.col = p1__col;


        grid[p1.row, p1.col] = p1;
        grid[p2.row, p2.col] = p2;
    }
}
