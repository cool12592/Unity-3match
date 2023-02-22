using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBlock : MonoBehaviour
{
    public int col, row, kind;
    public bool match;
    public float x, y,alpha;

    protected BasicBlock[,] grid;
    public BasicBlock rainbowTarget;
    public bool itemOn;
    
    protected float speed = 5f;


    // Start is called before the first frame update
    public virtual void init(BasicBlock[,] grid_,int i,int j)
    {
        kind = -1;
        alpha = 1f;
        grid = grid_;
        itemOn = false;
        match = false;

        

        col = j;
        row = i;

        x = j * ExecuteLogic.tileSize;
        y = -i * ExecuteLogic.tileSize;

        kind = Random.Range(0, 4);
        if (i == 0 || j == 0 || i == ExecuteLogic.n || j == ExecuteLogic.m)
            kind = -1;
    }

    private void Update()
    {

        if(100f<Mathf.Abs(x)+ Mathf.Abs(y))
            Debug.Log("ss");

       
    }

    public BasicBlock getNext()
    {
        if (row % 2 != 0)
        {
            if (col == ExecuteLogic.m - 1)
            {
                if (row < ExecuteLogic.n - 1)
                    return getDown();
                else
                    return null;
            }
            return getRight();
        }
        else
        {
            if (col == 1)
            {
                if (row < ExecuteLogic.n - 1)
                    return getDown();
                else
                    return null;
            }

            return getLeft(); 
        }
    }

    public BasicBlock getRight()
    {
        if (checkBoardRange( col + 1, row) == false)
            Debug.Log("머여");
        return grid[row,col + 1];
    }

    public BasicBlock getLeft()
    {
        if (checkBoardRange(col - 1, row) == false)
            Debug.Log("머여");

        return grid[row, col - 1];
    }

    public BasicBlock getDown()
    {
        if (checkBoardRange(col, row+1) == false)
            Debug.Log("머여");
        return grid[row+1, col];
    }

    public BasicBlock getUp() 
    {
        if (checkBoardRange(col, row - 1) == false)
            Debug.Log("머여");
        return grid[row - 1, col];
    }

    public bool checkBoardRange(int x, int y)
    {
        if (x < 0 || ExecuteLogic.m < x)
            return false;
        if (y < 0 || ExecuteLogic.n < y)
            return false;

        return true;
    }


    public bool moveToDest()
    {
        if (col == 2 && row == 7 && kind != 5)
        {
            if (Utilities.checkBoardRange((int)x, (int)-y) == false)
                Debug.Log("ss");
        }


        float dx = x - (col * ExecuteLogic.tileSize);
        float dy = y + (row * ExecuteLogic.tileSize);
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

                if (Utilities.checkBoardRange((int)x, (int)-y) == false)
                    Debug.Log("aaa");

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
        if (rainbowTarget && itemOn == false)
        {
            if(rainbowTarget is RainbowBlock)
            {
                rainbowTarget.tryToErase();
            }
            else
            {
                tryToErase();
            }
            //rainbowTarget = null;
        }

        x = col * ExecuteLogic.tileSize;
        y = -row * ExecuteLogic.tileSize;

        if (Utilities.checkBoardRange((int)x, (int)-y) == false)
            Debug.Log("aaa");
    }


    public virtual void tryToErase()
    {
        if (match)
            return;

        //alpha = 0.5f;
        match = true;
    }

    public virtual bool useItem()
    {
        return false;
    }

    public virtual void deactivate()
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

        ExecuteLogic.basicBlockPool.ReturnObject(this);
    }


    public virtual void reInit(ref int num)
    {

        itemOn = false;
        kind = Random.Range(0, 4);
        y = ExecuteLogic.tileSize * num++; //삭제된거는 판보다더 위로감 무빙애니에서 내려오게됨
        x = ExecuteLogic.tileSize * col;
        match = false;
        alpha = 1f;
        rainbowTarget = null;

        GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

    }
}
