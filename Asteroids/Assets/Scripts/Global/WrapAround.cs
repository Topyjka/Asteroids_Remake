using UnityEngine;

public class WrapAround : MonoBehaviour
{
    private Camera _camera;
    private float _cameraWidth;
    private float _cameraHeight;

    private bool _isInsideCamera = false;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraWidth = _camera.orthographicSize * _camera.aspect;
        _cameraHeight = _camera.orthographicSize;
    }

    private void Update()
    {
        if (_isInsideCamera)
        {
            WrapPosition(); // Применяем обёртку только для объектов, которые были в камере
        }
        else
        {
            CheckIfInsideCamera(); // Проверяем, попал ли объект в камеру
        }
    }

    /// <summary>
    /// Проверяет, находится ли объект в пределах камеры, и обновляет флаг _isInsideCamera.
    /// </summary>
    private void CheckIfInsideCamera()
    {
        Vector3 pos = transform.position;

        if (pos.x > -_cameraWidth && pos.x < _cameraWidth &&
            pos.y > -_cameraHeight && pos.y < _cameraHeight)
        {
            _isInsideCamera = true; // Как только объект попадает в камеру, активируем обёртку
        }
    }

    /// <summary>
    /// Проверяет позицию объекта и перемещает его на противоположную сторону, если он выходит за пределы камеры.
    /// </summary>
    private void WrapPosition()
    {
        Vector3 newPos = transform.position;

        // Проверяем, если объект вышел за пределы по горизонтали
        if (newPos.x > _cameraWidth)
        {
            newPos.x = -_cameraWidth;
        }
        else if (newPos.x < -_cameraWidth)
        {
            newPos.x = _cameraWidth;
        }

        // Проверяем, если объект вышел за пределы по вертикали
        if (newPos.y > _cameraHeight)
        {
            newPos.y = -_cameraHeight;
        }
        else if (newPos.y < -_cameraHeight)
        {
            newPos.y = _cameraHeight;
        }

        // Обновляем позицию объекта
        transform.position = newPos;
    }
}
