using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate = 2f;
    [SerializeField] private int _spawnAmount = 1;
    [SerializeField] private Asteroid _asteroid;
    [SerializeField] private float _spawnDistance = 15f;
    [SerializeField] private float _trajectoryVariance = 15f;
    [SerializeField] private ObjectPool<Asteroid> _asteroidsPool;
    [SerializeField] private int _maxAsteroidsOnDisplay = 20;

    private int _largeDebrisCount = 3;
    private int _mediumDebrisCount = 2;
    private int _smallDebrisCount = 0;
    private int _asteroidsOnDisplay = 0;

    private void Awake()
    {
        InvokeRepeating(nameof(Spawn), _spawnRate, _spawnRate);
    }

    private void OnEnable()
    {
        Asteroid.OnObjectDestroyed += CreateSplits;
    }

    private void OnDisable()
    {
        Asteroid.OnObjectDestroyed -= CreateSplits;
    }

    private void Spawn()
    {
        if (_asteroidsOnDisplay < _maxAsteroidsOnDisplay)
        {
            for (int i = 0; i < _spawnAmount; i++)
            {
                Vector3 spawnDirection = Random.insideUnitSphere.normalized * _spawnDistance;
                Vector3 spawnPoint = transform.position + spawnDirection;
                spawnPoint.z = 0;

                float variance = Random.Range(-_trajectoryVariance, _trajectoryVariance);
                Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

                Asteroid asteroid = _asteroidsPool.GetObject();
                asteroid.transform.position = spawnPoint;
                asteroid.transform.rotation = rotation;
                asteroid.ResetSize(Random.Range(_asteroid.Small, _asteroid.Large));
                asteroid.SetTrajectory(rotation * -spawnDirection);
                _asteroidsOnDisplay++;
            }
        }
    }

    private void CreateSplit(Asteroid asteroid, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 position = asteroid.transform.position;
            position += Random.insideUnitCircle * 0.5f;

            Asteroid tempAsteroid = _asteroidsPool.GetObject();
            tempAsteroid.transform.position = position;
            tempAsteroid.transform.rotation = asteroid.transform.rotation;
            tempAsteroid.ResetSize(asteroid.Size / 2f);
            tempAsteroid.SetTrajectory(Random.insideUnitCircle.normalized * asteroid.MoveSpeed);
        }

        _asteroidsPool.ReturnObject(asteroid);
        _asteroidsOnDisplay--;
    }

    private void CreateSplits(Asteroid asteroid)
    {
        if (asteroid.Size < asteroid.Large && asteroid.Size > asteroid.Medium)
        {
            CreateSplit(asteroid, _largeDebrisCount);
        }
        else if (asteroid.Size < asteroid.Medium && asteroid.Size > asteroid.Small)
        {
            CreateSplit(asteroid, _mediumDebrisCount);
        }
        else
        {
            CreateSplit(asteroid, _smallDebrisCount);
        }
    }
}
