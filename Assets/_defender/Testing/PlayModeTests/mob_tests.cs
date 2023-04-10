using System;
using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class mob_tests
{
    GameManager _gameManager;
    Human _human;

    [UnitySetUp]
    public IEnumerator TestSetup()
    {
        UserInput.Instance = Substitute.For<IUserInput>();
        yield return TestHelpers.LoadScene("MobTests");
        _gameManager = TestHelpers.GetGameManager();
        yield return new WaitForEndOfFrame();
        _human = TestHelpers.GetHuman();
    }

    [UnityTest]
    public IEnumerator human_should_scroll_when_player_ship_thrusts()
    {
        Vector3 startPosition = _human.transform.position;
        UserInput.Instance.IsThrusting.Returns(true);
        yield return new WaitForSeconds(0.25f);
        Assert.IsTrue(_human.transform.position.x < startPosition.x, "Expected human position to be left of start position");
    }

    [UnityTest]
    public IEnumerator enemies_in_range_should_be_destroyed_by_smart_bomb()
    {
        // Arrange
        var enemies = Object.FindObjectsOfType<SmartBombTarget>().Length;
        Assert.AreEqual(3, enemies, "We should start with 3 enemies.");

        // Act
        UserInput.Instance.OnSmartBombPressed += Raise.Event<Action>();
        yield return null;

        // Assert
        enemies = Object.FindObjectsOfType<SmartBombTarget>().Length;
        Assert.AreEqual(1, enemies, "Only 1 enemy should remain after smart bomb used.");
    }

    [UnityTest]
    public IEnumerator smart_bombs_should_decrement_when_used()
    {
        // Arrange
        Assert.AreEqual(3, _gameManager.SmartBombs);
        yield return null;
        
        // Act
        UserInput.Instance.OnSmartBombPressed += Raise.Event<Action>();
        yield return null;
        
        // Assert
        Assert.AreEqual(2, _gameManager.SmartBombs);
    }

    [UnityTest]
    public IEnumerator smart_bomb_event_should_not_fire_if_no_more_smart_bombs()
    {
        // Arrange
        Assert.AreEqual(3, _gameManager.SmartBombs);        
        yield return null;
        
        // Act
        var iterations = 0;
        while (_gameManager.SmartBombs > 0 && iterations++ < 10)
        {
            UserInput.Instance.OnSmartBombPressed += Raise.Event<Action>();
            yield return new WaitForSeconds(0.3f);
        }
        
        Assert.AreEqual(0, _gameManager.SmartBombs, "We should be out of smart bombs.");

        _gameManager.SmartBombsChanged += (bombs) =>
        {
            Assert.Fail("Smart bomb event called when no more smart bombs.");
        };
        UserInput.Instance.OnSmartBombPressed += Raise.Event<Action>();
        yield return new WaitForSeconds(0.3f);
        
        // Assert
        Assert.Pass();
    }
}