using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    public GameObject hook;
    public bool ropeActive;

    GameObject curHook;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {

            if (!ropeActive) {

                Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                curHook = Instantiate(hook, transform.position, Quaternion.identity) as GameObject;
                curHook.GetComponent<RopeScript>().target = target;

                ropeActive = true;
            }
            else {
                Destroy(curHook);
                ropeActive = false;
            }
        }


    }
}
