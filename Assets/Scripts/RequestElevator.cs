using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestElevator : MonoBehaviour
{
    public ElevatorController[] elevators;

    public void RequestClosestElevator(int floorNo)
    {
        int closestElevatorSteps = int.MaxValue;
        int closestElevator = 0, tempClosestElevatorSteps = 0;

        for(int i = 0; i < elevators.Length; i++)
        {
            tempClosestElevatorSteps = elevators[i].StepsToReachFloor(floorNo);            

            if(closestElevatorSteps > tempClosestElevatorSteps)
            {
                closestElevatorSteps = tempClosestElevatorSteps;
                closestElevator = i;
            }
        }

        elevators[closestElevator].RequestElevator(floorNo);
    }
}
