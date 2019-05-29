using UnityEngine;

public class TagLayerUtil {

    public static bool IsEqual(string objectTag, GameTags gameTag) {
        return objectTag.Equals(gameTag.ToString());
    }

	public static bool IsUntagged(string tag) {
		return tag.Equals(GameTags.Untagged.ToString());
	}

	public static bool IsTagPlayer(string tag) {
		return (tag.Equals(GameTags.Player.ToString()) || 
			tag.Equals(GameTags.PlayerShielded.ToString()));
	}

    public static bool IsEligibleForPlayerScoring(bool isPlayerProjectile, string targetTag) {
        return (isPlayerProjectile && (targetTag.Equals(GameTags.Enemy.ToString())));
    }

	public static bool IsLayerInteractor(int layer) {
		return (layer == LayerMask.NameToLayer (GameLayers.Interactor.ToString()));
	}

}


//NOTE: Declare in this enum all the TAGS you declared in Unity Editor.
/*
	UnityEditorInternal.InternalEditorUtility.tags would be a great tool to use instead of this class.
	But APIs under UnityEditorInternal are not officially supported and might be deleted at some point. So, I won't really rely on them working forever.
*/
public enum GameTags {

	Untagged,

	Projectile,

	NPC,

	Enemy,

	Player, 
	PlayerShielded,

    FX,

    Bounds

}

public enum GameLayers {

	Default,

	Interactor

}