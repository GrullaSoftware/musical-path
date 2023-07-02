using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StepController : MonoBehaviour
{
    [SerializeField]
    private string Key = string.Empty;
    [SerializeField]
    private string Octava = string.Empty;
    [SerializeField]
    private string[]  Harmony = new string[6];
    [SerializeField]
    private TextMeshProUGUI Note;
    [SerializeField]
    private TextMeshProUGUI NoteEsp;


    private void Start()
    {
    }
}
