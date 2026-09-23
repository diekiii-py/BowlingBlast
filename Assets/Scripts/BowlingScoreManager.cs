using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class BowlingRoll
{
    public int pinsDown;
    public List<int> fallenPinIndices = new List<int>();

    public BowlingRoll(bool[] fallenPins)
    {
        for (int i = 0; i < fallenPins.Length; i++)
        {
            if (!fallenPins[i])
                continue;

            pinsDown++;
            fallenPinIndices.Add(i);
        }
    }
}

public class BowlingScoreManager : MonoBehaviour
{
    public List<BowlingRoll> rolls = new List<BowlingRoll>();
    public int currentFrame;
    public int rollsInCurrentFrame;

    public void RecordRoll(bool[] fallenPins)
    {
        if (fallenPins == null || IsGameComplete())
            return;

        BowlingRoll roll = new BowlingRoll(fallenPins);
        rolls.Add(roll);

        if (currentFrame < 9)
        {
            if (rollsInCurrentFrame == 0 && roll.pinsDown == 10)
            {
                currentFrame++;
                return;
            }

            rollsInCurrentFrame++;
            if (rollsInCurrentFrame == 2)
            {
                currentFrame++;
                rollsInCurrentFrame = 0;
            }

            return;
        }

        rollsInCurrentFrame++;
        if (rollsInCurrentFrame == 1 && roll.pinsDown < 10)
            return;

        if (rollsInCurrentFrame == 2 && rolls[rolls.Count - 2].pinsDown < 10 &&
            rolls[rolls.Count - 2].pinsDown + roll.pinsDown < 10)
            currentFrame++;
        else if (rollsInCurrentFrame == 3)
            currentFrame++;
    }

    public bool IsGameComplete()
    {
        return currentFrame >= 10;
    }

    public int GetFrameScore(int frame)
    {
        if (frame < 0 || frame > 9)
            return 0;

        int rollIndex = GetFrameStartRoll(frame);
        if (rollIndex >= rolls.Count)
            return 0;

        BowlingRoll first = rolls[rollIndex];
        if (frame < 9 && first.pinsDown == 10)
        {
            if (rollIndex + 2 >= rolls.Count)
                return 0;

            return 10 + rolls[rollIndex + 1].pinsDown + rolls[rollIndex + 2].pinsDown;
        }

        if (rollIndex + 1 >= rolls.Count)
            return 0;

        BowlingRoll second = rolls[rollIndex + 1];
        if (frame < 9 && first.pinsDown + second.pinsDown == 10)
        {
            if (rollIndex + 2 >= rolls.Count)
                return 0;

            return 10 + rolls[rollIndex + 2].pinsDown;
        }

        return first.pinsDown + second.pinsDown;
    }

    public string GetFrameMark(int frame)
    {
        int rollIndex = GetFrameStartRoll(frame);
        if (rollIndex >= rolls.Count)
            return "-";

        BowlingRoll first = rolls[rollIndex];
        if (frame < 9 && first.pinsDown == 10)
            return "X";

        if (rollIndex + 1 >= rolls.Count)
            return first.pinsDown.ToString();

        BowlingRoll second = rolls[rollIndex + 1];
        if (frame < 9 && first.pinsDown + second.pinsDown == 10)
            return first.pinsDown + "/";

        return first.pinsDown + "|" + second.pinsDown;
    }

    public string GetScoreboardText()
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine("BOWLING");
        text.Append("FR ");
        for (int frame = 0; frame < 10; frame++)
            text.AppendFormat("{0,5}", frame + 1);

        text.AppendLine();
        text.Append("RK ");
        for (int frame = 0; frame < 10; frame++)
            text.AppendFormat("{0,5}", GetFrameMark(frame));

        text.AppendLine();
        text.Append("SC ");
        int runningScore = 0;
        for (int frame = 0; frame < 10; frame++)
        {
            int frameScore = GetFrameScore(frame);
            if (frameScore > 0)
                runningScore += frameScore;

            text.AppendFormat("{0,5}", frameScore > 0 ? runningScore.ToString() : "-");
        }

        return text.ToString();
    }

    public BowlingRoll GetRoll(int rollIndex)
    {
        if (rollIndex < 0 || rollIndex >= rolls.Count)
            return null;

        return rolls[rollIndex];
    }

    private int GetFrameStartRoll(int frame)
    {
        int rollIndex = 0;
        for (int current = 0; current < frame; current++)
        {
            if (rollIndex >= rolls.Count)
                return rolls.Count;

            rollIndex += rolls[rollIndex].pinsDown == 10 ? 1 : 2;
        }

        return rollIndex;
    }
}
