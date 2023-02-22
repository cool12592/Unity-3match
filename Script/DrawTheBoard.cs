using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTheBoard : MonoBehaviour
{
    BasicBlock[,] grid;
    
    [SerializeField]
    private Sprite[] blockSprites = new Sprite[4];
    [SerializeField]
    private Sprite[] itemBlockSprites = new Sprite[6];

    public void init(BasicBlock[,] grid_)
    {
        grid = grid_;
    }


    public void drawBoard()
    {
        for (int i = 1; i < ExecuteLogic.n; i++)
        {
            for (int j = 1; j < ExecuteLogic.m; j++)
            {
                setAlpha(grid[i, j]);
                setSprite(grid[i, j], grid[i, j].kind);
                setPosition(grid[i, j]);
            }
        }
    }

    void setPosition(BasicBlock go)
    {
        float z = 0f;

        if (go.kind == 4)
            z = -1f;

        if (go.y > 0f)
            z = 1000f;


        go.GetComponent<Transform>().position = new Vector3(go.x, go.y, z);

    }

    void setSprite(BasicBlock go, int kind)
    {
        var spriteRenderer_ = go.GetComponent<SpriteRenderer>();

        switch (kind)
        {
            case 0:
                if (go is RibbonBlock)
                    spriteRenderer_.sprite = itemBlockSprites[0];
                else
                    spriteRenderer_.sprite = blockSprites[0];
                break;
            case 1:
                if (go is RibbonBlock)
                    spriteRenderer_.sprite = itemBlockSprites[1];
                else
                    spriteRenderer_.sprite = blockSprites[1];
                break;
            case 2:
                if (go is RibbonBlock)
                    spriteRenderer_.sprite = itemBlockSprites[2];
                else
                    spriteRenderer_.sprite = blockSprites[2];
                break;
            case 3:
                if (go is RibbonBlock)
                    spriteRenderer_.sprite = itemBlockSprites[3];
                else
                    spriteRenderer_.sprite = blockSprites[3];
                break;
            case 4:
                spriteRenderer_.sprite = itemBlockSprites[4];
                break;
            case 5:
                spriteRenderer_.sprite = itemBlockSprites[5];
                break;
            default:
                break;
        }
    }

    void setAlpha(BasicBlock go)
    {
        var spriteRenderer_ = go.GetComponent<SpriteRenderer>();

        go.alpha = Mathf.Clamp(go.alpha, 0f, 1f);

        spriteRenderer_.color = new Color(spriteRenderer_.color.r, spriteRenderer_.color.g, spriteRenderer_.color.b, go.alpha);

    }


}
