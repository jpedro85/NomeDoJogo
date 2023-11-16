using System;
using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        #region Singleton

        public static Inventory instance;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("Theres more than one instance");
                return;
            }

            instance = this;
        }

        #endregion

        public delegate void onItemChanged();
        public onItemChanged OnItemChangedCallBack;

        public List<Item> items = new List<Item>();
        public int SPACE = 20;

        public bool add(Item item)
        {
            if (items.Count >= SPACE)
            {
                Debug.Log("Not Enough Space!");
                return false;
            }
            
            Debug.Log(item);
            items.Add(item);
            if (OnItemChangedCallBack != null)
                OnItemChangedCallBack.Invoke();
            return true;
        }

        public void remove(Item item)
        {
            items.Remove(item);
            if (OnItemChangedCallBack != null)
                OnItemChangedCallBack.Invoke();
        }
    }
}