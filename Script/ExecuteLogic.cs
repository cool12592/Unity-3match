using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class ExecuteLogic : MonoBehaviour
{
    public static readonly int n = 8, m = 8;
    public static float tileSize; //타일 사이즈

    BasicBlock[,] grid = new BasicBlock[n + 1, m + 1];
    public static bool isSwap = false, isMovePause = false, isGameEnd=false;
    public static bool isLocked { get; private set; } = false; 
    [SerializeField]
    private BasicBlock blockPrefab1, blockPrefab2, blockPrefab3, blockPrefab4;


    CheckTheMatch checkTheMatch;
    MouseInput mouseInput;
    DrawTheBoard drawtheBoard;

    public static ObjectPool basicBlockPool;
    public static ObjectPool snowBlockPool;
    public static ObjectPool rainBowBlockPool;
    public static ObjectPool ribbonBlockPool;

    // Start is called before the first frame update
    void Start()
    {
        Utilities.init();

        var pos1 = Utilities.startPoint1.position;
        var pos2 = Utilities.startPoint2.position;
        tileSize = Mathf.Abs(pos1.x - pos2.x);
 
        initObjectPool();
        boardInit();
        initObject();
    }

    void initObjectPool()
    {
        basicBlockPool = GameObject.Find("objectpool1").GetComponent<ObjectPool>();
        snowBlockPool = GameObject.Find("objectpool2").GetComponent<ObjectPool>();
        rainBowBlockPool = GameObject.Find("objectpool3").GetComponent<ObjectPool>();
        ribbonBlockPool = GameObject.Find("objectpool4").GetComponent<ObjectPool>();



        basicBlockPool.init(blockPrefab1);
        snowBlockPool.init(blockPrefab2);
        rainBowBlockPool.init(blockPrefab3);
        ribbonBlockPool.init(blockPrefab4);
    }

    void initObject()
    {
        checkTheMatch = GetComponent<CheckTheMatch>();
        checkTheMatch.init(grid);
        mouseInput = GetComponent<MouseInput>();
        mouseInput.init(grid);
        drawtheBoard = GetComponent<DrawTheBoard>();
        drawtheBoard.init(grid);
    }

    void boardInit()
    {
        for (int i = 0; i <= n; i++)
        {
            for (int j = 0; j <= m; j++)
            {
                grid[i, j] = basicBlockPool.GetObject();
                var nowBlock = grid[i, j];
                nowBlock.init(grid, i, j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isLocked = true;
            Utilities.ChangeBlock(grid, grid[1, 1], ribbonBlockPool.GetObject());
            Utilities.ChangeBlock(grid, grid[1, 2], ribbonBlockPool.GetObject());
            Utilities.ChangeBlock(grid, grid[1, 3], ribbonBlockPool.GetObject());
            Utilities.ChangeBlock(grid, grid[1, 4], ribbonBlockPool.GetObject());
            Utilities.ChangeBlock(grid, grid[1, 5], ribbonBlockPool.GetObject());
            Utilities.ChangeBlock(grid, grid[1, 6], ribbonBlockPool.GetObject());
            Utilities.ChangeBlock(grid, grid[1, 7], ribbonBlockPool.GetObject());


        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Utilities.ChangeBlock(grid, grid[1, 2], ribbonBlockPool.GetObject());


        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Utilities.ChangeBlock(grid, grid[2, 3], ribbonBlockPool.GetObject());
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Utilities.ChangeBlock(grid, grid[1, 4], ribbonBlockPool.GetObject());
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Utilities.ChangeBlock(grid, grid[1, 5], ribbonBlockPool.GetObject());
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
        for (int i = 1; i < n; i++)
        {
            for (int j = 1; j < m; j++)
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
        ////삭제 애니
        if (isLocked) //지금 재정비중이면 이거x
            return;

        for (int i = 1; i < n; i++)
        {
            for (int j = 1; j < m; j++)
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
        ////삭제 애니
        if (isLocked) //지금 재정비중이면 이거x
            return;

        for (int i = 1; i < n; i++)
        {
            for (int j = 1; j < m; j++)
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
        //스코어업
        bool match = false;
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
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

        //두번 쨰 스왑때 매칭이 안됐을 시
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

        for (int i = n-1; i > 0; i--)
        {
            for (int j = 1; j < m; j++)
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

        for (int j = 1; j < m; j++)
        {
            for (int i = n-1, num = 0; i > 0; i--)
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
