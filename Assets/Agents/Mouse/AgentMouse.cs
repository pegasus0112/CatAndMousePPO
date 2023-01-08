using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;
using System.Linq;

public class AgentMouse : Agent
{
    [Header("Rewards")]
    public float timeReward;
    [Space(10)]
    [Header("Penalties")]
    public float mouseKilled;
    public float walkingPenalty;
    public float rotationPenalty;
    [Space(10)]

    Vector3 startPosition;

    float maxX = 10f;
    float maxZ = 10f;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        AddReward(timeReward);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float move = Mathf.Clamp(actionBuffers.ContinuousActions[0], 0f, 1f);
        float rotate = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);

        AddReward(move * walkingPenalty);
        AddReward(Mathf.Abs(rotate) * rotationPenalty);

        GetComponent<NewMovement>().walkSpeed = move;
        GetComponent<NewMovement>().rotation = rotate;
    }

    public override void OnEpisodeBegin()
    {
        placeOnRandomPositionInGym();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }

    private void placeOnRandomPositionInGym()
    {
        float randomX = Random.Range(-maxX + 1, maxX - 1);
        float randomZ = Random.Range(-maxZ + 1, maxZ - 1);
        Vector3 randomLocation = new Vector3(startPosition.x + randomX, 0.25f, startPosition.z + randomZ);
        Collider[] colliders = Physics.OverlapSphere(randomLocation, 0.75f);

        while(colliders.Where(collider => collider.tag == "Wall").ToArray().Length > 0)
        {
            randomX = Random.Range(-maxX + 1, maxX - 1);
            randomZ = Random.Range(-maxZ + 1, maxZ - 1);
            randomLocation = new Vector3(startPosition.x + randomX, 0.25f, startPosition.z + randomZ);
            colliders = Physics.OverlapSphere(randomLocation, 0.8f);
        }
        gameObject.transform.SetPositionAndRotation(randomLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
    }

    public void kill()
    {
        AddReward(mouseKilled);
        EndEpisode();
        Debug.Log("Death");
    }
}
