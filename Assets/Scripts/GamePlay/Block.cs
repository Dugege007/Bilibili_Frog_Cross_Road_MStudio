using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{


    private void Update()
    {
        CheckPosition();    //HACK
    }

    private void CheckPosition()
    {
        if (Camera.main.transform.position.y-transform.position.y>25)
        {
            Destroy(this.gameObject);   //this¿ÉÒÔÊ¡ÂÔ
        }
    }
}
