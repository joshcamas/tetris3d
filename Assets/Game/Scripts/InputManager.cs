using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Spellcast
{
    public class InputManager : MonoBehaviour
    {
        public WorldGrid worldGrid;

        private void Update()
        {
            if (worldGrid.currentPiece == null)
                return;

            bool render = false;

            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                worldGrid.currentPiece.Move(new Vector3Int(-1, 0, 0));
                render = true;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                worldGrid.currentPiece.Move(new Vector3Int(1, 0, 0));
                render = true;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                worldGrid.currentPiece.Move(new Vector3Int(0, 0, 1));
                render = true;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                worldGrid.currentPiece.Move(new Vector3Int(0, 0, -1));
                render = true;
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                worldGrid.currentPiece.Rotate(new Vector3Int(1, 0, 0));
                render = true;
            }
            
            if (Input.GetKeyDown(KeyCode.W)) {
                worldGrid.currentPiece.Rotate(new Vector3Int(0, 0, 1));
                render = true;
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                worldGrid.currentPiece.Rotate(new Vector3Int(0, 0, 1));
                render = true;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                worldGrid.Drop();
                worldGrid.UpdateGrid();
                render = true;
            }

            if (render)
                worldGrid.RenderGrid();
        }

    }
}