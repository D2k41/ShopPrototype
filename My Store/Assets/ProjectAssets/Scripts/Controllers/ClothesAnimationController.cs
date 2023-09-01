using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesAnimationController : MonoBehaviour
{
    private Animator anim;
    private GameController gameController;

    void Awake()
    {
        gameController = GameController.Instance;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        RegulateRunWalk();
        RegulateDirection(gameController.PlayerMovement.Direction);
    }

    private void RegulateRunWalk()
    {
        if (anim != null)
        {
            anim.SetBool("Run",gameController.PlayerMovement.Running);
            anim.SetBool("Walk", gameController.PlayerMovement.Walking);
        }
    }

    private void RegulateDirection(float angle)
    {
        if ((angle >= 0 && angle < 45) || (angle >= 315 && angle < 360))
        {
            anim.SetBool("Right", true);
            anim.SetBool("Left", false);
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
        }
        else if (angle >= 45 && angle < 135)
        {
            anim.SetBool("Up", true);
            anim.SetBool("Left", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", false);
        }
        else if (angle >= 135 && angle < 225)
        {
            anim.SetBool("Left", true);
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", false);
        }
        else if (angle >= 225 && angle < 315)
        {
            anim.SetBool("Down", true);
            anim.SetBool("Left", false);
            anim.SetBool("Up", false);
            anim.SetBool("Right", false);
        }
    }
}
