namespace ElevatorsSimulator.Models;
public class MovementCostAnalysisResult
{
    public string ElevatorId { get; set; } = "";
    public int Cost { get; set; }
    public int MovementStartIndex { get; set; }
    public int MovementEndIndex { get; set; }
}
