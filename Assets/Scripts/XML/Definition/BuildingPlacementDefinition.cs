using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Definition for a placement of a building in a base. </summary>
public class BuildingPlacementDefinition 
{

	/// <summary> Name of building definition to attach </summary>
	public string Building = "";

	/// <summary> Name of an empty 'slot' game object under the base to attach to. </summary>
	/// <remarks> If empty, should select the base object itself. </remarks>
	public string Slot = "";

	/// <summary> Offset to place object at from attached slot or base </summary>
	public Vector3 Position = Vector3.zero;

	/// <summary> Offset rotation to apply to object relative to attached object </summary>
	public Vector3 Rotation = Vector3.zero;
	/* 
	XML Loading note
	<!-- kinda janky verbose way to get it to load Vector3's. Need to do them as such:
			<Position> <x>1</x> <y>2</y> <z>3</z> </Position> 
			<Rotation> <x>90</x> <y>180</y> <z>270</z> </Rotation> 
	
			 Would prefer something more like 
			 <Position x="0" y="0" z="0"/> <Rotation x="0" y="0" z="0"/> 
			 or
			 <Position>1,2,3</Position> <Rotation>90,180,280</Rotation>
			 but it doesn't seem possible with the builtin xml de/serializer
			-->
	*/

	public override string ToString() {
		return $"BuildingPlacement: {Building} - {Slot} @ {Position} / {Rotation}";
	}

}
