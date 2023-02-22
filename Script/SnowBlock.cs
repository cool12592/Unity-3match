using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBlock : BasicBlock
{
    public int snowLevel = 1;
    float snowSpeed = 2.5f;
    int origin_col=0, origin_row = 0;

    Direction myDir;

    // Start is called before the first frame update
    public override void init(BasicBlock[,] grid_, int i, int j)
    {
        base.init(grid_, i, j);
        alpha = 0.3f;
        kind = 4;
    }

    public override bool moveBlock(float dis, bool isXmove)
    {
        if (itemOn)
        {
            onTheMove(isXmove);
            if (myDir == Direction.RIGHT)
            {
                x += Time.deltaTime * snowSpeed;
            }
            else if(myDir == Direction.LEFT)
            {
                x -= Time.deltaTime * snowSpeed;
            }
            else if (myDir == Direction.UP)
            {
                y += Time.deltaTime * snowSpeed;
            }
            else if (myDir == Direction.DOWN)
            {
                y -= Time.deltaTime * snowSpeed;
            }

            if(Utilities.checkBoardRange((int)x,(int)-y)==false)
                arriveToDest();

            return true;
        }
        else
        {
            if (dis != 0f)
            {
                if (Mathf.Abs(dis) < 0.05f)
                {
                    arriveToDest();
                }
                else
                {
                    onTheMove(isXmove);

                    if (isXmove)
                        x -= dis / Mathf.Abs(dis) * Time.deltaTime * speed;
                    else
                        y -= dis / Mathf.Abs(dis) * Time.deltaTime * speed;

                    return true;

                }
            }
        }

        return false;
    }

    public override void arriveToDest()
    {


        if (itemOn)
        {

            itemOn = false;
            col = origin_col;
            row = origin_row;

            alpha = 0;
            match = true;
            ExecuteLogic.goalProgress();



        }



        x = col * ExecuteLogic.tileSize;
        y = -row * ExecuteLogic.tileSize;
    }

    public override void onTheMove(bool isXmove)
    {
        if (itemOn)
        {
            int target_y = (int)(-y);
            int target_x = (int)(x);

            if (Utilities.checkBoardRange(target_x, target_y))
            {
                snowPower(grid[target_y, target_x]);
            }

            if (snowLevel == 2)
            {
                if (isXmove)
                    target_y += 1;
                else
                    target_x += 1;

                if (Utilities.checkBoardRange(target_x, target_y))
                {
                    snowPower(grid[target_y, target_x]);
                }
                if (isXmove)
                    target_y -= 2;
                else
                    target_x -= 2;

                if (Utilities.checkBoardRange(target_x, target_y))
                {
                    snowPower(grid[target_y, target_x]);
                }
            }
        }
    }

    void snowPower(BasicBlock target)
    {

        if (target == this)
            return;
        target.alpha = 0.5f;

        if (target is SnowBlock)
        {
            StartCoroutine(bigSnow());
            target.match = true;
            
        }
        else
            target.tryToErase();
    }

    public void readyItem(Direction dir)
    {
        if (match || itemOn)
        {
            return;
        }

        myDir = dir;
        itemOn = true;
    }

    public override bool useItem()
    {
        if (origin_col == 0)
        {
            rollTheSnow(myDir);
            return true;
        }
        return false;
    }

    public override void tryToErase()
    {
       
        if (match || itemOn)
        {
            return;
        }
        itemOn = true;
        myDir = (Direction)(Random.Range(0, 4));
    }


    IEnumerator bigSnow()
    {
        ExecuteLogic.isStop = true;
        var num = GetComponent<Transform>().localScale.sqrMagnitude;
        var size = GetComponent<Transform>().localScale;
        while (num < 10f)
        {
            yield return null;

            size = GetComponent<Transform>().localScale;
            num = size.sqrMagnitude;


            GetComponent<Transform>().localScale = size + (size * Time.deltaTime);
           // Debug.Log("111");
        }
        snowLevel = 2;
        ExecuteLogic.isStop = false;

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

        ExecuteLogic.snowBlockPool.ReturnObject(this);
    }


    public void rollTheSnow(Direction dir)
    {
        origin_col = col;
        origin_row = row;
        switch (dir)
        {
            case Direction.RIGHT:
                myDir = Direction.RIGHT;
                break;
            case Direction.LEFT:
                myDir = Direction.LEFT;
                break;
            case Direction.UP:
                myDir = Direction.UP;
                break;
            case Direction.DOWN:
                    myDir = Direction.DOWN;
                break;
            default:
                break;
        }
    }
}
