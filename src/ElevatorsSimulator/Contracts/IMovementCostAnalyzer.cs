using ElevatorsSimulator.Models;

namespace ElevatorsSimulator.Contracts;
public interface IMovementCostAnalyzer
{
    public void LoadData(Elevator elevator, LinkedList<Movement> data);
    public void ClearMovementData();
    public MovementCostAnalysisResult GetAnalysisResult(ElevatorRequest request);
}
