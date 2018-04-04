using UnityEngine;
using System.Collections;

public class TargetPoint : MonoBehaviour {
    private int count = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddLink()
    {
        count++;
    }

    public void DeleteLink()
    {
        count--;
        if (count == 0)
            Destroy(gameObject);
    }
}
