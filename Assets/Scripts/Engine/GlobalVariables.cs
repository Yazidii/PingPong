using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Engine
{
    public static class GlobalVariables
    {
        public static int currentPosition;
        public static int previousPosition;
        public static int deviation;

        //Global
        public static bool cardIsActive;
        public static bool playerTurn;
        public static bool applicationQuitting = false;
        public static CardController activeCard;


    }
}
