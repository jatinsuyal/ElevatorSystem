using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public int totalNumberOfFloors = 9, currentFloor = 0, nextFloor, lowestFloor = 0, targetFloor = 0;
    public Queue<int> elevatorFloorInputQueue = new Queue<int>();

    public int passengerInsideElevator = 0;

    public ElevatorCurrentMotion elevatorCurrentMotion = ElevatorCurrentMotion.NotMoving;
    public ElevatorCurrentMotion elevatorLastMotion = ElevatorCurrentMotion.MovingUp;

    public float maxElevatorPosError = 0.2f, elevatorSpeed = 1f;

    public Transform elevator;
    public Transform[] elevatorsFloorPositions;

    private Coroutine startGoUp = null, startGoDown = null;

    public void MoveElevator()
    {
        if(elevatorFloorInputQueue.Count > 0)
        {
            DecideNextFloor();

            if(nextFloor > currentFloor)
            {
                startGoUp = StartCoroutine(GoUp(nextFloor - currentFloor));
            }
            else
            {
                startGoDown =  StartCoroutine(GoDown(currentFloor - nextFloor));
            }
        }
        else
        {
            //nowhere to move do nothing
        }
    }

    public void DecideNextFloor()
    {
        if (elevatorFloorInputQueue.Count > 0)
        {
            nextFloor = elevatorFloorInputQueue.Dequeue();
        } 
        else
        {
            // no Next destination
            return;
        }

        int dequeueEle = nextFloor;
        
        //to check if next floor is in direction of elevator
        nextFloor = CheckNextFloorInDirectionOfElevator();

        if(nextFloor == dequeueEle)
        {
            //Check For Direction Change
            if (elevatorLastMotion == ElevatorCurrentMotion.MovingUp)
            {
                if (nextFloor < currentFloor)
                {
                    //means direction is changed
                    //to check if next floor is in opposite direction of elevator
                    nextFloor = CheckNextFloorInOppositeDirectionOfElevator();
                }
            }
            else if (elevatorLastMotion == ElevatorCurrentMotion.MovingDown)
            {
                if(nextFloor > currentFloor)
                {
                    //means direction is changed
                    //to check if next floor is in opposite direction of elevator
                    nextFloor = CheckNextFloorInOppositeDirectionOfElevator();
                }
            }

        }

        if (nextFloor == dequeueEle)
        {

        }
        else
        {
            //add dequeueEle next back to queue
            elevatorFloorInputQueue.Enqueue(dequeueEle);
        }

        Queue<int> newQueue = new Queue<int>();

        foreach(int ele in elevatorFloorInputQueue)
        {
            if(ele == nextFloor)
            {
                continue;
            }
            else
            {
                newQueue.Enqueue(ele);
            }
        }

        elevatorFloorInputQueue = newQueue;
    }

    public int CheckNextFloorInDirectionOfElevator()
    {
        int tempNextFloor = nextFloor;

        foreach (int remainingFloor in elevatorFloorInputQueue)
        {
            if (elevatorLastMotion == ElevatorCurrentMotion.MovingUp)
            {
                if (remainingFloor < tempNextFloor && remainingFloor > currentFloor)
                {
                    //next floor is up
                    tempNextFloor = remainingFloor;
                }
                else
                {
                    //next floor is down
                }
            }
            else if (elevatorLastMotion == ElevatorCurrentMotion.MovingDown)
            {
                if (remainingFloor > tempNextFloor && remainingFloor < currentFloor)
                {
                    //next floor is down
                    tempNextFloor = remainingFloor;
                }
                else
                {
                    //next floor is up
                }
            }
        }

        return tempNextFloor;
    }

    public int StepsToReachFloor(int floorNo)
    {
        int steps = 0;

        if(elevatorCurrentMotion == ElevatorCurrentMotion.NotMoving)
        {
            int expectedNextFloor = CheckNextFloorInDirectionOfElevator();

            if(expectedNextFloor > currentFloor)
            {
                //will go up
                if (floorNo > currentFloor)
                {
                    steps = floorNo - currentFloor;
                }
                else
                {
                    int nextMaxFloor = CheckLastFloorInDirectionOfElevator();

                    steps = Mathf.Abs(currentFloor - nextMaxFloor) + Mathf.Abs(floorNo - nextMaxFloor);
                }
            }
            else
            {
                //going down
                if (floorNo < currentFloor)
                {
                    steps = currentFloor - floorNo;
                }
                else
                {
                    int nextMaxFloor = CheckLastFloorInDirectionOfElevator();

                    steps = Mathf.Abs(currentFloor - nextMaxFloor) + Mathf.Abs(floorNo - nextMaxFloor);
                }
            }
        }
        else
        {
            if(elevatorCurrentMotion == ElevatorCurrentMotion.MovingUp)
            {
                if (floorNo > currentFloor)
                {
                    steps = floorNo - currentFloor;
                }
                else
                {
                    int nextMaxFloor = CheckLastFloorInDirectionOfElevator();

                    steps = Mathf.Abs(currentFloor - nextMaxFloor) + Mathf.Abs(floorNo - nextMaxFloor);
                }
            }
            else
            {
                if (floorNo < currentFloor)
                {
                    steps = currentFloor - floorNo;
                }
                else
                {
                    int nextMaxFloor = CheckLastFloorInDirectionOfElevator();

                    steps = Mathf.Abs(currentFloor - nextMaxFloor) + Mathf.Abs(floorNo - nextMaxFloor);
                }
            }
        }

        return steps;
    }


    public int CheckLastFloorInDirectionOfElevator()
    {
        int lastFloor = nextFloor;

        foreach (int remainingFloor in elevatorFloorInputQueue)
        {
            if (elevatorLastMotion == ElevatorCurrentMotion.MovingUp)
            {
                if (remainingFloor > currentFloor && remainingFloor > lastFloor)
                {
                    //next floor is up
                    lastFloor = remainingFloor;
                }
                else
                {
                    //next floor is down
                }
            }
            else if (elevatorLastMotion == ElevatorCurrentMotion.MovingDown)
            {
                if (remainingFloor < currentFloor && remainingFloor < lastFloor)
                {
                    //next floor is down
                    lastFloor = remainingFloor;
                }
                else
                {
                    //next floor is up
                }
            }
        }


        return lastFloor;
    }

    public int CheckNextFloorInOppositeDirectionOfElevator()
    {
        int tempNextFloor = nextFloor;

        foreach (int remainingFloor in elevatorFloorInputQueue)
        {
            if (elevatorLastMotion == ElevatorCurrentMotion.MovingUp)
            {
                if (remainingFloor < tempNextFloor && remainingFloor > currentFloor)
                {
                    //next floor is up
                }
                else
                {
                    //next floor is down
                    tempNextFloor = remainingFloor;
                }
            }
            else if (elevatorLastMotion == ElevatorCurrentMotion.MovingDown)
            {
                if (remainingFloor > tempNextFloor && remainingFloor < currentFloor)
                {
                    //next floor is down
                }
                else
                {
                    //next floor is up
                    tempNextFloor = remainingFloor;
                }
            }
        }

        return tempNextFloor;
    }

    public void ChangeElevatorMotionDirection(ElevatorCurrentMotion elevatorNewMotion)
    {
        elevatorLastMotion = elevatorNewMotion;
    }

    public IEnumerator GoUp(int steps)
    {
        elevatorCurrentMotion = ElevatorCurrentMotion.MovingUp;
        Debug.Log($"Moving UP {steps} steps.");

        //Player Gravity
       // Player.gravityValue = -1f;

        targetFloor = nextFloor;
        nextFloor = currentFloor + 1;

        while (targetFloor >= nextFloor)
        {
            while (Mathf.Abs(elevator.position.y - elevatorsFloorPositions[nextFloor].position.y) > maxElevatorPosError)
            {
                elevator.position = Vector3.MoveTowards(elevator.position, elevatorsFloorPositions[nextFloor].position, elevatorSpeed * Time.deltaTime);
                yield return null;
            }

            currentFloor++;
            nextFloor++;
        }

        //reached
        elevatorLastMotion = ElevatorCurrentMotion.MovingUp;
        Stop();
    }

    public IEnumerator GoDown(int steps)
    {
        elevatorCurrentMotion = ElevatorCurrentMotion.MovingDown;
        Debug.Log($"Moving Down {steps} steps.");

        targetFloor = nextFloor;
        nextFloor = currentFloor - 1;

        while (targetFloor <= nextFloor)
        {
            while (Mathf.Abs(elevator.position.y - elevatorsFloorPositions[nextFloor].position.y) > maxElevatorPosError)
            {
                elevator.position = Vector3.MoveTowards(elevator.position, elevatorsFloorPositions[nextFloor].position, elevatorSpeed * Time.deltaTime);
                yield return null;
            }

            currentFloor--;
            nextFloor--;
        }
        //reached
        elevatorLastMotion = ElevatorCurrentMotion.MovingDown;
        Stop();
    }


    public void Stop()
    {
        currentFloor = nextFloor;
        elevatorCurrentMotion = ElevatorCurrentMotion.NotMoving;
        Debug.Log("Stopping");

       // Player.gravityValue = -10f;

    }

    public void RequestElevator(int floorNo)
    {
        if(floorNo > totalNumberOfFloors)
        {
            Debug.LogError($"Floor number {floorNo} doesn't exist.");
            return;
        }
        else
        {
            if(!elevatorFloorInputQueue.Contains(floorNo))
                elevatorFloorInputQueue.Enqueue(floorNo);
        }

        if (elevatorCurrentMotion == ElevatorCurrentMotion.NotMoving)
        {
            MoveElevator();
        }
        else
        {
            if(startGoUp != null)
                StopCoroutine(startGoUp);

            if(startGoDown != null)
                StopCoroutine(startGoDown);

            if(elevatorCurrentMotion == ElevatorCurrentMotion.MovingUp)
            {
                if(currentFloor <= floorNo && floorNo <= targetFloor)
                {
                    elevatorFloorInputQueue.Enqueue(targetFloor);
                    MoveElevator();
                }
            }
            else
            {
                if (currentFloor >= floorNo && floorNo >= targetFloor)
                {
                    elevatorFloorInputQueue.Enqueue(targetFloor);
                    MoveElevator();
                }
            }
        }
    }
}

public enum ElevatorCurrentMotion
{
    MovingUp,
    MovingDown,
    NotMoving
}

