using UnityEngine;
using UnityEngine.UI;

public class PlayerLIvesHandler : MonoBehaviour
{
    [SerializeField] private Image[] _lifeIcons;

    public void UpdateLifeUI(int currentLives)
    {
        for (int i = 0; i < _lifeIcons.Length; i++)
        {
            _lifeIcons[i].enabled = i < currentLives;
        }
    }
}
