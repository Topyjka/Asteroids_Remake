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
            WrapPosition(); // ��������� ������ ������ ��� ��������, ������� ���� � ������
        }
        else
        {
            CheckIfInsideCamera(); // ���������, ����� �� ������ � ������
        }
    }

    /// <summary>
    /// ���������, ��������� �� ������ � �������� ������, � ��������� ���� _isInsideCamera.
    /// </summary>
    private void CheckIfInsideCamera()
    {
        Vector3 pos = transform.position;

        if (pos.x > -_cameraWidth && pos.x < _cameraWidth &&
            pos.y > -_cameraHeight && pos.y < _cameraHeight)
        {
            _isInsideCamera = true; // ��� ������ ������ �������� � ������, ���������� ������
        }
    }

    /// <summary>
    /// ��������� ������� ������� � ���������� ��� �� ��������������� �������, ���� �� ������� �� ������� ������.
    /// </summary>
    private void WrapPosition()
    {
        Vector3 newPos = transform.position;

        // ���������, ���� ������ ����� �� ������� �� �����������
        if (newPos.x > _cameraWidth)
        {
            newPos.x = -_cameraWidth;
        }
        else if (newPos.x < -_cameraWidth)
        {
            newPos.x = _cameraWidth;
        }

        // ���������, ���� ������ ����� �� ������� �� ���������
        if (newPos.y > _cameraHeight)
        {
            newPos.y = -_cameraHeight;
        }
        else if (newPos.y < -_cameraHeight)
        {
            newPos.y = _cameraHeight;
        }

        // ��������� ������� �������
        transform.position = newPos;
    }
}
