using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class AgentSimpleScript : Agent
{
    #region Variables

    public bool IAPlaying = false;
    public bool demonstration = false;

    //public GameObject CameraCine;

    [Header("Score")]
    public float pointsPerObstacle = 100;

    [Header("Movement")]
    public float jumpAmount = 100f;
    public float horizontalSpeed = 10f;

    [Header("Object References")]
    public Text scoreText;
    public GameObject endGameScreen;
    public Text finalScoreText;
    public Transform scenario;
    //public Camera camera;

    public float Score = 0;
    private Rigidbody2D birdRigidbody2D;
    //private State state;
    //private Vector3 AgentPosition;
    //private enum State { WaitingToStart, Playing, Dead }

    #endregion



    #region AGENT COMMANDS
    // Start is called before the first frame update
    public override void Initialize() //Awake
    {
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        if (IAPlaying)
        {
            birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            //AgentPosition = transform.position;
            //state = State.Playing;
        }
        //else
        //{
        //    birdRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        //    state = State.Playing;
        //}
        //birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        //state = State.WaitingToStart;
        //base.Initialize();
        //DefaultParams = Academy.Instance.EnvironmentParameters;
    }
    public override void OnEpisodeBegin()
    {
        if (IAPlaying)
        {
            ResetaAgente();
        }
        //base.OnEpisodeBegin();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //base.OnActionReceived(vectorAction);
        if (IAPlaying)
        {
            if (vectorAction[0] == 1)
            {
                Jump();
            }
        }
        
    }

    public override void Heuristic(float[] actionsOut)
    {
        //base.Heuristic(actionsOut);
        if (IAPlaying)
        {
            //MoveForward();

            actionsOut[0] = 0;
            //actionsOut.DiscreteActions
            if (Input.GetKey(KeyCode.Space))
            {
                actionsOut[0] = 1;
            }

        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (IAPlaying)
        {
            MoveForward();
        }   
    }

    
    #region movement
    public void MoveForward() => transform.position = new Vector2(transform.position.x + horizontalSpeed * Time.deltaTime, transform.position.y);

    public bool PressedJumpKey() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

    public void Jump() => birdRigidbody2D.velocity = Vector2.up * jumpAmount;
    #endregion

    #region Collision Check

    public void ColisaoPontos()
    {
        Score += pointsPerObstacle;
        scoreText.text = "Score: " + Score;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if (!IAPlaying)
        //{

        //}
        if (collider.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-1f);
            if (IAPlaying)
            {
                EndEpisode();
            }
        }
        else if (collider.gameObject.CompareTag("Points"))
        {
            AddReward(1f);
            if (IAPlaying)
            {
                ColisaoPontos();
            }

        }
        else if (collider.gameObject.CompareTag("EndLine"))
        {
            AddReward(10.0f);
            if (IAPlaying)
            {
                ResetaposXKeepY();
            }
            //else
            //{
            //    ResetaAgente();
            //}

        }



        //if (collider.gameObject.CompareTag("Obstacle"))
        //{
        //    AddReward(-1.0f);
        //    EndEpisode();
        //}
        //else if (collider.gameObject.CompareTag("Points"))
        //{
        //    AddReward(1.0f);
        //}
        //else if (collider.gameObject.CompareTag("EndLine"))
        //{
        //    AddReward(10.0f);
        //    //Birb.ResetaposXKeepY();
        //}

    }

    #endregion

    #region Restart Game
    public void ResetaposXKeepY()
    {
        transform.position = scenario.position + new Vector3(-8, transform.position.y, 0);
    }
    
    public void ResetaAgente()
    {
        transform.position = scenario.position + new Vector3(-8, 0, 0);

        Score = 0;
        scoreText.text = "Score: " + Score;
        endGameScreen.SetActive(false);

        //OnReset?.invoke();
    }
    #endregion

    #region End Game Screen

    //private IEnumerator EndGameScreenCoroutine()
    //{
    //    yield return new WaitForSeconds(1f);
    //    finalScoreText.text = "Final Score: " + Score;
    //    endGameScreen.SetActive(true);
    //}

    #endregion
}
