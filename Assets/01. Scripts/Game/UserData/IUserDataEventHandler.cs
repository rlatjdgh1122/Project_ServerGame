public interface IUserDataEventHandler
{
	/// <summary>
	/// 유저가 추가될 때 실행
	/// 기존에 있던 유저의 정보도 추가됨
	/// </summary>
	void OnAddUser(UserData data);

	/// <summary>
	/// 유저가 나갈 때 실행
	/// 플레이어가 나가면 그 버튼은 다시 켜줌
	/// </summary>
	void OnRemoveUser(UserData data);

	/// <summary>
	/// 데이터가 변경될 때 실행
	/// 유저가 팀컬러를 선택했을 때 실행됨
	/// </summary>
	void OnValueChangedUser(UserData data);
}