using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Spellcast
{
    public class NextPieceVisual : MonoBehaviour
    {
        public WorldGrid worldGrid;

        private int showingValue = -1;
        private GameObject spawnedVisual;
        private MeshFilter visualFilter;
        private MeshRenderer visualRenderer;

        private void Update()
        {
            if(worldGrid.nextPieceIndex != showingValue)
            {
                showingValue = worldGrid.nextPieceIndex;

                if (spawnedVisual == null)
                {
                    spawnedVisual = new GameObject();
                    spawnedVisual.transform.parent = transform;
                    spawnedVisual.transform.localPosition = Vector3.zero;

                    visualFilter = spawnedVisual.AddComponent<MeshFilter>();
                    visualRenderer = spawnedVisual.AddComponent<MeshRenderer>();
                }

                visualFilter.sharedMesh = worldGrid.GetPiece(showingValue).GeneratedMesh;
                visualRenderer.sharedMaterial = worldGrid.GetMaterial(showingValue);
            }
        }
    }

}