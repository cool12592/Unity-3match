using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect2 : MonoBehaviour
{
    Vector3 targetPos;
    float speed = 10f;
    bool isRuning = false;

    public void init(Vector3 targetPos_)
    {
        targetPos = targetPos_;
        isRuning = true;
    }

    void Update()
    {
        if (isRuning)
        {
            var nowPos = GetComponent<Transform>().position;
            if ((nowPos - targetPos).sqrMagnitude <=0.1f)
            {
                isRuning = false;
                Destroy(gameObject);
            }
            else
            {
                GetComponent<Transform>().position = Vector3.Lerp(nowPos, targetPos, Time.deltaTime * speed);
            }
        }
    }
}
