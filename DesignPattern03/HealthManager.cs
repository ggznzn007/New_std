using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour, IHealthObserver
{
    [SerializeField] Slider hpSlider;
    public void OnHealthChanged(float newHealth)
    {
        hpSlider.value = newHealth;
    }
}
