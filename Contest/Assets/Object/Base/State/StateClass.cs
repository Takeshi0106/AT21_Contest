// =================================
// ��ԃN���X�̊��N���X
// =================================

// State�̊��N���X
public abstract class StateClass<T> where T : BaseState<T>
{
    // ��Ԃ�ύX����
    public abstract void Change(T state);
    // ��Ԃ̊J�n����
    public abstract void Enter(T currentState);
    // ��Ԓ��̏���
    public abstract void Excute(T currentState);
    // ��Ԓ��̏I������
    public abstract void Exit(T currentState);
}
