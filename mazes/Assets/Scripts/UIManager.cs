using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public Dropdown generateDropdown;
    public Slider pSlider;

    public Text dimensionText;
    public Text lengthText;
    public Text deadendText;
    public Text pText;

    public void updateGenerate()
    {
        generate(false);
    }

    public void updateGenerateBraid()
    {
        generate(true);
    }

    public void generate(bool b)
    {
        mazeGenerator.generationNum = generateDropdown.value;
        mazeGenerator.generateMaze(b);
        updateDimensionText();
        mazeGenerator.solveMaze();
        updateLengthText();
        updateDeadendText();
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
        deadendText.text = "Amount of Deadends: " + mazeGenerator.getDeadends().Count;
    }

    public void updateP()
    {
        mazeGenerator.p = pSlider.value;
        pText.text = string.Format("{0:0.0%}", pSlider.value);
    }
}
