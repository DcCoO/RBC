using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum State {
    IDLE,
    SAW_ENEMY,
    COLLIDE_WALL,
    COLLIDE_ENEMY,
    TAKE_DAMAGE    
}
