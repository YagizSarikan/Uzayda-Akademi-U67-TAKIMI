using System;
using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class player_ship_movement_tests
{
    PlayerShip _playerShip;

    [UnitySetUp]
    public IEnumerator TestSetup()
    {
        UserInput.Instance = Substitute.For<IUserInput>();
        yield return TestHelpers.LoadScene("PlayerShipTests");
        _playerShip = TestHelpers.GetPlayerShip();
    }
    
    [UnityTest]
    public IEnumerator player_ship_flip_direction_test()
    {
        Assert.AreEqual(1, _playerShip.Direction);
        UserInput.Instance.OnFlipDirectionPressed += Raise.Event<Action>();
        yield return null;
        Assert.AreEqual(-1, _playerShip.Direction);
    }
    
    [UnityTest]
    public IEnumerator player_ship_move_up_test()
    {
        Assert.AreEqual(Vector3.zero, _playerShip.transform.position);
        UserInput.Instance.UpPressed.Returns(true);
        yield return new WaitForSeconds(0.25f);
        Assert.IsTrue(_playerShip.transform.position.y > 0);
    }    
    
    
    [UnityTest]
    public IEnumerator player_ship_move_down_test()
    {
        Assert.AreEqual(Vector3.zero, _playerShip.transform.position);
        UserInput.Instance.DownPressed.Returns(true);
        yield return new WaitForSeconds(0.25f);
        Assert.IsTrue(_playerShip.transform.position.y < 0);
    }

    [UnityTest]
    public IEnumerator background_should_scroll_when_ship_thrusts()
    {
        // Arrange
        BackgroundScroller bgScroller = GameObject.FindObjectOfType<BackgroundScroller>();
        Renderer renderer = bgScroller.GetComponent<Renderer>();
        Vector2 offset = renderer.material.GetTextureOffset("_MainTex");
        Assert.AreEqual(Vector2.zero, offset, "Material offset should initially be zero");
        
        //Act
        UserInput.Instance.IsThrusting.Returns(true);
        yield return new WaitForSeconds(0.25f);
        
        // Assert
        offset = renderer.material.GetTextureOffset("_MainTex");
        Assert.IsTrue(offset.x > 0, "Offset should be greater than zero after ship thrusts");
    }

    [UnityTest]
    public IEnumerator hyperspace_should_teleport_ship()
    {
        // Arrange
        var startPosition = _playerShip.transform.position;
        
        // Act
        UserInput.Instance.OnHyperspacePressed += Raise.Event<Action>();
        yield return null;
        
        // Assert
        Assert.AreNotEqual(startPosition, _playerShip.transform.position, "Should have teleported.");
    }
}