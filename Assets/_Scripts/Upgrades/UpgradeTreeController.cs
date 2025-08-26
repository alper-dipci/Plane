using System;
using Helpers;
using UnityEngine;

namespace _Scripts.Upgrades
{
    public class UpgradeTreeController : SingletonMonoBehaviour<UpgradeTreeController>
    {
        [SerializeField] private GameObject selectedNodeIndicator;
        [SerializeField] private Vector2 indicatorOffset;
         
        [SerializeField] private UpgradeNode parentNode;
        // private void Start()
        // {
        //     OpenNodesRecursively(parentNode);
        // }

        // private void OpenNodesRecursively(UpgradeNode node)
        // {
        //     if (node == null) return;
        //
        //     if (node.req)
        //         node.Open();
        //
        //     foreach (var child in node.Children)
        //     {
        //         OpenNodesRecursively(child);
        //     }
        // }

        public void OnSelectedNodeChanged(UpgradeNode node)
        {
            if(selectedNodeIndicator.activeSelf == false)
                selectedNodeIndicator.SetActive(true);
            
            selectedNodeIndicator.transform.position = (Vector2)node.transform.position + indicatorOffset;
        }
    }
}