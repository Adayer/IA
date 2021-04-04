using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scream : MonoBehaviour
{
	[SerializeField] private Animator m_cmpAnimator = null; 
	// Start is called before the first frame update
    void Start()
    {
		m_cmpAnimator.SetTrigger("Idle");
		print("AAAAAAAAAAAAAAAAAAAAAAH");
    }
}
