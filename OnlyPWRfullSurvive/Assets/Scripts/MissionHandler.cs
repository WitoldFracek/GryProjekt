using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MissionHandler {
    public static List<OnTimeMission> allOnTimeMissions;
    public static List<CollectMission> allCollectMissions;
    public static List<TimeRestrictedMission> allTimeRestrictedMissions;
    public static List<ExecutableMission> allExecutableMissions;

    public static List<Mission> getAllMissions(bool relevant=true) {
        var missionList = new List<Mission>();
        foreach (var mission in GetAllMissions()) {
            if (mission.isStillRelevant() == relevant) {
                missionList.Add(mission);
            }
        }
        return missionList;
    }

    public static bool areMissionsSetup(){
        return allCollectMissions != null;
    }

    public static int GetCurrentECTSCount() {
        var count = 0;
        foreach (var mission in GetAllMissions()) {
            if(mission.WasFinalised && mission.GetType() != typeof(CollectMission)) {
                count += mission.GetECTSPoints();
            }
            else if (mission.GetType() == typeof(CollectMission)) {
                count += mission.GetECTSPoints();
            }
        }
        return count;
    }
    
    public static void setLevel1Missions() {
        allOnTimeMissions = new List<OnTimeMission>();
        allCollectMissions = new List<CollectMission>();
        allExecutableMissions = new List<ExecutableMission>();
        allTimeRestrictedMissions = new List<TimeRestrictedMission>();

        allOnTimeMissions.Add(new OnTimeMission(9, "101", "A1", 60f * 2f) { Description = "Lecture Data warehouse"});
        allCollectMissions.Add(new CollectMission(5, 5, "CD") { Description = "Collect CDs"});
        allExecutableMissions.Add(new ExecutableMission(9, 60f * 2f) { Description = "Report Data warehouse"});
        allTimeRestrictedMissions.Add(new TimeRestrictedMission(7, "102", "Dean", 60f * 4f) { Description = "Dean's office paperwork"});
    }

    public static void setLevel2Missions() {
        allOnTimeMissions = new List<OnTimeMission>();
        allCollectMissions = new List<CollectMission>();
        allExecutableMissions = new List<ExecutableMission>();
        allTimeRestrictedMissions = new List<TimeRestrictedMission>();

        allOnTimeMissions.Add(new OnTimeMission(5, "104", "A1", 60f*1f) { Description = "Lecture Microsoft admin"});
        allOnTimeMissions.Add(new OnTimeMission(5, "104", "C13", 60f*4f) { Description = "Laboratory Android"});

        allCollectMissions.Add(new CollectMission(5, 5, "PENDRIVE") { Description = "Collect pendrives"});

        allExecutableMissions.Add(new ExecutableMission(5, 30f) { Description = "Report Cybersecurity"});
        allExecutableMissions.Add(new ExecutableMission(5, 60f) { Description = "App Android"});

        allTimeRestrictedMissions.Add(new TimeRestrictedMission(5, "101", "Dean", 60f * 4f) { Description = "Sticker student ID"});
    }

    public static void setLevel3Missions() {
        allOnTimeMissions = new List<OnTimeMission>();
        allCollectMissions = new List<CollectMission>();
        allExecutableMissions = new List<ExecutableMission>();
        allTimeRestrictedMissions = new List<TimeRestrictedMission>();

        allOnTimeMissions.Add(new OnTimeMission(2, "105", "A1", 60f) { Description = "Lecture Microsoft admin"});
        allOnTimeMissions.Add(new OnTimeMission(2, "103", "C13", 60f*2f) { Description = "Laboratory Android"});
        allOnTimeMissions.Add(new OnTimeMission(2, "103", "A1", 60f*3f) { Description = "Lecture Data warehouse"});
        allOnTimeMissions.Add(new OnTimeMission(2, "102", "C13", 60f*4f) { Description = "Laboratory .NET"});

        allCollectMissions.Add(new CollectMission(7, 7, "CIRCUT_BOARD") { Description = "Collect Circut boards"});

        allExecutableMissions.Add(new ExecutableMission(3, 60f) { Description = "Code checkers MIN-MAX"});
        allExecutableMissions.Add(new ExecutableMission(2, 30f) { Description = "Report "});
        allExecutableMissions.Add(new ExecutableMission(4, 90f) { Description = "Website .NET"});

        allTimeRestrictedMissions.Add(new TimeRestrictedMission(6, "103", "Dean", 60f * 2.5f) { Description = "Internship paperwork"});
    }

    public static bool executeMissionForRoomNumber(string roomNumber, string buildingName) {
        var allRoomRelatedMissions = new List<RoomRelatedMission>();
        allRoomRelatedMissions.AddRange(allOnTimeMissions);
        allRoomRelatedMissions.AddRange(allTimeRestrictedMissions);
        foreach(var mission in allRoomRelatedMissions) {
            if(mission.roomNumber == roomNumber && mission.buildingName == buildingName) {
                return mission.setFinalisedIfAllowed();
            }
        }
        return false;
    }
    // public static void CollectOneItem(string itemTag) {
    //     var mission = allCollectMissions.FirstOrDefault(m => m.CollectableTag == itemTag);
    //     if(mission != null) {
    //         mission.AddOneItem();
    //     }
        
    // }
    public static void ExecuteExecutableMission(int missionInx) {
        allExecutableMissions[missionInx].setFinalisedIfAllowed();
    }

    public static List<Mission> GetAllMissions()
    {
        List<Mission> allMissions = new List<Mission>();
        if(allOnTimeMissions != null)
            allMissions.AddRange(allOnTimeMissions);
        if (allTimeRestrictedMissions != null)
            allMissions.AddRange(allTimeRestrictedMissions);
        if (allCollectMissions != null)
            allMissions.AddRange(allCollectMissions);
        if (allExecutableMissions != null)
            allMissions.AddRange(allExecutableMissions);
        return allMissions;
    }

    public static List<Mission> GetAllUnfinishedMissions()
    {
        return GetAllMissions().FindAll(m => !m.WasFinalised).ToList();
    }

    public static List<Mission> GetAllImpossibleToFinishMissions()
    {
        return GetAllMissions().FindAll(m => !m.canBeFinalised()).ToList();
    }

    public static int GetUnfinishedMissionsCount() {
        int counter = 0;
        List<Mission> allMissions = GetAllMissions();
        foreach(var mission in allMissions) {
            if(!mission.WasFinalised) {
                counter += 1;
            }
        }
        return counter;
    }

    public static void PassCollectable(CollectableHandler collectable)
    {
        foreach(var mission in allCollectMissions)
        {
            if(mission.CollectableTag == collectable.collectableTag)
            {
                mission.AddOneItem(collectable.id);
            }
        }
    }

    public static void ExecuteAllExecutableMissions()
    {
        foreach(var mission in allExecutableMissions)
        {
            mission.WasFinalised = true;
        }
    }

    public static void PassAllOnTimeMissions()
    {
        foreach(var mission in allOnTimeMissions)
        {
            mission.WasFinalised = true;
        }
    }
}