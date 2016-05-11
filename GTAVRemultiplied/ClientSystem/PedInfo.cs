using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.ClientSystem
{
    public class PedInfo
    {
        public Vector3 WorldPos;

        public bool hasBlip;

        public BlipSprite blipSprite;

        public BlipColor blipColor;

        public Vector3 lPos = Vector3.Zero;

        public Vector3 lGoal = Vector3.Zero;

        public float speed = 0;

        double lastRun = 0;

        public Ped Character;

        bool running = false;

        public void Tick()
        {
            Vector3 rel = lGoal - lPos;
            float rlen = rel.Length();
            if (rlen > 0.01f && speed > 0.01f)
            {
                rel /= rlen;
                if (speed * GTAVFrenetic.cDelta > rlen || rlen > 10)
                {
                    StopMove();
                    lPos = lGoal;
                    Character.PositionNoOffset = lGoal;
                }
                else
                {
                    AnimateMove(speed > 3);
                    lPos = lPos + rel * speed * GTAVFrenetic.cDelta;
                    Character.PositionNoOffset = lPos;
                }
            }
            else
            {
                StopMove();
                lPos = lGoal;
                Character.PositionNoOffset = lGoal;
            }
        }

        public void AnimateMove(bool run)
        {
            if (GTAVFrenetic.GlobalTickTime - lastRun > 5)
            {
                if (run)
                {
                    Function.Call(Hash.REQUEST_ANIM_DICT, "MOVE_M@MULTIPLAYER");
                    Function.Call(Hash.TASK_PLAY_ANIM, Character.Handle, "MOVE_M@MULTIPLAYER", "run", 8f, 1f, 5000, 1, 1f, false, false, false);
                }
                else
                {
                    Function.Call(Hash.REQUEST_ANIM_DICT, "MOVE_M@NON_CHALANT");
                    Function.Call(Hash.TASK_PLAY_ANIM, Character.Handle, "MOVE_M@NON_CHALANT", "walk", 8f, 1f, 5000, 1, 1f, false, false, false);
                }
                lastRun = GTAVFrenetic.GlobalTickTime;
                running = true;
            }
        }

        public void StopMove()
        {
            lastRun = 0.0;
            if (running)
            {
                Function.Call(Hash.TASK_PLAY_ANIM, Character.Handle, "MOVE_M@MULTIPLAYER", "run", 8f, 1f, 0, 1, 1f, false, false, false);
                running = false;
            }
        }

    }
}
