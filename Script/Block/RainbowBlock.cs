using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowBlock : BasicBlock
{

    [SerializeField]
    private GameObject effect;
    bool isNotArrive = true;
    BasicBlock target;
    kindType targetKind;

    public override void init(BasicBlock[,] grid_, int i, int j)
    {
        base.init(grid_, i, j);
        alpha = 0.3f;
        kind = kindType.Rainbow;
    }

    public void readyItem(BasicBlock block)
    {
        itemOn = true;
        target = block;
    }

    public override void arriveToDest()
    {
        arrive();
        x = col * MainLogic.tileSize;
        y = -row * MainLogic.tileSize;
    }

    public void arrive()
    {
        isNotArrive = false;
    }

    public override void tryToErase()
    {
        if (match || itemOn)
            return;

        alpha = 0.7f;
        isNotArrive = false;
        itemOn = true;
    }

    public override bool useItem()
    {
        if (match || isNotArrive)
            return true;

        match = true;
        alpha = 0.7f;
        itemOn = false;

        if(target == null || target?.kind == kindType.Snow)
        {
            targetKind = (kindType)(Random.Range(1, 5));
        }
        else
            targetKind = target.kind;

        if (targetKind == kindType.Rainbow)
        {
            clearTheAllBlock();
            return true;
        }

        clearTheTargetBlocks();
        target?.tryToErase();

        return true;
    }

    void clearTheAllBlock()
    {
        for (int i = 1; i < MainLogic.rowSize; i++)
        {
            for (int j = 1; j < MainLogic.colSize; j++)
            {
                if (grid[i, j].kind == kindType.Rainbow)
                {
                    grid[i, j].match = true;
                    continue;
                }
                effectSpawn(grid[i, j].GetComponent<Transform>().position);
                grid[i, j].tryToErase();
            }
        }
    }

    void clearTheTargetBlocks()
    {
        for (int i = 1; i < MainLogic.rowSize; i++)
        {
            for (int j = 1; j < MainLogic.colSize; j++)
            {
                if (grid[i, j].kind == targetKind)
                {
                    effectSpawn(grid[i, j].GetComponent<Transform>().position);
                    grid[i, j].tryToErase();
                }
            }
        }
    }

    void effectSpawn(Vector3 targetPos)
    {
        Instantiate(effect, new Vector3(col * MainLogic.tileSize, -row * MainLogic.tileSize, 0f), Quaternion.identity).GetComponent<effect2>().init(targetPos);
    }

    public override void reInit(ref int num)
    {
        base.reInit(ref num);
        Utilities.ChangeBlock(grid, this, MainLogic.basicBlockPool.GetObject());
    }

    public override void returnObjectPool()
    {
        MainLogic.rainBowBlockPool.ReturnObject(this);
    }
}
