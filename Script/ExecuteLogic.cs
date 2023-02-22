using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class ExecuteLogic : MonoBehaviour
{
    public static readonly int n = 8, m = 8;
    public static float tileSize; //타일 사이즈
    
    BasicBlock[,] grid = new BasicBlock[n+1,m+1];
    public static bool isSwap = false, isMoving = false, isStop = false;

    [SerializeField]
    private BasicBlock blockPrefab1, blockPrefab2, blockPrefab3, blockPrefab4;

    [SerializeField]
    static private Text goal_text;
    static int goal =3;

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

        goal_text = GameObject.Find("GoalText").GetComponent<Text>();
        goal_text.text = goal.ToString();

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

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Utilities.ChangeBlock(grid, grid[6, 2], rainBowBlockPool.GetObject());


        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Utilities.ChangeBlock(grid, grid[7, 2], rainBowBlockPool.GetObject());

            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Utilities.ChangeBlock(grid, grid[4, 2], ribbonBlockPool.GetObject());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            grid[6, 2].tryToErase();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            
        }



        mouseInput.updateMouseInput();
        
        if (isMoving == false )
        {
            checkTheMatch.checkMatch();
            checkTheMatch.checkItemMatch();
        }
        
        moveBlock();
        eraseInProgress();
        itemTime();

        undoNotMatchingBlock();
        orderBoard();
        drawtheBoard.drawBoard();

    }

    void boardInit()
    {
        for (int i = 0; i <= n; i++)
        {
            for (int j = 0; j <= m; j++)
            {
                grid[i, j] = basicBlockPool.GetObject(); 
                var nowBlock = grid[i, j];

                nowBlock.init(grid , i ,j);


               

               // setSprite(nowBlock, nowBlock.kind);
               // setPosition(nowBlock);
            }
        }
    }
    
   
    void moveBlock()
    {
        if (isStop)
            return;

        //////무빙 
        isMoving = false;
        for (int i = 1; i < n; i++)
        {
            for (int j = 1; j < m; j++)
            {
                var nowBlock = grid[i, j];
                if (nowBlock.moveToDest())
                {
                    isMoving = true;
                }

            }
        }
    }


    void checkErasePrgress()
    {
        ////삭제 애니
        if (!isMoving) //지금 재정비중이면 이거x
            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < m; j++)
                {
                    if (grid[i, j].match == true)
                    {
                        isMoving = true;
                        return;
                    }
                    else
                    {
                        if (grid[i, j].alpha < 1f)
                        {
                            isMoving = true;
                            return;
                        }
                    }
                }
            }
    }

    void eraseInProgress()
    {
        ////삭제 애니
        if (!isMoving) //지금 재정비중이면 이거x
            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < m; j++)
                {
                    if (grid[i, j].match == true)
                    {
                        if (grid[i, j].alpha > 0f)
                        {
                            grid[i, j].alpha -= Time.deltaTime;
                            isMoving = true;
                        }
                    }
                    else
                    {
                        if (grid[i, j].alpha < 1f)
                        {
                            isMoving = true;
                            grid[i, j].alpha += Time.deltaTime;
                        }
                    }
                }
            }
    }

    void itemTime()
    {
        ////삭제 애니
        if (!isMoving) //지금 재정비중이면 이거x
            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < m; j++)
                {
                    if (grid[i, j].itemOn == true)
                    {
                        if(grid[i, j].useItem())
                            isMoving = true;
                    }
                }
            }
    }

    void undoNotMatchingBlock()
    {
        //스코어업
        int score = 0;
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                if (grid[i, j].match)
                    score += 1;
            }
        }

        //두번 쨰 스왑때 매칭이 안됐을 시
        if (isSwap && !isMoving)
        {
            if (score == 0)
            {
                Utilities.swap(grid, Utilities.clickBlock1, Utilities.clickBlock2);
            }

            isSwap = false;
        }
    }

    void orderBoard()
    {
        if (isMoving)
            return;

        for (int i = n-1; i > 0; i--)
        {
            for (int j = 1; j < m; j++)
            {
                if (grid[i, j].match)
                {
                    grid[i, j].col = j;
                    grid[i, j].row = i;

                    isMoving = true;
                    UpAndDown(i, j);
                }
            }
        }

        for (int j = 1; j < m; j++)
        {
            for (int i = n-1, num = 0; i > 0; i--)
            {
                if (grid[i, j].match)
                {
                    isMoving = true;
                    grid[i, j].reInit(ref num);
                }
            }
        }


    }

    void UpAndDown(int i, int j)
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

    public static void goalProgress()
    {
        goal--;
        goal_text.text = goal.ToString();
    }

}
