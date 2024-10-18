using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReRollElement : MonoBehaviour
{
    [SerializeField] private int ReRollCost = 2;
    [SerializeField] private PlayerCharacterDataSO[] datas;
    private Button ReRollBtn;
    private List<PurchaseElement> elements;


    private void Awake()
    {
        elements = new List<PurchaseElement>();
        elements = FindObjectsOfType<PurchaseElement>().ToList();

        ReRollBtn = GetComponent<Button>();
        ReRollBtn.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        ReRollBtn.onClick.AddListener(() =>
        {
            if (GameManager.Instance.GoldChangeToValue(-ReRollCost))
            {
                ActionReRoll();
            }
        });
        ActionReRoll();
    }

    private void ActionReRoll()
    {
        foreach (PurchaseElement element in elements)
        {
            element.SetBuyCharacter(datas[Random.Range(0, datas.Length)]);
        }
    }
}
