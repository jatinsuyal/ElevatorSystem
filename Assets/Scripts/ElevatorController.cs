using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public int totalNumberOfFloors = 9, currentFloor = 0, nextFloor, lowestFloor = 0;
    public Queue<int> elevatorFloorInputQueue = new Queue<int>();

    public int passengerInsideElevator = 0;

    public ElevatorCurrentMotion elevatorCurrentMotion = ElevatorCurrentMotion.NotMoving;
    public ElevatorCurrentMotion elevatorLastMotion = ElevatorCurrentMotion.MovingUp;

    public float maxElevatorPosError = 0.2f, elevatorSpeed = 1f;

    public Transform elevator;
    public Transform[] elevatorsFloorPositions;
    public KeyCode UserKey;
    public List<KeyCode> KeyCodeForFloors;

    public void Update()
    {
        if (Input.GetKeyDown(UserKey/*B*/) && elevatorCurrentMotion == ElevatorCurrentMotion.NotMoving)
        {
            MoveElevator();
        }

        if (Input.GetKeyDown(/*KeyCode.Alpha1*/KeyCodeForFloors[1]))
            RequestElevator(1);
        if (Input.GetKeyDown(/*KeyCode.Alpha2*/KeyCodeForFloors[2]))
            RequestElevator(2);
        if (Input.GetKeyDown(/*KeyCode.Alpha3*/KeyCodeForFloors[3]))
            RequestElevator(3);
        if (Input.GetKeyDown(/*KeyCode.Alpha4*/KeyCodeForFloors[4]))
            RequestElevator(4);
        if (Input.GetKeyDown(/*KeyCode.Alpha5*/KeyCodeForFloors[5]))
            RequestElevator(5);
        if (Input.GetKeyDown(/*KeyCode.Alpha6*/KeyCodeForFloors[6]))
            RequestElevator(6);
        if (Input.GetKeyDown(/*KeyCode.Alpha7*/KeyCodeForFloors[7]))
            RequestElevator(7);
        if (Input.GetKeyDown(/*KeyCode.Alpha8*/KeyCodeForFloors[8]))
            RequestElevator(8);
        if (Input.GetKeyDown(/*KeyCode.Alpha9*/KeyCodeForFloors[9]))
            RequestElevator(9);
        if (Input.GetKeyDown(/*KeyCode.Alpha0*/KeyCodeForFloors[0]))
            RequestElevator(0);
    }

    public void MoveElevator()
    {
        if(elevatorFloorInputQueue.Count > 0)
        {
            DecideNextFloor();

            if(nextFloor > currentFloor)
            {
                StartCoroutine(GoUp(nextFloor - currentFloor));
            }
            else
            {
                StartCoroutine(GoDown(currentFloor - nextFloor));
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
        CheckNextFloorInDirectionOfElevator();

        if(nextFloor == dequeueEle)
        {
            //Check For Direction Change
            if (elevatorLastMotion == ElevatorCurrentMotion.MovingUp)
            {
                if (nextFloor < currentFloor)
                {
                    //means direction is changed
                    //to check if next floor is in opposite direction of elevator
                    CheckNextFloorInOppositeDirectionOfElevator();
                }
            }
            else if (elevatorLastMotion == ElevatorCurrentMotion.MovingDown)
            {
                if(nextFloor > currentFloor)
                {
                    //means direction is changed
                    //to check if next floor is in opposite direction of elevator
                    CheckNextFloorInOppositeDirectionOfElevator();
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

    public void CheckNextFloorInDirectionOfElevator()
    {
        foreach (int remainingFloor in elevatorFloorInputQueue)
        {
            if (elevatorLastMotion == ElevatorCurrentMotion.MovingUp)
            {
                if (remainingFloor < nextFloor && remainingFloor > currentFloor)
                {
                    //next floor is up
                    nextFloor = remainingFloor;
                }
                else
                {
                    //next floor is down
                }
            }
            else if (elevatorLastMotion == ElevatorCurrentMotion.MovingDown)
            {
                if (remainingFloor > nextFloor && remainingFloor < currentFloor)
                {
                    //next floor is down
                    nextFloor = remainingFloor;
                }
                else
                {
                    //next floor is up
                }
            }
        }
    }

    public void CheckNextFloorInOppositeDirectionOfElevator()
    {
        foreach (int remainingFloor in elevatorFloorInputQueue)
        {
            if (elevatorLastMotion == ElevatorCurrentMotion.MovingUp)
            {
                if (remainingFloor < nextFloor && remainingFloor > currentFloor)
                {
                    //next floor is up
                }
                else
                {
                    //next floor is down
                    nextFloor = remainingFloor;
                }
            }
            else if (elevatorLastMotion == ElevatorCurrentMotion.MovingDown)
            {
                if (remainingFloor > nextFloor && remainingFloor < currentFloor)
                {
                    //next floor is down
                }
                else
                {
                    //next floor is up
                    nextFloor = remainingFloor;
                }
            }
        }
    }

    public void ChangeElevatorMotionDirection(ElevatorCurrentMotion elevatorNewMotion)
    {
        elevatorLastMotion = elevatorNewMotion;
    }

    public IEnumerator GoUp(int steps)
    {
        elevatorCurrentMotion = ElevatorCurrentMotion.MovingUp;
        Debug.Log($"Moving UP {steps} steps.");

        while (Mathf.Abs(elevator.position.y - elevatorsFloorPositions[nextFloor].position.y) > maxElevatorPosError)
        {
            elevator.position = Vector3.MoveTowards(elevator.position, elevatorsFloorPositions[nextFloor].position, elevatorSpeed * Time.deltaTime);
            yield return null;
        }

        //reached
        elevatorLastMotion = ElevatorCurrentMotion.MovingUp;
        Stop();
    }

    public IEnumerator GoDown(int steps)
    {
        elevatorCurrentMotion = ElevatorCurrentMotion.MovingDown;
        Debug.Log($"Moving Down {steps} steps.");

        while (Mathf.Abs(elevator.position.y - elevatorsFloorPositions[nextFloor].position.y) > maxElevatorPosError)
        {
            elevator.position = Vector3.MoveTowards(elevator.position, elevatorsFloorPositions[nextFloor].position, elevatorSpeed * Time.deltaTime);
            yield return null;
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
    }
}

public enum ElevatorCurrentMotion
{
    MovingUp,
    MovingDown,
    NotMoving
}

