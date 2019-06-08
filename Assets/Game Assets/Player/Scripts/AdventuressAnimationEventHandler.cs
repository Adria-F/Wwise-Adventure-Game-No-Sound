////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class AdventuressAnimationEventHandler : MonoBehaviour
{
    [Header("Step Sounds")]
    public AudioClip[] dirtWalk;
    public AudioClip[] dirtRun;
    public AudioClip[] grassWalk;
    public AudioClip[] grassRun;
    public AudioClip[] rubbleWalk;
    public AudioClip[] rubbleRun;
    public AudioClip[] sandWalk;
    public AudioClip[] sandRun;
    public AudioClip[] stoneWalk;
    public AudioClip[] stoneRun;
    public AudioClip[] waterWalk;
    public AudioClip[] waterRun;
    public AudioClip[] woodWalk;
    public AudioClip[] woodRun;

    [Header("Pickup sounds")]
    public AudioClip book;
    public AudioClip evilEssence;
    public AudioClip mushroom;
    public AudioClip crystalShard;
    public AudioClip pinecone;
    public AudioClip key;
    public AudioClip scroll;
    public AudioClip sword;
    public AudioClip axe;
    public AudioClip pickaxe;
    public AudioClip hammer;
    public AudioClip swing1;
    public AudioClip swing2;
    public AudioClip swing3;

    [Header("Death sounds")]
    public AudioClip[] deathSounds;

    [Header("Object Links")]
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private GameObject runParticles;

    private PlayerFoot foot_L;
    private PlayerFoot foot_R;

    #region private variables
    private bool hasPausedMovement;
    private readonly int canShootMagicHash = Animator.StringToHash("CanShootMagic");
    private readonly int isAttackingHash = Animator.StringToHash("IsAttacking");
    #endregion

    private void Awake()
    {
        GameObject L = GameObject.Find("toe_left");
        GameObject R = GameObject.Find("toe_right");
        if (L != null)
        {
            foot_L = L.GetComponent<PlayerFoot>();
        }
        else {
            print("Left foot missing");
        }
        if (R != null)
        {
            foot_R = R.GetComponent<PlayerFoot>();
        }
        else
        {
            print("Right foot missing");
        }
    }


    void enableWeaponCollider()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.equippedWeaponInfo != null)
        {
            PlayerManager.Instance.equippedWeaponInfo.EnableHitbox();
        }
    }

    void disableWeaponCollider()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.equippedWeaponInfo != null)
        {
            PlayerManager.Instance.equippedWeaponInfo.DisableHitbox();
        }

    }

    void ScreenShake()
    {
        PlayerManager.Instance.cameraScript.CamShake(new PlayerCamera.CameraShake(0.4f, 0.7f));
    }

    bool onCooldown = false;
    public enum FootSide { left, right };
    public void TakeFootstep(FootSide side)
    {
        if (foot_L != null && foot_R != null) {
            if (!PlayerManager.Instance.inAir && !onCooldown)
            {
                Vector3 particlePosition;

                if (side == FootSide.left )
                {
                    //if (foot_L.FootstepSound.Validate())
                    { 
                        // HINT: Play left footstep sound
                        particlePosition = foot_L.transform.position;
                        FootstepParticles(particlePosition);
                        foot_L.PlayFootstepSound();
                    }
                }
                else
                {
                    //if (foot_R.FootstepSound.Validate())
                    {
                        // HINT: Play right footstep sound
                        particlePosition = foot_R.transform.position;
                        FootstepParticles(particlePosition);
                        foot_R.PlayFootstepSound();
                    }
                }
            }
        }
    }

    void FootstepParticles(Vector3 particlePosition) {
        GameObject p = Instantiate(runParticles, particlePosition + Vector3.up * 0.1f, Quaternion.identity) as GameObject;
        p.transform.parent = SceneStructure.Instance.TemporaryInstantiations.transform;
        Destroy(p, 5f);
        StartCoroutine(FootstepCooldown());
    }

    IEnumerator FootstepCooldown()
    {
        onCooldown = true;
        yield return new WaitForSecondsRealtime(0.2f);
        onCooldown = false;
    }

    void ReadyToShootMagic()
    {
        PlayerManager.Instance.playerAnimator.SetBool(canShootMagicHash, true);
    }

    public enum AttackState { NotAttacking, Attacking };
    void SetIsAttacking(AttackState state)
    {
        if (state == AttackState.NotAttacking)
        {
            playerAnimator.SetBool(isAttackingHash, false);
        }
        else
        {
            playerAnimator.SetBool(isAttackingHash, true);
        }
    }

    public void Weapon_SwingEvent()
    {
        // PLAY SOUND
        Weapon W = PlayerManager.Instance.equippedWeaponInfo;
        // HINT: PlayerManager.Instance.weaponSlot contains the selected weapon;
        // HINT: This is a good place to play the weapon swing sounds
        Weapon EquippedWeapon = PlayerManager.Instance.equippedWeaponInfo;
        AudioSource audioSource = GetComponent<AudioSource>();
        float volume = 1.0f;
        switch(EquippedWeapon.weaponType)
        {
            case WeaponTypes.Dagger:
                volume = 0.05f;
                break;
            case WeaponTypes.Sword:
                volume = 0.1f;
                break;
            case WeaponTypes.Axe:
                volume = 0.4f;
                break;
            case WeaponTypes.PickAxe:
                volume = 0.3f;
                break;
            case WeaponTypes.Hammer:
                volume = 0.5f;
                break;
        }
        AnimatorStateInfo currentAnimation = PlayerManager.Instance.playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.IsName("Player_RightSwing"))
        {
            // HINT: Weapon combo state 1, you may want to take this into account when playing the weapon swing sound
            audioSource.PlayOneShot(swing1, volume);
        }
        else if (currentAnimation.IsName("Player_LeftSwing"))
        {
            // HINT: Weapon combo state 2, you may want to take this into account when playing the weapon swing sound
            audioSource.PlayOneShot(swing2, volume);
        }
        else if (currentAnimation.IsName("Player_TopSwing"))
        {
            // HINT: Weapon combo state 3, you may want to take this into account when playing the weapon swing sound
            audioSource.PlayOneShot(swing3, volume);
        }
    }

    public void PauseMovement()
    {
        if (!hasPausedMovement)
        {
            hasPausedMovement = true;
            PlayerManager.Instance.motor.SlowMovement();
        }
    }

    public void ResumeMovement()
    {
        if (hasPausedMovement)
        {
            hasPausedMovement = false;
            PlayerManager.Instance.motor.UnslowMovement();
        }
    }

    public void FreezeMotor()
    {
        StartCoroutine(PickupEvent());
    }

    private IEnumerator PickupEvent()
    {
        PlayerManager.Instance.PauseMovement(gameObject);
        yield return new WaitForSeconds(2f);
        PlayerManager.Instance.ResumeMovement(gameObject);
    }

    public void PickUpItem()
    {
        PlayerManager.Instance.PickUpEvent();
        // HINT: This is a good place to play the Get item sound and stinger
        AudioSource audioSource = GetComponent<AudioSource>();
        Weapon EquippedWeapon = PlayerManager.Instance.equippedWeaponInfo;
        switch (EquippedWeapon.weaponType)
        {
            case WeaponTypes.Sword:
                audioSource.PlayOneShot(sword, 0.2f);
                break;
            case WeaponTypes.Axe:
                audioSource.PlayOneShot(axe, 0.2f);
                break;
            case WeaponTypes.PickAxe:
                audioSource.PlayOneShot(pickaxe, 0.2f);
                break;
            case WeaponTypes.Hammer:
                audioSource.PlayOneShot(hammer, 0.2f);
                break;
        }
    }

    public void WeaponSound()
    {
        Weapon EquippedWeapon = PlayerManager.Instance.equippedWeaponInfo;
        // HINT: This is a good place to play equipped weapon impact sound
        AudioSource audioSource = GetComponent<AudioSource>();

        SoundMaterial.Materials mat = SoundMaterial.Materials.STONE;
        if (EquippedWeapon.lastHit() && EquippedWeapon.lastHit().GetComponent<SoundMaterial>())
            mat = EquippedWeapon.lastHit().GetComponent<SoundMaterial>().material;

        audioSource.PlayOneShot(GetComponent<WeaponSwitch>().requestSound(EquippedWeapon.weaponType, mat), 0.05f);
    }
}
