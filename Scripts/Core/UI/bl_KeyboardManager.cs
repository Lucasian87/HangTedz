using UnityEngine;
using System.Collections.Generic;

public class bl_KeyboardManager : Singleton<bl_KeyboardManager> {

    [SerializeField]private KeyCode[] Keys;
    [SerializeField]private Transform KeyboardPanel;
    [SerializeField]private GameObject KeyPrefab;
    private List<bl_Key> cacheKeys = new List<bl_Key>();

    /// <summary>
    /// 
    /// </summary>
	void Start()
    {
        InstanceKeys();
    }

    /// <summary>
    /// 
    /// </summary>
    public void InstanceKeys()
    {
        for(int i = 0; i < Keys.Length; i++)
        {
            GameObject k = Instantiate(KeyPrefab) as GameObject;
            k.GetComponent<bl_Key>().GetInfo(Keys[i]);
            cacheKeys.Add(k.GetComponent<bl_Key>());
            k.transform.SetParent(KeyboardPanel, false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetKeys()
    {
        for (int i = 0; i < cacheKeys.Count; i++)
        {
            cacheKeys[i].Reset();
        }
    }

    public void DesactiveAllKeys()
    {
        for (int i = 0; i < cacheKeys.Count; i++)
        {
            cacheKeys[i].DesactiveKey();
        }
    }

    public static bl_KeyboardManager Instance
    {
        get
        {
            return ((bl_KeyboardManager)mInstance);
        }
        set
        {
            mInstance = value;
        }
    }
}