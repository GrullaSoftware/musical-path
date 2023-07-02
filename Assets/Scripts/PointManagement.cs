using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMananger:MonoBehaviour
{
    [SerializeField]
    private GameObject[] Points;

    public GameObject GetPointByIndex(int index) {
        if (Points != null)
        {
            if (Points.Length > 0) {
                if (index >= 0 && (index < Points.Length))
                {
                    return Points[index];
                }
            }
        }
        return null;
    }
    public int GetPointLength() {
        if (Points != null)
        {
            if (Points.Length > 0)
            {
                return Points.Length;
            }
        }
        return 0;
    }
}
