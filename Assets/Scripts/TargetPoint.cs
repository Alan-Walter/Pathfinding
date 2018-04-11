using UnityEngine;
using System.Collections;

public class TargetPoint : MonoBehaviour {
    private int count = 0;
    private Vector3 position;
    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

    void Awake() {
        position = new Vector3(Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.y),
            Mathf.RoundToInt(gameObject.transform.position.z));
    }
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
