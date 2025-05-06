using UnityEngine;
using TMPro;

public class ManaManager : MonoBehaviour
{
    public static ManaManager Instance;

    public int baseMana = 10;
    public int manaIncreasePerRound = 2;
    public TMP_Text manaText;

    private int currentMana;
    private int roundCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ResetManaForRound()
    {
        roundCount++;
        currentMana = baseMana + manaIncreasePerRound * (roundCount - 1);
        UpdateUI();
    }

    public bool SpendMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateUI();
            return true;
        }

        return false;
    }

    public void UpdateUI()
    {
        if (manaText != null)
        {
            manaText.text = "Mana: " + currentMana;
        }
    }

    public int GetCurrentMana()
    {
        return currentMana;
    }
}
