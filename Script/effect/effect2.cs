using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect2 : MonoBehaviour
{
    Vector3 target_pos;
    float speed = 10f;
    bool isRuning = false;
    public void init(Vector3 target_pos_)
    {
        target_pos = target_pos_;
        isRuning = true;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isRuning)
        {
            var nowPos = GetComponent<Transform>().position;
            if ((nowPos - target_pos).sqrMagnitude <=0.1f)
            {
                isRuning = false;
                Destroy(gameObject);
            }
            else
            {
                GetComponent<Transform>().position = Vector3.Lerp(nowPos, target_pos, Time.deltaTime * speed);
            }
        }
    }
}
