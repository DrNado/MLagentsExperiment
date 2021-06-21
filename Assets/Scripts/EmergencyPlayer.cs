using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Cinemachine;



public class EmergencyPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    #region Variables

    public bool IAPlaying = false;
    //public bool demonstration = false;

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

    private float Score = 0;
    private Rigidbody2D birdRigidbody2D;
    private State state;
    private enum State { WaitingToStart, Playing, Dead }

    #endregion
  
    #region Mono Behaviour


    public void Awake()
    {
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        if (!IAPlaying)
        {
            birdRigidbody2D.bodyType = RigidbodyType2D.Static;
            state = State.WaitingToStart;
        }

    }


    private void Update()
    {
        if (!IAPlaying)
        {
            ExecuteStateMachine();
        }
        //if (IAPlaying)
        //{
        //    return;
        //}
    }


    #endregion

    #region State Machine
    public void ExecuteStateMachine()
    {
        switch (state)
        {
            case State.WaitingToStart:
                if (PressedJumpKey() && !IAPlaying)
                {
                    state = State.Playing;
                    birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                }
                break;

            case State.Playing:
                MoveForward();
                if (PressedJumpKey() && !IAPlaying)
                {
                    Jump();
                }
                break;

            case State.Dead:
                break;

        }
    }

    #endregion

    #region Movement

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
        if (collider.tag == "Obstacle")
        {
            if (!IAPlaying)
            {
                //IAPlaying &&
                birdRigidbody2D.bodyType = RigidbodyType2D.Static;
                StartCoroutine(EndGameScreenCoroutine());
                state = State.Dead;
                //RestartGame();
            }
        }
        else if (collider.tag == "Points")
        {
            ColisaoPontos();

        }
        else if (collider.tag == "EndLine")
        {
            ResetaposXKeepY();
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
    public void RestartGame()
    {
        state = State.WaitingToStart;

        ResetaAgente();
    }
    public void ResetaAgente()
    {
        transform.position = scenario.position + new Vector3(-8, 0, 0);

        Score = 0;
        scoreText.text = "Score: " + Score;
        endGameScreen.SetActive(false);
    }
    #endregion

    #region End Game Screen

    private IEnumerator EndGameScreenCoroutine()
    {
        yield return new WaitForSeconds(1f);
        finalScoreText.text = "Final Score: " + Score;
        endGameScreen.SetActive(true);
    }

    #endregion

    #region Camera Position
    //public void DeactivateCamera()
    //{
    //    if (IAPlaying)
    //    {
    //        CameraCine.SetActive(false);
    //    }
    //}
    #endregion
}
