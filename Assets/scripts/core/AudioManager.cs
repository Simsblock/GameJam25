using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header("------- Audio Source --------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource TimerSFX;

    [Header("------- Backround Music --------")]
    public AudioClip casinoBackround;
    public AudioClip shopBackround;

    [Header("------- Audio Clip --------")]
    public AudioClip buttonClickSound;
    public AudioClip winSound;

    public AudioClip rollDice;
    public AudioClip timer;
    public AudioClip alarm;

    public AudioClip gainMoney;
    public AudioClip loseMoney;
    public AudioClip rollingWheels;
    public AudioClip changeEnemy;

    [Header("------- Enemy Laugh --------")]
    public AudioClip DevilLaugh;
    public AudioClip DinoLaugh;
    public AudioClip DragonLaugh;
    public AudioClip EvilLaugh;


    [Header("------- Normal Cards --------")]
    public AudioClip shuffleCards;
    public AudioClip drawCard;
    public AudioClip drawCard1;
    public AudioClip drawCard2;
    public AudioClip drawCard3;
    public AudioClip drawCard4;
    public AudioClip placeCard;




    [Header("------- Magic Cards Sounds --------")]

    public AudioClip BuyingMagicCard;
    public AudioClip BuyingMagicCard2;
    public AudioClip BuyingMagicCard3;

    public AudioClip PlayingMagicCard;
    public AudioClip PlayingMagicCard2;
    public AudioClip PlayingMagicCard3;

    private void Start()
    {
        musicSource.clip = shopBackround;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    public void ChangeBGMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    
    public void StartTimerAudio()
    {
        TimerSFX.clip = timer;
        TimerSFX.Play();
    }
    public void StopTimerSFX()
    {
        TimerSFX.Stop();
    }
}