using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTIme;
    public float maxGameTime = 2 * 10f;

    public PoolManager pool;
    public Player player;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        gameTIme += Time.deltaTime;

        if(gameTIme > maxGameTime)
        {
            gameTIme = maxGameTime;
        }
    }
}
