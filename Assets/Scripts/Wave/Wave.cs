using System.Collections.Generic;

using UnityEngine;

public class Wave : MonoBehaviour
{
    public float DelayBetweenItemSpawn;

    public List<GameObject> ItemsToSpawn;

    public float TimeBeforeConsideringWaveIsFinished;
}