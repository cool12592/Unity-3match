using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    
    int x1, y1, x2, y2;
    bool isMousePressed = false;
    Vector2 pos, offset;
    BasicBlock[,] grid;

    public void init(BasicBlock[,] grid_)
    {
        var pos1 = Utilities.startPoint1.position;
        var pos2 = Utilities.startPoint2.position;
        offset = new Vector2(Mathf.Abs(pos1.x), Mathf.Abs(pos1.y));

        grid = grid_;
    }

    public void updateMouseInput()
    {
        if (ExecuteLogic.isSwap || ExecuteLogic.isLocked)
            return;
        mouseClick();
        mouseDrag();
    }

    void mouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // offset/2를 빼주는 이유는 보드의 왼쪽상단 좌표를 0,0으로 맞춰주기 위함
            pos = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition)) - offset / 2f;
            x1 = (int)(pos.x / ExecuteLogic.tileSize) + 1;
            y1 = (int)(-pos.y / ExecuteLogic.tileSize);

            if (Utilities.checkBoardRange(x1, y1))
            {
                Utilities.clickBlock1 = grid[y1, x1];
                isMousePressed = true;
            }
        }
    }

    void mouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            if (isMousePressed == false)
                return;

            pos = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition)) - offset / 2f;
            x2 = (int)(pos.x / ExecuteLogic.tileSize) + 1;
            y2 = (int)(-pos.y / ExecuteLogic.tileSize);

            if (Utilities.checkBoardRange(x2, y2) == false)
            {
                return;
            }

            float x_dis = x1 - x2;
            float y_dis = y1 - y2;

            if (Mathf.Abs(x_dis) >= 1f || Mathf.Abs(y_dis) >= 1f)
                isMousePressed = false;

            if (x_dis >= 1f)
            {
                Utilities.swapMove(grid, Direction.LEFT);
            }
            else if (x_dis <= -1f)
            {
                Utilities.swapMove(grid, Direction.RIGHT);
            }
            else if (y_dis >= 1f)
            {
                Utilities.swapMove(grid, Direction.UP);
            }
            else if (y_dis <= -1f)
            {
                Utilities.swapMove(grid, Direction.DOWN);
            }
        }
    }
}
