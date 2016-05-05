using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameControlScript : MonoBehaviour 
{
	public static GameControlScript current;	// 当前控制全局对象
	public GUIText scoreText;					// 分数label
	public GameObject gameOvertext;				// 结束文字
	int score = 0;								// 角色的分数
    public GameObject clown;                    // 玩家对象
    public int gameLevel;                       // 游戏难度级别

    public GUIText levelText;   // 当前难度

	public bool isGameOver { get; set; }	    //游戏结束了没

	void Awake()
	{
        if (current == null)
        {
            current = this;
        }
        else if (current != this)
        {
			Destroy (gameObject);
        }

        gameLevel = 1;
	}

	void Update()
	{
		if (isGameOver && Input.anyKey) 
		{
            // 重玩
            SceneManager.LoadScene("Main");
		}

        levelText.text = "level:" + gameLevel.ToString();
    }

	public void BirdScored()
	{
		//the bird can't score if the game is over
		if (isGameOver)	
			return;
		//increase score
		score++;
		//adjust the score text
		scoreText.text = "Score: " + score;
	}

	public void BirdDied()
	{
		//show the game over text
		gameOvertext.SetActive (true);
		//set the game to be over
		isGameOver = true;
	}
}
