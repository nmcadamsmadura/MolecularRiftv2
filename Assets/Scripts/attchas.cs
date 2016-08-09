using UnityEngine;
using System;
using System.Linq; 
using System.Collections;

public class attchas : MonoBehaviour {
    void Update()
    {
        
        Vector3 pos1 = this.gameObject.GetComponent<SphereCollider>().transform.position;
        if (!this.gameObject.GetComponent<AudioSource>().isPlaying & Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, pos1) <= this.gameObject.GetComponent<SphereCollider>().contactOffset)
            {
            this.gameObject.GetComponent<AudioSource>().Play();
            Debug.Log(this.gameObject.GetComponent<AudioSource>().isPlaying);
            }
            else if (this.gameObject.GetComponent<AudioSource>().isPlaying & Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, pos1) > this.gameObject.GetComponent<SphereCollider>().contactOffset)
            {
            this.gameObject.GetComponent<AudioSource>().Pause();
        }
        
    }
}
