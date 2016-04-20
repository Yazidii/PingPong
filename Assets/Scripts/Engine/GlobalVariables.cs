﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Engine
{
    public static class GlobalVariables
    {

        //Global gameplay
        public static int currentPosition;
        public static int previousPosition;
        public static int deviation;

        public static bool cardIsActive;
        public static bool playerTurn;
        public static bool applicationQuitting = false;
        public static CardController activeCard;

        //Game
        public static int playerScore;
        public static int enemyScore;

    }
}
