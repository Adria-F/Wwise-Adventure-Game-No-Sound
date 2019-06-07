using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    [System.Serializable]
    public struct AudioSwitch
    {
        public WeaponTypes weapon;
        public SoundMaterial.Materials material;
        public AudioClip clip;
    }

    public AudioSwitch[] audioSwitch;

    public AudioClip requestSound(WeaponTypes weapon, SoundMaterial.Materials material)
    {
        foreach(AudioSwitch element in audioSwitch)
        {
            if (element.weapon == weapon && element.material == material)
                return element.clip;
        }

        foreach (AudioSwitch element in audioSwitch)//If not found return wood equivalent
        {
            if (element.weapon == weapon && element.material == SoundMaterial.Materials.STONE)
                return element.clip;
        }

        return null;
    }
}
