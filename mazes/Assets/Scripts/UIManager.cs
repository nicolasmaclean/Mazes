using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public Dropdown generateDropdown;

    public Text dimensionText;
    public Text lengthText;
    public Text deadendText;

    public void updateGenerate()
    {
        mazeGenerator.generationNum = generateDropdown.value;
        mazeGenerator.generateMaze();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void updateDimensionText()
    {
        dimensionText.text = "Maze Dimensions: " + mazeGenerator.height + " X " + mazeGenerator.width;
    }

    public void updateLengthText()
    {
        Cell temp = SolutionAlgorithms.getMax(mazeGenerator.maze, mazeGenerator.distances);
        lengthText.text = "Longest Path Length: " + mazeGenerator.distances[temp.y, temp.x];
    }

    public void updateDeadendText()
    {
        deadendText.text = "Amount of Deadends: " + mazeGenerator.deadends().Count;
    }
}
