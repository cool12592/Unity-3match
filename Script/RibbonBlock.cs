using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonBlock : BasicBlock
{

    [SerializeField]
    private GameObject effect;

    float timer = 0f;

    // Start is called before the first frame update
    public override void init(BasicBlock[,] grid_, int i, int j)
    {
        base.init(grid_, i, j);
        alpha = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (itemOn && alpha > 0f)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                timer = 0f;
                GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            }
        }
    }

    public override void tryToErase()
    {
        itemOn = true;
    }

    public override bool useItem()
    {
        if (match)
            return false;

        match = true;
        alpha = 0.5f;

        int ran = Random.Range(0, 4);
        if (ran <= 1)
        {
            Instantiate(effect, new Vector3(col * ExecuteLogic.tileSize, -row * ExecuteLogic.tileSize, 0f), Quaternion.identity).GetComponent<effect>().init(1);
            Instantiate(effect, new Vector3(col * ExecuteLogic.tileSize, -row * ExecuteLogic.tileSize, 0f), Quaternion.identity).GetComponent<effect>().init(2);

            for (int i = 1; i < ExecuteLogic.n; i++)
            {
                grid[i, col].tryToErase();
                
            }
        }
        else
        {
            Instantiate(effect, new Vector3(col * ExecuteLogic.tileSize, -row * ExecuteLogic.tileSize, 0f), Quaternion.identity).GetComponent<effect>().init(3);
            Instantiate(effect, new Vector3(col * ExecuteLogic.tileSize, -row * ExecuteLogic.tileSize, 0f), Quaternion.identity).GetComponent<effect>().init(4);

            for (int j = 1; j < ExecuteLogic.m; j++)
            {
                grid[row, j].tryToErase();
                
            }
        }
        return true;
        
    }

 

    public override void reInit(ref int num)
    {
        base.reInit(ref num);
        Utilities.ChangeBlock(grid, this, ExecuteLogic.basicBlockPool.GetObject());
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

        ExecuteLogic.ribbonBlockPool.ReturnObject(this);
    }


}
