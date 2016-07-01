using UnityEngine;
using System.Collections;
    /// <summary>
    /// Basically, the laser beam works by usign two transform and then
    /// each frame i set the line render to render between the two transforms 
    /// that are parented under the laserBeam cylinder object
    /// </summary>
    public class LaserBeam : MonoBehaviour
    {
        public LineRenderer laserBeam;
        public Transform endOfBeam;

        // Use this for initialization
        void Start()
        {
            //only need two vertices to render a line
            if (laserBeam != null)
            {
                laserBeam.SetVertexCount(2);
            }
        }

        // Update is called once per frame;
        //you need to set the line render every frame
        void Update()
        {
            Vector3[] verts = new Vector3[2];

            //im setting the postions according to this game objects transform which
            // is the start of the laser beam
            verts[0] = transform.position;
            verts[1] = endOfBeam.position;

            laserBeam.SetPositions(verts);
        }
    }
