using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public Button rotateButton;
    public Button downButton;
    public Button restartButton; 
    void Start()
    {
        if (leftButton != null)
        {
            leftButton.onClick.AddListener(() => MoveLeft());
        }

        if (rightButton != null)
        {
            rightButton.onClick.AddListener(() => MoveRight());
        }

        if (rotateButton != null)
        {
            rotateButton.onClick.AddListener(() => Rotate());
        }

        if (downButton != null)
        {
            downButton.onClick.AddListener(() => AccelerateFall());
        }

        if (restartButton != null) 
        {
            restartButton.onClick.AddListener(() => RestartScene());
        }
    }

    void MoveLeft()
    {
        if (Tetromino.ActiveTetromino != null)
        {
            Tetromino.ActiveTetromino.MoveLeft();
        }
    }

    void MoveRight()
    {
        if (Tetromino.ActiveTetromino != null)
        {
            Tetromino.ActiveTetromino.MoveRight();
        }
    }

    void Rotate()
    {
        if (Tetromino.ActiveTetromino != null)
        {
            Tetromino.ActiveTetromino.Rotate();
        }
    }

    void AccelerateFall()
    {
        if (Tetromino.ActiveTetromino != null)
        {
            Tetromino.ActiveTetromino.AccelerateFall();
        }
    }

    void RestartScene()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
