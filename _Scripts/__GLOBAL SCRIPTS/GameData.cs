using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using Illumina.Models;
using Illumina.Serialization;
using UnityEngine;
using UnityEngine.UI;

public static class GameData {

    public static bool PlayDataLoaded = false;
    static User user = new User();
    static PlayData playdata = new PlayData();
    public static User User { get => user; set => user = value; }
    public static PlayData PlayData { get => playdata; set => playdata = value; }
}