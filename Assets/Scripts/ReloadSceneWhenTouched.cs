using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Checkpoint_System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneWhenTouched : MonoBehaviour
{
    [SerializeField, ReadOnly] private Vector3 respawnPoint;
    [SerializeField, ReadOnly] private float rotation;
    [SerializeField, ReadOnly] private float distance;
    [SerializeField, ReadOnly] private Transform players;

    private Dictionary<Transform, Vector3> playersTransforms = new();

    private void Awake()
    {
        players = GameObject.FindWithTag("PlayerGroup").transform;
        respawnPoint = players.transform.position;
        rotation = players.transform.rotation.eulerAngles.y;
        distance = players.GetComponentInChildren<Movement2>().initialRadius;
    
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
        players.GetComponentsInChildren<SpringJoint>().ForEach(x => { x.minDistance = distance; x.maxDistance = distance; });
        players.GetComponentsInChildren<Movement2>().ForEach(x => { x.StopGrowing(); });
    }
    
    private void OnDisable()
    {
        Checkpoint.OnCheckpointReached -= UpdateLastCheckpoint;
    }
}
