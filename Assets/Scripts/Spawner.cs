using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs; // ����� ������� �����
    public Transform spawnPoint; // ����� ������ ����� �����
    private Game game;

    private List<int> tetrominoQueue; // ����� ��� ��������
    private int currentIndex = 0; // �������� ������ ������ � ����

    public float spawnDelay = 0.5f; // ��� �������� ����� ������� ���� ������

    void Start()
    {
        game = FindObjectOfType<Game>();
        InitializeTetrominoQueue(); // ���������� ����� ��������
        SpawnNewTetromino(); // �������� ����� ������
    }

    // ����������� ����� ��������
    private void InitializeTetrominoQueue()
    {
        tetrominoQueue = new List<int>();

        // ������ �� ������� �������� �� �����
        for (int i = 0; i < tetrominoPrefabs.Length; i++)
        {
            tetrominoQueue.Add(i);
        }

        // ��������� �����
        ShuffleQueue();
    }

    // ����� ��� ������������ �����
    private void ShuffleQueue()
    {
        for (int i = 0; i < tetrominoQueue.Count; i++)
        {
            int temp = tetrominoQueue[i];
            int randomIndex = Random.Range(i, tetrominoQueue.Count);
            tetrominoQueue[i] = tetrominoQueue[randomIndex];
            tetrominoQueue[randomIndex] = temp;
        }
    }

    // ����� ��� ������ ���� ������
    public void SpawnNewTetromino()
    {
        if (currentIndex >= tetrominoQueue.Count)
        {
            // ���� �� ����������� �� ������ � �����, ��������� ����
            InitializeTetrominoQueue();
            currentIndex = 0;
        }

        // �������� �������� � �����
        int tetrominoIndex = tetrominoQueue[currentIndex];
        GameObject newTetromino = Instantiate(tetrominoPrefabs[tetrominoIndex], spawnPoint.position, Quaternion.identity);

        // ����������, �� �������� ������ Tetromino � ���� �����
        Tetromino tetrominoScript = newTetromino.GetComponent<Tetromino>();
        if (tetrominoScript != null)
        {
            tetrominoScript.enabled = true; // �������� ������
        }

        currentIndex++; // ���������� �� �������� ������ � ����
    }

    // �����������, ���� ������ ����������
    public void OnTetrominoStopped()
    {
        // �������� �� ����� ������� ��������, ���� �������
        game.CheckAndDeleteFullRows();

        // ��������� �������� ��� ������ ���� ������ � ���������
        StartCoroutine(SpawnNewTetrominoWithDelay());
    }

    // �������� ��� �������� ����� ������� ���� ������
    private IEnumerator SpawnNewTetrominoWithDelay()
    {
        yield return new WaitForSeconds(spawnDelay); // �������� ����� �������
        SpawnNewTetromino(); // �������� ���� ������ ���� ��������
    }
}
