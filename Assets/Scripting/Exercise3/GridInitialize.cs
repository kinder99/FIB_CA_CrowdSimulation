using UnityEngine;
using PathFinding;
public class GridInitialize : MonoBehaviour
{
    [SerializeField]
    private GameObject obstacle;
    [SerializeField] private int maxX;
    [SerializeField] private int maxZ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Grid grid = new Grid(maxX, maxZ, 1, obstacle, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
