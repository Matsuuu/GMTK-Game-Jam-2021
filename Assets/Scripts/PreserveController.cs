using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreserveController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject preservedInstance = GameObject.Find(gameObject.name);
        if (!preservedInstance || preservedInstance == gameObject) {
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
