using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainLogic : MonoBehaviour
{
    public static readonly int rowSize = 8, colSize = 8;
    public static float tileSize; 

    public static bool isSwap = false, isMovePause = false;
    public static bool isLocked { get; private set; } = false;

    BasicBlock[,] grid = new BasicBlock[rowSize + 1, colSize + 1];

    [SerializeField]
    private BasicBlock[] blockPrefabs = new BasicBlock[4];
    public static ObjectPool basicBlockPool;
    public static ObjectPool snowBlockPool;
    public static ObjectPool rainBowBlockPool;
    public static ObjectPool ribbonBlockPool;

    private CheckTheMatch checkTheMatch;
    private MouseInput mouseInput;
    private DrawTheBoard drawtheBoard;

    void Start()
    {
        Utilities.init();

        var pos1 = Utilities.startPoint1.position;
        var pos2 = Utilities.startPoint2.position;
        tileSize = Mathf.Abs(pos1.x - pos2.x);
 
        initObjectPool();
        initBoard();
        initLogicClass();
    }

    void initObjectPool()
    {
        basicBlockPool = GameObject.Find("BasicBlockPool").GetComponent<ObjectPool>();
        snowBlockPool = GameObject.Find("SnowBlockPool").GetComponent<ObjectPool>();
        rainBowBlockPool = GameObject.Find("RainBowBlockPool").GetComponent<ObjectPool>();
        ribbonBlockPool = GameObject.Find("RibbonBlockPool").GetComponent<ObjectPool>();

        basicBlockPool.init(blockPrefabs[0]);
        snowBlockPool.init(blockPrefabs[1]);
        rainBowBlockPool.init(blockPrefabs[2]);
        ribbonBlockPool.init(blockPrefabs[3]);
    }

    void initBoard()
    {
        for (int i = 0; i <= rowSize; i++)
        {
            for (int j = 0; j <= colSize; j++)
            {
                grid[i, j] = basicBlockPool.GetObject();
                var nowBlock = grid[i, j];
                nowBlock.init(grid, i, j);
            }
        }
    }

    void initLogicClass()
    {
        checkTheMatch = GetComponent<CheckTheMatch>();
        checkTheMatch.init(grid);
        mouseInput = GetComponent<MouseInput>();
        mouseInput.init(grid);
        drawtheBoard = GetComponent<DrawTheBoard>();
        drawtheBoard.init(grid);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            var v = grid[1, 1].GetComponent<SpriteRenderer>().sprite;
        
            Debug.Log(v.name);
        }
       

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Utilities.ChangeBlock(grid, grid[6, 2], rainBowBlockPool.GetObject());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Utilities.ChangeBlock(grid, grid[7, 2], rainBowBlockPool.GetObject());
        }

        if (GameManager.Instance.isGameEnd == false)
            mouseInput.updateMouseInput();
        itemTime();
        checkTheMatch.checkMatch();
        moveBlock();
        updateAlpha();
        itemTime();
        undoNotMatchingBlock();
        orderBoard();
        drawtheBoard.drawBoard();
    }

    
    
   
    void moveBlock()
    {
        if (isMovePause)
            return;

        //////무빙 
        isLocked = false;
        for (int i = 1; i < rowSize; i++)
        {
            for (int j = 1; j < colSize; j++)
            {
                var nowBlock = grid[i, j];
                if (nowBlock.moveToDest())
                {
                    isLocked = true;
                }

            }
        }
    }

    void updateAlpha()
    {
        ////삭제된건 알파다운 새로생긴아이템은 알파업
        if (isLocked) 
            return;

        for (int i = 1; i < rowSize; i++)
        {
            for (int j = 1; j < colSize; j++)
            {
                var nowBlock = grid[i, j];
                if (nowBlock.match == true)
                {
                    alphaDown(nowBlock); 
                }
                else
                {
                    alphaUp(nowBlock);
                }
            }
        }

    }

    void alphaDown(BasicBlock block)
    {
        if (0f < block.alpha)
        {
            block.alpha -= Time.deltaTime;
            isLocked = true;
        }
    }

    void alphaUp(BasicBlock block)
    {
        if (block.alpha < 1f)
        {
            block.alpha += Time.deltaTime;
            isLocked = true;
        }
    }

    

    void itemTime()
    {
        if (isLocked)
            return;

        for (int i = 1; i < rowSize; i++)
        {
            for (int j = 1; j < colSize; j++)
            {
                if (grid[i, j].itemOn == true)
                {
                    if (grid[i, j].useItem())
                        isLocked = true;
                }
            }
        }
    }

    void undoNotMatchingBlock()
    {
        bool match = false;
        for (int i = 1; i <= rowSize; i++)
        {
            for (int j = 1; j <= colSize; j++)
            {
                if (grid[i, j].match)
                {
                    match = true;
                    break;
                }
            }
            if (match)
                break;
        }

        //스왑했지만 매칭이 안됐을 시 다시 제자리로
        if (isSwap && !isLocked)
        {
            if (match == false)
            {
                Utilities.swap(grid, Utilities.clickBlock1, Utilities.clickBlock2);
            }
            else
                GameManager.Instance.moveProgress();

            isSwap = false;
        }
    }

    void orderBoard()
    {
        if (isLocked)
            return;

        for (int i = rowSize-1; i > 0; i--)
        {
            for (int j = 1; j < colSize; j++)
            {
                if (grid[i, j].match)
                {



                    grid[i, j].col = j;
                    grid[i, j].row = i;

                    isLocked = true;
                    moveToUpMatchBlock(i, j);
                }
            }
        }

        for (int j = 1; j < colSize; j++)
        {
            for (int i = rowSize-1, num = 0; i > 0; i--)
            {
                if (grid[i, j].match)
                {

                    isLocked = true;
                    grid[i, j].reInit(ref num);
                }
            }
        }


    }

    void moveToUpMatchBlock(int i, int j)
    {
        for (int n = i; n > 0; n--)
        {
            if (grid[n, j].match == false)
            {


                Utilities.swap(grid,grid[n, j], grid[i, j]);
                break;
            }
        }
    }

   

}
