using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTime : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(System.DateTime.Now.Day);
        Debug.Log(System.DateTime.Now.Month);
    }
}