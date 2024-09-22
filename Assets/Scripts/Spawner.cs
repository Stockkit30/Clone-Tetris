using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs; // Масив префабів фігур
    public Transform spawnPoint; // Точка спавну нових фігур
    private Game game;

    private List<int> tetrominoQueue; // Черга для тетроміно
    private int currentIndex = 0; // Поточний індекс фігури у черзі

    public float spawnDelay = 0.5f; // Час затримки перед спавном нової фігури

    void Start()
    {
        game = FindObjectOfType<Game>();
        InitializeTetrominoQueue(); // Ініціалізуємо чергу тетроміно
        SpawnNewTetromino(); // Спавнимо першу фігуру
    }

    // Ініціалізація черги тетроміно
    private void InitializeTetrominoQueue()
    {
        tetrominoQueue = new List<int>();

        // Додаємо всі індекси тетроміно до черги
        for (int i = 0; i < tetrominoPrefabs.Length; i++)
        {
            tetrominoQueue.Add(i);
        }

        // Перемішуємо чергу
        ShuffleQueue();
    }

    // Метод для перемішування черги
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

    // Метод для спавну нової фігури
    public void SpawnNewTetromino()
    {
        if (currentIndex >= tetrominoQueue.Count)
        {
            // Якщо ми використали всі фігури з черги, створюємо нову
            InitializeTetrominoQueue();
            currentIndex = 0;
        }

        // Спавнимо тетроміно з черги
        int tetrominoIndex = tetrominoQueue[currentIndex];
        GameObject newTetromino = Instantiate(tetrominoPrefabs[tetrominoIndex], spawnPoint.position, Quaternion.identity);

        // Перевіряємо, чи активний скрипт Tetromino у новій фігурі
        Tetromino tetrominoScript = newTetromino.GetComponent<Tetromino>();
        if (tetrominoScript != null)
        {
            tetrominoScript.enabled = true; // Активуємо скрипт
        }

        currentIndex++; // Переходимо до наступної фігури у черзі
    }

    // Викликається, коли фігура зупинилася
    public void OnTetrominoStopped()
    {
        // Перевірка чи рядки потрібно видалити, якщо потрібно
        game.CheckAndDeleteFullRows();

        // Запускаємо корутину для спавну нової фігури з затримкою
        StartCoroutine(SpawnNewTetrominoWithDelay());
    }

    // Корутина для затримки перед спавном нової фігури
    private IEnumerator SpawnNewTetrominoWithDelay()
    {
        yield return new WaitForSeconds(spawnDelay); // Затримка перед спавном
        SpawnNewTetromino(); // Спавнимо нову фігуру після затримки
    }
}
