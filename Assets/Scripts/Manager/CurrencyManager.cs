using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public Action OnUpdateCurrency;

    public void UpdateCurrency()
    {
        OnUpdateCurrency?.Invoke();
    }
}
