using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gamelogic : MonoBehaviour
{
    public GameObject go;
    public GameObject wait;

    public TMP_Text bestTimeMesh;
    public TMP_Text recentTimesMesh;

    bool waitingToShowGo = true;
    bool waitingForButtonPress = false;
    bool updateStartTime = false;
    bool debounce = false;

    float goTime = 0.0f;

    float bestTime = 9000f;
    LinkedList<float> recentTimes = new LinkedList<float>();


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitingForButtonPress && !waitingToShowGo) {
            waitingToShowGo = true;
            goTime = Time.realtimeSinceStartup + UnityEngine.Random.value*5f+3f;
        }

        if (waitingToShowGo && Time.realtimeSinceStartup > goTime) {
            waitingToShowGo = false;
            waitingForButtonPress = true;
            updateStartTime = true;
            go.SetActive(true);
            wait.SetActive(false);
        }

        if (updateStartTime) {
            updateStartTime = false;
            goTime = Time.realtimeSinceStartup;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed!");

            if (!debounce && waitingForButtonPress) {
                debounce = true;
                waitingForButtonPress = false;
                float thisTime = ((Time.realtimeSinceStartup - goTime) * 1000f);
                Debug.Log("Response time: " + thisTime + "ms");
                
                go.SetActive(false);
                wait.SetActive(true);

                if (thisTime < bestTime) {
                    bestTime = thisTime;

                    bestTimeMesh.text = thisTime+"ms";
                }

                if (recentTimes.Count > 10) {
                    recentTimes.RemoveLast();
                }
                recentTimes.AddFirst(thisTime);

                string rt = "";
                foreach(float f in recentTimes) {
                    rt += f + "ms\n";
                }

                recentTimesMesh.text = rt;   
            }
        }
        else {
            debounce = false;
        }
    }
}
