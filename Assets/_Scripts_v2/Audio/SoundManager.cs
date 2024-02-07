using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the sound effects.
/// </summary>
public class SoundManager : MonoBehaviour {

    /// <summary>
    /// Singleton instance.
    /// Each room has a single instance of the sound manager.
    /// </summary>
	static SoundManager instance;

    /// <summary>
    /// Reference to the singleton instance.
    /// </summary>
	public static SoundManager Instance {
		get { return instance; }
	}

    /// <summary>
    /// Reference to the background music.
    /// </summary>
	[SerializeField] private AudioSource backgroundMusic;

	// Add an SFX for each object and a Play(AudibleNames.<Class> name) function
	// Also create an enum entry in AudibleNames.cs
    /// <summary>
    /// Reference to the hammer SFX.
    /// </summary>
	[SerializeField] private HammerSFX hammerSFX;
    /// <summary>
    /// Reference to the needle SFX.
    /// </summary>
	[SerializeField] private NeedleSFX needleSFX;
    /// <summary>
    /// Reference to the trampoline bounce SFX.
    /// </summary>
	[SerializeField] private TrampolineSFX trampolineSFX;
    /// <summary>
    /// Reference to the results SFX.
    /// </summary>
	[SerializeField] private ResultsSFX resultsSFX;

    /// <summary>
    /// Reference to the phantom SFX.
    /// </summary>
	[SerializeField] private PhantomSFX phantomSFX;
    /// <summary>
    /// Reference to the popcorn enemy SFX.
    /// </summary>
	[SerializeField] private PopcornSFX popcornSFX;

	[SerializeField] private LCDInterfaceSFX lcdInterfaceSFX;
    /// <summary>
    /// Reference to the breakable box SFX.
    /// </summary>
	[SerializeField] private BreakableBoxSFX breakableBoxSFX;
    /// <summary>
    /// Reference to the door SFX.
    /// </summary>
	[SerializeField] private DoorSFX doorSFX;
    /// <summary>
    /// Reference to the room SFX.
    /// </summary>
	[SerializeField] private RoomSFX roomSFX;
    /// <summary>
    /// Reference to the button click SFX.
    /// </summary>
	[SerializeField] private ButtonSFX buttonSFX;
    /// <summary>
    /// Reference to the player avatar SFX.
    /// </summary>
	[SerializeField] private YuniSFX yuniSFX;
    /// <summary>
    /// List of all SFX.
    /// </summary>
    public List<SFX> sfxList;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	private void Awake() {
		instance = this;
        sfxList = new List<SFX>();
        GetAllSFX();
	}

    /// <summary>
    /// Put all SFX reference into the SFX list.
    /// </summary>
    void GetAllSFX() {
        sfxList.Add(hammerSFX);
        sfxList.Add(needleSFX);
        sfxList.Add(trampolineSFX);
        sfxList.Add(resultsSFX);
        sfxList.Add(phantomSFX);
        sfxList.Add(popcornSFX);
        sfxList.Add(lcdInterfaceSFX);
        sfxList.Add(breakableBoxSFX);
        sfxList.Add(doorSFX);
        sfxList.Add(roomSFX);
        sfxList.Add(buttonSFX);
        sfxList.Add(yuniSFX);
    }

    /// <summary>
	/// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
	/// </summary>
	void Start () {
		if(backgroundMusic != null)
			backgroundMusic.Play();
	}

    /// <summary>
    /// Stop background music.
    /// </summary>
	public void StopBG() {
		if(backgroundMusic != null)
			backgroundMusic.Stop();
	}

    /// <summary>
    /// Play an audio source which can be an SFX or music.
    /// </summary>
    /// <param name="source">Audio Source</param>
	public void PlaySource (AudioSource source) {
		source.Play ();

	}

    /// <summary>
    /// Play an audio source. Uses Unity's PlayOneShot function.
    /// </summary>
    /// <param name="source">Audio Source</param>
	public void PlayOneShot(AudioSource source) {
		source.PlayOneShot (source.clip);
	}

    /// <summary>
    /// Override Play function for hammer SFX.
    /// </summary>
    /// <param name="name">SFX name</param>
    /// <param name="isLoop">If the sound will loop.</param>
	public void Play(AudibleNames.Hammer name, bool isLoop) {
		if (hammerSFX == null) {
			if (GetComponentInChildren<HammerSFX> () != null) {
				this.hammerSFX = GetComponentInChildren<HammerSFX> ();
				this.hammerSFX.GetAudioSource (name).loop = isLoop;
				this.PlayOneShot (hammerSFX.GetAudioSource(name));
			}
		}
		else {
			this.hammerSFX.GetAudioSource (name).loop = isLoop;
			this.PlayOneShot (hammerSFX.GetAudioSource(name));
		}
	}

    /// <summary>
    /// Override Play function for needle SFX.
    /// </summary>
    /// <param name="name">SFX name</param>
    /// <param name="isLoop">If the sound will loop.</param>
	public void Play(AudibleNames.Needle name, bool isLoop) {
		if (needleSFX == null) {
			if (GetComponentInChildren<NeedleSFX> () != null) {
				this.needleSFX = GetComponentInChildren<NeedleSFX> ();
				this.needleSFX.GetAudioSource (name).loop = isLoop;
				this.PlaySource (needleSFX.GetAudioSource(name));
			}
		}
		else {
			this.needleSFX.GetAudioSource (name).loop = isLoop;
			this.PlaySource (needleSFX.GetAudioSource(name));
		}
	}

    /// <summary>
    /// Override Play function for the results SFX.
    /// </summary>
    /// <param name="name">SFX name</param>
    /// <param name="isLoop">If the sound will loop.</param>
	public void Play(AudibleNames.Results name, bool isLoop) {
		if (resultsSFX == null) {
			if (GetComponentInChildren<ResultsSFX> () != null) {
				this.resultsSFX = GetComponentInChildren<ResultsSFX> ();
				this.resultsSFX.GetAudioSource (name).loop = isLoop;
				this.PlaySource (resultsSFX.GetAudioSource(name));
			}
		}
		else {
			this.resultsSFX.GetAudioSource (name).loop = isLoop;
			this.PlaySource (resultsSFX.GetAudioSource(name));
		}
	}

    /// <summary>
    /// Override Play function for phantom SFX.
    /// </summary>
    /// <param name="name">SFX name</param>
    /// <param name="isLoop">If the sound will loop.</param>
	public void Play(AudibleNames.Phantom name, bool isLoop) {
		if (phantomSFX == null) {
			if (GetComponentInChildren<PhantomSFX> () != null) {
				this.phantomSFX = GetComponentInChildren<PhantomSFX> ();
				this.phantomSFX.GetAudioSource (name).loop = isLoop;
				this.PlaySource (phantomSFX.GetAudioSource(name));
			}
		}
		else {
			this.phantomSFX.GetAudioSource (name).loop = isLoop;
			this.PlaySource (phantomSFX.GetAudioSource(name));
		}
	}

    /// <summary>
    /// Override Play function for popcorn enemy SFX.
    /// </summary>
    /// <param name="name">SFX name</param>
    /// <param name="isLoop">If the sound will loop.</param>
	public void Play(AudibleNames.Popcorn name, bool isLoop) {
		if (popcornSFX == null) {
			if (GetComponentInChildren<PopcornSFX> () != null) {
				this.popcornSFX = GetComponentInChildren<PopcornSFX> ();
				this.popcornSFX.GetAudioSource (name).loop = isLoop;
				this.PlaySource (popcornSFX.GetAudioSource(name));
			}
		}
		else {
			this.popcornSFX.GetAudioSource (name).loop = isLoop;
			this.PlaySource (popcornSFX.GetAudioSource(name));
		}
	}

    /// <summary>
    /// Override Play function for trampoline bounce SFX.
    /// </summary>
    /// <param name="name">SFX name</param>
    /// <param name="isLoop">If the sound will loop.</param>
	public void Play(AudibleNames.Trampoline name, bool isLoop) {
		if (trampolineSFX == null) {
			if (GetComponentInChildren<TrampolineSFX> () != null) {
				this.trampolineSFX = GetComponentInChildren<TrampolineSFX> ();
				this.trampolineSFX.GetAudioSource (name).loop = isLoop;
				this.PlaySource (trampolineSFX.GetAudioSource(name));
			}
		}
		else {
			this.trampolineSFX.GetAudioSource (name).loop = isLoop;
			this.PlaySource (trampolineSFX.GetAudioSource(name));
		}
	}

	public void Play(AudibleNames.LCDInterface name, bool isLoop) {
		if (lcdInterfaceSFX == null) {
			if (GetComponentInChildren<LCDInterfaceSFX> () != null) {
				this.lcdInterfaceSFX = GetComponentInChildren<LCDInterfaceSFX> ();
				this.lcdInterfaceSFX.GetAudioSource (name).loop = isLoop;
				this.PlayOneShot (lcdInterfaceSFX.GetAudioSource(name));
			}
		}
		else {
			this.lcdInterfaceSFX.GetAudioSource (name).loop = isLoop;
			this.PlayOneShot (lcdInterfaceSFX.GetAudioSource(name));
		}
	}

    /// <summary>
    /// Override Play function for breakable box SFX.
    /// </summary>
    /// <param name="name">SFX name</param>
    /// <param name="isLoop">If the sound will loop.</param>
	public void Play(AudibleNames.BreakableBox name, bool isLoop) {
		if (breakableBoxSFX == null) {
			if (GetComponentInChildren<BreakableBoxSFX> () != null) {
				this.breakableBoxSFX = GetComponentInChildren<BreakableBoxSFX> ();
				this.breakableBoxSFX.GetAudioSource (name).loop = isLoop;
				this.PlayOneShot (breakableBoxSFX.GetAudioSource(name));
			}
		}
		else {
			this.breakableBoxSFX.GetAudioSource (name).loop = isLoop;
			this.PlayOneShot (breakableBoxSFX.GetAudioSource(name));
		}
	}

    /// <summary>
    /// Override Play function for door SFX.
    /// </summary>
    /// <param name="name">SFX name</param>
    /// <param name="isLoop">If the sound will loop.</param>
	public void Play(AudibleNames.Door name, bool isLoop) {
		if (doorSFX == null) {
			if (GetComponentInChildren<DoorSFX> () != null) {
				this.doorSFX = GetComponentInChildren<DoorSFX> ();
				if (this.doorSFX.GetAudioSource (name) != null) {
					this.doorSFX.GetAudioSource (name).loop = isLoop;
					this.PlaySource (doorSFX.GetAudioSource(name));
				}
			}
		}
		else {
			if (this.doorSFX.GetAudioSource (name) != null) {
				this.doorSFX.GetAudioSource (name).loop = isLoop;
				this.PlaySource (doorSFX.GetAudioSource(name));
			}
		}
	}

    /// <summary>
    /// Override Play function for room SFX.
    /// </summary>
    /// <param name="name">SFX name</param>
    /// <param name="isLoop">If the sound will loop.</param>
	public void Play(AudibleNames.Room name, bool isLoop) {
		if (roomSFX == null) {
			if (GetComponentInChildren<RoomSFX> () != null) {
				this.roomSFX = GetComponentInChildren<RoomSFX> ();
				this.roomSFX.GetAudioSource (name).loop = isLoop;
				this.PlayOneShot (roomSFX.GetAudioSource(name));
			}
		}
		else {
			this.roomSFX.GetAudioSource (name).loop = isLoop;
			this.PlayOneShot (roomSFX.GetAudioSource(name));
		}
	}

    /// <summary>
    /// Override Play function for button click SFX.
    /// </summary>
    /// <param name="name">SFX name</param>
    /// <param name="isLoop">If the sound will loop.</param>
	public void Play(AudibleNames.Button name, bool isLoop) {
		if (buttonSFX == null) {
			if (GetComponentInChildren<ButtonSFX> () != null) {
				this.buttonSFX = GetComponentInChildren<ButtonSFX> ();
				this.buttonSFX.GetAudioSource (name).loop = isLoop;
				this.PlayOneShot (buttonSFX.GetAudioSource(name));
			}
		}
		else {
			this.buttonSFX.GetAudioSource (name).loop = isLoop;
			this.PlayOneShot (buttonSFX.GetAudioSource(name));
		}
	}

    /// <summary>
    /// Override Play function for player avatar SFX.
    /// </summary>
    /// <param name="name">SFX name</param>
    /// <param name="isLoop">If the sound will loop.</param>
	public void Play(AudibleNames.Yuni name, bool isLoop) {
		if (yuniSFX == null) {
			if (GetComponentInChildren<YuniSFX> () != null) {
				this.yuniSFX = GetComponentInChildren<YuniSFX> ();
				this.yuniSFX.GetAudioSource (name).loop = isLoop;
				this.PlayOneShot (yuniSFX.GetAudioSource(name));
			}
		}
		else {
			this.yuniSFX.GetAudioSource (name).loop = isLoop;
			this.PlayOneShot (yuniSFX.GetAudioSource(name));
		}
	}

    /// <summary>
    /// Returns the popcorn enemy SFX.
    /// </summary>
    /// <returns>Popcorn Enemy SFX</returns>
	public PopcornSFX GetPopcornSFX() {
		if (this.popcornSFX == null) {
			if (GetComponentInChildren<PopcornSFX> () != null) {
				this.popcornSFX = GetComponentInChildren<PopcornSFX> ();
			}
		}
		return this.popcornSFX;
	}

    /// <summary>
    /// Returns the room SFX.
    /// </summary>
    /// <returns>Room SFX</returns>
	public RoomSFX GetRoomSFX() {
		if (this.roomSFX == null) {
			if (GetComponentInChildren<RoomSFX> () != null) {
				this.roomSFX = GetComponentInChildren<RoomSFX> ();
			}
		}
		return this.roomSFX;
	}
}