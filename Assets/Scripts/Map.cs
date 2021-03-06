using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class Map : MonoBehaviour {
    [SerializeField] private Transform bottomLeftCorner;
    [SerializeField] private Transform topRightCorner;
    [SerializeField] private List<string> randomAreaNames;
    private List<int> randomAreas;
    private static Map instance;
    public static Map Instance { get => instance; }

    public void Awake() {
        randomAreas = randomAreaNames.Select((areaName) => NavMesh.GetAreaFromName(areaName)).ToList();
        instance = this;
    }

    public Vector3 BottomLeftCorner { get => bottomLeftCorner.position; }
    public Vector3 TopRightCorner { get => topRightCorner.position; }

    private int areaCount = 0;
    public int GetRandomArea() {
        int index = areaCount++;
        if (areaCount >= randomAreaNames.Count) areaCount = 0;
        return randomAreas[index];
    }
}
