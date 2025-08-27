using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace _Scripts.Upgrades
{
    [RequireComponent(typeof(Collider2D))]
    public class UpgradeNode : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private UpgradeNodeSO upgradeNodeSo;

        [SerializeField] private SpriteRenderer lockedSprite;
        [SerializeField] private SpriteRenderer progressSprite;
        [SerializeField] private SpriteRenderer maxedSprite;
        [SerializeField] private LineRenderer lineRenderer;

        public int currentLevel;

        [OnValueChanged(nameof(UpdateLines))] public List<UpgradeNode> childNodes = new();

        private Dictionary<UpgradeNode, LineRenderer> _connectionLines = new();
        
        public float dissolveDuration = 1f;
        public float outlineDuration = .1f;
        public float outlineThickness = 0.1f;

        private Coroutine _showOutlineCoroutine;
        public void UpdateLines()
        {
            // Remove old lines
            foreach (var connectionLine in _connectionLines.Values)
            {
                if (connectionLine != null)
                    DestroyImmediate(connectionLine.gameObject);
            }

            _connectionLines.Clear();

            // Create new lines
            foreach (var node in childNodes)
            {
                if (node == null) continue;
                var line = Instantiate(lineRenderer, transform.position, Quaternion.identity, transform);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, node.transform.position);
                _connectionLines[node] = line;
            }
        }

        private void OnMouseDown() => TryUpgrade();

        private void OnMouseEnter()
        {
            var material = GetComponent<SpriteRenderer>().material;
            material.SetFloat("_Thickness", outlineThickness);
            //UpgradeTreeController.Instance.OnSelectedNodeChanged(this);
        }
        
        private void OnMouseExit()
        {
            var material = GetComponent<SpriteRenderer>().material;
            material.SetFloat("_Thickness", 0);
        }
        

        private void TryUpgrade()
        {
            if (currentLevel >= upgradeNodeSo.MaxLevel)
                return;

            var upgradeData = upgradeNodeSo.GetUpgradeData(currentLevel + 1);
            var cost = upgradeData.costData;

            // TODO: currency kontrolü yap

            // TODO: currency düs

            currentLevel++;

            var effects = upgradeData.effects;
            foreach (var effect in effects)
                effect.ApplyEffect();

            UpdateVisual();


            foreach (var child in childNodes)
            {
                child.ShowNode();
            }
        }

        private void UpdateVisual()
        {
            var progressState = GetProgressState();
            switch (progressState)
            {
                case UpgradeNodeProgressState.Locked:
                    lockedSprite.enabled = true;
                    if (progressSprite != null)
                        progressSprite.enabled = false;
                    maxedSprite.enabled = false;
                    _connectionLines.ForEach(nodeLinePair => nodeLinePair.Value.enabled = false);
                    break;

                case UpgradeNodeProgressState.Maxed:
                    lockedSprite.enabled = false;
                    if (progressSprite != null)
                        progressSprite.enabled = false;
                    maxedSprite.enabled = true;
                    _connectionLines.ForEach(nodeLinePair => nodeLinePair.Value.enabled = true);
                    break;

                case UpgradeNodeProgressState.Unlocked:
                    lockedSprite.enabled = false;
                    if (progressSprite != null)
                        progressSprite.enabled = true;
                    maxedSprite.enabled = false;
                    _connectionLines.ForEach(nodeLinePair => nodeLinePair.Value.enabled = true);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsUnlocked()
        {
            return currentLevel > 0;
        }

        public void ShowNode()
        {
            gameObject.SetActive(true);
            _connectionLines.ForEach(nodeLinePair =>nodeLinePair.Value.enabled = true);
            UpdateVisual();
        }

        public void HideNode()
        {
            gameObject.SetActive(false);
            _connectionLines.ForEach(nodeLinePair => nodeLinePair.Value.enabled = false);
        }

        [Button]
        public void ShowNodeVisual()
        {
            var material = this.GetComponent<SpriteRenderer>().material;
            StartCoroutine(ShowMaterialCoroutine(material));
        }
        
        [Button]
        public void HideNodeVisual()
        {
            var material = this.GetComponent<SpriteRenderer>().material;
            StartCoroutine(HideMaterialCoroutine(material));
        }
        private IEnumerator ShowMaterialCoroutine(Material material)
        {
            float elapsed = 0f;
            var dissolve = material.GetFloat("_DisolveAmount");

            while (elapsed < dissolveDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / dissolveDuration;
                material.SetFloat("_DisolveAmount", Mathf.Lerp(dissolve, 0f, t));
                yield return null;
            }
            material.SetFloat("_DisolveAmount", 0);

        }
        private IEnumerator HideMaterialCoroutine(Material material)
        {
            float elapsed = 0f;
            var dissolve = material.GetFloat("_DisolveAmount");

            while (elapsed < dissolveDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / dissolveDuration;
                material.SetFloat("_DisolveAmount", Mathf.Lerp(dissolve, 1f, t));
                yield return null;
            }
            material.SetFloat("_DisolveAmount", 1);
        }
        
        

        public UpgradeNodeProgressState GetProgressState()
        {
            if (currentLevel == 0)
            {
                return UpgradeNodeProgressState.Locked;
            }
            else if (currentLevel >= upgradeNodeSo.MaxLevel)
            {
                return UpgradeNodeProgressState.Maxed;
            }
            else
            {
                return UpgradeNodeProgressState.Unlocked;
            }
        }
    }
}