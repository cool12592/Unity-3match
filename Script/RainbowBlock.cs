using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowBlock : BasicBlock
{

    [SerializeField]
    private GameObject effect;

    // Start is called before the first frame update
    public override void init(BasicBlock[,] grid_, int i, int j)
    {
        base.init(grid_, i, j);
        alpha = 0.3f;
        kind = 5;
    }

    public override void tryToErase()
    {
        //alpha = 0.5f;
        itemOn = true;
    }

    void effectSpawn(Vector3 target_pos)
    {
        Instantiate(effect, new Vector3(col * ExecuteLogic.tileSize, -row * ExecuteLogic.tileSize, 0f), Quaternion.identity).GetComponent<effect2>().init(target_pos);

    }

    public override bool useItem()
    {
        if (match)
            return false;
        match = true;
        alpha = 0.5f;

        if (rainbowTarget?.kind == 5)
        {
            for (int i = 1; i < ExecuteLogic.n; i++)
            {
                for (int j = 1; j < ExecuteLogic.m; j++)
                {
                    if (grid[i, j] == rainbowTarget)
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


        int targetKindNum=0;
        if (rainbowTarget == null || rainbowTarget?.kind == 4)
        {
            targetKindNum = Random.Range(0, 4);
        }
        else
        {
            targetKindNum = rainbowTarget.kind;
        }

        for (int i = 1; i < ExecuteLogic.n; i++)
        {
            for (int j = 1; j < ExecuteLogic.m; j++)
            {
                if (grid[i, j].kind == targetKindNum)
                {
                    if (grid[i, j] == rainbowTarget)
                        Debug.Log("hmm");
                    effectSpawn(grid[i, j].GetComponent<Transform>().position);

                    grid[i, j].tryToErase();
                }
            }
        }

        rainbowTarget?.tryToErase();

        return true;

    }

    public override void reInit(ref int num)
    {
        base.reInit(ref num);
        Utilities.ChangeBlock(grid,this, ExecuteLogic.basicBlockPool.GetObject());
    }

    public override void deactivate()
    {
        itemOn = false;
        match = false;
        rainbowTarget = null;

        kind = Random.Range(0, 4);
        y = ExecuteLogic.tileSize * -100; //삭제된거는 판보다더 위로감 무빙애니에서 내려오게됨
        x = ExecuteLogic.tileSize * -100;
        alpha = 1f;


        GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        ExecuteLogic.rainBowBlockPool.ReturnObject(this);
    }
}
