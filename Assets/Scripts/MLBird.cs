using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class MLBird : Agent
{
    //public Transform scenario;

    public EmergencyPlayer Birb;

    private Rigidbody2D birdRigidbody2D;

    EnvironmentParameters DefaultParams;

    public override void Initialize() //Awake
    {
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        if (Birb.IAPlaying)
        {
            birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
        
    }

    public override void OnEpisodeBegin()
    {
        if (Birb.IAPlaying)
        {
            Birb.ResetaAgente();
            
        }
        //base.OnEpisodeBegin();
    }

    //public override void CollectObservations(VectorSensor sensor)
    //{


    //    //base.CollectObservations(sensor);
    //}

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 1)
        {
            Birb.Jump();
        }
        //base.OnActionReceived(vectorAction);

        //EndEpisode();

    }
    public override void Heuristic(float[] actionsOut)
    {
        if (Birb.IAPlaying)
        {
            actionsOut[0] = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                actionsOut[0] = 1;
            }
        }
        //base.Heuristic(actionsOut);
    }
    //private void FixedUpdate()
    //{
    //    Birb.MoveForward();
    //    RequestDecision();
    //}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (Birb.IAPlaying)
        {
            if (collider.gameObject.CompareTag("Obstacle"))
            {
                AddReward(-1.0f);
                if (Birb.IAPlaying)
                {
                    EndEpisode();
                }
               
            }
            else if (collider.gameObject.CompareTag("Points"))
            {
                AddReward(1.0f);
            }
            else if (collider.gameObject.CompareTag("EndLine"))
            {
                AddReward(10.0f);
            }
        }


        }



    }
