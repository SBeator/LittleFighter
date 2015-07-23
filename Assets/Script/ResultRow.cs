using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultRow : MonoBehaviour 
{
	public Image playerImage;
	public Text playerName;
	public Text kill;
	public Text attack;
	public Text heal;
	public Text bearing;

	public void ShowPlayerInfo(PlayerInfo playerInfo)
	{
		this.playerImage.sprite = playerInfo.PlayerImage;
		this.playerName.text = playerInfo.PlayerName;
		this.kill.text = playerInfo.KillValue.ToString();
		this.attack.text = playerInfo.AttackValue.ToString();
		this.heal.text = playerInfo.HealValue.ToString();
		this.bearing.text = playerInfo.BearingValue.ToString();

		this.Show(true);
	}

	void Start()
	{
		this.Show(false);
	}

	private void Show(bool show)
	{
		var color = show ? Color.white : Color.clear;

		this.playerImage.color = color;
		this.playerName.color = color;
		this.kill.color = color;
		this.attack.color = color;
		this.heal.color = color;
		this.bearing.color = color;
	}
}
