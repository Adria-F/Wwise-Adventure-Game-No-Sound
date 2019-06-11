////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Audio;

public delegate void dayNightPush(bool condition);
public delegate void OnMusic();

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    [Header("Wwise")]
    public float OnMenu = 0f; // RTPC
    //public AK.Wwise.State MusicStart_Region = new AK.Wwise.State();
    //public AK.Wwise.Trigger EnemyMusicTrigger = new AK.Wwise.Trigger();

    [Space(10f)]

    public float EvilCrawlerRTPC = 0f;
    public float EvilHeadRTPC = 0f;
    public float EvilSpitPlantRTPC = 0f;
    public float CombatLevelRTPC = 0f;
    public float proximityThreshold = 10f;

    [Space(10f)]

    [Header("Coins")]
    public CoinHandler coinHandler;

    [Header("Game Speed")]
    public GameSpeedHandler gameSpeedHandler;

    [Header("DLC Management")]
    public List<DLC> activeDLCs;

    public static float TimeOfDay = 0;
    public static void SetTimeOfDay(float t) {
        TimeOfDay = t;
    }

    public Vector2 DayAndNightChange;
    public bool dayTime = false;
    private bool lastDayTime = false;

    public static event dayNightPush DayNightCall;
    public static event OnMusic OnMusicAction;

    #region private variables
    private bool overrideEnemyRTPCs = false;

    //Camera Effects
    private Camera ActiveCamera;
    private BlurOptimized blur;
    private IEnumerator blurRoutine;
    private bool fadingout = false;
    #endregion

    [HideInInspector]
    GameObject MusicGameObject;
    AudioSource G_M_audio;
    AudioSource audio_source_Music;

    public AudioClip defaultDay;
    public AudioClip defaultNight;
    public AudioClip defaultAmbientMix;

    [Header("CHEATS")]
    public bool BigHeadMode;
    public bool AIPaused;
    [Header("MODES")]
    public bool DisableWwizardMagicStateOnStart = true;

    /// <summary>
    /// Used with dynamic bools from UnityEvents on the UI
    /// </summary>
    /// <param name="on">If set to <c>true</c> on.</param>
    public void SetGodMode(bool on)
    {
        PlayerManager.Instance.Immortal = on;
    }

    /// <summary>
    /// Used with dynamic bools from UnityEvents on the UI
    /// </summary>
    /// <param name="on">If set to <c>true</c> on.</param>
    public void SetBigHeadMode(bool on)
    {
        BigHeadMode = on;
    }

    /// <summary>
    /// Function meant to be called from a Callback function. This will push all RhythmActions scripts subscribed to the OnMusicAction OnMusic type Event. 
    /// </summary>
    public static void PushRhythmAction() {
        // If nothing is subscribed to it, don't call. 
        if (OnMusicAction != null) {
            OnMusicAction();
        }
    }

    void Start()
    {
        if (DisableWwizardMagicStateOnStart) {
            // HINT: New context state: MagicZone, Outside
        }

        //MusicGameObject = GameObject.Find("Ak_PlayMusic");
        MusicGameObject = GameObject.FindGameObjectWithTag("SoundZones");
        G_M_audio = MusicGameObject.GetComponent<AudioSource>();
        G_M_audio.loop = true;

        audio_source_Music = GameObject.FindGameObjectWithTag("MusicZones").GetComponent<AudioSource>();
        audio_source_Music.loop = true;

        audio_source_Music.outputAudioMixerGroup = (AudioMixerGroup)FindObjectOfType(typeof(AudioMixerGroup));
        
        DayNightCall += dayNightPush;
        //MusicStart_Region.SetValue();

        //AkCallbackType CallbackType = AkCallbackType.AK_MusicSyncUserCue;
        //MusicEvent.Post(gameObject, (uint)CallbackType, CallBackFunction);
        // HINT: Place to update game music according to region and daylight variables

        UpdateMusic();

        StartCoroutine(DistanceToEnemies());
        // HINT: Enemy music event?
    }


    void Awake()
    {
        ActiveCamera = Camera.main;
        if (ActiveCamera != null)
        {
            if (ActiveCamera.GetComponent<BlurOptimized>() != null)
            {
                blur = ActiveCamera.GetComponent<BlurOptimized>();
            }
            else
            {
                BlurOptimized b = ActiveCamera.gameObject.AddComponent<BlurOptimized>();
                blur = b;
            }
            blur.blurIterations = 1;
            blur.enabled = false;
        }
    }

    private void OnDisable()
    {
        // HINT: Enemy music event stop
        //EnemyMusicEvent.Stop(gameObject);
    }

    void Update()
    {
        // ------------------------ DAY / NIGHT CYCLE
        if (TimeOfDay < DayAndNightChange.x || TimeOfDay > DayAndNightChange.y)
        {
            dayTime = false;
        }
        else if (TimeOfDay > DayAndNightChange.x && TimeOfDay < DayAndNightChange.y)
        {
            dayTime = true;
        }

        if (dayTime != lastDayTime)
        {
            DayNightCall(dayTime);
            lastDayTime = dayTime;
        }
    }

    public void BlurCam()
    {
        if (blur != null)
        {
            if (blurRoutine != null)
            {
                StopCoroutine(blurRoutine);
            }
            blurRoutine = Blur(4f, 1f);
            StartCoroutine(blurRoutine);
        }
    }
    public void UnBlurCam()
    {
        if (blur != null)
        {
            if (blurRoutine != null)
            {
                StopCoroutine(blurRoutine);
            }
            blurRoutine = Blur(0f, 0.15f);
            StartCoroutine(blurRoutine);
        }
    }

    IEnumerator Blur(float target, float seconds)
    {
        if (blur.blurSize == target)
        {
            yield break;
        }

        blur.enabled = true;
        float currentBlur = blur.blurSize;

        for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / seconds)
        {
            blur.blurSize = Mathf.Lerp(currentBlur, target, t);
            yield return null;
        }
        blur.blurSize = target;
        blur.enabled = target == 0f ? false : true;
    }

    void dayNightPush(bool condition) { }

    [Header("Zones")]
    public List<ZoneTrigger> CurrentZones = new List<ZoneTrigger>();
    public List<Creature> Enemies = new List<Creature>();



    public void LeaveZone(ZoneTrigger Z)
    {
        StartCoroutine("FadeOut", Z);
    }

    IEnumerator FadeOut(ZoneTrigger Z)
    {
        float startTime = Time.time;        
        while (true)
        {
            fadingout = true;
            float elapsed = Time.time - startTime;

            audio_source_Music.volume = Mathf.Clamp01(Mathf.Lerp(audio_source_Music.volume, 0, elapsed / 5.0f));

            if (audio_source_Music.volume == 0)
            {
                fadingout = false;
                CurrentZones.Remove(Z);
                UpdateMusic();
                break;
            }

            yield return null;
        }

    }

    IEnumerator FadeIn(ZoneTrigger Z)
    {
        yield return new WaitUntil(() => audio_source_Music.volume == 0);

        CurrentZones.Insert(0, Z);
        UpdateMusic();
        float startTime = Time.time;

        while (true)
        {
            float elapsed = Time.time - startTime;

            audio_source_Music.volume = Mathf.Clamp01(Mathf.Lerp(0.0f, 0.5f, elapsed / 5.0f));

            if (audio_source_Music.volume == 0.5f)
            {

                break;
            }

            yield return null;
        }
        
    }

    public void EnterZone(ZoneTrigger Z)
    {
        StartCoroutine("FadeIn", Z);
    }

    void UpdateMusic()
    {
        Debug.Log(CurrentZones.Count);
        if (CurrentZones.Count > 0)
        {            
            // HINT: Place to update game music according to region and daylight variable
            if(dayTime)
            {
                G_M_audio.clip = CurrentZones[0].music_Day;
                audio_source_Music.clip = CurrentZones[0].musicMix_Day;
            }
            else
            {
                G_M_audio.clip = CurrentZones[0].music_Night;
                audio_source_Music.clip = CurrentZones[0].musicMix_Night;
            }

        }
        else
        {
            // HINT: Place to update game music according to region and daylight variables
            if (dayTime)
            {
                G_M_audio.clip = defaultDay;
                
            }
            else
            {
                G_M_audio.clip = defaultNight;
            }
            audio_source_Music.clip = defaultAmbientMix;
        }

        G_M_audio.Play();
        G_M_audio.loop = true;

        audio_source_Music.Play();
        audio_source_Music.loop = true;
    }

    bool CanPostEnemyMusic = true;
    IEnumerator EnemyMusicFailsaferoutine() {
        CanPostEnemyMusic = false;
        while (true) {
            yield return new WaitForSeconds(1f);
            CanPostEnemyMusic = true;
        }
    }

    /*public void CallBackFunction(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_MusicSyncUserCue)
        {
            if (CanPostEnemyMusic) {
                EnemyMusicEvent.Post(gameObject);
                StartCoroutine(EnemyMusicFailsaferoutine());
            }
            
        }
    }*/

    public void SetInCombat(Creature G)
    {
        if (!Enemies.Contains(G))
        {
            Enemies.Add(G);
        }
    }

    [SerializeField, Range(0f, 1f)]
    private float EvilHeadValue = 1f;
    [SerializeField, Range(0f, 1f)]
    private float EvilSpitPlantValue = 1f;
    [SerializeField, Range(0f, 1f)]
    private float EvilCrawlerValue = 1f;

    IEnumerator DistanceToEnemies()
    {
        while (true)
        {
            float combatLevel = Enemies.Count;

            if (!overrideEnemyRTPCs)
            {
                EvilHeadValue = EvilCrawlerValue = EvilSpitPlantValue = 1f;

                if (Enemies.Count > 0)
                {
                    Transform playerTransform = PlayerManager.Instance.playerTransform;
                    if (Enemies.Count > 2)
                    {
                        Enemies.Sort((Creature a, Creature b) => Vector3.SqrMagnitude(playerTransform.position - a.transform.position).CompareTo(Vector3.SqrMagnitude(playerTransform.position - b.transform.position)));
                    }

                    Creature closestEvilHead = Enemies.FirstOrDefault(x => x.AttackType == WeaponTypes.EvilHead);
                    Creature closestEvilSpitPlant = Enemies.FirstOrDefault(x => x.AttackType == WeaponTypes.EvilSpitPlant);
                    Creature closestEvilCrawler = Enemies.FirstOrDefault(x => x.AttackType == WeaponTypes.EvilCrawler);

                    EvilHeadValue = Mathf.Clamp01(closestEvilHead != null ? (Vector3.SqrMagnitude(closestEvilHead.transform.position - playerTransform.position) / (proximityThreshold * proximityThreshold)) : 1f);
                    EvilSpitPlantValue = Mathf.Clamp01(closestEvilSpitPlant != null ? (Vector3.SqrMagnitude(closestEvilSpitPlant.transform.position - playerTransform.position) / (proximityThreshold * proximityThreshold)) : 1f);
                    EvilCrawlerValue = Mathf.Clamp01(closestEvilCrawler != null ? (Vector3.SqrMagnitude(closestEvilCrawler.transform.position - playerTransform.position) / (proximityThreshold * proximityThreshold)) : 1f);

                }
            }
			EvilHeadRTPC = EvilHeadValue * 100f;
			EvilCrawlerRTPC = EvilCrawlerValue * 100f;
			EvilSpitPlantRTPC = EvilSpitPlantValue * 100f;
            CombatLevelRTPC = combatLevel;
            // HINT: You may want to reflect something in game sound after updating the previous RTPC values

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OverrideEnemyRTPCValues(bool enableOverride, float value = 100f)
    {
        overrideEnemyRTPCs = enableOverride;

        if (enableOverride)
        {
            EvilHeadValue = EvilCrawlerValue = EvilSpitPlantValue = value / 100f;
        }
    }

    public void RemoveFromCombat(Creature G)
    {
        if (Enemies.Contains(G))
        {
            bool removed = Enemies.Remove(G);
            if (!removed)
            {
                print("Was not able to remove " + G.name);
            }
        }
    }

    public float GetTimeOfDayParameter() {
        return TimeOfDay;
    }


    public static void DamageObject(GameObject objectToDamage, Attack attack)
    {
        IDamageable[] damageables = objectToDamage.gameObject.GetComponentsInChildren<IDamageable>();
        for (int i = 0; i < damageables.Length; i++)
        {
            damageables[i].OnDamage(attack);
        }
    }

    public static void InteractWithObject(GameObject objectToInteractWith)
    {
        IInteractable[] interactables = objectToInteractWith.gameObject.GetComponentsInChildren<IInteractable>();

        for (int i = 0; i < interactables.Length; i++)
        {
            interactables[i].OnInteract();
        }
    }



}