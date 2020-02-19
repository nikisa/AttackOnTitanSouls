using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FrameRateDamage : MonoBehaviour
{
    public int how = 10;

    // Start is called before the first frame update
    void Update()
    {
        for (int i = 0; i < how; i++)
        {
            List<string> lista = new List<string>();
            for (int i2 = 0; i2 < how; i2++)
            {
                    lista.Add(i2.ToString());

                    lista = lista.OrderBy(a => a).ToArray().ToList();
                    Debug.Log(lista[0]);
            }
        }
    }

}
