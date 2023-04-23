using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameEnd;

    private Text goal_text;
    private Text move_text;
    int move = 18;
    int goal = 5;

    [SerializeField]
    private GameObject stageClearImage;
    [SerializeField]
    private GameObject stageFaiImage;

    private static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {
        isGameEnd = false;
        goal_text = GameObject.Find("GoalText").GetComponent<Text>();
        goal_text.text = goal.ToString();

        move_text = GameObject.Find("MoveText").GetComponent<Text>();
        move_text.text = move.ToString();
    }

    public void goalProgress()
    {
        if (goal <= 0)
            return;

        goal--;
        goal_text.text = goal.ToString();

        if (goal == 0)
            stageClear();
    }

    public void moveProgress()
    {
        if (move <= 0)
            return;

        move--;
        move_text.text = move.ToString();

        if (move == 0)
            stageFail();
    }

    void stageClear()
    {
        isGameEnd = true;
        stageClearImage.SetActive(true);
    }

    void stageFail()
    {
        stageFaiImage.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

}
