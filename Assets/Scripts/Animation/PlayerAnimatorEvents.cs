using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    public void OnPlayerAttackStart()
    {
        GameEvents.current.PlayerAttackStart();
    }

    public void OnPlayerAttackEnd()
    {
        GameEvents.current.PlayerAttackEnd();
    }

}
