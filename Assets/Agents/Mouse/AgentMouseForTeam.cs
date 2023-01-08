using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;
using System.Linq;

public class AgentMouseForTeam : Agent
{
    public GameManager manager;

    [Header("Rewards")]
    public float timeReward;
    [Space(10)]
    [Header("Penalties")]
    public float mouseKilled;
    public float walkingPenalty;
    public float rotationPenalty;

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

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }

    public void kill()
    {
        AddReward(mouseKilled);
        manager.deadMice++;
        EndEpisode();
        gameObject.SetActive(false);
    }
}
