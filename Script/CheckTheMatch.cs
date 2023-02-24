using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTheMatch : MonoBehaviour
{
    BasicBlock[,] grid;

    public void init(BasicBlock[,] grid_)
    {
        grid = grid_;
    }

    public void checkMatch()
    {
        if (ExecuteLogic.isLocked)
            return;

        checkNormalMatch();
        checkItemMatch();
    }

    void checkNormalMatch()
    {
        //매칭o
        for (int i = 1; i < ExecuteLogic.n; i++)
        {
            for (int j = 1; j < ExecuteLogic.m; j++)
            {
                var nowBlock = grid[i, j];
                nowBlock.isItemMatch = false;

                if (nowBlock.kind == nowBlock.getDown().kind)
                {
                    if (nowBlock.kind == nowBlock.getUp().kind)
                    {
                        nowBlock.tryToErase();
                        nowBlock.getUp().tryToErase();
                        nowBlock.getDown().tryToErase();
                    }
                }

                if (nowBlock.kind == nowBlock.getRight().kind)
                {
                    if (nowBlock.kind == nowBlock.getLeft().kind)
                    {
                        nowBlock.tryToErase();
                        nowBlock.getRight().tryToErase();
                        nowBlock.getLeft().tryToErase();
                    }
                }
            }
        }
    }

    void checkItemMatch()
    {
        FiveBlock();
        TwoByTwoBlock();
        FourBlock();
    }

    void TwoByTwoBlock()
    {
        for (int i = 1; i < ExecuteLogic.n; i++)
        {
            for (int j = 1; j < ExecuteLogic.m; j++)
            {
                var block1 = grid[i, j];
                var block2 = block1.getRight();
                var block3 = block1.getDown();
                var block4 = block1.getRight().getDown();

                if(checkItemMatchBlock(block1,block2,block3,block4))
                    continue;
                
                if (block1.kind == block2.kind &&
                    block1.kind == block3.kind &&
                    block1.kind == block4.kind)
                {
                    markItemMatchBlocks(block1, block2, block3, block4);
                    changeToItemBlock(ExecuteLogic.snowBlockPool.GetObject(), block1, block2, block3, block4);
                }
            }
        }
    }

    void FiveBlock()
    {
        for (int i = 1; i < ExecuteLogic.n; i++)
        {
            for (int j = 1; j <= ExecuteLogic.m - 5; j++)
            {
                var block1 = grid[i, j];
                var block2 = block1.getRight();
                var block3 = block2.getRight();
                var block4 = block3.getRight();
                var block5 = block4.getRight();

                if (checkItemMatchBlock(block1, block2, block3, block4,block5))
                    continue;

                if (block1.kind == block2.kind &&
                    block1.kind == block3.kind &&
                    block1.kind == block4.kind &&
                    block1.kind == block5.kind)
                {
                    markItemMatchBlocks(block1, block2, block3, block4, block5);
                    changeToItemBlock(ExecuteLogic.rainBowBlockPool.GetObject(), block1, block2, block3, block4, block5);
                }
            }
        }

        for (int i = 1; i <= ExecuteLogic.n - 5; i++)
        {
            for (int j = 1; j < ExecuteLogic.m; j++)
            {
                var block1 = grid[i, j];
                var block2 = block1.getDown();
                var block3 = block2.getDown();
                var block4 = block3.getDown();
                var block5 = block4.getDown();

                if (checkItemMatchBlock(block1, block2, block3, block4, block5))
                    continue;

                if (block1.kind == block2.kind &&
                    block1.kind == block3.kind &&
                    block1.kind == block4.kind &&
                    block1.kind == block5.kind)
                {
                    markItemMatchBlocks(block1, block2, block3, block4,block5);
                    changeToItemBlock(ExecuteLogic.rainBowBlockPool.GetObject(), block1, block2, block3, block4, block5);
                }
            }
        }
    }

    void FourBlock()
    {
        for (int i = 1; i < ExecuteLogic.n; i++)
        {
            for (int j = 1; j <= ExecuteLogic.m - 4; j++)
            {
                var block1 = grid[i, j];
                var block2 = block1.getRight();
                var block3 = block2.getRight();
                var block4 = block3.getRight();

                if (checkItemMatchBlock(block1, block2, block3, block4))
                    continue;

                if (block1.kind == block2.kind &&
                    block1.kind == block3.kind &&
                    block1.kind == block4.kind)
                {
                    markItemMatchBlocks(block1, block2, block3, block4);
                    changeToItemBlock(ExecuteLogic.ribbonBlockPool.GetObject(), block1, block2, block3, block4);
                }
            }
        }


        for (int i = 1; i <= ExecuteLogic.n - 4; i++)
        {
            for (int j = 1; j < ExecuteLogic.m; j++)
            {
                var block1 = grid[i, j];
                var block2 = block1.getDown();
                var block3 = block2.getDown();
                var block4 = block3.getDown();

                if (checkItemMatchBlock(block1, block2, block3, block4))
                    continue;

                if (block1.kind == block2.kind &&
                    block1.kind == block3.kind &&
                    block1.kind == block4.kind)
                {
                    markItemMatchBlocks(block1, block2, block3, block4);
                    changeToItemBlock(ExecuteLogic.ribbonBlockPool.GetObject(), block1, block2, block3, block4);
                }
            }
        }
    }


    bool checkItemMatchBlock(params BasicBlock[] blocks)
    {
        foreach (BasicBlock block in blocks)
        {
            if (block.isItemMatch)
                return true;
        }
        return false;
    }

    void markItemMatchBlocks(params BasicBlock[] blocks)
    {
        foreach (BasicBlock block in blocks)
        {
            block.setItemMatch();
        }
    }

    void changeToItemBlock(BasicBlock newBlock, params BasicBlock[] blocks)
    {
        if (Utilities.clickBlock1 == null || Utilities.clickBlock2 == null)
        {
            Utilities.ChangeBlock(grid, blocks[0], newBlock);
            return;
        }

        foreach (BasicBlock block in blocks)
        {
            if (block == Utilities.clickBlock1 || block == Utilities.clickBlock2)
            {
                Utilities.ChangeBlock(grid, block, newBlock);
                return;
            }
        }

        Utilities.ChangeBlock(grid,blocks[0], newBlock);
    }

}
