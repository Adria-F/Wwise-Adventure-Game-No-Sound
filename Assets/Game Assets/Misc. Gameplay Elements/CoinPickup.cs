////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinPickup : MonoBehaviour {

    public bool playSpawnSoundAtSpawn = true;

    public AudioClip drop;
    public AudioClip pickup;

	void Start(){
        if (playSpawnSoundAtSpawn){
            // HINT: You might want to play the Coin pickup sound here
            GetComponent<AudioSource>().PlayOneShot(drop);
        }
	}

	public void AddCoinToCoinHandler(){
		InteractionManager.SetCanInteract(this.gameObject, false);
		GameManager.Instance.coinHandler.AddCoin ();
        GetComponent<AudioSource>().PlayOneShot(pickup);
        //Destroy (gameObject, 0.1f); //TODO: Pool instead?
    }
}
