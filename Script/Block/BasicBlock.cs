using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BasicBlock : MonoBehaviour
{
    public int col, row;
    public kindType kind;
    public bool match,isItemMatch;
    public float x, y, alpha;
    
    public bool itemOn;
    protected float speed = 5f;
    protected BasicBlock[,] grid;

    public event Action arriveDestAction;

    public virtual void init(BasicBlock[,] grid_,int i,int j)
    {
        grid = grid_;
        kind = kindType.None;
        alpha = 1f;
        itemOn = false;
        match = false;

        col = j;
        row = i;

        x = j * MainLogic.tileSize;
        y = -i * MainLogic.tileSize;

        kind =(kindType)(UnityEngine.Random.Range(1, 5));
        if (i == 0 || j == 0 || i == MainLogic.rowSize || j == MainLogic.colSize)
            kind = kindType.None;
    }

    public BasicBlock getRight()
    {
        return grid[row,col + 1];
    }

    public BasicBlock getLeft()
    {
        return grid[row, col - 1];
    }

    public BasicBlock getDown()
    {
        return grid[row+1, col];
    }

    public BasicBlock getUp() 
    {
        return grid[row - 1, col];
    }

    public bool moveToDest()
    {
        float dx = x - (col * MainLogic.tileSize);
        float dy = y + (row * MainLogic.tileSize);
        bool b1 = moveBlock(dx, true);
        bool b2 = moveBlock(dy, false);

        if (b1 || b2)
            return true;
        return false;
    }

    public virtual bool moveBlock(float dis, bool isXmove)
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

        return false;
    }

    public virtual void onTheMove(bool isXmove)
    {
        return;
    }

    public virtual void arriveToDest()
    {
        arriveDestAction?.Invoke();
        arriveDestAction = null;
      
        x = col * MainLogic.tileSize;
        y = -row * MainLogic.tileSize;
    }

    public void setItemMatch()
    {
        isItemMatch = true;
        tryToErase();
    }

    public virtual void tryToErase()
    {
        if (match)
            return;

        alpha = 0.7f;
        match = true;
    }

    public virtual bool useItem()
    {
        return false;
    }

    public void deactivate()
    {
        itemOn = false;
        match = false;

        kind = (kindType)(UnityEngine.Random.Range(1, 5));
        y = MainLogic.tileSize * -100;
        x = MainLogic.tileSize * -100;
        alpha = 1f;

        GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        returnObjectPool();
    }

    public virtual void returnObjectPool()
    {
        MainLogic.basicBlockPool.ReturnObject(this);
    }

    public virtual void reInit(ref int num)
    {
        itemOn = false;
        kind = (kindType)(UnityEngine.Random.Range(1, 5));
        y = MainLogic.tileSize * num++; //삭제된거는 위칸으로 올라감
        x = MainLogic.tileSize * col;
        match = false;
        alpha = 1f;

        GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
}
