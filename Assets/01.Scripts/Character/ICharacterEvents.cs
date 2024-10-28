using System;

public interface ICharacterEvents
{
    // Ǯ�� ���� �̺�Ʈ
    event Action OnResetPool;
    // ü�� ������ �̺�Ʈ
    event Action<float> OnHpChange;
    event Action<int> OnSpiritChange;
    // ���� ���� �̺�Ʈ
    event Action OnSetDefaultData; // ĳ���� �⺻ ������ �ʱ�ȭ
    event Action OnStartCharacter; // ���� ���۽� ����
    event Action OnEndCharacter; // ���� ����/ĳ���� ����� ����
}