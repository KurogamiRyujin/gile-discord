using UnityEngine;
using System.Collections;

/// <summary>
/// Holder for event names
/// Created By: NeilDG
/// </summary>
public class EventNames {

    public const string HIDE_SKINS = "HIDE_SKINS";
    public const string UPDATE_SKIN_NAME = "UPDATE_SKIN_NAME";
    public const string UPDATE_SKIN_IMAGE = "UPDATE_SKIN_IMAGE";

    public const string STABLE_AREA = "STABLE_AREA";
	public const string BREAK_AREA = "BREAK_AREA";
	public const string UNSTABLE_AREA = "UNSTABLE_AREA";

	public const string YUNI_ACQUIRED_NEEDLE = "YUNI_ACQUIRED_NEEDLE";
	public const string YUNI_THREW_NEEDLE = "YUNI_THREW_NEEDLE";

	public const string SHOW_CHECKPOINT = "SHOW_CHECKPOINT";
	public const string HIDE_CHECKPOINT = "HIDE_CHECKPOINT";
    public const string HAMMER_FORCE_ACQUIRE = "HAMMER_FORCE_ACQUIRE";
    
    public const string SHOW_CONGRATULATIONS = "SHOW_CONGRATULATIONS";
    public const string ACQUIRE_ALL = "ACQUIRE_ALL";
	public const string RETRY = "RETRY";
	public const string REQUEST_UPDATE_SESSION = "REQUEST_UPDATE_SESSION";
    public const string RINGLEADER_DEATH = "RINGLEADER_DEATH";
    
    public const string SPAWN_BLOCKS_PROCESS_OFF = "SPAWN_BLOCKS_PROCESS_OFF";
    public const string SPAWN_BLOCKS_PROCESS_ON = "SPAWN_BLOCKS_PROCESS_ON";
    public const string RECONFIGURE_HIGHLIGHTS = "RECONFIGURE_HIGHLIGHTS";
    public const string STABLE_AREA_NO_PROMPT = "STABLE_AREA_NO_PROMPT";
    public const string STABLE_AREA_WITH_PROMPT = "STABLE_AREA_WITH_PROMPT";
    
    public const string BREAKABLE_BROKEN = "BREAKABLE_BROKEN";
	public const string STABILITY_MAX_CHANGE = "STABILITY_MAX_CHANGE";
	public const string TARGET_MARKER_CHANGED = "TARGET_MARKER_CHANGED";
	public const string STABILITY_MARKER_CHANGED = "STABILITY_MARKER_CHANGED";

    public const string ON_HOLLOW_STABILITY_UPDATE_INSTANT = "ON_HOLLOW_STABILITY_UPDATE_INSTANT";
    
    public const string NEEDLE_PICKUP = "NEEDLE_PICKUP";
	public const string THREAD_PICKUP = "THREAD_PICKUP";
	public const string HAMMER_PICKUP = "HAMMER_PICKUP";
	public const string CHARM_PICKUP = "CHARM_PICKUP";
	public const string OPEN_CHARM = "OPEN_CHARM";
	public const string CHARM_PICKUP_SWITCH = "CHARM_PICKUP_SWITCH";

	public const string PICKUP_ITEM_CLOSE = "PICKUP_ITEM_CLOSE";


	public const string SKY_FRAGMENT_PIECE_RELEASED = "SKY_FRAGMENT_PIECE_RELEASED";

	public const string FROM_TUTORIAL_CALL = "FROM_TUTORIAL_CALL";

	public const string CLOSE_TUTORIAL_UNSTABLE_ROOM = "CLOSE_TUTORIAL_UNSTABLE_ROOM";
	public const string SHOW_TUTORIAL_UNSTABLE_ROOMS = "SHOW_TUTORIAL_UNSTABLE_ROOMS";

	public const string CLOSE_TUTORIAL_CARRY_ITEM = "CLOSE_TUTORIAL_CARRY_ITEM";
	public const string SHOW_TUTORIAL_CARRY_ITEM = "SHOW_TUTORIAL_CARRY_ITEM";

	public const string CLOSE_TUTORIAL_FIXING_BLOCKS = "CLOSE_TUTORIAL_FIXING_BLOCKS";
	public const string SHOW_TUTORIAL_FIXING_BLOCKS = "SHOW_TUTORIAL_FIXING_BLOCKS";

	public const string CLOSE_TUTORIAL_USING_NEEDLE = "CLOSE_TUTORIAL_USING_NEEDLE";
	public const string SHOW_TUTORIAL_USING_NEEDLE = "SHOW_TUTORIAL_USING_NEEDLE";

	public const string CLOSE_TUTORIAL_USING_HAMMER = "CLOSE_TUTORIAL_USING_HAMMER";
	public const string SHOW_TUTORIAL_USING_HAMMER = "SHOW_TUTORIAL_USING_HAMMER";


	public const string CLOSE_TUTORIAL_POWER_CHARMS = "CLOSE_TUTORIAL_POWER_CHARMS";
	public const string SHOW_TUTORIAL_POWER_CHARMS = "SHOW_TUTORIAL_POWER_CHARMS";

	public const string CLOSE_TUTORIAL_SUBDIVIDING = "CLOSE_TUTORIAL_SUBDIVIDING";
	public const string SHOW_TUTORIAL_SUBDIVIDING = "SHOW_TUTORIAL_SUBDIVIDING";

	public const string HIDE_HELP_BUTTON = "HIDE_HELP_BUTTON";
	public const string SHOW_HELP_BUTTON = "SHOW_HELP_BUTTON";


	public const string CARRY_OVERRIDE = "CARRY_OVERRIDE";
	public const string ON_CHARGING = "ON_CHARGING";
	public const string ON_PLAYER_HAND_PRESS = "ON_PLAYER_HAND_PRESS";
	public const string ON_HINT_SUCCESS = "ON_HINT_SUCCESS";
	public const string PLAY_HINT_RESULTS = "PLAY_HINT_RESULTS";

	public const string ON_SINGLE_ACTION_BROADCAST = "ON_SINGLE_ACTION_BROADCAST";
	public const string OPEN_YARNBALL = "OPEN_YARNBALL";


	public const string ON_DENOMINATOR_CHANGE = "ON_DENOMINATOR_CHANGE";
	public const string ON_PLAYER_GEM_CARRY = "ON_PLAYER_GEM_CARRY";
	public const string ON_PLAYER_GEM_DROP = "ON_PLAYER_GEM_DROP";

	public const string ON_PLAYER_LIFT_CARRY = "ON_PLAYER_LIFT_CARRY";
	public const string ON_PLAYER_LIFT_DROP = "ON_PLAYER_LIFT_DROP";
	public const string ON_BLOCK_SPAWN = "ON_BLOCK_SPAWN";
	public const string ON_BLOCK_DESTROY = "ON_BLOCK_DESTROY";

	public const string DISABLE_TUTORIALS = "DISABLE_TUTORIALS";

	public const string SHOW_PLAYER_CARRY = "SHOW_PLAYER_CARRY";
	public const string HIDE_SIMPLE_PLAYER_CARRY = "HIDE_SIMPLE_PLAYER_CARRY";
	public const string HIDE_PLAYER_CARRY = "HIDE_PLAYER_CARRY";
    
    public const string SET_MAX_POINT = "SET_MAX_POINT";
    public const string DESTABILIZE = "DESTABILIZE";
    public const string ON_HOLLOW_STABILITY_UPDATE = "ON_HOLLOW_STABILITY_UPDATE";
	public const string ON_PLAYER_GEM_ENTER = "ON_PLAYER_GEM_ENTER";
	public const string ON_PLAYER_LIFT_ENTER = "ON_PLAYER_LIFT_ENTER";
	public const string ON_HEALTH_CIRCLE_UPDATE = "ON_HEALTH_CIRCLE_UPDATE";

	public const string DISPLAY_HINT = "DISPLAY_HINT";
	public const string REMOVE_HINT = "REMOVE_HINT";
	public const string DISPLAY_SCENE_TITLE = "DISPLAY_SCENE_TITLE";
	public const string REMOVE_SCENE_TITLE = "REMOVE_SCENE_TITLE";
	public const string RESET_SCENE_TITLE_TRIGGER = "RESET_SCENE_TITLE_TRIGGER";

	public const string TOGGLE_INTERACT = "TOGGLE_INTERACT";
	public const string TOGGLE_JUMP = "TOGGLE_JUMP";
	public const string TOGGLE_PICKUP_BUTTON = "TOGGLE_PICKUP_BUTTON";
	public const string TOGGLE_SWITCH_WEAPON_BUTTON = "TOGGLE_SWITCH_WEAPON_BUTTON";
	public const string TOGGLE_MOBILE_UI = "TOGGLE_MOBILE_UI";
	public const string TOGGLE_BASE_MOBILE_UI = "TOGGLE_BASE_MOBILE_UI";
	public const string TOGGLE_LEFT_RIGHT_BUTTONS = "TOGGLE_LEFT_RIGHT_BUTTONS";

	public const string ON_PLAYER_DAMAGE = "ON_PLAYER_DAMAGE";
	public const string ON_PLAYER_DEATH = "ON_PLAYER_DEATH";
	public const string ON_PLAYER_ENABLE = "ON_PLAYER_ENABLE";


	public const string ON_NEEDLE_HIT = "ON_NEEDLE_HIT";

	// For activating crosshair on lcd pause
	public const string ON_LCD_PAUSE = "ON_LCD_PAUSE";
	public const string ON_LCD_UNPAUSE = "ON_LCD_UNPAUSE";

	// For Yuni's propel animation
	public const string ON_LCD_DONE = "ON_LCD_DONE";


	public const string ON_HAMMER_SMASH_END = "ON_HAMMER_SMASH_END";
	public const string ON_HAMMER_DOWN_ZERO = "ON_HAMMER_DOWN_ZERO";
	public const string ON_YUNI_PROPEL_END = "ON_YUNI_PROPEL_END";


	public const string ON_DOOR_LOCK = "ON_DOOR_LOCK";
	public const string ON_DOOR_UNLOCK = "ON_DOOR_UNLOCK";
	public const string ON_DOOR_CLOSE = "ON_DOOR_CLOSE";
	public const string ON_DOOR_OPEN = "ON_DOOR_OPEN";

	//For Ringleader
	public const string INITIATE_PLATFORM_DETONATION = "INITIATE_PLATFORM_DETONATION";
	public const string REPLENISH_DETONATED_PLATFORMS = "REPLENISH_DETONATED_PLATFORMS";

	public const string ROBIN_HOOD_PLATFORMS_ACTIVATE = "ROBIN_HOOD_PLATFORMS_ACTIVATE";
	public const string ROBIN_HOOD_PLATFORMS_DEACTIVATE = "ROBIN_HOOD_PLATFORMS_DEACTIVATE";

	//For Enemies
	public const string INVULNERABLE_ENEMIES = "INVULNERABLE_ENEMIES";
	public const string VULNERABLE_ENEMIES = "VULNERABLE_ENEMIES";
	public const string REGISTER_ENEMY = "REGISTER_ENEMY";
	public const string UNREGISTER_ENEMY = "UNREGISTER_ENEMY";

	//For Camera Controlling
	public const string DISABLE_CAMERA = "DISABLE_CAMERA";
	public const string ENABLE_CAMERA = "ENABLE_CAMERA";
	public const string ZOOM_CAMERA_TOWARDS = "ZOOM_CAMERA_TOWARDS";
	public const string SHOULD_CAMERA_ZOOM_IN = "SHOULD_CAMERA_ZOOM_IN";

	// For Weapon UI
	public const string ON_SWITCH_NEEDLE = "ON_SWITCH_NEEDLE";
	public const string ON_SWITCH_HAMMER = "ON_SWITCH_HAMMER";

	// For Healtch Counter
	public const string ON_HEALTH_UPDATE = "ON_HEALTH_UPDATE";

	//for Loading and Saving Data (Scene Objects Controller)
	public const string LOAD_DATA = "LOAD_DATA";
	public const string SAVE_DATA = "SAVE_DATA";

	public const string RECORD_ON_AREA_STABLE = "RECORD_ON_AREA_STABLE";
	public const string RECORD_HOLLOW_BLOCK = "RECORD_HOLLOW_BLOCK";
}







