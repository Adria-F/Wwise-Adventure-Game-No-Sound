////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultSpellcraft : MonoBehaviour
{
    [System.Serializable]
    public class ChargeInfo
    {
        public List<Spell> OnCharge;
        public float ChargeAmount = 0f;
    }

    [System.Serializable]
    public class ReleaseInfo
    {
        public List<Spell> OnRelease;
    }

    [System.Serializable]
    public class SpellDesigns
    {
        public string spellName;
        public ChargeInfo Charge;
        public ReleaseInfo Release;
        public float MaxDamage = 10f;
        public float ImpactVelocity = 0f;
        public bool IsAvailable;
    }
    public List<SpellDesigns> Spellcraft;

    public int SpellSelect = 0;

    [Header("Sounds")]
    public AudioClip charge_end;
    public AudioClip charge_loop;
    public AudioClip spellCast;

    [Header("WWISE")]
    public float SpellChargeLevel = 0f;

    #region private variables
    private Quaternion startRotation;
    private Vector3 targetPosition;
    private float startVolume = 0.0f;
    //Chached animator hashes
    private readonly int canShootMagicHash = Animator.StringToHash("CanShootMagic");
    private readonly int shootMagicHash = Animator.StringToHash("ShootMagic");
    private readonly int chargeMagicHash = Animator.StringToHash("ChargeMagic");
    private readonly int chargingMagicHash = Animator.StringToHash("ChargingMagic");
    #endregion

    public void EnableMagic()
    {

        SpellChargeLevel = 0f;
        // HINT: Spell starts charging, you may want to play the appropiate sound effect here

        InputManager.OnUseDown += OnCharge;
        InputManager.OnUseUp += OffCharge;
    }

    public void DisableMagic()
    {
        // HINT: Spell stops charging, you may want to stop the charge effect here       
        Spellcraft[SpellSelect].Charge.OnCharge[0].Deactivate();
        InputManager.OnUseDown -= OnCharge;
        InputManager.OnUseUp -= OffCharge;
    }

    private void OnDestroy()
    {
        DisableMagic();
    }

    void OnCharge()
    {
        PlayerManager.Instance.playerAnimator.SetBool(canShootMagicHash, false);
        // Activate charges
        if (Spellcraft[SpellSelect].IsAvailable)
        {
            PlayerManager.Instance.PauseMovement(this.gameObject);
            PlayerManager.Instance.PauseAttacking(this.gameObject);
            PlayerManager.Instance.playerAnimator.ResetTrigger(shootMagicHash);
            PlayerManager.Instance.playerAnimator.SetTrigger(chargeMagicHash);
            PlayerManager.Instance.playerAnimator.SetBool(chargingMagicHash, true);
            PlayerManager.Instance.playerAnimator.SetBool(canShootMagicHash, false);
            for (int s = 0; s < Spellcraft[SpellSelect].Charge.OnCharge.Count; s++)
            {
                Spellcraft[SpellSelect].Charge.OnCharge[s].Activate();
            }

            // SPELL SOUND
            // HINT: Spell charge start sound effect should be played here
            AudioSource audioSource = transform.parent.gameObject.GetComponent<AudioSource>();
            startVolume = audioSource.volume;
            audioSource.volume = 0.0f;
            audioSource.loop = true;
            audioSource.PlayOneShot(charge_loop);
            startRotation = transform.rotation;
        }
    }

    void OnCharging(float playbacktime)
    {
        Vector3 targetDir = targetPosition - transform.position;
        Quaternion LRotation = Quaternion.LookRotation(targetDir);

        float s = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f).Evaluate(playbacktime);
        PlayerManager.Instance.player.transform.rotation = Quaternion.Lerp(startRotation, LRotation, s);
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
    }

    void FixedUpdate()
    {
        if (PlayerManager.Instance != null && Spellcraft.Count > 0)
        {
            if (Spellcraft[SpellSelect].IsAvailable && PlayerManager.Instance.playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("ChargeMagic_wEvents") && !PlayerManager.Instance.playerAnimator.GetBool("CanShootMagic"))
            {
                AnimatorStateInfo currentState = PlayerManager.Instance.playerAnimator.GetCurrentAnimatorStateInfo(0);
                float playbackTime = currentState.normalizedTime % 1;
                UpdateCharge(playbackTime);
                OnCharging(playbackTime);
            }
            else if (Spellcraft[SpellSelect].IsAvailable && PlayerManager.Instance.playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("ReadyToShootMagic"))
            {
                UpdateCharge(1f);
                if (!updatedSpellReady)
                {
                    EventManager.SpellReady();
                    updatedSpellReady = true;
                }
                updatedSpellNotReady = false;
            }
            else
            {
                UpdateCharge(0f);
                if (!updatedSpellNotReady)
                {
                    EventManager.SpellNotReady();
                    updatedSpellNotReady = true;
                }
                updatedSpellReady = false;
            }
        }

    }

    bool updatedSpellNotReady;
    bool updatedSpellReady;

    public void UpdateCharge(float charge)
    {
        Spellcraft[SpellSelect].Charge.ChargeAmount = charge;
        SpellChargeLevel = Spellcraft[SpellSelect].Charge.ChargeAmount * 100;
        // HINT: You might want to take the spell charge level change into account to modify the spell charge sound appropiately
        AudioSource audioSource = transform.parent.gameObject.GetComponent<AudioSource>();
        if (charge > 0.0f && audioSource.loop)//Check that it is charging
        {            
            audioSource.volume = startVolume * charge;
        }
    }

    void OffCharge()
    {
        PlayerManager.Instance.ResumeAttacking(this.gameObject);
        PlayerManager.Instance.ResumeMovement(this.gameObject);
        AudioSource audioSource = transform.parent.gameObject.GetComponent<AudioSource>();
        audioSource.volume = startVolume;
        audioSource.loop = false;
        audioSource.Stop();
        if (Spellcraft[SpellSelect].IsAvailable)
        {
            PlayerManager.Instance.playerAnimator.ResetTrigger(chargeMagicHash);
            PlayerManager.Instance.playerAnimator.SetTrigger(shootMagicHash);
            PlayerManager.Instance.playerAnimator.SetBool(chargingMagicHash, false);

            if (PlayerManager.Instance.playerAnimator.GetBool(canShootMagicHash))
            {
                // HINT: Spell charge stop sound effect should be played here               
                audioSource.PlayOneShot(charge_end);
                // Activate Spells
                PlayerManager.Instance.playerAnimator.SetBool(canShootMagicHash, false);
                for (int R = 0; R < Spellcraft[SpellSelect].Release.OnRelease.Count; R++)
                {
                    Spellcraft[SpellSelect].Release.OnRelease[R].ChargeValue = Spellcraft[SpellSelect].Charge.OnCharge[0].ChargeValue;
                    Spellcraft[SpellSelect].Release.OnRelease[R].ImpactVel = Spellcraft[SpellSelect].ImpactVelocity;
                    Spellcraft[SpellSelect].Release.OnRelease[R].Damage = Spellcraft[SpellSelect].MaxDamage;
                    Spellcraft[SpellSelect].Release.OnRelease[R].Activate();

                    // SPELL SOUND
                    SpellChargeLevel = Spellcraft[SpellSelect].Charge.ChargeAmount * 100;
                    // HINT: Spell is casted, you might want to play the appropiate sound here
                    audioSource.PlayOneShot(spellCast);
                }
            }
        }

        // Deactivate Charges
        for (int s = 0; s < Spellcraft[SpellSelect].Charge.OnCharge.Count; s++)
        {
            Spellcraft[SpellSelect].Charge.OnCharge[s].Deactivate();
        }
    }

}
