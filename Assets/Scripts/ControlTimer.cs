using System;
using UnityEngine;

public class ControlTimer : MonoBehaviour
{
    [SerializeField] private GameObject GetDataObj;
    public bool goForData;
    public bool stopData;

    private void Update()
    {
        if (goForData)
        {
            InvokeRepeating("GetDataTimer", 0, 60);
            goForData = false;
        }
        if (stopData)
        {
            CancelInvoke("GetDataTimer");
            stopData = false;
        }
    }

    void GetDataTimer()
    {
        GetDataObj.GetComponent<GetDataFromWeb>().GetData();
    }
}
