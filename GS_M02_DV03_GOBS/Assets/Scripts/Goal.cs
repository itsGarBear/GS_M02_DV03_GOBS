using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public string goalName;
    public float goalValue;

    public Goal (string goalName, float goalValue)
    {
        this.goalName = goalName;
        this.goalValue = goalValue;
    }

    public float GetDiscontentment(float newVal)
    {
        return newVal * newVal;
    }
}

public class Action
{
    public string actionName;

    public List<Goal> targetGoals;

    public Action(string actionName)
    {
        this.actionName = actionName;
        targetGoals = new List<Goal>();
    }

    public float GetGoalChange(Goal myGoal)
    {
        foreach(Goal targetGoal in targetGoals)
        {
            if(targetGoal.goalName == myGoal.goalName)
            {
                return targetGoal.goalValue;
            }
        }

        return 0f;
    }
}
