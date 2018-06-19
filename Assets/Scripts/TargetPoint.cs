using UnityEngine;
using System.Collections;

/// <summary>
/// Класс объекта флага
/// </summary>
public class TargetPoint : MonoBehaviour {
    private int count = 0;
    private Vector2Int gridPosition;
    public Vector2Int Position
    {
        get
        {
            return gridPosition;
        }
    }

    void Awake() {
        gridPosition = new Vector2Int(Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.z));
    }
    // Use this for initialization
    void Start () {
        this.gameObject.transform.position = new Vector3(gridPosition.x, TerrainHeightMap.Instance.GetHeight(gridPosition), gridPosition.y);
    }

    public void AddLink() {
        count++;
    }

    public void DeleteLink() {
        count--;
        if (count == 0)
            Destroy(gameObject);
    }
}
