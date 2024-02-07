[System.Serializable]
public class PartitionableObjectJSONParams {
	public string scene;//Scene the object belongs to
	public int count;//Number which denotes its count; this is for counting multiple instances of a particular object's kind
	public float posX;//x position in world transform
	public float posY;//y position in world transform
	public bool isTangible;//value is written as either "true" or "false"; denotes whether the object has been made whole or not
}
