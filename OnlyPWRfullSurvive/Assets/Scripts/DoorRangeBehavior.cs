using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRangeBehavior : PlayerOver
{
    [SerializeField]
    private string doorNumber;
    [SerializeField]
    private string buildingName;
    [SerializeField]
    private TextMesh text;

    [SerializeField] AudioClip successClip;

    private void Start() {
        text.text = doorNumber;
    }

    override protected void whenInRange() {
        //Debug.Log($"{doorNumber} {buildingName}");
        if (MissionHandler.executeMissionForRoomNumber(doorNumber, buildingName))
        {
            AudioSource.PlayClipAtPoint(successClip, transform.position);
        }
        // Debug.Log(timeToEnter + " " + TimeTracker.timeTracker + " " + (timeToEnter + maxHowLateSec));
        // if(timeToEnter <= TimeTracker.timeTracker && TimeTracker.timeTracker <= timeToEnter + maxHowLateSec) {
        //     gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        // }
        // else {
        //     gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        // }
    }
}
