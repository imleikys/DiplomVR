using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    static List<string> firstCircuit = new List<string>() {"c2000kpb c2000m", "c2000m c2000b", "c2000b rip12rs", "rip12rs c2000kdl"};
    static List<string> secondCircuit = new List<string>() {"c2000kdl mk13", "mk13 ipr513", "ipr513 mk13"};
    static List<string> thirdCircuit = new List<string>() { "c2000kpb exit", "exit pki1" };
    static List<string> connections = new List<string>();
    static GameObject Player;

    static GameObject winCanvas;
    static GameObject firstCircuitLight;
    static GameObject secondCircuitLight;
    static GameObject thirdCircuitLight;

    static bool isWin = false;
    static bool finish = false;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        winCanvas = GameObject.FindGameObjectWithTag("Canvas");
        winCanvas.SetActive(false);

        firstCircuitLight = GameObject.FindGameObjectWithTag("firstCircuitLight");
        secondCircuitLight = GameObject.FindGameObjectWithTag("secondCircuitLight");
        thirdCircuitLight = GameObject.FindGameObjectWithTag("thirdCircuitLight");

        firstCircuitLight.SetActive(false);
        secondCircuitLight.SetActive(false);
        thirdCircuitLight.SetActive(false);
        Debug.Log(Player);
    }

    private void Update()
    {
        if(isWin == true)
        {
            StartCoroutine(StartTimer(10f));
            isWin = false;
        }

        if(finish == true)
        {
            finish = false;
            StopAllCoroutines();
            Object.DestroyImmediate(Player);
            SceneManager.LoadScene("Game");
        }
    }

    public void OnHandleCap(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().isKinematic = false;
    }

    public static void OnWireConnect(string connection)
    {
        if (!connections.Contains(connection))
        {
            connections.Add(connection);
        }

        Debug.Log(connection);
        CheckConnections();
    }

    private static void CheckConnections()
    {
        SortConnections();
        if (firstCircuit.SequenceEqual(connections))
        {
            winCanvas.gameObject.SetActive(true);
            firstCircuitLight.SetActive(true);
            isWin = true;
        }
        else if (secondCircuit.SequenceEqual(connections))
        {
            winCanvas.gameObject.SetActive(true);
            secondCircuitLight.SetActive(true);
            isWin = true;
        } else if (thirdCircuit.SequenceEqual(connections))
        {
            winCanvas.gameObject.SetActive(true);
            thirdCircuitLight.SetActive(true);
            isWin = true;
        }
    }

    private static void SortConnections()
    {
        connections.Sort();
        firstCircuit.Sort();
        secondCircuit.Sort();
        thirdCircuit.Sort();
    }

    private IEnumerator StartTimer(float time)
    {
        while (time > 0)
        {
            time -= 1;
            yield return new WaitForSeconds(1);
        }

        finish = true;
    }
}

