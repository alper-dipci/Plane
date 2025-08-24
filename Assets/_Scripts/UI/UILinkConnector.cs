using System.Collections.Generic;
using _Scripts.Upgrades;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEditor;

public class UILinkConnector : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField] private Image linkImage; // Bağlantı için Image
    [SerializeField] private GameObject NodesParent;

    [Button]
    public void SetAllLinks()
    {
        DestroyAllChildren();
        var nodes = NodesParent.GetComponentsInChildren<UpgradeNode>();
        RemoveAllConnections(nodes);

        foreach (var node in nodes)
        {
            if (node.childNodes.Count > 0)
                SetLinks(node, node.childNodes);
        }
    }

    private void RemoveAllConnections(UpgradeNode[] nodes)
    {
        foreach (var node in nodes)
        {
            foreach (var line in node.connectionLines)
            {
                if (line != null)
                    DestroyImmediate(line.gameObject);
            }

            node.connectionLines.Clear();
        }
    }

    private void DestroyAllChildren()
    {
        var children = new List<GameObject>();

        foreach (Transform child in transform)
            children.Add(child.gameObject);

        foreach (var child in children)
            DestroyImmediate(child);
    }

    private void SetLinks(UpgradeNode parentNode, List<UpgradeNode> childNodes)
    {
        foreach (var childNode in childNodes)
        {
            CreateLink(parentNode, childNode);
        }
    }

    private void CreateLink(UpgradeNode parentNode, UpgradeNode childNode)
    {
        var linkInstance = Instantiate(linkImage, transform);

        // İki nokta arası orta nokta
        Vector3 midPoint = (parentNode.transform.position + childNode.transform.position) / 2f;
        linkInstance.rectTransform.position = midPoint;

        // Yön ve uzunluk
        Vector3 dir = childNode.transform.position - parentNode.transform.position;
        float distance = dir.magnitude;

        // Uzatma (height'ta uzat)
        linkInstance.rectTransform.sizeDelta = new Vector2(linkInstance.rectTransform.sizeDelta.x, distance);

        // Rotasyon (height eksenine göre döndür)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        linkInstance.rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        parentNode.connectionLines.Add(linkInstance);
        childNode.parentNode = parentNode;
    
        EditorUtility.SetDirty(parentNode);
        EditorUtility.SetDirty(childNode);
    }

#endif
}