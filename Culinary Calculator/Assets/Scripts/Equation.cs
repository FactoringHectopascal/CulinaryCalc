using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Equation : MonoBehaviour
{
    [SerializeField]
    public TMP_InputField cost;
    [SerializeField]
    public TMP_InputField unit;
    [SerializeField]
    TMP_Text unitResult;
    [SerializeField]
    public TMP_InputField qty;
    [SerializeField]
    TMP_Text qtyResult;
    // Start is called before the first frame update
    void Start()
    {
        qty.onValueChanged.AddListener(delegate { UpdatePrices(); });
        unit.onValueChanged.AddListener(delegate { UpdatePrices(); });
        cost.onValueChanged.AddListener(delegate { UpdatePrices(); });
        UpdatePrices();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePrices()
    {
        if (unit.text == "" || cost.text == "")
        {
            unitResult.text = "-.--";
            return;
        }

        float.TryParse(unit.text, out float unitFloat);
        float.TryParse(cost.text, out float costFloat);
        float unitCost = costFloat / unitFloat;
        unitResult.text = MathF.Round(unitCost, 2).ToString("0.00");

        if (qty.text == "")
        {
            qtyResult.text = "-.--";
            return;
        }
            
        float.TryParse(qty.text, out float qtyFloat);
        float qtyCost = unitCost * qtyFloat;
        qtyResult.text =  MathF.Round(qtyCost, 2).ToString("0.00");
    }
}
