using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public Dropdown generateDropdown;

    public void updateGenerate()
    {
        mazeGenerator.generationNum = generateDropdown.value;
        mazeGenerator.generateMaze();
    }

    public void quit()
    {
        Application.Quit();
    }
}
