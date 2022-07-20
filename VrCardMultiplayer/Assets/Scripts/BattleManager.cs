using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    public List<GameObject> Team1 = new List<GameObject>();
    public List<GameObject> Team2 = new List<GameObject>();

    public Transform Team1Pos;
    public Transform Team2Pos;

    public float Rotation;

    public int MaxCards = 5;
    public Vector3 Spacing = new Vector3(0.02f, 0f, 0f);
    public float[] Cords = { 0, 0, 0 };

    private void OnTriggerEnter(Collider other)
    {
        if(!(other.tag == "Card")) { return; }

        int team = other.GetComponent<CardDisplay>().Team;
        if(team == 1 && Team1.Count <= MaxCards)
        {
            Team1.Add(other.gameObject);
            UpdatePositions(Team1, Team1Pos, -1, Rotation);
        }
        else if(team == 2 && Team2.Count <= MaxCards)
        {
            Team2.Add(other.gameObject);
            UpdatePositions(Team2, Team2Pos, 1f, -Rotation);
        }
    }

    private void UpdatePositions(List<GameObject> Team, Transform Left, float dir, float Rot)
    {
        for(int i = 0; i<Team.Count; i++)
        {
            Team[i].transform.position = (Left.position + new Vector3(Cords[0] * i * dir, Cords[1], Cords[2]) + Spacing * dir);
            Team[i].transform.rotation = Quaternion.Euler(0, Rot, 0);
        }
    }
}
