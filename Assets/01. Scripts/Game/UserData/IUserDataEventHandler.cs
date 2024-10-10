public interface IUserDataEventHandler
{
	/// <summary>
	/// ������ �߰��� �� ����
	/// ������ �ִ� ������ ������ �߰���
	/// </summary>
	void OnAddUser(UserData data);

	/// <summary>
	/// ������ ���� �� ����
	/// �÷��̾ ������ �� ��ư�� �ٽ� ����
	/// </summary>
	void OnRemoveUser(UserData data);

	/// <summary>
	/// �����Ͱ� ����� �� ����
	/// ������ ���÷��� �������� �� �����
	/// </summary>
	void OnValueChangedUser(UserData data);
}