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
    [SerializeField, ReadOnly] private GameObject[] doors;

    private Dictionary<Transform, Vector3> playersTransforms = new();
    private float timeToWait = 1f;
    private IEnumerator fadeToBlack;

    private void Awake()
    {
        players = GameObject.FindWithTag("PlayerGroup").transform;
        respawnPoint = players.transform.position;
        rotation = players.transform.rotation.eulerAngles.y;
        distance = players.GetComponentInChildren<Movement2>().initialRadius;
        doors = GameObject.FindGameObjectsWithTag("Door");

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
        players.GetComponentsInChildren<SpringJoint>().ForEach(x => { x.minDistance = distance; x.maxDistance = distance; });
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(FadeInThenOutOfBlack());
        // players.GetComponentsInChildren<SpringJoint>().ForEach(x => { x.breakForce = 0; });
    }
    
    private void OnDisable()
    {
        Checkpoint.OnCheckpointReached -= UpdateLastCheckpoint;
    }

    IEnumerator FadeInThenOutOfBlack()
    {
        fadeToBlack = FadeToBlackSystem.TryCueFadeInToBlack(timeToWait); // Start the coroutine and keep a reference
        yield return fadeToBlack; // Wait until the coroutine is finished

        playersTransforms.ForEach(x => x.Key.localPosition = x.Value);
        players.position = respawnPoint;
        var currentRotation = players.rotation.eulerAngles;
        currentRotation.y = rotation;
        players.rotation = Quaternion.Euler(currentRotation);
        players.GetComponentsInChildren<SpringJoint>().ForEach(x => { x.minDistance = distance; x.maxDistance = distance; });
        players.GetComponentsInChildren<Movement2>().ForEach(x => { x.StopGrowing(); });
        players.GetComponentsInChildren<Rigidbody>().ForEach(x => { x.velocity = new Vector3(0,0,0); });

        // Renable doors
        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }

        StartCoroutine(FadeToBlackSystem.TryCueFadeOutOfBlack(timeToWait));
    }
}
