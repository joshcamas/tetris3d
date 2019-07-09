using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Spellcast
{
    public class TetrisPiece : MonoBehaviour
    {
        public Color color;
        public Material material;
        public Vector3Int dimensions;

        public List<int> values;

        //Editor value
        public bool wireframeMode = false;

        private Material coloredMaterial;

        private Mesh generatedMesh;

        public Mesh GeneratedMesh
        {
            get
            {
                if (generatedMesh != null)
                    return generatedMesh;

                //Super terrible and slow way of generating this. TERRIBLE.
                List<CombineInstance> createdInstances = new List<CombineInstance>();

                int i = 0;
                for (int x = 0; x < dimensions.x; x++)
                {
                    for (int y = 0; y < dimensions.y; y++)
                    {
                        for (int z = 0; z < dimensions.z; z++)
                        {
                            Debug.Log(values[i]);
                            if (values[i] == 0)
                            {
                                i++;
                                continue;
                            }

                            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            block.transform.position = new Vector3(x, y, z);

                            CombineInstance inst = new CombineInstance();
                            inst.mesh = block.GetComponent<MeshFilter>().mesh;
                            inst.transform = block.transform.localToWorldMatrix;
                            createdInstances.Add(inst);

                            Debug.Log("pass");

                            Destroy(block);
                            i++;
                        }
                    }
                }

                generatedMesh = new Mesh();
                generatedMesh.CombineMeshes(createdInstances.ToArray());
                return generatedMesh;
            }
        }

        public Material ColoredMaterial
        {
            get
            {
                if(coloredMaterial == null)
                {
                    coloredMaterial = new Material(material);
                    coloredMaterial.color = color;
                }
                return coloredMaterial;
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            float blockSize = 1;
            Vector3 blockSize3 = Vector3.one * blockSize;
            
            //Bounding box
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position + ((Vector3)dimensions * blockSize) / 2, (Vector3)dimensions * blockSize);

            //Values
            int i = 0;
            for(int x=0;x<dimensions.x;x++)
            {
                for (int y = 0; y < dimensions.y; y++)
                {
                    for (int z = 0; z < dimensions.z; z++)
                    {
                        //Catch
                        if (values.Count <= i)
                            values.Add(0);

                        //Draw cube
                        if(values[i] == 1)
                        {
                            Vector3 pos = new Vector3(x, y, z);
                            Gizmos.color = color;

                            if(wireframeMode)
                                Gizmos.DrawWireCube(transform.position + pos + blockSize3 / 2, blockSize3 - new Vector3(0.1f, 0.1f, 0.1f));
                            else
                                Gizmos.DrawCube(transform.position + pos + blockSize3/2, blockSize3 - new Vector3(0.1f, 0.1f, 0.1f));
                        }

                        i++;
                    }
                }
            }

            //Remove extra values
            int max = dimensions.x * dimensions.y * dimensions.z;
            if (values.Count > dimensions.x * dimensions.y * dimensions.z)
            {
                values.RemoveRange(max, values.Count- max);
            }
        }

#endif

    }

}