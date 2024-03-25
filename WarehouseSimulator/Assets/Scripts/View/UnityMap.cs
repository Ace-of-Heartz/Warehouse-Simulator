using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityMap : MonoBehaviour
{
    
    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField] private Color wallColor = Color.grey;
    [SerializeField] private Color emptyColor = Color.green;

    private Grid gridComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        gridComponent = GetComponent<Grid>();
        
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void GenerateMap()
    {
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject ins = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity, this.gameObject.transform);
                ins.transform.position = gridComponent.GetCellCenterWorld(new Vector3Int(i, j));
                if((i+j) % 2 == 0)
                {
                    ins.GetComponent<SpriteRenderer>().color = wallColor;
                }
                else
                {
                    ins.GetComponent<SpriteRenderer>().color = emptyColor;
                }
            }
        }
    }
    
}
