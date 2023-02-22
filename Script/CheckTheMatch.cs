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
        //매칭o
        for (int i = 1; i < ExecuteLogic.n; i++)
        {
            for (int j = 1; j < ExecuteLogic.m; j++)
            {
                var nowBlock = grid[i, j];

                //if (NotCheckBlock(nowBlock.kind))
                //   continue;

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

    public void checkItemMatch()
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
                var nowBlock = grid[i, j];


                //if (NotCheckBlock(nowBlock.kind))
                //    continue;

                var block2 = nowBlock.getRight();
                var block3 = nowBlock.getDown();
                var block4 = nowBlock.getRight().getDown();

                if (nowBlock.kind == block2.kind &&
                    nowBlock.kind == block3.kind &&
                    nowBlock.kind == block4.kind)
                {
                    nowBlock.tryToErase();
                    block2.tryToErase();
                    block3.tryToErase();
                    block4.tryToErase();
                    changeToItemBlock(ExecuteLogic.snowBlockPool.GetObject(), nowBlock, block2, block3, block4);
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
                var nowBlock = grid[i, j];
                if (nowBlock.kind == nowBlock.getLeft().kind) continue; //6줄이상방지


                var block2 = nowBlock.getRight();
                var block3 = block2.getRight();
                var block4 = block3.getRight();
                var block5 = block4.getRight();

                if (nowBlock.kind == block2.kind &&
                    nowBlock.kind == block3.kind &&
                    nowBlock.kind == block4.kind &&
                    nowBlock.kind == block5.kind)
                {
                    changeToItemBlock(ExecuteLogic.rainBowBlockPool.GetObject(), nowBlock, block2, block3, block4, block5);
                }
            }
        }

        for (int i = 1; i <= ExecuteLogic.n - 5; i++)
        {
            for (int j = 1; j < ExecuteLogic.m; j++)
            {
                var nowBlock = grid[i, j];
                if (nowBlock.kind == nowBlock.getUp().kind) continue; //6줄이상방지

                //if (NotCheckBlock(nowBlock.kind))
                // continue;

                var block2 = nowBlock.getDown();
                var block3 = block2.getDown();
                var block4 = block3.getDown();
                var block5 = block4.getDown();


                if (nowBlock.kind == block2.kind &&
                    nowBlock.kind == block3.kind &&
                    nowBlock.kind == block4.kind &&
                    nowBlock.kind == block5.kind)
                {
                    changeToItemBlock(ExecuteLogic.rainBowBlockPool.GetObject(), nowBlock, block2, block3, block4, block5);
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
                var nowBlock = grid[i, j];
                if (nowBlock.kind == nowBlock.getLeft().kind) continue; //5줄이상방지


                var block2 = nowBlock.getRight();
                var block3 = block2.getRight();
                var block4 = block3.getRight();

                if (nowBlock.kind == block2.kind &&
                    nowBlock.kind == block3.kind &&
                    nowBlock.kind == block4.kind)
                {
                    changeToItemBlock(ExecuteLogic.ribbonBlockPool.GetObject(), nowBlock, block2, block3, block4);
                }
            }
        }


        for (int i = 1; i <= ExecuteLogic.n - 4; i++)
        {
            for (int j = 1; j < ExecuteLogic.m; j++)
            {
                var nowBlock = grid[i, j];
                if (nowBlock.kind == nowBlock.getUp().kind) continue; //5줄이상방지
                // if (NotCheckBlock(nowBlock.kind))
                // continue;

                var block2 = nowBlock.getDown();
                var block3 = block2.getDown();
                var block4 = block3.getDown();

                if (nowBlock.kind == block2.kind &&
                    nowBlock.kind == block3.kind &&
                    nowBlock.kind == block4.kind)
                {
                    changeToItemBlock(ExecuteLogic.ribbonBlockPool.GetObject(), nowBlock, block2, block3, block4);
                }
            }
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
