using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static RaycastHit hitInfo;
    public MazeGenerator mazegenerator;

    void Start()
    {
        mazegenerator = GetComponent<MazeGenerator>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hitInfo, 100);

        for(int y = 0; y < mazegenerator.height; y++)
            for(int x = 0; x < mazegenerator.width; x++) {
                Collider collider = mazegenerator.maze[y, x].transform.Find("Floor").GetComponent<Collider>();
                if(collider != null &&  hitInfo.collider == collider) {
                    //hover
                }
            }
    }
}
