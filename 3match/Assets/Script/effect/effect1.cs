using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect1 : MonoBehaviour
{
    int dir = 0;
    float speed = 10f;

    public void init(int dir_=1)
    {
        dir = dir_;
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        if(dir!=0)
        {
            switch (dir)
            {
                case 1:
                    transform.Translate(transform.up * Time.deltaTime * speed);
                    break;
                case 2:
                    transform.Translate(-transform.up * Time.deltaTime * speed);
                    break;
                case 3:
                    transform.Translate(transform.right * Time.deltaTime * speed);
                    break;
                case 4:
                    transform.Translate(-transform.right * Time.deltaTime * speed);
                    break;
                default:
                    break;
            }
        }
    }
}
