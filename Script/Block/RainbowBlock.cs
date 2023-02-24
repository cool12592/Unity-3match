using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowBlock : BasicBlock
{

    [SerializeField]
    private GameObject effect;
    bool isNotArrive = true;
    int targetKind = -1;

    public override void init(BasicBlock[,] grid_, int i, int j)
    {
        base.init(grid_, i, j);
        alpha = 0.3f;
        kind = 5;
    }

    public void readyItem(int kind_)
    {
        itemOn = true;
        targetKind = kind_;
    }

    public override void arriveToDest()
    {
        arrive();
        x = col * ExecuteLogic.tileSize;
        y = -row * ExecuteLogic.tileSize;
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
        alpha = 0.5f;
        itemOn = false;

        if (targetKind == 5)
        {
            for (int i = 1; i < ExecuteLogic.n; i++)
            {
                for (int j = 1; j < ExecuteLogic.m; j++)
                {
                    if (grid[i, j].kind == 5)
                    {
                        grid[i, j].match = true;
                        continue;
                    }
                    effectSpawn(grid[i, j].GetComponent<Transform>().position);
                    grid[i, j].tryToErase();
                }
            }
            return true;
        }

        if (targetKind == -1 || targetKind ==4)
        {
            targetKind = Random.Range(0, 4);
        }


        for (int i = 1; i < ExecuteLogic.n; i++)
        {
            for (int j = 1; j < ExecuteLogic.m; j++)
            {
                if (grid[i, j].kind == targetKind)
                {
                    effectSpawn(grid[i, j].GetComponent<Transform>().position);
                    grid[i, j].tryToErase();
                }
            }
        }
        return true;
    }

    void effectSpawn(Vector3 target_pos)
    {
        Instantiate(effect, new Vector3(col * ExecuteLogic.tileSize, -row * ExecuteLogic.tileSize, 0f), Quaternion.identity).GetComponent<effect2>().init(target_pos);
    }

    public override void reInit(ref int num)
    {
        base.reInit(ref num);
        Utilities.ChangeBlock(grid,this, ExecuteLogic.basicBlockPool.GetObject());
    }

    public override void returnObjectPool()
    {
        ExecuteLogic.rainBowBlockPool.ReturnObject(this);
    }
}
