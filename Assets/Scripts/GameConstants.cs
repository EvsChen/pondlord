using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants {
  public static int mBoardWidth = 8;
  public static int mBoardHeight = 8;
  public static int sunlight = 40;
  public static int countDownTime = 300;
  public static bool enablePlant = true;

  public readonly struct Tags {
     public static string lily = "lily",
                       protector = "protector",
                       bullet = "bullet",
                       cell = "cell",
                       fish = "fish",
                       frog = "frog";
  }
}
