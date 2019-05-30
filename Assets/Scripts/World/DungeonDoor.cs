using UnityEngine;
using System.Collections;

public class DungeonDoor : MonoBehaviour
{
    #region private variables
    private bool isOpen = false;
    private Animator anim;
    private readonly int openDoorHash = Animator.StringToHash("OpenDoor");
    private readonly int closeDoorHash = Animator.StringToHash("CloseDoor");

    private bool playDoorSound;
    #endregion

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ToggleDoor()
    {
        playDoorSound = true;
        if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            anim.SetTrigger(openDoorHash);
            isOpen = true;
        }
    }

    public void OpenDoorNoSound()
    {
        playDoorSound = false;
        if (!isOpen)
        {
            anim.SetTrigger(openDoorHash);
            isOpen = true;
        }
    }

    public void CloseDoorNoSound()
    {
        playDoorSound = false;
        if (isOpen)
        {
            anim.SetTrigger(closeDoorHash);
            isOpen = false;
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            anim.SetTrigger(closeDoorHash);
            isOpen = false;
        }
    }

    public void PlayDoorSound()
    {
        if (playDoorSound)
        {
            // HINT: Door has been moved, place some code here to make it sound
        }
    }
}
