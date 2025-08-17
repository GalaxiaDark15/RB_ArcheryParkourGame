using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float _maxHealth;
    private float _currentHealth;
    [SerializeField] private Image _healthBarFill;

    // Reference to PlayerHealth script
    [SerializeField] private PlayerHealth playerHealth;

    void Start()
    {
        if (playerHealth != null)
        {
            _maxHealth = playerHealth.playerHealth;
            _currentHealth = _maxHealth;
            UpdateHealthBar();
        }
        else
        {
            Debug.LogError("PlayerHealth reference not set in HealthBar script.");
        }
    }

    void Update()
    {
        if (playerHealth != null)
        {
            _currentHealth = playerHealth.playerHealth;
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        float targetFillAmount = _currentHealth / _maxHealth;
        _healthBarFill.fillAmount = Mathf.Clamp01(targetFillAmount);
    }
}
