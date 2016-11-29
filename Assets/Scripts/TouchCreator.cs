using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// Touch creator. A class to generate a touch based on input.
/// </summary>
public class TouchCreator
{
	static BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
	static Dictionary<string, FieldInfo> fields;

	object touch;

	/// <summary>
	/// Gets or sets the delta time.
	/// </summary>
	/// <value>The delta time.</value>
	public float deltaTime 
	{ 
		get 
		{ 
			return ((Touch)touch).deltaTime; 
		} 
		set 
		{ 
			fields ["m_TimeDelta"].SetValue (touch, value); 
		} 
	}

	/// <summary>
	/// Gets or sets the tap count.
	/// </summary>
	/// <value>The tap count.</value>
	public int tapCount 
	{ 
		get 
		{ 
			return ((Touch)touch).tapCount; 
		} 
		set 
		{ 
			fields ["m_TapCount"].SetValue (touch, value); 
		} 
	}

	/// <summary>
	/// Gets or sets the phase.
	/// </summary>
	/// <value>The phase.</value>
	public TouchPhase phase 
	{ 
		get 
		{ 
			return ((Touch)touch).phase; 
		} 
		set 
		{ 
			fields ["m_Phase"].SetValue (touch, value); 
		} 
	}

	/// <summary>
	/// Gets or sets the delta position.
	/// </summary>
	/// <value>The delta position.</value>
	public Vector2 deltaPosition 
	{ 
		get 
		{ 
			return ((Touch)touch).deltaPosition; 
		} 
		set 
		{ 
			fields ["m_PositionDelta"].SetValue (touch, value); 
		} 
	}

	/// <summary>
	/// Gets or sets the finger identifier.
	/// </summary>
	/// <value>The finger identifier.</value>
	public int fingerId 
	{ 
		get 
		{ 
			return ((Touch)touch).fingerId; 
		} 
		set 
		{ 
			fields ["m_FingerId"].SetValue (touch, value); 
		} 
	}

	/// <summary>
	/// Gets or sets the position.
	/// </summary>
	/// <value>The position.</value>
	public Vector2 position 
	{ 
		get 
		{ 
			return ((Touch)touch).position; 
		} 
		set 
		{ 
			fields ["m_Position"].SetValue (touch, value); 
		} 
	}

	/// <summary>
	/// Gets or sets the raw position.
	/// </summary>
	/// <value>The raw position.</value>
	public Vector2 rawPosition 
	{ 
		get 
		{ 
			return ((Touch)touch).rawPosition; 
		} 
		set 
		{ 
			fields ["m_RawPosition"].SetValue (touch, value); 
		} 
	}

	/// <summary>
	/// Create this instance.
	/// </summary>
	public Touch Create ()
	{
		return (Touch)touch;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TouchCreator"/> class.
	/// </summary>
	public TouchCreator ()
	{
		touch = new Touch ();
	}

	/// <summary>
	/// Initializes the <see cref="TouchCreator"/> class.
	/// </summary>
	static TouchCreator ()
	{
		fields = new Dictionary<string, FieldInfo> ();
		foreach (var f in typeof(Touch).GetFields(BindingFlags.Instance | BindingFlags.NonPublic)) {
			fields.Add (f.Name, f);
			Debug.Log ("name: " + f.Name);
		}
	}
}