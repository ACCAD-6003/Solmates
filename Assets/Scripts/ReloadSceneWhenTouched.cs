using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Checkpoint_System;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneWhenTouched : MonoBehaviour
{
    [SerializeField] private Vector3 respawnPoint;
    [SerializeField] private float rotation;
    [SerializeField] private Transform players;

    private Dictionary<Transform, Vector3> playersTransforms = new();

    private void Awake()
    {
        players.GetComponentsInChildren<Transform>().ForEach(x => playersTransforms.Add(x, x.localPosition));
    }

    private void OnEnable()
    {
        Checkpoint.OnCheckpointReached += UpdateLastCheckpoint;
    }
    
    private void UpdateLastCheckpoint(Checkpoint checkpoint)
    {
        Debug.Log("Checkpoint Reached");
        respawnPoint = checkpoint.PointToRespawn;
        rotation = checkpoint.Rotation;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        playersTransforms.ForEach(x => x.Key.localPosition = x.Value);
        players.position = respawnPoint;
        var currentRotation = players.rotation.eulerAngles;
        currentRotation.y = rotation;
        players.rotation = Quaternion.Euler(currentRotation);
    }
    
    private void OnDisable()
    {
        Checkpoint.OnCheckpointReached -= UpdateLastCheckpoint;
    }
}
