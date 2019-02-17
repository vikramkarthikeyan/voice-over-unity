using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLevel : MonoBehaviour
{
    // Start is called before the first frame update
   private void Start()
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // color is controlled like this
        Material m_material = GetComponent<Renderer>().material;
        m_material.color = Color.magenta; // for example
                                                    // There are lots more colours to choose
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
