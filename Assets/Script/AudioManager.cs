using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")] // SFX : 효과음
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels; // 당량의 효과음을 낼 수 있도록 채널 개수 선언
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx {Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win};


    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false; // 시작 시 재생여부
        bgmPlayer.loop = true; // 반복
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int index=0; index<channels; index++)
        {
            AudioSource sfx = sfxObject.AddComponent<AudioSource>();
            sfx.playOnAwake = false;
            sfx.volume = sfxVolume;
            sfx.bypassListenerEffects = true;
            sfxPlayers[index] = sfx;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if(isPlay)
        {
            bgmPlayer.Play();
        }else
        {
            bgmPlayer.Stop();
        }
    }
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int index=0; index<sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) {
                continue;
            }

            int randIndex = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                randIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + randIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
