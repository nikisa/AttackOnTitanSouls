using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    float trailTimeStart;
    public float TimeDecay;
    // Start is called before the first frame update
    void Start()
    {
        trailTimeStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - trailTimeStart > TimeDecay)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("morto");
        }
    }
}
