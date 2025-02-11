using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() 
    {
    }
    // Update is called once per frame
    void Update() 
    {
        Spin();
    }

    [SerializeField]
    GameObject penis;

    public void Spin()
    {
        penis.transform.Rotate(10f, 5f, -10f);
   }
}