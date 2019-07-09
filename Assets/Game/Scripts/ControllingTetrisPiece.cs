using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellcast
{
    public class ControllingTetrisPiece : MonoBehaviour
    {
        public WorldGrid grid;
        public int pieceIndex;
        public TetrisPiece piece;

        public Vector3Int rotation;

        public void Init()
        {
            ApplyToGrid();
        }

        public bool Rotate(Vector3Int rotate)
        {
            //Check if possible
            if (Overlaps(GetGridPosition(), WrapRotation(rotation + rotate),true, true))
                return false;

            UnApplyFromGrid();

            rotation = WrapRotation(rotation + rotate);

            ApplyToGrid();

            return true;
        }

        public bool Move(Vector3Int move)
        {
            //Check if possible
            if (Overlaps(GetGridPosition() + move, rotation, true,true))
                return false;

            UnApplyFromGrid();
            
            transform.position += (Vector3)move * grid.blockSize;

            ApplyToGrid();

            return true;
        }

        private void UnApplyFromGrid()
        {
            ForEachGridBlock((gridPos, valueIndex) =>
            {
                if (piece.values[valueIndex] == 1)
                    grid.SetGridPoint(gridPos,0);
            }, GetGridPosition(), rotation);
        }

        private void ApplyToGrid()
        {
            ForEachGridBlock((gridPos, valueIndex) =>
            {
                if(piece.values[valueIndex] == 1)
                    grid.SetGridPoint(gridPos,(pieceIndex + 1));
            }, GetGridPosition(),rotation);
            
        }

        public Vector3Int GetGridPosition()
        {
            Vector3 gp = (transform.position - grid.transform.position) / grid.blockSize;
            return new Vector3Int(Mathf.FloorToInt(gp.x), Mathf.FloorToInt(gp.y), Mathf.FloorToInt(gp.z));
        }

        public bool InBounds(Vector3Int position, Vector3Int rotation, bool ignorePositiveYInBounds = false)
        {
            bool inbounds = true;

            //Must unapply to ensure we don't overlap with self
            UnApplyFromGrid();

            ForEachGridBlock((gridPos, valueIndex) =>
            {
                //Already know we're out of bounds, so don't care
                if (!inbounds)
                    return;

                if (piece.values[valueIndex] == 0)
                    return;

                if (!grid.GridPointInBounds(gridPos, ignorePositiveYInBounds))
                    inbounds = false;

            }, position, rotation);

            ApplyToGrid();

            return inbounds;
        }

        //Returns whether a specific rotation and position would cause overlaps
        public bool Overlaps(Vector3Int position,Vector3Int rotation, bool boundsCheck, bool ignorePositiveYInBounds = false)
        {
            bool overlaps = false;

            //Must unapply to ensure we don't overlap with self
            UnApplyFromGrid();

            ForEachGridBlock((gridPos, valueIndex) =>
            {
                //Already know we overlap, so don't care
                if (overlaps)
                    return;

                if (piece.values[valueIndex] == 0)
                    return;

                int gp = grid.GetGridPoint(gridPos);

                if (gp > 0)
                {
                    Debug.Log(gp);
                    overlaps = true;
                }

                if(gp == -1 && boundsCheck)
                    if(!ignorePositiveYInBounds || !grid.GridPointInBounds(gridPos, true))
                        overlaps = true;

            }, position, rotation);

            ApplyToGrid();
            return overlaps;
        }

        private void ForEachGridBlock(System.Action<Vector3Int, int> function,Vector3Int position,Vector3Int rotation)
        {
            //Position of tetris piece in grid space
           // Vector3 gp = (transform.position - grid.transform.position) / grid.blockSize;
            //Vector3Int gridPositionInt = new Vector3Int(Mathf.FloorToInt(gp.x), Mathf.FloorToInt(gp.y), Mathf.FloorToInt(gp.z));

            //No rotation for now (!!!!)
            int i = 0;
            for (int x = 0; x < piece.dimensions.x; x++)
            {
                for (int y = 0; y < piece.dimensions.y; y++)
                {
                    for (int z = 0; z < piece.dimensions.z; z++)
                    {
                        Vector3Int bp = new Vector3Int(x, y, z);

                        //X
                        if (rotation.x == 1)
                            bp = new Vector3Int(bp.x, -bp.z, bp.y);

                        else if (rotation.x == 2)
                            bp = new Vector3Int(bp.x, -bp.y, -bp.z);

                        else if (rotation.x == 3)
                            bp = new Vector3Int(bp.x, bp.z, -bp.y);

                        //Y
                        if (rotation.y == 1)
                            bp = new Vector3Int(-bp.z, bp.y, bp.x);

                        else if (rotation.y == 2)
                            bp = new Vector3Int(-bp.x, bp.y, -bp.z);

                        else if (rotation.y == 3)
                            bp = new Vector3Int(bp.z, bp.y, -bp.x);

                        //Z
                        if (rotation.z == 1)
                            bp = new Vector3Int(bp.y, -bp.x, bp.z);

                        else if (rotation.z == 2)
                            bp = new Vector3Int(-bp.x, -bp.y, bp.z);

                        else if (rotation.z == 3)
                            bp = new Vector3Int(-bp.y, bp.x, bp.z);

                        function(position + bp, i);
                        i++;
                    }
                }
            }
        }

        private Vector3Int WrapRotation(Vector3Int rotation)
        {
            if (rotation.x > 3)
                rotation.x = 0;

            if (rotation.y > 3)
                rotation.y = 0;

            if (rotation.z > 3)
                rotation.z = 0;

            return rotation;
        }
    }
}

