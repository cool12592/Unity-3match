using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBlock : BasicBlock
{
    public int snowLevel = 1;
    float snowSpeed = 2.5f;
    int origin_col=0, origin_row = 0;

    Direction myDir;

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
            if(snowMove(isXmove))
                return true;
        }
        else
        {
            if (base.moveBlock(dis, isXmove))
                return true;
        }

        return false;
    }

    bool snowMove(bool isXmove)
    {
        onTheMove(isXmove);
        if (myDir == Direction.RIGHT)
        {
            x += Time.deltaTime * snowSpeed;
        }
        else if (myDir == Direction.LEFT)
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

        if (Utilities.checkBoardRange((int)x, (int)-y) == false)
            arriveToDest();

        return true;
    }

    public override void onTheMove(bool isXmove)
    {
        if (itemOn == false)
            return;

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

    public override void arriveToDest()
    {
        if (itemOn)
        {
            itemOn = false;
            col = origin_col;
            row = origin_row;

            alpha = 0;
            match = true;
            GameManager.Instance.goalProgress();
        }

        x = col * ExecuteLogic.tileSize;
        y = -row * ExecuteLogic.tileSize;
    }


    void snowPower(BasicBlock target)
    {
        if (target == this)
            return;
        target.alpha = 0.5f;

        if (target is SnowBlock)
        {
            if (snowLevel == 1)
            {
                target.alpha = 0f;
                StartCoroutine(bigSnow());
            }
            
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

    public override void tryToErase()
    {
        if (match || itemOn)
        {
            return;
        }
        itemOn = true;
        myDir = (Direction)(Random.Range(0, 4));
    }

    public override bool useItem()
    {
        if (origin_col == 0 && origin_row ==0)
        {
            rollTheSnow(myDir);
            return true;
        }
        return false;
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

    IEnumerator bigSnow()
    {
        ExecuteLogic.isMovePause = true;
        var num = GetComponent<Transform>().localScale.sqrMagnitude;
        var size = GetComponent<Transform>().localScale;
        while (num < 10f)
        {
            yield return null;
            size = GetComponent<Transform>().localScale;
            num = size.sqrMagnitude;
            GetComponent<Transform>().localScale = size + (size * Time.deltaTime);
        }
        snowLevel = 2;
        ExecuteLogic.isMovePause = false;
    }

    public override void reInit(ref int num)
    {
        base.reInit(ref num);
        Utilities.ChangeBlock(grid, this, ExecuteLogic.basicBlockPool.GetObject());
    }

    public override void returnObjectPool()
    {
        ExecuteLogic.snowBlockPool.ReturnObject(this);
    }
}
