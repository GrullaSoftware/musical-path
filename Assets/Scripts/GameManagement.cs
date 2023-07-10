using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject PointManagement;
    [SerializeField]
    private TextMeshProUGUI DiceStep;
    [SerializeField]
    private GameObject SlotMachine; 
    [SerializeField]
    private Sprite[] SpritesEmotions;
    [SerializeField]
    private Sprite[] SpritesNumbers;
    [SerializeField]
    private Sprite[] SpritesGrades;
    [SerializeField]
    private float WaitTimeJumpStep = 1f;
    private int Steps = 0;
    private int IndexNumber = 0;
    private int IndexEmotion = 0;
    private int IndexGrade = 0;

    public void ThrowDice()
    {
        if (Player != null) 
        {

            if (PointManagement != null)
            {
                SlotMachine.GetComponent<Animator>().SetBool("isroll", true);
                StartCoroutine(ThrowDiceCoroutine((float)0.1));
            }
        }
    }

    private IEnumerator ThrowDiceCoroutine(float waitTime) {
        if (waitTime <= 0 ) 
        {
            SlotMachine.GetComponent<Animator>().SetBool("isroll", false);
            MovePlayerPosition();
            yield return null;
        }
        else
        {
            waitTime = waitTime - (float)0.001;
            if (DiceStep != null)
            {
                int stepsRamdon = Random.Range(0, SpritesNumbers.Length);
                Steps = stepsRamdon+1;
                IndexNumber = stepsRamdon;
                IndexEmotion = Random.Range(0, SpritesEmotions.Length);
                IndexGrade = Random.Range(0, SpritesGrades.Length);
                SetSpritRenderSlotMachineCase();

                //DiceStep.text = "" + stepsRamdon;
            }
            yield return new WaitForSecondsRealtime(waitTime);
      
            StartCoroutine(ThrowDiceCoroutine(waitTime));
        }


    }
    private void SetSpritRenderSlotMachineCase()
    {
        SlotMachine.transform.Find("Numbers").GetComponent<SpriteRenderer>().sprite = SpritesNumbers[IndexNumber];
        SlotMachine.transform.Find("Emotions").GetComponent<SpriteRenderer>().sprite = SpritesEmotions[IndexEmotion];
        SlotMachine.transform.Find("Grades").GetComponent<SpriteRenderer>().sprite = SpritesGrades[IndexGrade];
    }
    private void MovePlayerPosition()
    {
        if (Player != null)
        {
            if (PointManagement != null)
            {                
                int playerPosition = Player.GetComponent<PlayerController>().GetPosition();
                if (playerPosition >= 0 )
                {
                    StartCoroutine(MovePlayerPositionCoroutine(playerPosition));
                }
            }
        }
    }
    private IEnumerator MovePlayerPositionCoroutine(int playerPosition)
    {
        Debug.Log("Steps"+ Steps);
        

        for (int i = 0 ; i < Steps; i++)
        {
            playerPosition = playerPosition + 1;
            if (playerPosition >= PointManagement.GetComponent<PointMananger>().GetPointLength())
            {
                break;
            }
            else
            {
                Transform trsf = PointManagement.GetComponent<PointMananger>().GetPointByIndex(playerPosition).transform.Find("Borde");
                if (trsf)
                {
                    SpriteRenderer spr = PointManagement.GetComponent<PointMananger>().GetPointByIndex(playerPosition).transform.Find("Borde").GetComponent<SpriteRenderer>();
                    spr.color = Color.red;
                    yield return new WaitForSecondsRealtime(WaitTimeJumpStep);
                    spr.color = Color.white;
                }
                else
                {
                    yield return new WaitForSecondsRealtime(WaitTimeJumpStep);
                }
                Player.GetComponent<PlayerController>().SetPosition(playerPosition);
            }
        }
    }
}
