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
        return _elevators.Select(e => $"{e.Id} - {e.ElevatorStatus.CurrentFloor} {DirectionExtensions.ToString(e.ElevatorStatus.Direction)}")
            .ToArray();
    }

    public int GetElevatorLimit()
    {
        if (_elevators.Count == 0)
        {
            throw new Exception(StringConstants.ERROR_NO_ELEVATORS);
        }

        return _elevators.First().LoadLimit;
    }

    private string GetElevatorId(int count)
    {
        string chars = string.Empty;
        if (count / 26 > 0)
        {
            chars = GetElevatorId(count / 26);
        }

        return chars + alpha[count % 26];
    }
}
