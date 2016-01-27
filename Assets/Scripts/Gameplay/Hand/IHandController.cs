public interface IHandController
{
    bool PlayerHand { get; }

    void RemoveCard(CardController card);
    void UpdateCardPositions();
    int GetDirection();
    void DrawCard();
}