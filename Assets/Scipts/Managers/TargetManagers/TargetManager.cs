using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hydrogen
{

    public class TargetManager : MonoBehaviour
    {
        MovementManager temp = new MovementManager();
        private float SCALECONSTANT = 5;
        private Anchor[] _anchorList;
        private GameObject[] _activeAnchor;
        [SerializeField]
        private GameObject anchorPrefab;
        [SerializeField]
        private GameObject targetPrefab;

        private IEnumerator<Anchor> anchorList()
        {
            for(int i=0;i<_anchorList.Length;i++)
                yield return _anchorList[i];
        }
        
        #region Initialize
        /// <summary>
        /// Initializes the list of anchors so that it can spawn them during the wave
        /// </summary>
        /// <param name="difficultyRating">The difficulty of the wave based on how well the player did the last wave</param>
        /// <param name="difficultySetting">The setting the player chose for difficulty.  Easy =1, Medium = 2, Hard = 3</param>
        public void initializeWave(float difficultyRating,int difficultySetting)
        {

            int anchorCount = Mathf.RoundToInt(Mathf.Log(difficultyRating, SCALECONSTANT) - 0.5f);
            _anchorList = new Anchor[anchorCount];
            anchorList().Reset();
            for (int i = 0; i < anchorCount; i++)
            {
                _anchorList[i] = new Anchor();
                Anchor currentAnchor=_anchorList[i].GetComponent<Anchor>();
                //gives anchor a list of targets to spawn
                currentAnchor.setTargetList(generateTargetList(difficultyRating / anchorCount,difficultySetting));
                //Generate Anchor position in polar coordinates, with 0 radians being directly forward, around the player
                float angleOfPlay = Mathf.Deg2Rad * 180;
                float distanceScale = 10;
                float angle = Random.value * angleOfPlay - (angleOfPlay / 2);
                float distance = (anchorCount + Random.value) * distanceScale - (distanceScale / 2);
                //convert polar coordinates to cartesian
                Vector3 position = new Vector3(distance * Mathf.Cos(Mathf.Deg2Rad * angle), distance * Mathf.Sin(Mathf.Deg2Rad * angle));
                //Randomize initial Movement Vector
                Vector3 initialMovement = new Vector3(Random.value, Random.value, Random.value).normalized;
                currentAnchor.setInitial(position, 
                    initialMovement, 
                    //Generate Movement Delegate
                    temp.generateAnchorMovement(currentAnchor.transform, 
                        anchorCount, 
                        difficultySetting, 
                        difficultySetting / 6));
            }
        }
        /// <summary>
        /// Generate list of Targets based on difficulty level
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="difficultySetting"></param>
        /// <returns></returns>
        private Target[] generateTargetList(float difficulty,int difficultySetting)
        {
            int targetCount = Mathf.RoundToInt(Mathf.Log(difficulty, SCALECONSTANT) - 0.5f);
            Target[] tList = new Target[targetCount];
            for (int i = 0; i < targetCount; i++) 
            {
                tList[i] = generateTarget(difficulty / targetCount,difficultySetting);
            }
            return tList;
        }

        /// <summary>
        /// Generate Target based on Difficulty rating
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="difficultySetting"></param>
        /// <returns></returns>
        private Target generateTarget(float difficulty,int difficultySetting)
        {
            Target gen=new Target();
            float maxDistance = Mathf.Log(difficulty, SCALECONSTANT);
            float minDistance = Mathf.Log(difficulty, SCALECONSTANT * 5);
            //randomize Position and initial movement vector
            Vector3 position = new Vector3(Random.value * maxDistance - maxDistance / 2,
                Random.value * maxDistance - maxDistance / 2, 
                Random.value * maxDistance - maxDistance / 2);
            Vector3 movement = new Vector3(Random.value, 
                Random.value, 
                Random.value);
            movement = movement.normalized;
            gen.setInitial(position, 
                movement, 
                temp.generateTargetMovement(minDistance, 
                    maxDistance, 
                    difficultySetting, 
                    difficultySetting / 6));
            return gen;
        }


        #endregion

        void Update()
        {
            if (activeAnchors < 5)
            {
                anchorList().MoveNext();
                for(int i = 0; i < 5; i++)
                {
                    if(_activeAnchor[i]== null)
                    {
                        GameObject t = Instantiate(anchorPrefab);
                        _activeAnchor[i] = t;
                        Anchor curr = t.GetComponent<Anchor>();
                        curr.setInitial(anchorList().Current);

                    }
                }

            }
        }

        int activeAnchors
        {
            get
            {
                int i = 0;
                foreach (GameObject a in _activeAnchor)
                {
                    if (a!=null?(a.GetComponent < Anchor >() is Anchor):false) i++;
                }
                return i;
            }
        }


    }
}