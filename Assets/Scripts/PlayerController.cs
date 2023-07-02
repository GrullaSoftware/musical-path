using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int Position = 0;
    public void SetPosition(int position)
    {
        if (Position >= 0)
        {
            this.Position = position;
        }
    }
    public int GetPosition() 
    { 
       return this.Position; 
    }
}
