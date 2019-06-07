////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class SoundMaterial : MonoBehaviour
{
    // HINT: You can identify your material here, so you can play the appropiate footstep sound
    public enum Materials
    {
        DIRT,
        GRASS,
        RUBBLE,
        SAND,
        STONE,
        WATER,
        WOOD,
        LEAVES,
        CRATE
    }

    public Materials material;

    private void Start()
    {
        if (gameObject.name.Contains("Dirt"))
            material = Materials.DIRT;
        else if (gameObject.name.Contains("Grass") || gameObject.name.Contains("Forest") || gameObject.name.Contains("Plant") || gameObject.name.Contains("Crawler"))
            material = Materials.GRASS;
        else if (gameObject.name.Contains("Rubble"))
            material = Materials.RUBBLE;
        else if (gameObject.name.Contains("Sand") || gameObject.name.Contains("Desert"))
            material = Materials.SAND;
        else if (gameObject.name.Contains("Stone") || gameObject.name.Contains("Pillar") || gameObject.name.Contains("Head"))
            material = Materials.STONE;
        else if (gameObject.name.Contains("Water"))
            material = Materials.WATER;
        else if (gameObject.name.Contains("Wood") || gameObject.name.Contains("Tree") || gameObject.name.Contains("Fence"))
            material = Materials.WOOD;
        else if (gameObject.name.Contains("Bush"))
            material = Materials.LEAVES;
        else if (gameObject.name.Contains("Crate") || gameObject.name.Contains("Barrel") || gameObject.name.Contains("Blockade"))
            material = Materials.CRATE;
    }
}
