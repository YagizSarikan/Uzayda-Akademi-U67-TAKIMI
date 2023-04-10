using NSubstitute;
using NUnit.Framework;

public class state_machine_tests
{
    [Test]
    public void initial_set_state_switches_to_state()
    {
        var stateMachine = new StateMachine();
        var firstState = Substitute.For<IState>();
        stateMachine.SetState(firstState);
        Assert.AreSame(firstState, stateMachine.CurrentState, "Expected first state");
    }

    [Test]
    public void second_set_state_switches_to_state()
    {
        var stateMachine = new StateMachine();
        var firstState = Substitute.For<IState>();
        var secondState = Substitute.For<IState>();
        stateMachine.SetState(firstState);
        Assert.AreSame(firstState, stateMachine.CurrentState, "Expected first state");
        stateMachine.SetState(secondState);
        Assert.AreSame(secondState, stateMachine.CurrentState, "Expected second state");
    }

    [Test]
    public void transition_switches_state_when_condition_is_met()
    {
        var stateMachine = new StateMachine();
        var firstState = Substitute.For<IState>();
        var secondState = Substitute.For<IState>();
        stateMachine.SetState(firstState);
        Assert.AreSame(firstState, stateMachine.CurrentState, "Expected first state");

        bool ShouldTransitionState() => true;
        stateMachine.AddTransition(firstState, secondState, ShouldTransitionState);

        stateMachine.Tick();
        Assert.AreSame(secondState, stateMachine.CurrentState, "Expected second state");
    }

    [Test]
    public void transition_does_not_switch_state_when_condition_is_not_met()
    {
        var stateMachine = new StateMachine();
        var firstState = Substitute.For<IState>();
        var secondState = Substitute.For<IState>();
        stateMachine.SetState(firstState);
        Assert.AreSame(firstState, stateMachine.CurrentState, "Expected first state");

        bool ShouldTransitionState() => false;
        stateMachine.AddTransition(firstState, secondState, ShouldTransitionState);

        stateMachine.Tick();
        Assert.AreSame(firstState, stateMachine.CurrentState, "Expected first state");    
    }

    [Test]
    public void transition_does_not_switch_state_when_not_in_correct_source_state()
    {
        var stateMachine = new StateMachine();
        var firstState = Substitute.For<IState>();
        var secondState = Substitute.For<IState>();
        var thirdState = Substitute.For<IState>();
        stateMachine.SetState(firstState);
        Assert.AreSame(firstState, stateMachine.CurrentState, "Expected first state");

        bool ShouldTransitionState() => true;
        stateMachine.AddTransition(secondState, thirdState, ShouldTransitionState);

        stateMachine.Tick();
        Assert.AreSame(firstState, stateMachine.CurrentState, "Expected first state");         
    }

    [Test]
    public void transition_from_any_switches_state_when_condition_is_met()
    {
        var stateMachine = new StateMachine();
        var firstState = Substitute.For<IState>();
        var secondState = Substitute.For<IState>();
        stateMachine.SetState(firstState);
        Assert.AreSame(firstState, stateMachine.CurrentState, "Expected first state");

        bool ShouldTransitionState() => true;
        stateMachine.AddAnyTransition(secondState, ShouldTransitionState);

        stateMachine.Tick();
        Assert.AreSame(secondState, stateMachine.CurrentState, "Expected second state");    
    }

    [Test]
    public void state_onenter_called_when_setting_state()
    {
        var stateMachine = new StateMachine();
        IState firstState = Substitute.For<IState>();
        stateMachine.SetState(firstState);
        firstState.Received(1).OnEnter();
    }

    [Test]
    public void state_methods_called_when_transition()
    {
        var stateMachine = new StateMachine();
        IState firstState = Substitute.For<IState>();
        IState secondState = Substitute.For<IState>();

        bool ShouldTransitionState() => true;
        stateMachine.AddAnyTransition(secondState, ShouldTransitionState);
        stateMachine.SetState(firstState);
        stateMachine.Tick();
        firstState.Received(1).OnEnter();
        firstState.Received(1).OnExit();
        secondState.Received(1).OnEnter();
        secondState.Received(1).Tick();
    }

    [Test]
    public void statemachine_onstatechangedevent_fired_when_setting_state()
    {
        var stateMachine = new StateMachine();
        IState firstState = Substitute.For<IState>();
        IState actualState = null;
        stateMachine.OnStateChanged += (state) => { actualState = state; };
        stateMachine.SetState(firstState);
        Assert.AreSame(firstState, actualState, "Exited event with firstState");
    }
}
