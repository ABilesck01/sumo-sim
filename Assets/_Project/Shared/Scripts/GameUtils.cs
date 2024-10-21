using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils : MonoBehaviour
{
    public static GameUtils instance;

    public GameObject arena;
    public float arenaRadius;

    private void Awake()
    {
        instance = this;
    }


}
