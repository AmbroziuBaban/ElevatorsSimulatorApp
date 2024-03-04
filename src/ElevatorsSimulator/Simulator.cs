using ElevatorsSimulator.Logic;
using ElevatorsSimulator.Contracts;
using ElevatorsSimulator.Models;
using ElevatorsSimulator.Common;
using Microsoft.Extensions.Logging;

namespace ElevatorsSimulator;
public class Simulator : ISimulator
{
    private IOrchestrator _orchestrator;
    private IMenu _menu;
    private bool _isSimulatorRunning;
    private readonly ILogger<ISimulator> _logger;
    public Simulator(IOrchestrator orchestrator, IMenu menu, ILogger<ISimulator> logger)
    {
        _orchestrator = orchestrator;
        _logger = logger;
        _menu = menu;

        InitElevators(numberOfFloors: 10, numberOfElevators: 5, loadLimit: 4);
        InitMenu();

        _isSimulatorRunning = true;
    }

    public void Run()
    {
        while (_isSimulatorRunning)
        {
            try
            {
                _menu.DisplayMenu();

                var input = Console.ReadLine();
                _menu.ExecuteCommand(input);
                _orchestrator.ExecuteStep();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _menu.Reset();
            }
        }
    }

    #region Private Methods
    private void InitMenu()
    {
        IMenuScreen defaultScreen = new SimpleScreen(StringConstants.SCREEN_TITLE_MAIN);
        IMenuScreen showStatusScreen = new SimpleScreen(StringConstants.SCREEN_TITLE_STATUS_ELEVATORS);
        IMenuScreen callElevatorScreen = new CallElevatorScreen(StringConstants.SCREEN_TITLE_ELEVATOR_REQUEST);

        defaultScreen.AddMenuItem(
            new MenuItem(
                title: showStatusScreen.Title,
                command: () =>
                {
                    _menu.NavigateToScreen(showStatusScreen.Title);
                    Console.WriteLine(string.Join('\n', _orchestrator.GetElevatorsStatus()));
                }
            )
        );

        defaultScreen.AddMenuItem(
            new MenuItem(
                title: callElevatorScreen.Title,
                command: () => _menu.NavigateToScreen(callElevatorScreen.Title)
            )
        );

        defaultScreen.AddMenuItem(
            new MenuItem(
                title: StringConstants.ITEM_TITLE_EXIT,
                command: () => _isSimulatorRunning = false
            )
        );


        showStatusScreen.AddMenuItem(
            new MenuItem(
                title: StringConstants.ITEM_TITLE_STATUS_RELOAD,
                command: () => Console.WriteLine(string.Join('\n', _orchestrator.GetElevatorsStatus()))
            )
        );

        showStatusScreen.AddMenuItem(
            new MenuItem(
                title: StringConstants.ITEM_TITLE_BACK,
                command: () => _menu.NavigateToScreen(defaultScreen.Title)
            )
        );


        callElevatorScreen.AddMenuItem(
            new MenuItem(
                title: StringConstants.ITEM_TITLE_BACK,
                command: () => _menu.NavigateToScreen(defaultScreen.Title)
            ),
            prefferedKey: StringConstants.ITEM_KEY_BACK_ELEVATOR_REQUEST
        );

        callElevatorScreen.AddMenuItem(
            new MenuItem(
                title: StringConstants.ITEM_TITLE_ELEVATOR_REQUEST_INPUT,
                command: AddRequestDelegate
            ),
            prefferedKey: "-"
        );

        _menu.AddScreen(defaultScreen);
        _menu.AddScreen(showStatusScreen);
        _menu.AddScreen(callElevatorScreen);
    }

    public string AddRequestDelegate(object input)
    {
        var elevatorsIds = _orchestrator.AddRequest((ElevatorRequest)input);
        _menu.NavigateToScreen(StringConstants.SCREEN_TITLE_MAIN);

        return elevatorsIds;
    }

    private void InitElevators(int numberOfFloors, int numberOfElevators, int loadLimit)
    {
        _orchestrator.NumberOfFloors = numberOfFloors;

        for (int i = 0; i <= numberOfElevators; i++)
        {
            _orchestrator.AddElevator(loadLimit);
        }
    }
    #endregion
}
