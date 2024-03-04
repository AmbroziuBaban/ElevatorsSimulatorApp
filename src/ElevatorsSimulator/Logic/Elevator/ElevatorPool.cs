using ElevatorsSimulator.Common;
using ElevatorsSimulator.Contracts;
using ElevatorsSimulator.Models;

namespace ElevatorsSimulator.Logic;
public class ElevatorPool : IElevatorPool
{
    public IEnumerable<Elevator> Elevators
    {
        get
        {
            return _elevators;
        }
        private set { }
    }
    private readonly char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    private List<Elevator> _elevators = new List<Elevator>();

    public void AddElevator(int loadLimit)
    {
        Elevator elevator = new Elevator
        {
            Id = GetElevatorId(_elevators.Count),
            LoadLimit = loadLimit
        };

        _elevators.Add(elevator);
    }

    public string[] GetElevatorInfo()
    {
        return _elevators.Select(ev => $"{ev.Id} - {ev.ElevatorStatus.CurrentFloor} {DirectionExtensions.ToString(ev.ElevatorStatus.Direction)}").ToArray();
    }

    public int GetElevatorLimit()
    {
        if (_elevators.Count == 0)
        {
            throw new Exception("No elevators available.");
        }

        return _elevators.First().LoadLimit;
    }

    private string GetElevatorId(int count)
    {
        string chars = "";
        if (count / 26 > 0)
        {
            chars = GetElevatorId(count / 26);
        }

        return chars + alpha[count % 26];
    }
}
