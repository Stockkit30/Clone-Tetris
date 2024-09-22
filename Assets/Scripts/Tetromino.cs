using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public static Tetromino ActiveTetromino; // Статична змінна для активного об'єкта

    float fall = 0;
    public float fallSpeed = 1;
    public bool allowRotation = true;
    public bool limitRotation = false;

    private Game game;
    private Spawner spawner;
    private bool isFalling = true;

    void Start()
    {
        game = FindObjectOfType<Game>();
        spawner = FindObjectOfType<Spawner>();

        // Робимо цей Tetromino активним, якщо він щойно заспавнився
        ActiveTetromino = this;
    }

    void Update()
    {
        // Перевіряємо, чи цей об'єкт є активним
        if (ActiveTetromino == this)
        {
            CheckUserInput();
        }
    }

    void CheckUserInput()
    {
        if (isFalling)
        {
            // Перевірка введення через клавіатуру для тестування
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRight();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Rotate();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
            {
                AccelerateFall();
            }
        }
    }

    public void MoveRight()
    {
        if (!isFalling) return;
        transform.position += new Vector3(1, 0, 0);
        if (!CheckIsValidPosition())
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        else
        {
            game.UpdateGrid(this);
        }
    }

    public void MoveLeft()
    {
        if (!isFalling) return;
        transform.position += new Vector3(-1, 0, 0);
        if (!CheckIsValidPosition())
        {
            transform.position += new Vector3(1, 0, 0);
        }
        else
        {
            game.UpdateGrid(this);
        }
    }

    public void Rotate()
    {
        if (!isFalling || !allowRotation) return;

        if (limitRotation)
        {
            if (transform.rotation.eulerAngles.z >= 90)
            {
                transform.Rotate(0, 0, -90);
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }
        }
        else
        {
            transform.Rotate(0, 0, 90);
        }

        if (!CheckIsValidPosition())
        {
            if (limitRotation)
            {
                if (transform.rotation.eulerAngles.z >= 90)
                {
                    transform.Rotate(0, 0, -90);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
            else
            {
                transform.Rotate(0, 0, -90);
            }
        }
        else
        {
            game.UpdateGrid(this);
        }
    }

    public void AccelerateFall()
    {
        if (!isFalling) return;

        transform.position += new Vector3(0, -1, 0);
        if (!CheckIsValidPosition())
        {
            transform.position += new Vector3(0, 1, 0);
            OnTetrominoStopped();
        }
        else
        {
            game.UpdateGrid(this);
        }
        fall = Time.time;
    }

    void OnTetrominoStopped()
    {
        isFalling = false;
        game.UpdateGrid(this);
        spawner.OnTetrominoStopped();
        enabled = false;
    }

    bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = game.Round(mino.position);

            if (!game.CheckIsInsideGrid(pos))
            {
                return false;
            }

            if (game.GetTransformAtGridPosition(pos) != null && game.GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
