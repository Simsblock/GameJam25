using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpinningCards : MonoBehaviour
{
    System.Random rand = new System.Random();
    int xSpin;
    int ySpin;
    int zSpin;
    int MaxMinSpin=5;

    // Start is called before the first frame update
    void Start()
    {
        xSpin = rand.Next(-MaxMinSpin, MaxMinSpin);
        ySpin = rand.Next(-MaxMinSpin, MaxMinSpin);
        zSpin = rand.Next(-MaxMinSpin, MaxMinSpin);
        Spin();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x < -14 || transform.localPosition.y < -14 || transform.localPosition.z < -14)
        {
            Destroy(gameObject); 
        }
    }

    public void Spin()
    {
        gameObject.transform.Rotate(xSpin, ySpin, zSpin);
    }
}
