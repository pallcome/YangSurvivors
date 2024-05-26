using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	[Header("# Game Control")]
	public bool isLive; // 게임이 일시정시 됐는가?
	public float gameTime;
	public float maxGameTime = 2 * 10f;
    [Header("# Player Info")]
    public int playerId;
	public float health;
	public float maxHealth = 100;
	public int level;
	public int kill;
	public int exp;
	public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
	[Header("# Game Object")]
	public PoolManager pool;
	public Player player;
	public LevelUp uiLevelUp;
    public Result uiResult;
    public Transform uiJoy;
    public GameObject enemyCleaner;

	void Awake()
	{
		instance = this;
        Application.targetFrameRate = 60; // Frame 지정
	}

	public void GameStart(int id)
	{
        playerId = id;
		health = maxHealth;

        player.gameObject.SetActive(true);
		uiLevelUp.Select(playerId % 2);
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }
    public void GameRetry()
	{
        SceneManager.LoadScene(0);
	}

    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
	{
		if (!isLive)
		{
			return;
		}

		gameTime += Time.deltaTime;

		if(gameTime > maxGameTime)
		{
			gameTime = maxGameTime;
            GameVictory();
		}
	}

	public void GetExp()
	{
        if(!isLive)
        {
            return;
        }

		exp++;

		if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)]) // 설정한 최대레벨을 초과할경우 설정한 레벨만 나오도록
		{
			level++;
			exp = 0;
			uiLevelUp.Show();
		}
	}

	public void Stop()
	{
		isLive = false;
		Time.timeScale = 0; // 유니티의 시간 속도(배율)
        uiJoy.localScale = Vector3.zero;
	}
	public void Resume()
	{
		isLive = true;
		Time.timeScale = 1; // 유니티의 시간 속도(배율), 기본1, 높을수록 빨라짐
        uiJoy.localScale = Vector3.one;
    }
}
