using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goalie : MonoBehaviour
{
    Goal[] myGoals;
    Action[] myActions;
    Action changeOverTime;
    const float TIME_BTWN_TICKS = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        myGoals = new Goal[3];
        myGoals[0] = new Goal("Block Shot", 5f);
        myGoals[1] = new Goal("Hydrate", 3f);
        myGoals[2] = new Goal("Score", 1f);

        myActions = new Action[5];
        myActions[0] = new Action("block the shot");
        myActions[0].targetGoals.Add(new Goal("Block Shot", -6f));
        myActions[0].targetGoals.Add(new Goal("Hydrate", +3f));
        myActions[0].targetGoals.Add(new Goal("Score", +1f));
        
        myActions[1] = new Action("drink water");
        myActions[1].targetGoals.Add(new Goal("Block Shot", +2f));
        myActions[1].targetGoals.Add(new Goal("Hydrate", -2f));
        myActions[1].targetGoals.Add(new Goal("Score", +1f));
        
        myActions[2] = new Action("drink Gatorade");
        myActions[2].targetGoals.Add(new Goal("Block Shot", +1f));
        myActions[2].targetGoals.Add(new Goal("Hydrate", -4f));
        myActions[2].targetGoals.Add(new Goal("Score", +5f));
        
        myActions[3] = new Action("do the trebuchet throw thing");
        myActions[3].targetGoals.Add(new Goal("Block Shot", -3f));
        myActions[3].targetGoals.Add(new Goal("Hydrate", -2f));
        myActions[3].targetGoals.Add(new Goal("Score", +4f));
        
        myActions[4] = new Action("somehow score a goal?!");
        myActions[4].targetGoals.Add(new Goal("Block Shot", -1f));
        myActions[4].targetGoals.Add(new Goal("Hydrate", +2f));
        myActions[4].targetGoals.Add(new Goal("Score", -10f));

        changeOverTime = new Action("tick");
        changeOverTime.targetGoals.Add(new Goal("Block Shot", +3f));
        changeOverTime.targetGoals.Add(new Goal("Hydrate", +2f));
        changeOverTime.targetGoals.Add(new Goal("Score", +1f));

        Debug.Log("Starting clock. One hour will pass every " + TIME_BTWN_TICKS + " seconds.");
        InvokeRepeating("Tick", 0f, TIME_BTWN_TICKS);

        Debug.Log("Hit 'Space' to do something.");
    }

    void Tick()
    {
        foreach (Goal goal in myGoals)
        {
            goal.goalValue += changeOverTime.GetGoalChange(goal);
            goal.goalValue = Mathf.Max(goal.goalValue, 0);
        }

        PrintGoals();
    }

    void PrintGoals()
    {
        string goalString = "";
        foreach (Goal goal in myGoals)
        {
            goalString += goal.goalName + ": " + goal.goalValue + "; ";
        }
        goalString += "Discontentment: " + CurrentDiscontentment();
        Debug.Log(goalString);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Action bestAction = ChooseAction(myActions, myGoals);
            Debug.Log("I choose to " + bestAction.actionName);

            foreach(Goal goal in myGoals)
            {
                goal.goalValue += bestAction.GetGoalChange(goal);
                goal.goalValue = Mathf.Max(goal.goalValue, 0f);
            }

            PrintGoals();
        }
    }

    Action ChooseAction(Action[] actions, Goal[] goals)
    {
        Action bestActionToTake = null;
        float bestVal = float.PositiveInfinity;

        foreach(Action action in actions)
        {
            float currVal = Discontentment(action, goals);
            if(currVal < bestVal)
            {
                bestVal = currVal;
                bestActionToTake = action;
            }
        }

        return bestActionToTake;
    }

    float Discontentment(Action action, Goal[] goals)
    {
        float discontentment = 0f;

        foreach(Goal goal in goals)
        {
            float newVal = goal.goalValue + action.GetGoalChange(goal);
            newVal = Mathf.Max(newVal, 0f);

            discontentment += goal.GetDiscontentment(newVal);
        }

        return discontentment;
    }

    float CurrentDiscontentment()
    {
        float totalDiscontentment = 0f;

        foreach(Goal goal in myGoals)
        {
            totalDiscontentment += goal.GetDiscontentment(goal.goalValue);
        }

        return totalDiscontentment;
    }
}
