using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine.Events;

public class AgentShooter : Agent
{
    // Victim, Shooter
    public UnityEvent<AgentShooter, AgentShooter> onTakeDamage = new UnityEvent<AgentShooter, AgentShooter>();
    public UnityEvent<AgentShooter> onDied = new UnityEvent<AgentShooter>();
    public enum Team
    {
        Blue, Red
    }
    public Gun agentGun;
    public float maxHp = 3;
    public Team team { get; private set; }

    float hp = 3;
    float m_LateralSpeed = 0.15f;
    float m_ForwardSpeed = 0.5f;


    [HideInInspector]
    public Rigidbody agentRb;
    BehaviorParameters behaviorParameters;

    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 500;
        behaviorParameters = GetComponent<BehaviorParameters>();
        team = (Team)behaviorParameters.TeamId;
        agentGun.owner = this;
        Reset();
    }

    public void Reset()
    {
        hp = maxHp;
        agentGun.Reset();
    }

    public void TakeDamage(float damage, AgentShooter source)
    {
        if (hp <= 0) { return; }
        hp -= damage;
        onTakeDamage.Invoke(this, source);
        if (hp <= 0) { onDied.Invoke(this); }
    }
    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;


        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                break;
            case 2:
                dirToGo = transform.forward * -m_ForwardSpeed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * m_LateralSpeed;
                break;
            case 2:
                dirToGo = transform.right * -m_LateralSpeed;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        if (act[3] == 1)
        {
            agentGun.Fire();
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        agentRb.AddForce(dirToGo, ForceMode.VelocityChange);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        //forward
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        //rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[2] = 2;
        }
        //right
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 2;
        }
        //fire
        if (Input.GetKey(KeyCode.Space))
        {
            discreteActionsOut[3] = 1;
        }
    }
    public override void OnEpisodeBegin()
    {
    }

}
