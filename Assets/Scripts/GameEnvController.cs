using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class GameEnvController : MonoBehaviour
{
    public int buttonsOnEpisode = 4;
    public int boxesOnEpisode = 3;

    public GridedDistributor[] teamsSpawners;

    void Start()
    {
        var agents = ResetScene();
        foreach(var agent in agents) {
            // UnityEventTools.AddPersistentListener(agent.onDied, onAgentDied);
            agent.onDied.AddListener(onAgentDied);
        }
    }

    AgentShooter[] ResetScene()
    {
        var agents = new AgentShooter[2];
        agents[0] = teamsSpawners[0].Respawn(1)[0].GetComponent<AgentShooter>();
        agents[0].Reset();
        agents[1] = teamsSpawners[1].Respawn(1)[0].GetComponent<AgentShooter>();
        agents[1].Reset();
        return agents;
    }

    void onAgentDied(AgentShooter agent) {
        ResetScene();
    }
}
