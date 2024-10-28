using System;

public interface ICharacterEvents
{
    // 풀링 시점 이벤트
    event Action OnResetPool;
    // 체력 변동시 이벤트
    event Action<float> OnHpChange;
    event Action<int> OnSpiritChange;
    // 전투 관련 이벤트
    event Action OnSetDefaultData; // 캐릭터 기본 정보로 초기화
    event Action OnStartCharacter; // 전투 시작시 발행
    event Action OnEndCharacter; // 전투 종료/캐릭터 사망시 발행
}