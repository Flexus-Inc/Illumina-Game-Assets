using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using Illumina.Models;
using Illumina.Serialization;
using UnityEngine;
using UnityEngine.UI;

public static class GameData {

    static User user = new User();

    public static User User { get => user; set => user = value; }

}