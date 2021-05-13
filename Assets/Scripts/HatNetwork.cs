using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class HatNetwork : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator animator = null;

    [SerializeField] private int initialHat;
    [SyncVar] private int Hat;

    void Start()
    {
        initialHat = 0;
    }
    private void Awake()
    {
        
    }

    // Update is called once per frame
    [ClientCallback]
    void Update()
    {
        Hat = initialHat;
        
        switch (Hat)
        {
            case 0:
                animator.SetInteger("HatType", 0);
                break;
            case 1:
                animator.SetInteger("HatType", 1);
                break;
            case 2:
                animator.SetInteger("HatType", 2);
                break;
            case 3:
                animator.SetInteger("HatType", 3);
                break;
            case 4:
                animator.SetInteger("HatType", 4);
                break;
            case 5:
                animator.SetInteger("HatType", 5);
                break;
        }
        
    }


}
