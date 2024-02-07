using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudibleNames {

	public enum Hammer {
		BASIC,
		HIT,
		MISS,
		HIT_CRATE
	}
	public enum Room {
		BREAKING,
		HIT,
		MISS,
		DROP,
		PICKUP_FRAGMENT,
		ITEM_GET
	}
	public enum Button {
		BASIC,
		CONFIRM,
		CANCEL
	}
	public enum Yuni {
		BASIC,
		DAMAGE
	}
	public enum Needle {
		BASIC,
		HIT,
		MISS
	}
	public enum Door {
		OPEN,
		CLOSE
	}
	public enum BreakableBox {
		BREAK,
		GLASS
	}

	public enum LCDInterface {
		INCREASE,
		DECREASE,
		CONFIRM,
		SYNC,
		GLOW,
		CARRY
	}

	public enum Trampoline {
		BOUNCE,
		OPERATE,
		LOCK
	}

	public enum Results {
		SUCCESS,
		MISTAKE,
		BREAK
	}

	public enum Phantom {
		DEATH
	}

	public enum Popcorn {
		WALK,
		JUMP,
		HIT,
		ATTACK,
		DEATH
	}

}
