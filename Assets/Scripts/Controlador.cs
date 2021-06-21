using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Controlador : MonoBehaviour
{
    public AgentSimpleScript AgentSimpleScript;
    private Rigidbody2D birdRigidbody2D;
    

    private enum State { WaitingToStart, Playing, Dead }
    private State state;
    // Start is called before the first frame update
    #region Mono Behaviour


    public void Awake()
    {
        birdRigidbody2D = GetComponent<Rigidbody2D>();

        if (!AgentSimpleScript.IAPlaying)
        {
            birdRigidbody2D.bodyType = RigidbodyType2D.Static;
            state = State.WaitingToStart;
        }

    }


    private void Update()
    {
        if (!AgentSimpleScript.IAPlaying)
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
                if (AgentSimpleScript.PressedJumpKey() && !AgentSimpleScript.IAPlaying)
                {
                    state = State.Playing;
                    birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    AgentSimpleScript.Jump();
                }
                break;

            case State.Playing:
                AgentSimpleScript.MoveForward();
                if (AgentSimpleScript.PressedJumpKey() && !AgentSimpleScript.IAPlaying)
                {
                    AgentSimpleScript.Jump();
                }
                break;

            case State.Dead:
                break;

        }
    }
    #endregion
    #region restartPlayer
    public void RestartGame()
    {
        state = State.WaitingToStart;

        AgentSimpleScript.ResetaAgente();
    }
    #endregion
    #region Collision Check




    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if (!IAPlaying)
        //{

        //}
        if (collider.gameObject.CompareTag("Obstacle"))
        {
            if (!AgentSimpleScript.IAPlaying)
            {

                //IAPlaying &&
                birdRigidbody2D.bodyType = RigidbodyType2D.Static;
                StartCoroutine(EndGameScreenCoroutine());
                state = State.Dead;
                //AgentSimpleScript.RestartGame();
            }
        }
        else if (collider.gameObject.CompareTag("Points"))
        {
            if (!AgentSimpleScript.IAPlaying)
            {
                AgentSimpleScript.ColisaoPontos();
            }

        }
        else if (collider.gameObject.CompareTag("EndLine"))
        {

            if (!AgentSimpleScript.IAPlaying)
            {
                AgentSimpleScript.ResetaposXKeepY();
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
    private IEnumerator EndGameScreenCoroutine()
    {
        yield return new WaitForSeconds(1f);
        AgentSimpleScript.finalScoreText.text = "Final Score: " + AgentSimpleScript.Score;
        AgentSimpleScript.endGameScreen.SetActive(true);
    }
    //public void MoveForward() => transform.position = new Vector2(transform.position.x + horizontalSpeed * Time.deltaTime, transform.position.y);

    //public bool PressedJumpKey() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

    //public void Jump() => birdRigidbody2D.velocity = Vector2.up * jumpAmount;
}
