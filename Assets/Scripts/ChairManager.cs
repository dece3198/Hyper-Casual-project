using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairManager : MonoBehaviour
{
    public Chair[] chair;

    public void Specify()
    {
        int rand = Random.Range(0, chair.Length);

        for(int i = 0; i < chair.Length; i++)
        {
            if (chair[rand].guest == null)
            {

            }
        }
    }
}
