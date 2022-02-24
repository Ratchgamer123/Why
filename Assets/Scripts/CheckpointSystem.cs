using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//EXPERIMENTAL
public class CheckpointSystem : MonoBehaviour
{
    [SerializeField] private Transform currentRespawnPosition;

    public static event Action<Transform> SetPlayerTransform;

    private void OnEnable()
    {
        Checkpoint.SetCurrentCheckpoint += SetCurrentCheckpointLocation;
        Player.GetPlayerLocation += GivePlayerLocation;
    } 
    private void OnDisable()
    {
        Checkpoint.SetCurrentCheckpoint -= SetCurrentCheckpointLocation;
        Player.GetPlayerLocation -= GivePlayerLocation;
    }

    private void SetCurrentCheckpointLocation(Transform location)
    {
        currentRespawnPosition = location;
    }

    private void GivePlayerLocation()
    {
        SetPlayerTransform?.Invoke(currentRespawnPosition);
    }
}
