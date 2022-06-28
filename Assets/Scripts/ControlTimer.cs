using UnityEngine;

public class ControlTimer : MonoBehaviour
{
    [SerializeField] private GameObject GetDataObj;
    public bool goForData;
    public bool stopData;
    private bool gettingData;

    private void Update()
    {
        if (goForData)
        {
            InvokeRepeating("GetDataTimer", 0, 60);
            goForData = false;
            gettingData = true;
        }
        if (stopData)
        {
            CancelInvoke("GetDataTimer");
            stopData = false;
            gettingData = false;
        }
    }

    void GetDataTimer()
    {
        GetDataObj.GetComponent<GetDataFromWeb>().GetData();
    }

    public void GoForData()
    {
        if (!gettingData)
            goForData = true;
    }
    public void StopData()
    {
        stopData = true;
    }
}
