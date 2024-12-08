using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate = 2f;
    [SerializeField] private int _spawnAmount = 1;
    [SerializeField] private UFO _ufo;
    [SerializeField] private float _spawnDistance = 15f;
    [SerializeField] private float _trajectoryVariance = 15f;
    [SerializeField] private ObjectPool<UFO> _ufoPool;
    [SerializeField] private int _maxUFOOnDisplay = 2;

    private int _ufoOnDisplay = 0;
    private float _smallSize = 1.5f;
    private float _bigSize = 2.5f;

    private void Awake()
    {
        InvokeRepeating(nameof(Spawn), _spawnRate, _spawnRate);
    }

    private void Spawn()
    {
        if (_ufoOnDisplay < _maxUFOOnDisplay)
        {
            for (int i = 0; i < _spawnAmount; i++)
            {
                // Вычисляем границы экрана
                Camera mainCamera = Camera.main;
                Vector3 screenBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
                Vector3 screenTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

                float screenHeightMin = screenBottomLeft.y; // Нижняя граница экрана
                float screenHeightMax = screenTopRight.y;  // Верхняя граница экрана

                // Определяем случайную высоту спавна в пределах видимой высоты
                float randomHeight = Random.Range(screenHeightMin, screenHeightMax);

                // Определяем сторону спавна (слева или справа)
                bool spawnOnLeft = Random.value > 0.5f;
                float spawnX = spawnOnLeft ? screenBottomLeft.x - _spawnDistance : screenTopRight.x + _spawnDistance;

                // Позиция спавна
                Vector3 spawnPoint = new Vector3(spawnX, randomHeight, 0);

                // Случайный угол отклонения траектории
                float variance = Random.Range(-_trajectoryVariance, _trajectoryVariance);

                // Получаем UFO из пула и задаём параметры
                UFO ufo = _ufoPool.GetObject();
                ufo.transform.position = spawnPoint;

                // Размер UFO (например, маленький или большой)
                ufo.ResetSize(Random.Range(_smallSize, _bigSize) > 2 ? _bigSize : _smallSize);

                _ufoOnDisplay++;
            }
        }
    }


    private void ReturnUfo(UFO ufo)
    {
        if (ufo.IsVisible == false)
            _ufoPool.ReturnObject(ufo);
        _ufoOnDisplay--;
    }

    private void OnEnable()
    {
        UFO.OnVisible += ReturnUfo;
    }

    private void OnDisable()
    {
        UFO.OnVisible -= ReturnUfo;
    }
}
